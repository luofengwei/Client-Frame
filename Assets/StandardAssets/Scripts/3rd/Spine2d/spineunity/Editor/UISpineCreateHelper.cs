using UnityEngine;
using UnityEditor;
using System.Collections;
using Ghost.Utils;

public class UISpineCreateHelper : MonoBehaviour
{
	[MenuItem("Assets/Spine/CreateUISpine")]
	static void CreateUISpine ()
	{
		Object[] selects = Selection.objects;
		foreach (Object o in selects) {
			SkeletonDataAsset s = o as SkeletonDataAsset;
			if (s != null && s.atlasAsset != null) {
				AtlasAsset a = s.atlasAsset;
				if (a.materials.Length > 0) {
					Material m = a.materials [0];

					string path = "Assets/";//Application.persistentDataPath;
					path = PathUnity.Combine (path, "ABResources/Spine2D");
					path = PathUnity.Combine (path, o.name + ".prefab");
					GameObject g = new GameObject ();
					g.name = o.name;

					g.AddComponent<MeshRenderer> ();
					g.AddComponent<MeshFilter> ();
					
					NGUISpine nus = g.AddComponent<NGUISpine> ();
					nus.skeletonDataAsset = s;
					var anims = s.GetSkeletonData (true).Animations;
					if (anims.Count > 0) {
						nus.Reset ();
						nus.AnimationName = anims [1].Name;
					}

					UISpine us = g.AddComponent<UISpine> ();
					us.spineMaterial = m;
					us.depth = 100;

					GameObject newGo = PrefabUtility.CreatePrefab (path, g);
					newGo.name = g.name;
					GameObject.DestroyImmediate (g);
				}
			}
		}
		AssetDatabase.Refresh ();
	}
}
