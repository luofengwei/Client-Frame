using UnityEngine;
using System.Collections.Generic;
using Ghost.Extensions;
using System;
using System.IO;
using RenderHeads.Media.AVProVideo;

[CustomLuaClassAttribute]
public class GameObjectUtils
{
    [DoNotToLua]
    public static void SetDepths (GameObject root, int depth, int index = 0)
	{
		var widget = root.transform.GetComponent<UIWidget> ();
		if (widget != null)
			widget.depth = depth + index;

		index += 1;
        for (int i = 0; i < root.transform.childCount;i++ )
        {
            Transform c = root.transform.GetChild(i);
            widget = c.GetComponent<UIWidget>();
            if (widget != null)
                widget.depth = depth;
            SetDepths(c.gameObject, depth, index);
        }
	}

    public static GameObject Find(string name)
    {
        GameObject ret = GameObject.Find(name);
        if (ret == null)
            return null;
        else
            return ret;
    }

    public static GameObject DeepFind(GameObject parent, string name)
	{
        for (int i = 0; i < parent.transform.childCount; ++i)
        {
            Transform c = parent.transform.GetChild(i);
            if (c.name.Equals(name))
                return c.gameObject;
            else
            {
                GameObject resultGo = DeepFind(c.gameObject, name);
                if (resultGo != null)
                    return resultGo;
            }
        }
		return null;
	}

    public static GameObject DirectFind(string path, GameObject obj)
    {
        if (obj != null)
        {
            Transform trans = obj.transform.Find(path);
            if (trans != null)
                return trans.gameObject;
        }       
        return null;
    }

    public static GameObject DeepFindChild(GameObject parent, string name)
	{
        for (int i = 0; i < parent.transform.childCount;++i )
        {
            Transform c = parent.transform.GetChild(i);
            if (c.name.Equals(name) || c.name.StartsWith(name))
                return c.gameObject;
            else
            {
                GameObject resultGo = DeepFindChild(c.gameObject, name);
                if (resultGo != null)
                    return resultGo;
            }
        }
		return null;
	}

    [DoNotToLua]
    public static GameObject[] DeepFindChildren(GameObject parent, string name)
	{
		List<GameObject> objs = new List<GameObject> ();
		if (parent.transform.childCount > 0)
        {
            for (int i = 0; i < parent.transform.childCount;++i )
            {
                Transform c = parent.transform.GetChild(i);
                if (c.name.Equals(name))
                {
                    objs.Add(c.gameObject);
                }
                objs.AddRange(DeepFindChildren(c.gameObject, name));
            }
		} 
		return objs.ToArray ();
	}

    [DoNotToLua]
    public static GameObject[] GetAllChildren(GameObject parent)
	{
		if (parent == null) {
			return null;
		}
		List<GameObject> results = new List<GameObject> ();
        for (int i = 0; i < parent.transform.childCount;++i )
        {
            Transform t = parent.transform.GetChild(i);
            if (t != null)
            {
                results.Add(t.gameObject);

                GameObject[] cs = GetAllChildren(t.gameObject);
                if (cs != null)
                {
                    results.AddRange(cs);
                }
            }
        }
        
		return results.ToArray ();
	}

    [DoNotToLua]
    public static bool DestroyAllChildren(GameObject parent)
	{
		if (parent == null)
			return false;

		int count = parent.transform.childCount;
		for (int i = 0; i<count; i++) {
			Transform t = parent.transform.GetChild (0);
			if (t != null)
				GameObject.DestroyImmediate (t.gameObject);
		}
		return true;
	}

	/// <summary>
	/// get All Comps with noActive 
	/// </summary>
    public static Component[] GetAllComponentsInChildren(GameObject p, System.Type t, bool containSelf = true)
	{
		List<Component> comps = new List<Component> ();
		Component sp = p.GetComponent (t);
		if (containSelf && sp != null)
			comps.Add(sp);

        for (int i = 0; i < p.transform.childCount; ++i)
        {
            Transform child = p.transform.GetChild(i);
            Component ct = child.GetComponent(t);
            if (ct != null)
            {
                comps.Add(ct);
            }
            comps.AddRange(GetAllComponentsInChildren(child.gameObject, t, false));
        }
		return comps.ToArray();
	}

