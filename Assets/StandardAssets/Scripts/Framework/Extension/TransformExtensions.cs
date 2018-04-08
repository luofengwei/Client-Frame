using UnityEngine;
using System.Collections;

namespace Ghost.Extensions
{
	public static class TransformExtensions
	{
		public static void ResetLocal(this Transform t)
		{
			t.localPosition = Vector3.zero;
			t.localRotation = new Quaternion();
			t.localScale = Vector3.one;
		}

		public static void ResetParent(this Transform t, Transform parent)
		{
			t.parent = parent;
			t.ResetLocal();
		}
	}
} // namespace Ghost.Extensions
