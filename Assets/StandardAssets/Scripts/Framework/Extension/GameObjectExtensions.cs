using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Ghost.Extensions
{
	public static class GameObjectExtensions {

        public static Transform FindNode(this GameObject obj, string strNodeName)
        {
            Transform[] nodeList = obj.transform.GetComponentsInChildren<Transform>();
            for (int i = 0; i < nodeList.Length; i++)
            {
                if(nodeList[i].name.Equals(strNodeName))
                {
                    return nodeList[i];
                }
            }
            return null;
        } 

		public static Rect CalcCompositeRect2D(this GameObject obj)
		{
			Vector2 min = Vector2.zero;
			Vector2 max = Vector2.zero;
			if (obj.GetComponent<Renderer>())
			{
				min = obj.GetComponent<Renderer>().bounds.min;
				max = obj.GetComponent<Renderer>().bounds.max;
			}
			var renderers = obj.GetComponentsInChildren<Renderer>();
			if (renderers != null)
			{
				foreach (var renderer in renderers)
				{
					var bounds = renderer.bounds;
					min.x = Mathf.Min(min.x, bounds.min.x);
					min.y = Mathf.Min(min.y, bounds.min.y);
					max.x = Mathf.Max(max.x, bounds.max.x);
					max.y = Mathf.Max(max.y, bounds.max.y);
				}
			}
			return new Rect(min.x-obj.transform.position.x, min.y-obj.transform.position.y, max.x-min.x, max.y-max.y);
		}

		public static Bounds CalcCompositeBounds2D(this GameObject obj)
		{
			var rect = obj.CalcCompositeRect2D();
			var center = rect.center + (Vector2)obj.transform.position;
			return new Bounds(new Vector3(center.x, center.y, obj.transform.position.z), new Vector3(rect.size.x, rect.size.y, obj.transform.position.z));
		}

		public static void FindGameObjectsInChildren(this GameObject obj, System.Predicate<GameObject> pred, ref List<GameObject> list)
		{
			if (pred(obj))
			{
				list.Add(obj);
			}
			
			var parentTransform = obj.transform;
			var childCount = parentTransform.childCount;
			if (0 < childCount)
			{
				for (int i = 0; i < childCount; ++i)
				{
					var childObj = parentTransform.GetChild(i).gameObject;
					childObj.FindGameObjectsInChildren(pred, ref list);
				}
			}
		}

		public static GameObject[] FindGameObjectsInChildren(this GameObject obj, System.Predicate<GameObject> pred)
		{
			var objs = new List<GameObject>();
			obj.FindGameObjectsInChildren(pred, ref objs);
			return objs.ToArray();
		}

		public static T FindComponentInChildren<T>(this GameObject obj) where T:Component
		{
			var component = obj.GetComponent<T>();
			if (null != component)
			{
				return component;
			}
			var transform = obj.transform;
			var childCount = transform.childCount;
			for (int i = 0; i < childCount; ++i)
			{
				var child = transform.GetChild(i);
				component = child.gameObject.FindComponentInChildren<T>();
				if (null != component)
				{
					return component;
				}
			}
			return null;
		}

		public static T[] FindComponentsInChildren<T>(this GameObject obj) where T:Component
		{
			var components = new List<T>();

			var parents = new List<Transform>();
			parents.Add(obj.transform);
			var nextParents = new List<Transform>();

			while (0 < parents.Count)
			{
				foreach (var parent in parents)
				{
					var component = parent.GetComponent<T>();
					if (null != component)
					{
						components.Add(component);
					}

					var childCount = parent.childCount;
					for (int i = 0; i < childCount; ++i)
					{
						nextParents.Add(parent.GetChild(i));
					}
				}
				parents.Clear();
				var temp = parents;
				parents = nextParents;
				nextParents = temp;
			}

			return components.ToArray();
		}

		public static T GetComponentInChildrenTopLevel<T>(this GameObject obj) where T:Component
		{
			var component = obj.GetComponent<T>();
			if (null != component)
			{
				return component;
			}
			var transform = obj.transform;
			var childCount = transform.childCount;
			for (int i = 0; i < childCount; ++i)
			{
				var child = transform.GetChild(i);
				component = child.GetComponent<T>();
				if (null != component)
				{
					return component;
				}
			}
			return null;
		}
		
	}
} // namespace Ghost.Extensions