    [DoNotToLua]
    public static void DestroyImmediateResOnCompents(GameObject obj)
    {
        Component[] objArray = GameObjectUtils.GetAllComponentsInChildren(obj, typeof(UITexture), true);
        if (!objArray.IsNullOrEmpty())
        {
            for (int i = 0; i < objArray.Length; ++i)
            {
                UnityEngine.Object.DestroyImmediate(((UITexture)objArray[i]).mainTexture, true);               
            }
        }
        objArray = null;

        Component[] objArray2 = GameObjectUtils.GetAllComponentsInChildren(obj, typeof(UISprite), true);
        if (!objArray2.IsNullOrEmpty())
        {
            for (int i = 0; i < objArray2.Length; ++i)
            {             
                UnityEngine.Object.DestroyImmediate(((UISprite)objArray2[i]).atlas, true);
                UnityEngine.Object.DestroyImmediate(((UISprite)objArray2[i]).mainTexture, true);               
            }
        }
        objArray2 = null;

        Component[] objArray3 = GameObjectUtils.GetAllComponentsInChildren(obj, typeof(UILabel), true);
        if (!objArray3.IsNullOrEmpty())
        {
            for (int i = 0; i < objArray3.Length; ++i)
            {            
                UnityEngine.Object.DestroyImmediate(((UILabel)objArray3[i]).ambigiousFont, true);                
            }
        }
        objArray3 = null;
    }

    [DoNotToLua]
    public static void DestoryResOnCompents(GameObject obj)
    {
        Component[] objArray = GameObjectUtils.GetAllComponentsInChildren(obj, typeof(UITexture), true);
        if (!objArray.IsNullOrEmpty())
        {
            for (int i = 0; i < objArray.Length; ++i)
            {
                if (ResourceManager.Instance != null && objArray[i] != null)
                    ResourceManager.Instance.UnLoadIcon(((UITexture)objArray[i]).mainTexture);
            }
        }
        objArray = null;

        Component[] objArray2 = GameObjectUtils.GetAllComponentsInChildren(obj, typeof(UISprite), true);
        if (!objArray2.IsNullOrEmpty())
        {
            for (int i = 0; i < objArray2.Length; ++i)
            {
                if (ResourceManager.Instance != null && objArray2[i] != null)
                {
                    ResourceManager.Instance.UnLoadIcon(((UISprite)objArray2[i]).atlas);
                    ResourceManager.Instance.UnLoadIcon(((UISprite)objArray2[i]).mainTexture);
                }
            }
        }
        objArray2 = null;
    }

    public static void ChangeLayersRecursively(GameObject parent, string LayerName)
	{
        if (parent == null)
        {
            return;
        }
        if (parent.layer != LayerMask.NameToLayer (LayerName)) {
			parent.layer = LayerMask.NameToLayer (LayerName);
		}
        for (int i = 0; i < parent.transform.childCount; ++i)
        {
            Transform c = parent.transform.GetChild(i);
            ChangeLayersRecursively(c.gameObject, LayerName);
        }
	}

    public static void ChangeLayersRecursively(GameObject parent, int layer)
	{
        if (parent == null)
        {
            return;
        }

        if (parent.layer != layer) {
			parent.layer = layer;
		}

        for (int i = 0; i < parent.transform.childCount; ++i)
        {
            Transform c = parent.transform.GetChild(i);
            ChangeLayersRecursively(c.gameObject, layer);
        }
	}

    /// <summary>
    /// 设置OBJ的Parent
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="parent"></param>
    public static void SetObjParent(GameObject obj, GameObject parent)
    {
        if (obj == null || parent == null)
        {
            return;
        }

        obj.transform.SetParent(parent.transform, false);
    }

    public static void SetObjSpriteName(UISprite uiSprite, string spritename)
    {
        if (uiSprite != null && spritename != null)
        {
            uiSprite.spriteName = spritename;
        }
    }
    /// <summary>
    /// 设置OBJ的LocalPosition
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public static void SetObjLocalPosition(GameObject obj, int x, int y, int z)
    {
        if (obj == null)
        {
            return;
        }
        obj.transform.localPosition = new Vector3(x, y, z);
    }

    public static void SetObjLocalPosition(GameObject obj, float x, float y, float z)
    {
        if (obj == null)
        {
            return;
        }
        obj.transform.localPosition = new Vector3(x, y, z);
    }

