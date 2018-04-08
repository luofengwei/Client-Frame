﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CreativeSpore.PathFindingLib;

namespace CreativeSpore.RpgMapEditor
{

    public class MapTileNode : IPathNode
    {

        public int TileX { get; private set; }
        public int TileY { get; private set; }
        public int TileIdx { get; private set; }
        public Vector3 Position { get; private set; }
        public bool IsEmptyTilePassable = false; //TODO: Fix when this is true, is not working because is not taking into account when any of the layer has an empty tile
       
        List<int> m_neighborIdxList = new List<int>();
        MapPathFinding m_owner;

        internal MapTileNode(int idx, MapPathFinding owner) 
        {
            m_owner = owner;
            TileIdx = idx;
            TileX = idx % AutoTileMap.Instance.MapTileWidth;
            TileY = idx / AutoTileMap.Instance.MapTileWidth;
            Position = RpgMapHelper.GetTileCenterPosition(TileX, TileY);

            // get all neighbors row by row, neighIdx will be the idx of left most tile per each row
            int neighIdx = (idx-1)-AutoTileMap.Instance.MapTileWidth;
            for (int i = 0; i < 3; ++i, neighIdx += AutoTileMap.Instance.MapTileWidth)
            {
                for (int j = 0; j < 3; ++j)
                {
                    if (i != 1 || j != 1) // skip this node
                    {
                        m_neighborIdxList.Add(neighIdx + j);
                    }
                }
            }
        }

