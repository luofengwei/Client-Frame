///Data         2018.02.01
///Description
///

using UnityEngine;

namespace GameFramework
{
    public class TestGui : MonoBehaviour
    {    
        public enum PlacementType { TopRight, TopLeft, BottomRight, BottomLeft}
               
        [Tooltip("Where on screen the buttons should be placed.")]
        public PlacementType Placement = PlacementType.BottomRight;

        const int ButtonWidth = 75;
        const int ButtonHeight = 25;

        void OnGUI()
        {
            float x = 0, y = 0;
            switch (Placement)
            {
                case PlacementType.TopRight:
                    x = Screen.width - ButtonWidth - 10;
                    y = ButtonHeight + 20;
                    break;
                case PlacementType.TopLeft:
                    x = 10;
                    y = ButtonHeight + 20;
                    break;
                case PlacementType.BottomRight:
                    x = Screen.width - ButtonWidth - 10;
                    y = Screen.height - ButtonHeight - 10;
                    break;
                case PlacementType.BottomLeft:
                    x = 10;
                    y = Screen.height - ButtonHeight - 10;
                    break;
            }

            if (GUI.Button(new Rect(x, y, ButtonWidth, ButtonHeight), "test1"))
            {
                
            }
            if (GUI.Button(new Rect(x, y - ButtonHeight - 10, ButtonWidth, ButtonHeight), "test2"))
            {
               
            }
        }
    }
}