    public static void ChangeWidgetValue(GameObject obj,int leftAnchor,int rightAnchor, int bottomAnchor, int topAnchor)
    {
        if(obj == null)
        {
            return;
        }
        UIWidget widgetObj = obj.GetComponent<UIWidget>();
        if(widgetObj == null || !widgetObj.isAnchored)
        {
            return;
        }
        if (widgetObj.leftAnchor != null)
            widgetObj.leftAnchor.absolute = leftAnchor;
        if (widgetObj.rightAnchor != null)
            widgetObj.rightAnchor.absolute = rightAnchor;
        if (widgetObj.bottomAnchor != null)
            widgetObj.bottomAnchor.absolute = bottomAnchor;
        if (widgetObj.topAnchor != null)
            widgetObj.topAnchor.absolute = topAnchor;
    }

    /// <summary>
    /// 设置Sprite元素的宽高
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public static void SetSpriteSize(GameObject obj,int width,int height)
    {
        if(obj == null)
        {
            return;
        }

        UISprite objSprite = obj.GetComponent<UISprite>();
        if(objSprite == null)
        {
            return;
        }

        objSprite.width = width;
        objSprite.height = height;
    }

    /// <summary>
    /// 设置OBJ的LocalRotation
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public static void SetObjLocalRotation(GameObject obj, int x, int y, int z)
    {
        if (obj == null)
        {
            return;
        }        
        obj.transform.localRotation = Quaternion.Euler(x, y, z);
    }

    public static void SetObjLocalRotation(GameObject obj, float x, float y, float z)
    {
        if (obj == null)
        {
            return;
        }
        obj.transform.localRotation = Quaternion.Euler(x, y, z);
    }

    /// <summary>
    /// 设置OBJ的LocalScale
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public static void SetObjLocalScale(GameObject obj, int x, int y, int z)
    {
        if (obj == null)
        {
            return;
        }
        obj.transform.localScale = new Vector3(x, y, z);
    }

    public static void SetObjLocalScale(GameObject obj, float x, float y, float z)
    {
        if (obj == null)
        {
            return;
        }
        obj.transform.localScale = new Vector3(x, y, z);
    }

    /// <summary>
    /// 调整SphereCollider碰撞体的半径
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="radius"></param>
    public static void SetSphereColliderRadius(GameObject obj,int radius)
    {
        if (obj == null)
        {
            return;
        }
        SphereCollider sphereCollider = obj.GetComponent<SphereCollider>();
        if (sphereCollider == null)
        {
            return;
        }

        sphereCollider.radius = radius;
    }