        #region IPathNode
        public override bool IsPassable() 
        {
            AutoTileMap autoTileMap = AutoTileMap.Instance;
            if (autoTileMap.IsValidAutoTilePos(TileX, TileY))
            {
                for( int iLayer = autoTileMap.GetLayerCount() - 1; iLayer >= 0; --iLayer )
                {
                    if( autoTileMap.MapLayers[iLayer].LayerType == eLayerType.Ground )
                    {
                        AutoTile autoTile = autoTileMap.GetAutoTile(TileX, TileY, iLayer);
                        eTileCollisionType collType = autoTile.Id >= 0 ? autoTileMap.Tileset.AutotileCollType[autoTile.Id] : eTileCollisionType.EMPTY;
                        if( IsEmptyTilePassable && collType == eTileCollisionType.EMPTY || 
                            collType == eTileCollisionType.PASSABLE || collType == eTileCollisionType.WALL )
                        {
                            return true;
                        }
                        else if( collType == eTileCollisionType.BLOCK || collType == eTileCollisionType.FENCE )
                        {
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        public override float GetHeuristic( ) 
        {
            //NOTE: 10f in Manhattan and 14f in Diagonal should rally be 1f and 1.41421356237f, but I discovered by mistake these values improve the performance

            float h = 0f;

            switch( m_owner.HeuristicType )
            {
                case MapPathFinding.eHeuristicType.None: h = 0f; break;
                case MapPathFinding.eHeuristicType.Manhattan:
                    {
                        h = 10f * (Mathf.Abs(TileX - m_owner.EndNode.TileX) + Mathf.Abs(TileY - m_owner.EndNode.TileY));
                        break;
                    }
                case MapPathFinding.eHeuristicType.Diagonal:
                    {
                        float xf = Mathf.Abs(TileX - m_owner.EndNode.TileX);
                        float yf = Mathf.Abs(TileY - m_owner.EndNode.TileY);
                        if (xf > yf)
                            h = 14f * yf + 10f * (xf - yf);
                        else
                            h = 14f * xf + 10f * (yf - xf); 
                        break;
                    }
            }
            return h; 
        }

        // special case for walls
        bool _IsWallPassable( MapTileNode neighNode )
        {
            AutoTileMap autoTileMap = AutoTileMap.Instance;
            eTileCollisionType collType = eTileCollisionType.EMPTY;
            eTileCollisionType collTypeNeigh = eTileCollisionType.EMPTY;
            for (int iLayer = autoTileMap.GetLayerCount() - 1; iLayer >= 0; --iLayer)
            {
                if (autoTileMap.MapLayers[iLayer].LayerType == eLayerType.Ground)
                {
                    AutoTile autoTile = autoTileMap.GetAutoTile(TileX, TileY, iLayer);
                    AutoTile autoTileNeigh = autoTileMap.GetAutoTile(neighNode.TileX, neighNode.TileY, iLayer);

                    if (autoTile.Id == autoTileNeigh.Id) // you can walk over two wall tiles if they have the same type
                    {
                        if (autoTile.Id >= 0) 
                            return true;
                        else
                            continue;
                    }
                    else
                    {
                        // collType will keep the first collision type found of type wall or passable
                        if (collType != eTileCollisionType.PASSABLE && collType != eTileCollisionType.WALL)
                            collType = autoTile.Id >= 0 ? autoTileMap.Tileset.AutotileCollType[autoTile.Id] : eTileCollisionType.EMPTY;
                        if (collTypeNeigh != eTileCollisionType.PASSABLE && collTypeNeigh != eTileCollisionType.WALL)
                            collTypeNeigh = autoTileNeigh.Id >= 0 ? autoTileMap.Tileset.AutotileCollType[autoTileNeigh.Id] : eTileCollisionType.EMPTY;

                        if( collType == eTileCollisionType.PASSABLE && collTypeNeigh == eTileCollisionType.PASSABLE )
                        {
                            return true;
                        }
                        else if (collType == eTileCollisionType.WALL || collTypeNeigh == eTileCollisionType.WALL)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public override float GetNeigborMovingCost(int neigborIdx) 
        {
            float fCost = 1f;
            //012 // 
            //3X4 // neighbor index positions, X is the position of this node
            //567
            if( neigborIdx == 0 || neigborIdx == 2 || neigborIdx ==  5 || neigborIdx == 7 )
            {
                //check if can reach diagonals as it could be not possible if flank tiles are not passable      
                MapTileNode nodeN = GetNeighbor(1) as MapTileNode;
                MapTileNode nodeW = GetNeighbor(3) as MapTileNode;
                MapTileNode nodeE = GetNeighbor(4) as MapTileNode;
                MapTileNode nodeS = GetNeighbor(6) as MapTileNode;
                if (
                    !m_owner.AllowDiagonals ||
                    (neigborIdx == 0 && (!nodeN.IsPassable() || !nodeW.IsPassable() || !_IsWallPassable(nodeN) || !_IsWallPassable(nodeW))) || // check North West
                    (neigborIdx == 2 && (!nodeN.IsPassable() || !nodeE.IsPassable() || !_IsWallPassable(nodeN) || !_IsWallPassable(nodeE))) || // check North East
                    (neigborIdx == 5 && (!nodeS.IsPassable() || !nodeW.IsPassable() || !_IsWallPassable(nodeS) || !_IsWallPassable(nodeW))) || // check South West
                    (neigborIdx == 7 && (!nodeS.IsPassable() || !nodeE.IsPassable() || !_IsWallPassable(nodeS) || !_IsWallPassable(nodeE)))    // check South East
                )
                {
                    return PathFinding.k_InfiniteCostValue;
                }
                else
                {
                    fCost = 1.41421356237f;
                }
            }
            else
            {
                fCost = 1f;
            }

            MapTileNode neighNode = GetNeighbor(neigborIdx) as MapTileNode;
            if (!_IsWallPassable(neighNode))
            {
                return PathFinding.k_InfiniteCostValue;
            }

            return fCost;  
        }
        public override int GetNeighborCount() { return m_neighborIdxList.Count; }
        public override IPathNode GetNeighbor(int idx) { return m_owner.GetMapTileNode(m_neighborIdxList[idx]); }
        #endregion
    }

    public class MapPathFinding
    {

        public enum eHeuristicType
        {
            /// <summary>
            /// Very slow but guarantees the shortest path
            /// </summary>
            None,
            /// <summary>
            /// Faster than None, but does not guarantees the shortest path
            /// </summary>
            Manhattan,
            /// <summary>
            /// Faster than Manhattan but less accurate
            /// </summary>
            Diagonal
        }

        public eHeuristicType HeuristicType = eHeuristicType.Manhattan;

        /// <summary>
        /// Set if diagonal movement is allowed
        /// </summary>
        public bool AllowDiagonals = true;

        /// <summary>
        /// Max iterations to find a path. Use a value <= 0 for infinite iterations.
        /// Remember max iterations will be reached when trying to find a path with no solutions.
        /// </summary>
        public int MaxIterations 
        {
            get { return m_pathFinding.MaxIterations; }
            set { MaxIterations = m_pathFinding.MaxIterations; }

        }
        
        public bool IsComputing { get { return m_pathFinding.IsComputing; } }

        PathFinding m_pathFinding = new PathFinding();
        Dictionary<int, MapTileNode> m_dicTileNodes = new Dictionary<int, MapTileNode>(); //TODO: using this dictionary could lead to a memory problem. Use a pool of MapTileNodes instead.
        internal MapTileNode EndNode { get; private set; }

        /// <summary>
        /// Get a map tile node based on its index
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        public MapTileNode GetMapTileNode( int idx )
        {
            MapTileNode mapTileNode;
            bool wasFound = m_dicTileNodes.TryGetValue(idx, out mapTileNode);
            if(!wasFound)
            {
                mapTileNode = new MapTileNode(idx, this);
                m_dicTileNodes[idx] = mapTileNode;
            }
            return mapTileNode;
        }

        /// <summary>
        /// Return a list of path nodes from the start tile to the end tile ( Use RpgMapHelper class to get the tile index )
        /// </summary>
        /// <param name="startIdx"></param>
        /// <param name="endIdx"></param>
        /// <returns></returns>
        public LinkedList<IPathNode> GetRouteFromTo(int startIdx, int endIdx)
        {
            LinkedList<IPathNode> nodeList = new LinkedList<IPathNode>();
            if (m_pathFinding.IsComputing)
            {
                Debug.LogWarning("PathFinding is already computing. GetRouteFromTo will not be executed!");
            }
            else
            {                
                IPathNode start = GetMapTileNode(startIdx);
                EndNode = GetMapTileNode(endIdx);
                nodeList = m_pathFinding.ComputePath(start, EndNode);         //NOTE: the path is given from end to start ( change the order? )
            }
            return nodeList;
        }

        /// <summary>
        /// Return a list of path nodes from the start tile to the end tile ( Use RpgMapHelper class to get the tile index )
        /// </summary>
        /// <param name="startIdx"></param>
        /// <param name="endIdx"></param>
        /// <returns></returns>
        public IEnumerator GetRouteFromToAsync( int startIdx, int endIdx )
        {
            IPathNode start = GetMapTileNode(startIdx);
            EndNode = GetMapTileNode(endIdx);
            return m_pathFinding.ComputePathAsync(start, EndNode);
        }
    }

}
