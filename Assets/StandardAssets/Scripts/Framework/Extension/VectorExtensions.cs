using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Ghost.Extensions
{
	public static class VectorExtensions {

		public static Vector2 XZ(this Vector3 p)
		{
			return new Vector2(p.x, p.z);
		}
		
	}
} // namespace Ghost.Extensions