    /// <summary>
    /// 为边上的按钮特别定制的盒碰撞体
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="centerX"></param>
    /// <param name="centerY"></param>
    /// <param name="sizeX"></param>
    /// <param name="sizeY"></param>
    public static void SetBoxColliderParams(GameObject obj,int centerX,int centerY,int sizeX,int sizeY)
    {
        if (obj == null)
        {
            return;
        }
        BoxCollider boxCollider = obj.GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            return;
        }
        boxCollider.center = new Vector3(centerX, centerY,0);
        boxCollider.size = new Vector3(sizeX, sizeY,1);
    }

    /// <summary>
    /// 为右下角的按钮特别定制的两个盒碰撞体
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="radius"></param>
    public static void SetDShootBoxColliderParams(GameObject obj,int radius)
    {
        if (obj == null)
        {
            return;
        }
        BoxCollider[] boxCollider = obj.GetComponents<BoxCollider>();
        if (boxCollider == null || boxCollider.Length < 2)
        { 
            return;
        }

        boxCollider[0].center = new Vector3(0, -radius, 0);
        boxCollider[0].size = new Vector3(radius * 2, radius * 2, 1);

        boxCollider[1].center = new Vector3(radius, 0, 0);
        boxCollider[1].size = new Vector3(radius * 2, radius * 2, 1);
    }

    /// <summary>
    /// 将sourceObj的位置保持在边缘（加载进度条）
    /// </summary>
    /// <param name="sourceObj"></param>
    /// <param name="tgtObj"></param>
    public static void KeepObjectInEdge(GameObject sourceObj, GameObject tgtObj)
    {
        if(sourceObj == null || tgtObj == null)
        {
            return;
        }

        if(tgtObj.transform.localPosition.x + 70 > 640)
        {
            sourceObj.transform.localPosition = new Vector3(25 - (tgtObj.transform.localPosition.x + 70 - 640),-10,0);
        }        
        else if(tgtObj.transform.localPosition.x - 20 < -640)
        {
            sourceObj.transform.localPosition = new Vector3(25 - (tgtObj.transform.localPosition.x - 20 + 640), -10, 0);
        }
        else
        {
            sourceObj.transform.localPosition = new Vector3(25, -10, 0);
        }
    }

    [DoNotToLua]
    public static GameObject CopyObjTo(GameObject obj, Transform parent)
	{
		if (parent == null) {
			parent = obj.transform.parent;
		}
		GameObject copy = GameObject.Instantiate (obj) as GameObject;
		if (!copy.activeInHierarchy) {
			copy.SetActive (true);
		}
		copy.transform.SetParent (parent, false);
		copy.name = obj.name;
		return copy;
	}

    [DoNotToLua]
    public static int ToHashCode(string ori)
	{
		return ori.GetHashCode ();
	}

    public static bool ObjectIsNULL(UnityEngine.Object go)
	{
		return go == null;
	}

    [DoNotToLua]
    public static bool SystemObjectIsNULL(object obj)
	{
		return obj == null;
	}

    [DoNotToLua]
    public static float GetUIActiveHeight(GameObject go)
	{
		UIRoot ur = NGUITools.FindInParents<UIRoot> (go.transform);
		return ur.activeHeight;
	}

    [DoNotToLua]
    public static Vector3 WorldToScreenPoint(Vector3 position)
	{
		Vector3 pos = Camera.main.WorldToScreenPoint (position);
		pos.x -= Screen.width / 2;
		pos.y -= Screen.height / 2;
        float adjustment = CGameRoot.UIRoot.pixelSizeAdjustment;
		pos.x *= adjustment;
		pos.y *= adjustment;
		return pos;
	}

    #region 视频播放接口

    /// <summary>
    /// 视频播放测试接口
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="movieType"></param>
    public static void ChangeMovieShow(GameObject obj,int movieType)
    {
        Debug.Log("<color=red>该视频播放接口已废弃</color>");
    }

    /// <summary>
    /// 视频播放正式接口
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="moviePath"></param>
    public static void MovieShow(GameObject obj, string moviePath, bool autoPlay)
    {
        if (obj != null)
        {
            MediaPlayer mediaPlayer = obj.GetComponent<MediaPlayer>();
            if (mediaPlayer)
            {
                bool bMovieExist = true;
                if (File.Exists(ABPathHelper.AssetsURL + moviePath))
                {
#if UNITY_EDITOR
                    if (GameUtils.UseSelfBundle)
                        mediaPlayer.m_VideoPath = "AssetBundles/" + ABPathHelper.platformFolder + ABPathHelper.Separator + moviePath;
                    else
                        mediaPlayer.m_VideoPath = "LocalCache/" + ABPathHelper.platformFolder + ABPathHelper.Separator + moviePath;
#else
                    mediaPlayer.m_VideoPath = "LocalCache/" + ABPathHelper.platformFolder + ABPathHelper.Separator + moviePath;
#endif
                }
                else if(File.Exists(ABPathHelper.AssetsLocalABURL + moviePath))
                {
                    mediaPlayer.m_VideoPath = "LocalAB/" + ABPathHelper.platformFolder + ABPathHelper.Separator + moviePath;
                }
                else
                {
                    mediaPlayer.m_VideoPath = ABPathHelper.platformFolder + ABPathHelper.Separator + moviePath;
                    bMovieExist = false;
                }
#if UNITY_EDITOR
                if(bMovieExist)
                    mediaPlayer.m_VideoLocation = MediaPlayer.FileLocation.RelativeToProjectFolder;
                else
                    mediaPlayer.m_VideoLocation = MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder;
#else
                if (bMovieExist)
                    mediaPlayer.m_VideoLocation = MediaPlayer.FileLocation.RelativeToPeristentDataFolder;
                else
                    mediaPlayer.m_VideoLocation = MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder;
#endif
                mediaPlayer.OpenVideoFromFile(mediaPlayer.m_VideoLocation, mediaPlayer.m_VideoPath, true);
            }
        }
    }

    /// <summary>
    /// 播放视频
    /// </summary>
    /// <param name="obj"></param>
    public static void MoviePlay(GameObject obj)
    {
        if (obj != null)
        {
            MediaPlayer mediaPlayer = obj.GetComponent<MediaPlayer>();
            if (mediaPlayer)
            {
                mediaPlayer.Control.Play();
            }
        }
    }

    /// <summary>
    /// 暂停视频
    /// </summary>
    /// <param name="obj"></param>
    public static void MoviePause(GameObject obj)
    {
        if (obj != null)
        {
            MediaPlayer mediaPlayer = obj.GetComponent<MediaPlayer>();
            if (mediaPlayer)
            {
                mediaPlayer.Control.Pause();
            }
        }
    }

    /// <summary>
    /// 重置视频
    /// </summary>
    /// <param name="obj"></param>
    public static void MovieRewind(GameObject obj)
    {
        if (obj != null)
        {
            MediaPlayer mediaPlayer = obj.GetComponent<MediaPlayer>();
            if (mediaPlayer)
            {
                mediaPlayer.Control.Rewind();
            }
        }
    }

    /// <summary>
    /// 视频是否在播放中
    /// </summary>
    /// <param name="obj"></param>
    public static bool MovieIsPlaying(GameObject obj)
    {
        if (obj != null)
        {
            MediaPlayer mediaPlayer = obj.GetComponent<MediaPlayer>();
            if (mediaPlayer)
            {
                return mediaPlayer.Control.IsPlaying();
            }
        }
        return false;
    }

    /// <summary>
    /// 视频是否被暂停
    /// </summary>
    /// <param name="obj"></param>
    public static bool MovieIsPause(GameObject obj)
    {
        if (obj != null)
        {
            MediaPlayer mediaPlayer = obj.GetComponent<MediaPlayer>();
            if (mediaPlayer)
            {
                return mediaPlayer.Control.IsPaused();
            }
        }
        return false;
    }

    public static void StopMovieShow(GameObject obj)
    {
        if(obj != null)
        {
            MediaPlayer mediaPlayer = obj.GetComponent<MediaPlayer>();
            if(mediaPlayer)
            {
                mediaPlayer.CloseVideo();
            }
        }
    }

    #endregion

    #region Spine2D的接口

    /// <summary>
    /// 设置Obj新增RenderQueeuModifier脚本解决Spine的层级问题
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="panelObj"></param>
    /// <param name="targetObj"></param>
    public static void AddRenderQueueModifier(GameObject obj,GameObject panelObj,GameObject targetObj)
    {
        if (obj == null || panelObj == null || targetObj == null)
        {
            return;
        }
        RenderQueueModifier renderModifier = obj.GetComponent<RenderQueueModifier>();
        if (renderModifier == null)
        {
            renderModifier = obj.AddComponent<RenderQueueModifier>();
        }
        UIPanel uiPanelObj = panelObj.GetComponent<UIPanel>();
        UISprite uiSpriteObj = targetObj.GetComponent<UISprite>();
        if (renderModifier == null || uiPanelObj == null || uiSpriteObj == null)
        {
            return;
        }
        renderModifier.Set(uiPanelObj, uiSpriteObj, true);
    }

    /// <summary>
    /// 新增一个Spine2D动画需要调用的参数接口
    /// </summary>
    /// <param name="obj">动画OBJ</param>
    /// <param name="panelObj">Parent和相应的Panel节点</param>
    /// <param name="targetObj">需要超过的Obj-RenderQueue</param>
    /// <param name="posX">位置X</param>
    /// <param name="posY">位置Y</param>
    /// <param name="scale">缩放Scale</param>
    /// <param name="layer">层级Layer</param>
    public static void AddSpine2D(GameObject obj,GameObject panelObj,GameObject targetObj,int posX,int posY,int scale)
    {
        SetObjParent(obj, panelObj);
        SetObjLocalPosition(obj, posX, posY, 0);
        SetObjLocalScale(obj, scale, scale, 1);
        AddRenderQueueModifier(obj, panelObj, targetObj);
    }

    /// <summary>
    /// 播放Spine2D的动画
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="animName"></param>
    public static void PlaySpine2DAnim(GameObject obj, string animName, int loop)
    {
        if (obj == null)
        {
            return;
        }

        SkeletonAnimation skeletonAnimation = obj.GetComponent<SkeletonAnimation>();
        if (skeletonAnimation == null)
        {
            return;
        }

        skeletonAnimation.state.AddAnimation(0, animName, loop != 0, 0);
    }

    /// <summary>
    /// 清除spine2D动画
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static void ClearSpine2DAnim(GameObject obj)
    {
        if (obj == null)
            return;

        SkeletonAnimation skeletonAnimation = obj.GetComponent<SkeletonAnimation>();
        if (skeletonAnimation == null)
            return;

        skeletonAnimation.state.ClearTracks();
    }

    /// <summary>
    /// 重置Spine2D动画
    /// </summary>
    /// <param name="obj"></param>
    public static void ResetSpin2DAnim(GameObject obj)
    {
        if (obj == null)
            return;
        SkeletonAnimation skeletonAnimation = obj.GetComponent<SkeletonAnimation>();
        if (skeletonAnimation == null)
            return;

        skeletonAnimation.Reset();
    }

    #endregion
}

