using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CreativeSpore.RpgMapEditor
{

    /// <summary>
    /// Create and manage the auto tile map
    /// </summary>
	[RequireComponent(typeof(AutoTileMapGui))]
	[RequireComponent(typeof(AutoTileMapEditorBehaviour))]
	public class AutoTileMap : MonoBehaviour 
	{
		public static AutoTileMap Instance{ get; private set; }

        public const int k_emptyTileId = -1;
        public const int k_outofboundsTileId = -2;

        public delegate void OnMapLoadedDelegate( AutoTileMap autoTileMap );
        /// <summary>
        /// Called when map has been loaded
        /// </summary>
        public OnMapLoadedDelegate OnMapLoaded;

        [Serializable]
        public class MapLayer
        {
            public bool Visible = true;
            public string Name = "layer";
            public eLayerType LayerType = eLayerType.Ground;
            public string SortingLayer = "Default";
            public int SortingOrder = 0;
            public float Depth = 0;

            /// <summary>
            /// Index of TileLayers with tiles of this layer. Used only to be able to rearrange elements using ReorderableList in AutoTileMapEditor.
            /// </summary>
            public int TileLayerIdx = -1;
        }

        [SerializeField]
        AutoTileset m_autoTileset;
        /// <summary>
        /// Tileset used by this map to draw the tiles
        /// </summary>
		public AutoTileset Tileset
        {
            get { return m_autoTileset; }
            set
            {
                bool isChanged = m_autoTileset != value;
                m_autoTileset = value;
                if (isChanged)
                {
                    LoadMap();
                }
            }
        }

		[SerializeField]
		AutoTileMapData m_mapData;
        /// <summary>
        /// Tile data for this map
        /// </summary>
		public AutoTileMapData MapData
		{ 
			get{ return m_mapData; } 
			set
			{
				bool isChanged = m_mapData != value;
				m_mapData = value;
				if( isChanged )
				{
					LoadMap();
				}
			}
		}

		[SerializeField]
		AutoTileBrush m_brushGizmo;
        /// <summary>
        /// Brush used to paint tiles on this map
        /// </summary>
		public AutoTileBrush BrushGizmo
		{
			get
			{
				if( m_brushGizmo == null )
				{
					GameObject objBrush = new GameObject();
					objBrush.name = "BrushGizmo";
					objBrush.transform.parent = transform;
					m_brushGizmo = objBrush.AddComponent<AutoTileBrush>();
					m_brushGizmo.MyAutoTileMap = this;
				}
				return m_brushGizmo;
			}	
		}

        /// <summary>
        /// Reference to the Sprite Renderer used to draw the minimap in the editor
        /// </summary>
		public SpriteRenderer EditorMinimapRender;

        /// <summary>
        /// Minimap texture for this map
        /// </summary>
		public Texture2D MinimapTexture{ get; private set; }

        /// <summary>
        /// Width of this map in tiles
        /// </summary>
		public int MapTileWidth		{ get{ return MapData != null? MapData.Data.TileMapWidth : 0; } }
        /// <summary>
        /// Height of this map in tiles
        /// </summary>
		public int MapTileHeight 	{ get{ return MapData != null? MapData.Data.TileMapHeight : 0; } }
        /// <summary>
        /// The size of the tilemap cell in units
        /// </summary>
        public Vector2 CellSize { get { return m_cellSize; } set { m_cellSize = value; } }

        /// <summary>
        /// Main camera used to view this map
        /// </summary>
		public Camera ViewCamera;

        /// <summary>
        /// Component used to edit the map on play
        /// </summary>
		public AutoTileMapGui AutoTileMapGui{ get; private set; }

        /// <summary>
        /// Speed of animated tiles in frames per second
        /// </summary>
		public float AnimatedTileSpeed = 6f;

        /// <summary>
        /// If true, map collisions will be enabled.
        /// </summary>
		public bool IsCollisionEnabled = true;

        /// <summary>
        /// If map has been initialized
        /// </summary>
        public bool IsInitialized { get { return TileLayers != null && TileLayers.Count > 0; } }

        /// <summary>
        /// If map is loading
        /// </summary>
        public bool IsLoading { get; private set; }

        //NOTE: can't be inside MapLayer because ReorderableList only copy serialized data, and this is not serializable. Make AutoTile will brake performance.
        public List<AutoTile[]> TileLayers; 

        /// <summary>
        /// Set map visibility
        /// </summary>
		public bool IsVisible
		{
			get{ return m_isVisible; }
			set{ m_isVisible = value;}
		}
		private bool m_isVisible = true;

        /// <summary>
        /// If true, changes made to the map in game will be applied after stop playing and going back to the editor.
        /// If you set this to true while playing and load a different scene with a different map with also this set to true, after going back to the editor,
        /// the scene map will be modified with second map. So be careful.
        /// </summary>
		public bool SaveChangesAfterPlaying = true;

        /// <summary>
        /// If true, display the tile grid when AutoTileMap object is selected in editor
        /// </summary>
        public bool ShowGrid = true;

        /// <summary>
        /// The current frame of a 3 frames tile animation
        /// </summary>
		public int TileAnim3Frame{ get{ return (int)m_tileAnim3Frame; } }
        /// <summary>
        /// The current frame of a 4 frames tile animation
        /// </summary>
        public int TileAnim4Frame { get { return (int)m_tileAnim4Frame; } }

        /// <summary>
        /// If a frame in the animation has changed
        /// </summary>
		public bool TileAnimFrameHasChanged{ get; private set; }

        public List<MapLayer> MapLayers = null;


		private Texture2D m_minimapTilesTexture;
		private float m_tileAnim3Frame = 0f;
        private float m_tileAnim4Frame = 0f;
        [SerializeField]
        private Vector2 m_cellSize;
		[SerializeField]
		private TileChunkPool m_tileChunkPoolNode;

		void Awake()
		{
			if( Instance == null )
			{
				//DontDestroyOnLoad(gameObject); //check how to deal this after make demo with transitions. Should be only one AutoTileMap instance but not persistent
				Instance = this;
                IsLoading = false;

				if( CanBeInitialized() )
				{
					if( Application.isPlaying && ViewCamera && ViewCamera.name == "SceneCamera" )
					{
						ViewCamera = null;
					}
					LoadMap();
                    //StartCoroutine(LoadMapAsync()); // test asynchronous load
					BrushGizmo.Clear();
                    BrushGizmo.SelectedLayer = 0;
					IsVisible = true;
				}
				else
				{
					Debug.LogWarning(" Autotilemap cannot be initialized because Tileset and/or Map Data is missing. Press create button in the inspector to create the missing part or select one.");
				}
			}
			else if( Instance != this )
			{
				Destroy( transform.gameObject );
			}		
		}

        void OnValidate()
        {
            IsLoading = false;
        }

		void OnDisable()
		{
			if( IsInitialized && SaveChangesAfterPlaying )
			{
				SaveMap();
			}
		}

		void OnDestroy()
		{
			if( m_brushGizmo != null )
			{
	#if UNITY_EDITOR
				DestroyImmediate(m_brushGizmo.gameObject);
	#else
				Destroy(m_brushGizmo.gameObject);
	#endif
			}

            DestroyImmediate( m_minimapTilesTexture );
            DestroyImmediate( MinimapTexture );
		}

        /// <summary>
        /// When the game object is selected this will draw the grid
        /// ref: http://wiki.unity3d.com/index.php/2D_Tilemap_Starter_Kit
        /// </summary>
        /// <remarks>Only called when in the Unity editor.</remarks>
        private void OnDrawGizmosSelected()
        {
            if (ShowGrid && Tileset != null)
            {

                // store map width, height and position
                var mapWidth = MapTileWidth * CellSize.x;
                var mapHeight = -MapTileHeight * CellSize.y;
                var position = this.transform.position;
                position.z = Camera.current.transform.position.z + 0.1f;

                // draw layer border
                Gizmos.color = Color.white;
                Gizmos.DrawLine(position, position + new Vector3(mapWidth, 0, 0));
                Gizmos.DrawLine(position, position + new Vector3(0, mapHeight, 0));
                Gizmos.DrawLine(position + new Vector3(mapWidth, 0, 0), position + new Vector3(mapWidth, mapHeight, 0));
                Gizmos.DrawLine(position + new Vector3(0, mapHeight, 0), position + new Vector3(mapWidth, mapHeight, 0));

                // draw tile cells
                Gizmos.color = Color.grey;
                for (float i = 1; i < MapTileWidth; i++)
                {
                    Gizmos.DrawLine(position + new Vector3(i * CellSize.x, 0, 0), position + new Vector3(i * CellSize.x, mapHeight, 0));
                }

                for (float i = 1; i < MapTileHeight; i++)
                {
                    Gizmos.DrawLine(position + new Vector3(0, -i * CellSize.y, 0), position + new Vector3(mapWidth, -i * CellSize.y, 0));
                }
            }
        }

        /// <summary>
        /// Update tile chunks of the map.
        /// </summary>
		public void UpdateChunks()
		{
			m_tileChunkPoolNode.UpdateChunks();
		}

        /// <summary>
        /// Find the last layer found of provided type or null if not found.
        /// </summary>
        /// <param name="layerType">Layer type to be found</param>
        /// <param name="startIndex">The zero-based starting index of the search</param>
        public MapLayer FindLastLayer(eLayerType layerType, int startIndex = 0)
        {
            for (int i = MapLayers.Count - 1; i >= startIndex; --i)
                if (MapLayers[i].LayerType == layerType) return MapLayers[i];
            return null;
        }

        public int FindLastLayerIdx(eLayerType layerType, int startIndex = 0)
        {
            for (int i = MapLayers.Count - 1; i >= startIndex; --i)
                if (MapLayers[i].LayerType == layerType) return i;
            return -1;
        }

        /// <summary>
        /// Find the first layer found of provided type or null if not found.
        /// </summary>
        /// <param name="layerType">Layer type to be found</param>
        /// <param name="startIndex">The zero-based starting index of the search</param>
        /// <returns></returns>
        public MapLayer FindFirstLayer(eLayerType layerType, int startIndex = 0)
        {
            for (int i = startIndex; i < MapLayers.Count; ++i)
                if (MapLayers[i].LayerType == layerType) return MapLayers[i];
            return null;
        }

        public int FindFirstLayerIdx(eLayerType layerType, int startIndex = 0)
        {
            for (int i = startIndex; i < MapLayers.Count; ++i)
                if (MapLayers[i].LayerType == layerType) return i;
            return -1;
        }

        /// <summary>
        /// Update tile chunk nodes using data from MapLayers. Call this method after changing depth and/or layer in MapLayers.
        /// </summary>
        public void UpdateChunkLayersData()
        {
            if (MapLayers.Count > 0)
            {
                m_tileChunkPoolNode.CreateLayers(MapLayers.Count);
                m_tileChunkPoolNode.UpdateLayersData();
            }
        }

        /// <summary>
        /// Mark all tilechunks of a map layer to be updated
        /// </summary>
        /// <param name="mapLayer"></param>
        public void MarkLayerChunksForUpdate( MapLayer mapLayer )
        {
            m_tileChunkPoolNode.MarkLayerChunksForUpdate(mapLayer.TileLayerIdx);
        }

        /// <summary>
        /// Reset the pool of tile chunks.
        /// </summary>
        public void ResetTileChunkPool()
        {
            m_tileChunkPoolNode.Initialize(this);
        }

        /// <summary>
        /// Load Map according to MapData.
        /// </summary>
		public void LoadMap()
        {
            if( Tileset != null && m_cellSize == Vector2.zero)
            {
                m_cellSize = new Vector2(Tileset.TileWidth / AutoTileset.PixelToUnits, Tileset.TileHeight / AutoTileset.PixelToUnits);
            }
            IEnumerator coroutine = LoadMapAsync();
            while (coroutine.MoveNext()) ;
        }

        /// <summary>
        /// Load Map asynchronously according to MapData.
        /// </summary>
		public IEnumerator LoadMapAsync()
		{
            if (IsLoading)
            {
                //Debug.LogWarning("Cannot load map while loading");
            }
            else
            {
                IsLoading = true;
                if (Tileset != null && Tileset.AtlasTexture != null)
                {

                    if (MapData != null)
                    {

                        IEnumerator coroutine = MapData.Data.LoadToMap(this);
                        while (coroutine.MoveNext()) yield return null;

                        coroutine = m_tileChunkPoolNode.UpdateChunksAsync();
                        while (coroutine.MoveNext()) yield return null;

                        //+++ Fog Of War
                        StopCoroutine(FogOfWarCoroutine());
                        m_fogOfWarSetRequests.Clear();
                        m_fogOfWarTilesToUpdate.Clear();
                        //NOTE: finds first fog of war layer, not more than one
                        m_fogOfWarMapLayer = MapLayers.Find(x => x.LayerType == eLayerType.FogOfWar);
                        if (m_fogOfWarMapLayer != null && Application.isPlaying)
                        {
                            //NOTE: for some unknown reasong, starting the fog of war coroutine here, make it to be frozen
                            // when calling "yield return null" for the first time during several seconds. 
                            // This only happens the first time after clearing the fog of war layer. This trick fix the issue.
                            m_fogOfWarInitCoroutineOnNextUpdate = true;
                        }
                        //---

                        //+++free unused resources
                        Resources.UnloadUnusedAssets();
                        System.GC.Collect();
                        //---
                    }
                }

                IsLoading = false;
                if (OnMapLoaded != null)
                {
                    OnMapLoaded(this);
                }
            }
            yield return null;
		}
		
        /// <summary>
        /// Save current map to MapData
        /// </summary>
        /// <returns></returns>
		public bool SaveMap( int width = -1, int height = -1 )
		{
            bool isOk = false;
            if (IsLoading)
            {
                //Debug.LogWarning("Cannot save map while loading");
            }
            else
            {
                //Debug.Log("AutoTileMap:SaveMap");
                if (width < 0) width = MapTileWidth;
                if (height < 0) height = MapTileHeight;
                isOk = MapData.Data.SaveData(this, width, height);
#if UNITY_EDITOR
                EditorUtility.SetDirty(MapData);
                AssetDatabase.SaveAssets();
#endif
            }
            return isOk;
		}

        /// <summary>
        /// Display a load dialog to load a map saved as xml
        /// </summary>
        /// <returns></returns>
		public bool ShowLoadDialog()
		{
	#if UNITY_EDITOR
			string filePath = EditorUtility.OpenFilePanel( "Load tilemap",	"", "xml");
			if( filePath.Length > 0 )
			{
				AutoTileMapSerializeData mapData = AutoTileMapSerializeData.LoadFromFile( filePath );
				MapData.Data.CopyData( mapData );
				LoadMap();
				return true;
			}
	#else
			string xml = PlayerPrefs.GetString("XmlMapData", "");
			if( !string.IsNullOrEmpty(xml) )
			{
				AutoTileMapSerializeData mapData = AutoTileMapSerializeData.LoadFromXmlString( xml );
				MapData.Data.CopyData( mapData );
				LoadMap();
				
				return true;
			}
	#endif
			return false;
		}

        /// <summary>
        /// Display a save dialog to save the current map in xml format
        /// </summary>
		public void ShowSaveDialog()
		{
	#if UNITY_EDITOR
			string filePath = EditorUtility.SaveFilePanel( "Save tilemap",	"",	"map" + ".xml",	"xml");
			if( filePath.Length > 0 )
			{
				SaveMap();
				MapData.Data.SaveToFile( filePath );
			}
	#else
			SaveMap();
			string xml = MapData.Data.GetXmlString( );
			PlayerPrefs.SetString("XmlMapData", xml);
	#endif
		}

        /// <summary>
        /// If map can be initialized
        /// </summary>
        /// <returns></returns>
		public bool CanBeInitialized()
		{
			return Tileset != null && Tileset.AtlasTexture != null && MapData != null;
		}

        /// <summary>
        /// Initialize the map
        /// </summary>
		public void Initialize()
		{
			//Debug.Log("AutoTileMap:Initialize");

			if( MapData == null )
			{
				Debug.LogError(" AutoTileMap.Initialize called when MapData was null");
			}
			else if( Tileset == null || Tileset.AtlasTexture == null )
			{
				Debug.LogError(" AutoTileMap.Initialize called when Tileset or Tileset.TilesetsAtlasTexture was null");
			}
			else
			{
                //Set the instance if executed in editor where Awake is not called
                if( Instance == null ) Instance = this;

				Tileset.GenerateAutoTileData();

                //TODO: Allow changing minimap offset to allow minimaps smaller than map size
                int minimapWidth = Mathf.Min(MapTileWidth, 2048); //NOTE: 2048 is a maximum safe size for a texture
                int minimapHeigh = Mathf.Min(MapTileHeight, 2048);
                if( MinimapTexture != null )
                {
                    DestroyImmediate( MinimapTexture );
                }
                MinimapTexture = new Texture2D(minimapWidth, minimapHeigh);
				MinimapTexture.anisoLevel = 0;
				MinimapTexture.filterMode = FilterMode.Point;
				MinimapTexture.name = "MiniMap";
                MinimapTexture.hideFlags = HideFlags.DontSave;

                int tileNb = m_autoTileset.TilesCount;
                int minimapSize = Mathf.CeilToInt( (float)Math.Sqrt( tileNb ) );
                if (m_minimapTilesTexture != null)
                {
                    DestroyImmediate(m_minimapTilesTexture);
                }
                m_minimapTilesTexture = new Texture2D(minimapSize, minimapSize);
				m_minimapTilesTexture.anisoLevel = 0;
				m_minimapTilesTexture.filterMode = FilterMode.Point;
				m_minimapTilesTexture.name = "MiniMapTiles";
                m_minimapTilesTexture.hideFlags = HideFlags.DontSave;
				
				_GenerateMinimapTilesTexture();

				if( Application.isEditor )
				{
					if( EditorMinimapRender == null )
					{
						GameObject objMinimap = new GameObject();
						objMinimap.name = "Minimap";
						objMinimap.transform.parent = transform;
						EditorMinimapRender = objMinimap.AddComponent<SpriteRenderer>();
						EditorMinimapRender.GetComponent<Renderer>().enabled = false;
					}
					Rect rMinimap = new Rect(0f, 0f, MinimapTexture.width, MinimapTexture.height);
					Vector2 pivot = new Vector2(0f, 1f);
					EditorMinimapRender.sprite = Sprite.Create(MinimapTexture, rMinimap, pivot, AutoTileset.PixelToUnits);
                    EditorMinimapRender.transform.localScale = new Vector3(Tileset.TileWidth, Tileset.TileHeight);
				}
				
                MapLayers = new List<MapLayer>();
                TileLayers = new List<AutoTile[]>();
				
				AutoTileMapGui = GetComponent<AutoTileMapGui>();

				if( m_tileChunkPoolNode == null )
				{
					string nodeName = name+" Data";
					GameObject obj = GameObject.Find( nodeName );
					if( obj == null ) obj = new GameObject();
					obj.name = nodeName;
					m_tileChunkPoolNode = obj.AddComponent<TileChunkPool>();
				}
				m_tileChunkPoolNode.Initialize( this );                
			}
		}

        /// <summary>
        /// Clean all tiles of the map
        /// </summary>
		public void ClearMap()
		{
			if( MapLayers != null )
			{
                foreach (MapLayer mapLayer in MapLayers)
				{
                    for (int i = 0; i < TileLayers[mapLayer.TileLayerIdx].Length; ++i)
					{
                        TileLayers[mapLayer.TileLayerIdx][i] = null;
					}
				}
			}
            // remove all tile chunks
            m_tileChunkPoolNode.Initialize(this);
		}

        /// <summary>
        /// Clear all tiles of the map layer
        /// </summary>
        /// <param name="mapLayer"></param>
        public void ClearLayer(MapLayer mapLayer)
        {
            if (mapLayer.LayerType == eLayerType.FogOfWar)
            {
                for ( int ty = 0; ty < MapTileHeight; ++ty )
                {
                    for ( int tx = 0; tx < MapTileWidth; ++tx )
                    {
                        SetAutoTile(tx, ty, -1, mapLayer.TileLayerIdx);
                    }
                }
            }
            else
            {
                for (int i = 0; i < TileLayers[mapLayer.TileLayerIdx].Length; ++i)
                {
                    AutoTile autoTile = TileLayers[mapLayer.TileLayerIdx][i];
                    if (autoTile != null)
                    {
                        autoTile.Id = -1;
                    }
                }
            }
        }

		//public int _Debug_SpriteRenderCounter = 0; //debug
		private int __prevTileAnimFrame = -1;
		void Update () 
		{
			if( !IsInitialized )
			{
				return;
			}

			BrushGizmo.gameObject.SetActive( AutoTileMapGui.enabled );

            m_tileAnim4Frame += Time.deltaTime * AnimatedTileSpeed;
            while (m_tileAnim4Frame >= 4f) m_tileAnim4Frame -= 4f;
			m_tileAnim3Frame += Time.deltaTime * AnimatedTileSpeed;
			while( m_tileAnim3Frame >= 3f ) m_tileAnim3Frame -= 3f;
			TileAnimFrameHasChanged = (int)m_tileAnim3Frame != __prevTileAnimFrame ;
			__prevTileAnimFrame = (int)m_tileAnim3Frame;	

			m_tileChunkPoolNode.UpdateChunks();

            if( m_fogOfWarInitCoroutineOnNextUpdate )
            {
                m_fogOfWarInitCoroutineOnNextUpdate = false;
                StartCoroutine(FogOfWarCoroutine());
            }
		}

        /// <summary>
        /// Return the number of map layers
        /// </summary>
        /// <returns></returns>
        public int GetLayerCount()
        {
            return MapLayers.Count;
        }

        /// <summary>
        /// Check if a tile in the given position has alpha
        /// </summary>
        /// <param name="autoTileIdx">Index position of the tile in the map</param>
        /// <returns></returns>
		public bool IsAutoTileHasAlpha( int autoTileIdx )
		{
			if( (autoTileIdx >= 0) && (autoTileIdx < Tileset.IsAutoTileHasAlpha.Length) )
			{
				return Tileset.IsAutoTileHasAlpha[ autoTileIdx ];
			}
			return false;
		}

        /// <summary>
        /// Check if the tile position is inside the map
        /// </summary>
        /// <param name="gridX">Tile x position of the map</param>
        /// <param name="gridY">Tile y position of the map</param>
        /// <returns></returns>
		public bool IsValidAutoTilePos( int gridX, int gridY )
		{
            return !(gridX < 0 || gridX >= MapData.Data.TileMapWidth || gridY < 0 || gridY >= MapData.Data.TileMapHeight);
		}

        private AutoTile m_emptyAutoTile = new AutoTile() { Id = k_emptyTileId };
        
        /// <summary>
        /// Return the AutoTile data for a tile in the provided tile position and layer
        /// </summary>
        /// <param name="gridX">Tile x position of the map</param>
        /// <param name="gridY">Tile y position of the map</param>
        /// <param name="iLayer">Tile layer, see eTileLayer </param>
        /// <returns></returns>
		public AutoTile GetAutoTile( int gridX, int gridY, int iLayer )
		{
            //TODO: check if AutoTile could be an struct to avoid this
			//ProfilerSample.BeginSample("AutoTile");
            //m_emptyAutoTile = new AutoTile() { Id = k_emptyTileId }; //FIX: some issues (ex: Movingbehaviour) where this is taking as reference causing bugs
			//ProfilerSample.EndSample();

            //m_emptyAutoTile.TileX = gridX;
            //m_emptyAutoTile.TileY = gridY;

            if (IsValidAutoTilePos(gridX, gridY) && iLayer < MapLayers.Count)
            {
                //m_emptyAutoTile.Id = k_emptyTileId;
                AutoTile autoTile = TileLayers[MapLayers[iLayer].TileLayerIdx][gridX + gridY * MapTileWidth];
				if (MapLayers == null || autoTile == null) {
					ProfilerSample.BeginSample("AutoTile1");
					m_emptyAutoTile = new AutoTile () { Id = k_emptyTileId };
					ProfilerSample.EndSample();
					m_emptyAutoTile.TileX = gridX;
					m_emptyAutoTile.TileY = gridY;
					m_emptyAutoTile.Id = k_emptyTileId;
					return m_emptyAutoTile;
				} else {
					return autoTile;
				}
                //return (MapLayers == null || autoTile == null) ? m_emptyAutoTile : autoTile;
            }
            else
			{
				ProfilerSample.BeginSample("AutoTile2");
				m_emptyAutoTile = new AutoTile() { Id = k_emptyTileId };
				ProfilerSample.EndSample();
                m_emptyAutoTile.Id = k_outofboundsTileId;
                return m_emptyAutoTile;
            }
		}

		/// <summary>
		/// Return the AutoTile data for a tile in the provided tile position and layer
		/// </summary>
		/// <param name="gridX">Tile x position of the map</param>
		/// <param name="gridY">Tile y position of the map</param>
		/// <param name="iLayer">Tile layer, see eTileLayer </param>
		/// <returns></returns>
		public AutoTile GetAutoTileOpti( int gridX, int gridY, int iLayer )
		{
			//TODO: check if AutoTile could be an struct to avoid this
			//ProfilerSample.BeginSample("AutoTile");
			//m_emptyAutoTile = new AutoTile() { Id = k_emptyTileId }; //FIX: some issues (ex: Movingbehaviour) where this is taking as reference causing bugs
			//ProfilerSample.EndSample();

			//m_emptyAutoTile.TileX = gridX;
			//m_emptyAutoTile.TileY = gridY;

			if (IsValidAutoTilePos(gridX, gridY) && iLayer < MapLayers.Count)
			{
				//m_emptyAutoTile.Id = k_emptyTileId;
				AutoTile autoTile = TileLayers[MapLayers[iLayer].TileLayerIdx][gridX + gridY * MapTileWidth];
				if (MapLayers == null || autoTile == null) {
					return null;
				} else {
					return autoTile;
				}
				//return (MapLayers == null || autoTile == null) ? m_emptyAutoTile : autoTile;
			}
			else
			{
				return null;
			}
		}

		// calculate tileset idx of autotile base in the number of tiles of each tileset
		private eTileType _GetAutoTileType( AutoTile autoTile )
		{
            SubTilesetConf tilesetConf = Tileset.SubTilesets[autoTile.TilesetIdx];
			if( tilesetConf.HasAutotiles )
            {
                int relTileIdx = autoTile.Id % AutoTileset.k_TilesPerSubTileset;
                if( relTileIdx >= 0 && relTileIdx < 16 )        return eTileType.ANIMATED;
                else if( relTileIdx >= 16 && relTileIdx < 48 )  return eTileType.GROUND;
                else if (relTileIdx >= 48 && relTileIdx < 80 )  return eTileType.BUILDINGS;
                else if (relTileIdx >= 80 && relTileIdx < 128)  return eTileType.WALLS;
                else return eTileType.NORMAL;
            }
            else return eTileType.OBJECTS;
		}

        /// <summary>
        /// Add a map layer to the map
        /// </summary>
        /// <returns></returns>
        public MapLayer AddMapLayer()
        {
            AutoTileMap.MapLayer mapLayer = MapLayers.Count > 0 ? MapLayers[MapLayers.Count - 1] : null;
            if (mapLayer != null)
            {
                string sName = "new layer ";
                int i = 0;
                for (; MapLayers.Find(x => x.Name == (sName + i)) != null; ++i) ;
                sName += i;
                MapLayers.Add(new AutoTileMap.MapLayer()
                {
                    Depth = mapLayer.Depth,
                    SortingLayer = mapLayer.SortingLayer,
                    SortingOrder = mapLayer.SortingOrder,
                    LayerType = mapLayer.LayerType,
                    Name = sName,
                    Visible = true,
                    TileLayerIdx = MapLayers.Count
                });
            }
            else
            {
                MapLayers.Add(new AutoTileMap.MapLayer() { TileLayerIdx = MapLayers.Count });
            }
            TileLayers.Add(new AutoTile[MapTileWidth * MapTileHeight]);
            return MapLayers[MapLayers.Count - 1];
        }

        /// <summary>
        /// Set a tile in the grid coordinates specified and layer ( 0: ground, 1: overground, 2: overlay )
        /// </summary>
        /// <param name="gridX">Tile x position of the map</param>
        /// <param name="gridY">Tile y position of the map</param>
        /// <param name="tileId">This is the id of the tile. You can see it in the editor while editing the map in the top left corner. Use -1 for an empty tile</param>
        /// <param name="iLayer"> Layer where to set the tile ( 0: ground, 1: overground, 2: overlay )</param>
        /// <param name="refreshTile">If tile and neighbors should be refreshed by this method or do it layer</param>
        public void SetAutoTile(int gridX, int gridY, int tileId, int iLayer, bool refreshTile = true)
		{
			if( !IsValidAutoTilePos( gridX, gridY ) || iLayer >= MapLayers.Count )
			{
				return;
			}

            bool tileHasChange = false;

            //+++ Special Case for Fog of War
            if (MapLayers[iLayer].LayerType == eLayerType.FogOfWar)
            {
                int idx = gridX + gridY * MapTileWidth;
                AutoTile autoTile = TileLayers[MapLayers[iLayer].TileLayerIdx][idx];
                if (autoTile == null)
                {
                    autoTile = new AutoTile();
                    TileLayers[MapLayers[iLayer].TileLayerIdx][idx] = autoTile;
                }
                tileHasChange = autoTile.Id != tileId;
                autoTile.Id = tileId;
                autoTile.TileX = gridX;
                autoTile.TileY = gridY;
                autoTile.Layer = iLayer;
                if (refreshTile && tileHasChange)
                {
                    RefreshTile(autoTile);
                }
            }
            else
            //----
            {
                tileId = Mathf.Clamp(tileId, -1, Tileset.ThumbnailRects.Count - 1);
                int idx = gridX + gridY * MapTileWidth;
                AutoTile autoTile = TileLayers[MapLayers[iLayer].TileLayerIdx][idx];

                if (autoTile == null)
                {
                    autoTile = new AutoTile();
                    TileLayers[MapLayers[iLayer].TileLayerIdx][idx] = autoTile;
                    autoTile.TilePartsType = new eTilePartType[4];
                    autoTile.TilePartsIdx = new int[4];
                }
                int tilesetIdx = tileId / AutoTileset.k_TilesPerSubTileset;
                tileHasChange = autoTile.Id != tileId;
                autoTile.Id = tileId;
                autoTile.TilesetIdx = tilesetIdx;
                autoTile.MappedIdx = tileId < 0 ? -1 : Tileset.AutotileIdxMap[tileId % AutoTileset.k_TilesPerSubTileset];
                autoTile.TileX = gridX;
                autoTile.TileY = gridY;
                autoTile.Layer = iLayer;
                autoTile.Type = _GetAutoTileType(autoTile);

                // refresh tile and neighbours
                if (refreshTile && tileHasChange)
                {
                    for (int xf = -1; xf < 2; ++xf)
                    {
                        for (int yf = -1; yf < 2; ++yf)
                        {
                            RefreshTile(gridX + xf, gridY + yf, iLayer);
                        }
                    }
                }
            }
		}

        /// <summary>
        /// Refresh all tiles of the map. 
        /// Used for optimization, when calling SetAutoTile with refreshTile = false, for a big amount of tiles, this can be called later and refresh all at once.
        /// </summary>
        public void RefreshAllTiles()
        {
            for (int i = 0; i < MapLayers.Count; ++i)
            {
                for (int j = 0; j < TileLayers[MapLayers[i].TileLayerIdx].Length; ++j)
                {
                    RefreshTile(TileLayers[MapLayers[i].TileLayerIdx][j]);
                }
            }            
        }

		private int[,] aTileAff = new int[,]
		{
			{2, 0},
			{0, 2},
			{2, 4},
			{2, 2},
			{0, 4},
		};
		
		private int[,] aTileBff = new int[,]
		{
			{3, 0},
			{3, 2},
			{1, 4},
			{1, 2},
			{3, 4},
		};
		
		private int[,] aTileCff = new int[,]
		{
			{2, 1},
			{0, 5},
			{2, 3},
			{2, 5},
			{0, 3},
		};
		
		private int[,] aTileDff = new int[,]
		{
			{3, 1},
			{3, 5},
			{1, 3},
			{1, 5},
			{3, 3},
		};

        /// <summary>
        /// Refresh a tile according to neighbors
        /// </summary>
        /// <param name="gridX">Tile x position of the map</param>
        /// <param name="gridY">Tile y position of the map</param>
        /// <param name="iLayer"> Layer where to set the tile ( 0: ground, 1: overground, 2: overlay )</param>
		public void RefreshTile( int gridX, int gridY, int iLayer )
		{
			AutoTile autoTile = GetAutoTile( gridX, gridY, iLayer );
			RefreshTile( autoTile );
		}

        /// <summary>
        /// Refresh a tile according to neighbors
        /// </summary>
        /// <param name="autoTile">Tile to be refreshed</param>
		public void RefreshTile( AutoTile autoTile )
		{
            if (autoTile == null) return;

			m_tileChunkPoolNode.MarkUpdatedTile( autoTile.TileX, autoTile.TileY, autoTile.Layer);

            if( MapLayers[autoTile.Layer].LayerType == eLayerType.FogOfWar )
            {
                return;
            }

            SubTilesetConf tilesetConf = Tileset.SubTilesets[autoTile.TilesetIdx];
			if( autoTile.Id >= 0 )
			{
                int relativeTileIdx = autoTile.Id % AutoTileset.k_TilesPerSubTileset;
                // Check if the tile is a normal tile, non autotile
                if (relativeTileIdx >= 128 || !tilesetConf.HasAutotiles) // 128 start with NORMAL tileset, treated differently )
				{
                    if( tilesetConf.HasAutotiles )
                    {
                        relativeTileIdx -= 128; // relative idx to its normal tileset
                    }
                    int tx = relativeTileIdx % Tileset.AutoTilesPerRow;
                    int ty = relativeTileIdx / Tileset.AutoTilesPerRow;

					//fix tileset OBJECTS, the other part of the tileset is in the right side
					if( ty >= 16 )
					{
						ty -= 16;
						tx += 8;
					}
					//---

                    int tileBaseIdx = tilesetConf.TilePartOffset[autoTile.Type == eTileType.OBJECTS? 0 : 4]; // set base tile idx of autoTile tileset ( 4 is the index of Normal tileset in autotilesets )
                    int tileIdx = (autoTile.Type == eTileType.OBJECTS) ? ty * 2 * Tileset.AutoTilesPerRow + tx : ty * Tileset.AutoTilesPerRow + tx;
					tileIdx +=  tileBaseIdx;

					autoTile.TilePartsIdx[ 0 ] = tileIdx;

					// set the kind of tile, for collision use
					autoTile.TilePartsType[ 0 ] = eTilePartType.EXT_CORNER;
					
					// Set Length of tileparts
					autoTile.TilePartsLength = 1;
				}
				else
				{
					int gridX = autoTile.TileX;
					int gridY = autoTile.TileY;
					int iLayer = autoTile.Layer;
					int tilePartIdx = 0;
					for( int j = 0; j < 2; ++j )
					{
						for( int i = 0; i < 2; ++i, ++tilePartIdx )
						{
							int tile_x = gridX*2 + i;
							int tile_y = gridY*2 + j;

							int tilePartX = 0;
							int tilePartY = 0;

							eTilePartType tilePartType;
							if (tile_x % 2 == 0 && tile_y % 2 == 0) //A
							{
								tilePartType = _getTileByNeighbours( autoTile.Id, 
								                               GetAutoTile( gridX, gridY-1, iLayer ).Id, //V 
								                               GetAutoTile( gridX-1, gridY, iLayer ).Id, //H 
								                               GetAutoTile( gridX-1, gridY-1, iLayer ).Id  //D
								                               );
								tilePartX = aTileAff[ (int)tilePartType, 0 ];
								tilePartY = aTileAff[ (int)tilePartType, 1 ];
							} 
							else if (tile_x % 2 != 0 && tile_y % 2 == 0) //B
							{
								tilePartType = _getTileByNeighbours( autoTile.Id, 
								                               GetAutoTile( gridX, gridY-1, iLayer ).Id, //V 
								                               GetAutoTile( gridX+1, gridY, iLayer ).Id, //H 
								                               GetAutoTile( gridX+1, gridY-1, iLayer ).Id  //D
								                               );
								tilePartX = aTileBff[ (int)tilePartType, 0 ];
								tilePartY = aTileBff[ (int)tilePartType, 1 ];
							}
							else if (tile_x % 2 == 0 && tile_y % 2 != 0) //C
							{
								tilePartType = _getTileByNeighbours( autoTile.Id, 
								                               GetAutoTile( gridX, gridY+1, iLayer ).Id, //V 
								                               GetAutoTile( gridX-1, gridY, iLayer ).Id, //H 
								                               GetAutoTile( gridX-1, gridY+1, iLayer ).Id  //D
								                               );
								tilePartX = aTileCff[ (int)tilePartType, 0 ];
								tilePartY = aTileCff[ (int)tilePartType, 1 ];
							}
							else //if (tile_x % 2 != 0 && tile_y % 2 != 0) //D
							{
								tilePartType = _getTileByNeighbours( autoTile.Id, 
								                               GetAutoTile( gridX, gridY+1, iLayer ).Id, //V 
								                               GetAutoTile( gridX+1, gridY, iLayer ).Id, //H 
								                               GetAutoTile( gridX+1, gridY+1, iLayer ).Id  //D
								                               );
								tilePartX = aTileDff[ (int)tilePartType, 0 ];
								tilePartY = aTileDff[ (int)tilePartType, 1 ];
							}

							// set the kind of tile, for collision use
							autoTile.TilePartsType[ tilePartIdx ] = tilePartType;

							int tileBaseIdx = tilesetConf.TilePartOffset[ (int)autoTile.Type ]; // set base tile idx of autoTile tileset
							//NOTE: All tileset have 32 autotiles except the Wall tileset with 48 tiles ( so far it's working because wall tileset is the last one )
							relativeTileIdx = autoTile.MappedIdx - ((int)autoTile.Type * 32); // relative to owner tileset ( All tileset have 32 autotiles )
                            int tx = relativeTileIdx % Tileset.AutoTilesPerRow;
                            int ty = relativeTileIdx / Tileset.AutoTilesPerRow;
							int tilePartSpriteIdx;
							if( autoTile.Type == eTileType.BUILDINGS )
							{
								tilePartY = Mathf.Max( 0, tilePartY - 2);
                                tilePartSpriteIdx = tileBaseIdx + ty * (Tileset.AutoTilesPerRow * 4) * 4 + tx * 4 + tilePartY * (Tileset.AutoTilesPerRow * 4) + tilePartX;
							}
							//NOTE: It's not working with stairs shapes
							// XXXXXX
							// IIIXXX
							// IIIXXX
							// IIIIII
							else if( autoTile.Type == eTileType.WALLS )
							{
								if( ty % 2 == 0 )
								{
									tilePartSpriteIdx = tileBaseIdx + (ty/2) * (Tileset.AutoTilesPerRow * 4) * 10 + tx * 4 + tilePartY * (Tileset.AutoTilesPerRow * 4) + tilePartX;
								}
								else
								{
									//tilePartY = Mathf.Max( 0, tilePartY - 2);
									tilePartY -= 2;
									if( tilePartY < 0 )
									{
										if( tilePartX == 2 && tilePartY == -2 ) 	 {tilePartX = 2; tilePartY = 0;}
										else if( tilePartX == 3 && tilePartY == -2 ) {tilePartX = 1; tilePartY = 0;}
										else if( tilePartX == 2 && tilePartY == -1 ) {tilePartX = 2; tilePartY = 3;}
										else if( tilePartX == 3 && tilePartY == -1 ) {tilePartX = 1; tilePartY = 3;}
									}
									tilePartSpriteIdx = tileBaseIdx + (Tileset.AutoTilesPerRow * 4) * ((ty/2) * 10 + 6) + tx * 4 + tilePartY * (Tileset.AutoTilesPerRow * 4) + tilePartX;
								}
							}
							else
							{
								tilePartSpriteIdx = tileBaseIdx + ty * (Tileset.AutoTilesPerRow * 4) * 6 + tx * 4 + tilePartY * (Tileset.AutoTilesPerRow * 4) + tilePartX;
							}

							autoTile.TilePartsIdx[ tilePartIdx ] = tilePartSpriteIdx;

							// Set Length of tileparts
							autoTile.TilePartsLength = 4;
						}
					}
				}
			}
		}

        /// <summary>
        /// Get the map collision at world position
        /// </summary>
        /// <param name="vPos">World position</param>
        /// <returns></returns>
		public eTileCollisionType GetAutotileCollisionAtPosition( Vector3 vPos )
		{
			vPos -= transform.position;

			// transform to pixel coords
			vPos.y = -vPos.y;

            vPos.x = vPos.x * Tileset.TileWidth / CellSize.x;
            vPos.y = vPos.y * Tileset.TileHeight / CellSize.y;

			if( vPos.x >= 0 && vPos.y >= 0 )
			{
                int tile_x = (int)vPos.x / Tileset.TileWidth;
                int tile_y = (int)vPos.y / Tileset.TileWidth;
                Vector2 vTileOffset = new Vector2((int)vPos.x % Tileset.TileWidth, (int)vPos.y % Tileset.TileHeight);
				for( int iLayer = MapLayers.Count - 1; iLayer >= 0; --iLayer )
				{
                    if (MapLayers[iLayer].LayerType == eLayerType.Ground)
                    {
                        eTileCollisionType tileCollType = GetAutotileCollision(tile_x, tile_y, iLayer, vTileOffset);
                        if (tileCollType != eTileCollisionType.EMPTY && tileCollType != eTileCollisionType.OVERLAY) //remove Overlay check???
                        {
                            return tileCollType;
                        }
                    }
				}
			}
			return eTileCollisionType.PASSABLE;
		}

        /// <summary>
        /// Gets first map collision found for a given grid position
        /// </summary>
        /// <param name="tile_x">Grid position X</param>
        /// <param name="tile_y">Grid position Y</param>
        /// <returns></returns>
        public eTileCollisionType GetCellAutotileCollision(int tile_x, int tile_y)
        {
            for (int iLayer = MapLayers.Count - 1; iLayer >= 0; --iLayer)
            {
                if (MapLayers[iLayer].LayerType == eLayerType.Ground)
                {
                    AutoTile autoTile = GetAutoTile(tile_x, tile_y, iLayer);
                    if (autoTile != null && autoTile.Id >= 0 && autoTile.TilePartsIdx != null)
                    {
                        eTileCollisionType tileCollType = Tileset.AutotileCollType[autoTile.Id];
                        if (tileCollType != eTileCollisionType.EMPTY && tileCollType != eTileCollisionType.OVERLAY) //remove Overlay check???
                        {
                            return tileCollType;
                        }
                    }
                }
            }
            return eTileCollisionType.PASSABLE;
        }

        /// <summary>
        /// Gets map collision over a tile and an offset position relative to the tile
        /// </summary>
        /// <param name="tile_x">X tile coordinate of the map</param>
        /// <param name="tile_y">Y tile coordinate of the map</param>
        /// <param name="layer">Layer of the map (see eLayerType)</param>
        /// <param name="vTileOffset"></param>
        /// <returns></returns>
		public eTileCollisionType GetAutotileCollision( int tile_x, int tile_y, int layer, Vector2 vTileOffset )
		{
			if( IsCollisionEnabled )
			{
				ProfilerSample.BeginSample ("GetAutoTile");
				AutoTile autoTile = GetAutoTileOpti( tile_x, tile_y, layer );
				ProfilerSample.EndSample ();
				if( autoTile != null && autoTile.Id >= 0 && autoTile.TilePartsIdx != null )
				{
                    Vector2 vTilePartOffset = new Vector2(vTileOffset.x % Tileset.TilePartWidth, vTileOffset.y % Tileset.TilePartHeight);
                    int tilePartIdx = autoTile.TilePartsLength == 4 ? 2 * ((int)vTileOffset.y / Tileset.TilePartHeight) + ((int)vTileOffset.x / Tileset.TilePartWidth) : 0;

					ProfilerSample.BeginSample ("_GetTilePartCollision");
					eTileCollisionType tileCollType = _GetTilePartCollision( Tileset.AutotileCollType[ autoTile.Id ], autoTile.TilePartsType[tilePartIdx], tilePartIdx, vTilePartOffset );
					ProfilerSample.EndSample ();
					return tileCollType;
				}
			}
			return eTileCollisionType.EMPTY;
		}

		// NOTE: depending of the collType and tilePartType, this method returns the collType or eTileCollisionType.PASSABLE
		// This is for special tiles like Fence and Wall where not all of tile part should return collisions
		eTileCollisionType _GetTilePartCollision( eTileCollisionType collType, eTilePartType tilePartType, int tilePartIdx, Vector2 vTilePartOffset )
		{
            int tilePartHalfW = Tileset.TilePartWidth / 2;
            int tilePartHalfH = Tileset.TilePartHeight / 2;
			if( collType == eTileCollisionType.FENCE )
			{
				if( tilePartType == eTilePartType.EXT_CORNER || tilePartType == eTilePartType.V_SIDE )
				{
					// now check inner collision ( half left for tile AC and half right for tiles BD )
					// AX|BX|A1|B1	A: 0
					// CX|DX|C1|D1	B: 1
					// A2|B4|A4|B2	C: 2
					// C5|D3|C3|D5	D: 3
					// A5|B3|A3|B5
					// C2|D4|C4|D2
					if( 
					   (tilePartIdx == 0 || tilePartIdx == 2) && (vTilePartOffset.x < tilePartHalfW ) ||
					   (tilePartIdx == 1 || tilePartIdx == 3) && (vTilePartOffset.x > tilePartHalfW )
					)
					{
						return eTileCollisionType.PASSABLE;
					}
                    /* test: removing top part of fence collider
                    else if (tilePartType == eTilePartType.EXT_CORNER && (tilePartIdx == 0 || tilePartIdx == 1) && (vTilePartOffset.y < Tileset.TilePartHeight))
                    {
                        return eTileCollisionType.PASSABLE;
                    }*/
				}
                /* test: removing top part of fence collider
                else if ((tilePartIdx == 0 || tilePartIdx == 1) && (vTilePartOffset.y < Tileset.TilePartHeight))
                {                   
                    return eTileCollisionType.PASSABLE;
                }*/
			}
			else if( collType == eTileCollisionType.WALL )
			{
				if( tilePartType == eTilePartType.INTERIOR )
				{
					return eTileCollisionType.PASSABLE;
				}
				else if( tilePartType == eTilePartType.H_SIDE )
				{
					if( 
					   (tilePartIdx == 0 || tilePartIdx == 1) && (vTilePartOffset.y >= tilePartHalfH ) ||
					   (tilePartIdx == 2 || tilePartIdx == 3) && (vTilePartOffset.y < tilePartHalfH )
					   )
					{
						return eTileCollisionType.PASSABLE;
					}
				}
				else if( tilePartType == eTilePartType.V_SIDE )
				{
					if( 
					   (tilePartIdx == 0 || tilePartIdx == 2) && (vTilePartOffset.x >= tilePartHalfW ) ||
					   (tilePartIdx == 1 || tilePartIdx == 3) && (vTilePartOffset.x < tilePartHalfW )
					   )
					{
						return eTileCollisionType.PASSABLE;
					}
				}
				else
				{
					Vector2 vRelToIdx0 = vTilePartOffset; // to check only the case (tilePartIdx == 0) vTilePartOffset coords are mirrowed to put position over tileA with idx 0
					vRelToIdx0.x = (int)vRelToIdx0.x; // avoid precission errors when mirrowing, as 0.2 is 0, but -0.2 is 0 as well and should be -1
					vRelToIdx0.y = (int)vRelToIdx0.y;
                    if (tilePartIdx == 1) vRelToIdx0.x = -vRelToIdx0.x + Tileset.TilePartWidth - 1;
                    else if (tilePartIdx == 2) vRelToIdx0.y = -vRelToIdx0.y + Tileset.TilePartHeight - 1;
                    else if (tilePartIdx == 3) vRelToIdx0 = -vRelToIdx0 + new Vector2(Tileset.TilePartWidth - 1, Tileset.TilePartHeight - 1);

					if( tilePartType == eTilePartType.INT_CORNER )
					{
						if( (int)vRelToIdx0.x / tilePartHalfW == 1 || (int)vRelToIdx0.y / tilePartHalfH == 1 )
						{
							return eTileCollisionType.PASSABLE;
						}
					}
					else if( tilePartType == eTilePartType.EXT_CORNER )
					{
						if( (int)vRelToIdx0.x / tilePartHalfW == 1 && (int)vRelToIdx0.y / tilePartHalfH == 1 )
						{
							return eTileCollisionType.PASSABLE;
						}
					}

				}
			}
			return collType;
		}

		// V vertical, H horizontal, D diagonal
		private eTilePartType _getTileByNeighbours( int tileId, int tileIdV, int tileIdH, int tileIdD )
		{
            if (tileIdV == k_outofboundsTileId) tileIdV = tileId;
            if (tileIdH == k_outofboundsTileId) tileIdH = tileId;
            if (tileIdD == k_outofboundsTileId) tileIdD = tileId;

			if (
				(tileIdV == tileId) &&
				(tileIdH == tileId) &&
				(tileIdD != tileId)
				) 
			{
				return eTilePartType.INT_CORNER;
			}
			else if (
				(tileIdV != tileId) &&
				(tileIdH != tileId)
				) 
			{
				return eTilePartType.EXT_CORNER;
			}
			else if (
				(tileIdV == tileId) &&
				(tileIdH == tileId) &&
				(tileIdD == tileId)
				) 
			{
				return eTilePartType.INTERIOR;
			}
			else if (
				(tileIdV != tileId) &&
				(tileIdH == tileId)
				) 
			{
				return eTilePartType.H_SIDE;
			}
			else /*if (
				(tile_typeV == tile_type) &&
				(tile_typeH != tile_type)
				)*/
			{
				return eTilePartType.V_SIDE;
			}
		}

		Color _support_GetAvgColorOfTexture( Texture2D _texture, Rect _srcRect )
		{
			float r, g, b, a;
			r = g = b = a = 0;
			Color[] aColors = _texture.GetPixels( Mathf.RoundToInt(_srcRect.x), Mathf.RoundToInt(_srcRect.y), Mathf.RoundToInt(_srcRect.width), Mathf.RoundToInt(_srcRect.height));
			for( int i = 0; i < aColors.Length; ++i )
			{
				r += aColors[i].r;
				g += aColors[i].g;
				b += aColors[i].b;
				a += aColors[i].a;
			}
			r /= aColors.Length;
			g /= aColors.Length;
			b /= aColors.Length;
			a /= aColors.Length;
			return new Color(r, g, b, a);
		}

		void _GenerateMinimapTilesTexture()
		{
			Color[] aColors = Enumerable.Repeat<Color>( new Color(0f, 0f, 0f, 0f) , m_minimapTilesTexture.GetPixels().Length).ToArray();

            Rect srcRect = new Rect(0, 0, Tileset.TileWidth, Tileset.TileHeight);
			int idx = 0;
			foreach( SubTilesetConf tilesetConf in Tileset.SubTilesets)
			{
				Texture2D thumbTex = UtilsAutoTileMap.GenerateTilesetTexture( Tileset, tilesetConf);
                for (srcRect.y = thumbTex.height - Tileset.TileHeight; srcRect.y >= 0; srcRect.y -= Tileset.TileHeight)
				{
                    for (srcRect.x = 0; srcRect.x < thumbTex.width; srcRect.x += Tileset.TileWidth, ++idx)
					{
						// improved tile color by using the center square as some autotiles are surrounded by ground pixels like water tiles
						Rect rRect = new Rect( srcRect.x + srcRect.width/4, srcRect.y + srcRect.height/4, srcRect.width/2, srcRect.height/2 );
						aColors[idx] = _support_GetAvgColorOfTexture( thumbTex, rRect );
					}
				}
			}
			
			m_minimapTilesTexture.SetPixels( aColors );
			m_minimapTilesTexture.Apply();
		}

        /// <summary>
        /// Refresh full minimp texture
        /// </summary>
		public void RefreshMinimapTexture( )
		{
			RefreshMinimapTexture( 0, 0, MapTileWidth, MapTileHeight );
		}

        /// <summary>
        /// Refresh minimap texture partially
        /// </summary>
        /// <param name="tile_x">X tile coordinate of the map</param>
        /// <param name="tile_y">Y tile coordinate of the map</param>
        /// <param name="width">Width in tiles</param>
        /// <param name="height">Height in tiles</param>
		public void RefreshMinimapTexture( int tile_x, int tile_y, int width, int height )
        {
			tile_x = Mathf.Clamp( tile_x, 0, MinimapTexture.width - 1 );
			tile_y = Mathf.Clamp( tile_y, 0, MinimapTexture.height - 1 );
			width = Mathf.Min( width, MinimapTexture.width - tile_x );
			height = Mathf.Min( height, MinimapTexture.height - tile_y );

			Color[] aTilesColors = m_minimapTilesTexture.GetPixels();
			Color[] aMinimapColors = Enumerable.Repeat<Color>( new Color(0f, 0f, 0f, 1f) , MinimapTexture.GetPixels(tile_x, MinimapTexture.height - tile_y - height, width, height).Length).ToArray();
			foreach( MapLayer mapLayer in MapLayers )
			{
                AutoTile[] aAutoTiles = TileLayers[mapLayer.TileLayerIdx];
                // read tile type in the same way that texture pixel order, from bottom to top, right to left
                for (int yf = 0; yf < height; ++yf)
                {
                    for (int xf = 0; xf < width; ++xf)
                    {
                        int tx = tile_x + xf;
                        int ty = tile_y + yf;
                        int tileIdx = tx + ty * MapTileWidth;

                        int type = aAutoTiles[tileIdx] != null ? aAutoTiles[tileIdx].Id : -1;
                        if (mapLayer.LayerType == eLayerType.Ground || mapLayer.LayerType == eLayerType.Overlay)
                        {
                            if (type >= 0)
                            {
                                int idx = (height - 1 - yf) * width + xf;
                                Color baseColor = aMinimapColors[idx];
                                Color tileColor = aTilesColors[type];
                                aMinimapColors[idx] = baseColor * (1 - tileColor.a) + tileColor * tileColor.a;
                                aMinimapColors[idx].a = 1f;
                            }
                        }
                        else if (mapLayer.LayerType == eLayerType.FogOfWar)
                        {
                            Color cFogColor = new Color32(32, 32, 32, 255); ;
                            byte[] fogAlpha = System.BitConverter.GetBytes(type);
                            for( int i = 0; i < fogAlpha.Length; ++i )
                            {
                                cFogColor.a += (fogAlpha[i] / 0xff);
                            }
                            cFogColor.a /= fogAlpha.Length;
                            int idx = (height - 1 - yf) * width + xf;
                            aMinimapColors[idx] = aMinimapColors[idx] * (1 - cFogColor.a) + cFogColor * cFogColor.a;
                            aMinimapColors[idx].a = 1f;
                        }
                    }
                }
			}
			MinimapTexture.SetPixels( tile_x, MinimapTexture.height - tile_y - height, width, height, aMinimapColors );
			MinimapTexture.Apply();
		}


        private Dictionary<int, KeyValuePair<byte[], byte[]>> m_fogOfWarTilesToUpdate = new Dictionary<int, KeyValuePair<byte[], byte[]>>();
        private List<KeyValuePair<int, byte[]>> m_fogOfWarSetRequests = new List<KeyValuePair<int, byte[]>>();
        private MapLayer m_fogOfWarMapLayer = null;
        private System.Threading.Mutex m_fogOfWarMutex = new System.Threading.Mutex();
        private bool m_fogOfWarInitCoroutineOnNextUpdate = false;

        /// <summary>
        /// Set tile fog values. Each tile is divided in 4 tile parts and each tile part have a byte value for alpha.
        /// The fog tile will change its alpha values smoothly using a coroutine
        /// </summary>
        /// <param name="tileIdx">Tile index in the map (tileX + tileY*MapWidth)</param>
        /// <param name="values">Alpha value for each part of the tile. Should be an array of 4 elements</param>
        public void AddFogOfWarSetToQueue(int tileIdx, byte[] values)
        {
            m_fogOfWarMutex.WaitOne();
            m_fogOfWarSetRequests.Add( new KeyValuePair<int, byte[]>(tileIdx, values));
            m_fogOfWarMutex.ReleaseMutex();
        }

        private void _SetFogOfWarAsync( int tileIdx, byte[] values )
        {
            KeyValuePair<byte[], byte[]> fromToPair;
            if (m_fogOfWarTilesToUpdate.TryGetValue(tileIdx, out fromToPair))
            {
                for (int i = 0; i < fromToPair.Value.Length; ++i)
                {
                    fromToPair.Value[i] = (byte)Mathf.Min(fromToPair.Value[i], values[i]);
                }
            }
            else
            {
                byte[] fromData = System.BitConverter.GetBytes(TileLayers[m_fogOfWarMapLayer.TileLayerIdx][tileIdx].Id);
                byte[] toData = new byte[4];
                System.Array.Copy(values, toData, toData.Length);
                fromToPair = new KeyValuePair<byte[], byte[]>(fromData, toData );
                m_fogOfWarTilesToUpdate[tileIdx] = fromToPair;
            }
        }


        private IEnumerator FogOfWarCoroutine()
        {
            List<int> tileIdxToRemove = new List<int>();
            float offsetDt = 0f;
            float prevTime = Time.time;
            while (m_fogOfWarMapLayer != null)
            {
                offsetDt += (Time.time - prevTime) * 0xff; // * (value) = offset changed by second ( used with option A. see below )
                prevTime = Time.time;

                int iOffsetDt = (int)offsetDt;
                offsetDt -= iOffsetDt;
                m_fogOfWarMutex.WaitOne();
                foreach( KeyValuePair<int, byte[]> request in m_fogOfWarSetRequests )
                {
                    _SetFogOfWarAsync(request.Key, request.Value);
                }
                m_fogOfWarSetRequests.Clear();
                m_fogOfWarMutex.ReleaseMutex();
                foreach (KeyValuePair<int, KeyValuePair<byte[], byte[]>> entry in m_fogOfWarTilesToUpdate)
                {
                    int tileIdx = entry.Key;
                    byte[] fromData = entry.Value.Key;
                    byte[] toData = entry.Value.Value;

                    bool isDone = true;
                    for( int i = 0; i < fromData.Length; ++i )
                    {
                        if (fromData[i] > toData[i])
                        {
                            //NOTE: Option A: smooth fade based on time, but it has an effect of removing fog from inside to outside
                            //fromData[i] = ( iOffsetDt > (int)(fromData[i] - toData[i]) )? toData[i] : (byte)(fromData[i] - iOffsetDt);
                            
                            //Note: Option B: smooth fade based on frame but it has an effect of removing fog from inside to outside
                            iOffsetDt = (fromData[i] - toData[i]) / 20;
                            if (iOffsetDt == 0) iOffsetDt = 1;
                            fromData[i] -= (byte)iOffsetDt;
                            isDone = false;
                        }
                    }                    

                    if( isDone )
                    {
                        tileIdxToRemove.Add( tileIdx );
                    }
                    else
                    {
                        AutoTile autoTile = TileLayers[m_fogOfWarMapLayer.TileLayerIdx][tileIdx];
                        autoTile.Id = System.BitConverter.ToInt32(fromData, 0);
                        AutoTileMap.Instance.RefreshTile(autoTile);
                    }
                }

                foreach( int idx in tileIdxToRemove )
                {
                    m_fogOfWarTilesToUpdate.Remove(idx);
                }
                tileIdxToRemove.Clear();

                yield return null;
            }
        }
	}
}
