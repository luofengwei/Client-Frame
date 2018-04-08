using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CreativeSpore.RpgMapEditor
{
	public class UtilsGuiDrawing
	{
		public static void DrawRectWithOutline( Rect rect, Color color, Color colorOutline )
		{
	#if UNITY_EDITOR
			Vector3[] rectVerts = { new Vector3(rect.x, rect.y, 0), 
				new Vector3(rect.x + rect.width, rect.y, 0), 
				new Vector3(rect.x + rect.width, rect.y + rect.height, 0), 
				new Vector3(rect.x, rect.y + rect.height, 0) };
			Handles.DrawSolidRectangleWithOutline(rectVerts, color, colorOutline);
	#else
			Texture2D texture = new Texture2D(1, 1);
			texture.SetPixel(0,0,colorOutline);
			texture.Apply();

			Rect rLine = new Rect( rect.x, rect.y, rect.width, 1 );
			GUI.DrawTexture(rLine, texture);
			rLine.y = rect.y + rect.height - 1;
			GUI.DrawTexture(rLine, texture);
			rLine = new Rect( rect.x, rect.y+1, 1, rect.height-2 );
			GUI.DrawTexture(rLine, texture);
			rLine.x = rect.x + rect.width - 1;
			GUI.DrawTexture(rLine, texture);

			rect.x += 1;
			rect.y += 1;
			rect.width -= 2;
			rect.height -= 2;
			texture.SetPixel(0,0,color);
			texture.Apply();
			GUI.DrawTexture(rect, texture);
	#endif
		}
	}
}