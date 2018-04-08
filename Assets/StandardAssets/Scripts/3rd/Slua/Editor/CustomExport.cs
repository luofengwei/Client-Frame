// The MIT License (MIT)

// Copyright 2015 Siney/Pangweiwei siney@yeah.net
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace SLua
{
    using System.Collections.Generic;
    using System;

    public class CustomExport
    {
        public static void OnGetAssemblyToGenerateExtensionMethod(out List<string> list) {
            list = new List<string> {
                "Assembly-CSharp",
            };
        }

        public static void OnAddCustomClass(LuaCodeGen.ExportGenericDelegate add)
        {
			// below lines only used for demostrate how to add custom class to export, can be delete on your app

            add(typeof(System.Func<int>), null);
            add(typeof(System.Action<int, string>), null);
            add(typeof(System.Action<int, Dictionary<int, object>>), null);
            add(typeof(List<int>), "ListInt");
            add(typeof(Dictionary<int, string>), "DictIntStr");
            add(typeof(string), "String");

            #region NGUI
            add(typeof(UIPanel), null);
            add(typeof(UIRoot), null);
            add(typeof(UIBasicSprite), null);
            add(typeof(UITexture), null);
            add(typeof(UIInput), null);
            add(typeof(UIAtlas), null);
            add(typeof(UITextList), null);
            add(typeof(UISprite), null);
            add(typeof(TweenScale), null);
            add(typeof(TweenAlpha), null);
            add(typeof(UIScrollView), null);
            add(typeof(UIPlayTween), null);
            add(typeof(UIButton), null);
            add(typeof(UICenterOnChild), null);
            add(typeof(UIToggle), null);
            add(typeof(UIGrid), null);
            add(typeof(UISlider), null);
            add(typeof(UIScrollBar), null);
            add(typeof(SpringPanel), null);
            add(typeof(EventDelegate), null);
            add(typeof(UIEventListener), null);
            add(typeof(UILabel), null);
            add(typeof(UIRect), null);
            add(typeof(UIWidget), null);
            add(typeof(UIWidgetContainer), null);
            add(typeof(UIProgressBar), null);
            add(typeof(UIButtonColor), null);
            add(typeof(UITweener), null);
            add(typeof(UIDragDropItem), null);

            //add(typeof(SlotLine), null);
            //add(typeof(SlotMachine), null);
            //add(typeof(SlotTile), null);
            //add(typeof(LeanTween), null);
            //add(typeof(LeanTweenType), null);
            //add(typeof(LoopScrollView_ZX), null);
            //add(typeof(LoopScrollView_Items), null);
            //add(typeof(LTDescr), null);

            //add (typeof(UIBasicSprite.Flip), null);  
            //add (typeof(UIWidget.Pivot), null);     
            //add (typeof(List<UIWidget>), null);
            //add (typeof(UIViewport), null);
            //add (typeof(UIFont), null);
            //add (typeof(UIInputOnGUI), null);
            //add (typeof(UILocalize), null);
            //add (typeof(UIAnchor), null);
            //add (typeof(UICamera), null);
            //add (typeof(UITooltip), null);

            //add (typeof(UISpriteData), null);
            //add (typeof(UISpriteAnimation), null);
            //add (typeof(UIOrthoCamera), null);
            //add (typeof(UIStretch), null);
            //add (typeof(UI2DSprite), null);
            //add (typeof(UI2DSpriteAnimation), null);
            //add (typeof(TweenOrthoSize), null);
            //add (typeof(TweenHeight), null);
            //add (typeof(TweenColor), null);
            //add (typeof(SpringPosition), null);
            //add (typeof(AnimatedWidget), null);
            //add (typeof(TweenPosition), null);
            //add (typeof(TweenFOV), null);
            //add (typeof(TweenWidth), null);
            //add (typeof(TweenTransform), null);
            //add (typeof(TweenRotation), null);
            //add (typeof(AnimatedColor), null);
            //add (typeof(TweenVolume), null);
            //add (typeof(AnimatedAlpha), null);
            //add (typeof(UIButtonMessage), null);
            //add (typeof(UIDragResize), null);
            //add (typeof(UIForwardEvents), null);
            //add (typeof(UIToggledComponents), null);
            //add (typeof(UIButtonScale), null);
            //add (typeof(UIDragObject), null);
            //add (typeof(UICenterOnClick), null);
            //add (typeof(UIButton), null);
            //add (typeof(UIDraggableCamera), null);
            //add (typeof(UISoundVolume), null);
            //add (typeof(UIPlayAnimation), null);
            //add (typeof(UISavedOption), null);
            //add (typeof(UIEventTrigger), null);
            //add (typeof(UIPlaySound), null);
            //add (typeof(UIPopupList), null);
            //add (typeof(TypewriterEffect), null);
            //add (typeof(UIDragDropContainer), null);
            //add (typeof(UIDragScrollView), null);
            //add (typeof(UIDragDropRoot), null);
            //add (typeof(UIButtonRotation), null);
            //add (typeof(UIWrapContent), null);
            //add (typeof(UIButtonOffset), null);
            //add (typeof(UIButtonActivate), null);
            //add (typeof(UIKeyNavigation), null);
            //add (typeof(UIButtonKeys), null);
            //add (typeof(UIToggledObjects), null);
            //add (typeof(UIKeyBinding), null);
            //add (typeof(LanguageSelection), null);
            //add (typeof(UIDragCamera), null);
            //add (typeof(UITable), null);
            //add (typeof(UIImageButton), null);
            //add (typeof(BetterList));
            //add (typeof(Localization), null);
            //add (typeof(ByteReader), null);
            //add (typeof(BMGlyph), null);
            //add (typeof(NGUIMath), null);
            //add (typeof(UISnapshotPoint), null);
            //add (typeof(BMFont), null);
            //add (typeof(PropertyBinding), null);
            //add (typeof(UIDrawCall), null);
            //add (typeof(AnimationOrTween));
            //add (typeof(PropertyReference), null);
            //add (typeof(BMSymbol), null);
            //add (typeof(NGUIDebug), null);
            //add (typeof(NGUIText), null);
            //add (typeof(UIGeometry), null);
            //add (typeof(ActiveAnimation), null);
            //add (typeof(NGUITools), null);
            //add (typeof(RealTime), null);
            #endregion
            // add your custom class here
            // add( type, typename)
            // type is what you want to export
            // typename used for simplify generic type name or rename, like List<int> named to "ListInt", if not a generic type keep typename as null or rename as new type name
        }

        public static void OnAddCustomAssembly(ref List<string> list)
        {
            // add your custom assembly here
            // you can build a dll for 3rd library like ngui titled assembly name "NGUI", put it in Assets folder
            // add its name into list, slua will generate all exported interface automatically for you

            //list.Add("NGUI");
        }

        public static HashSet<string> OnAddCustomNamespace()
        {
            return new HashSet<string>
            {
                //"NLuaTest.Mock"
            };
        }

        // if uselist return a white list, don't check noUseList(black list) again
        public static void OnGetUseList(out List<string> list)
        {
            list = new List<string>
            {
                //"UnityEngine.GameObject",
            };
        }

        public static List<string> FunctionFilterList = new List<string>()
        {
            "UIWidget.showHandles",
            "UIWidget.showHandlesWithMoveTool",
        };
        // black list if white list not given
        public static void OnGetNoUseList(out List<string> list)
        {
            list = new List<string>
            {      
                "HideInInspector",
                "ExecuteInEditMode",
                "AddComponentMenu",
                "ContextMenu",
                "RequireComponent",
                "DisallowMultipleComponent",
                "SerializeField",
                "AssemblyIsEditorAssembly",
                "Attribute", 
                "Types",
                "UnitySurrogateSelector",
                "TrackedReference",
                "TypeInferenceRules",
                "FFTWindow",
                "RPC",
                "Network",
                "MasterServer",
                "BitStream",
                "HostData",
                "ConnectionTesterStatus",
                "GUI",
                "EventType",
                "EventModifiers",
                "FontStyle",
                "TextAlignment",
                "TextEditor",
                "TextEditorDblClickSnapping",
                "TextGenerator",
                "TextClipping",
                "Gizmos",
                "ADBannerView",
                "ADInterstitialAd",            
                "Android",
                "Tizen",
                "jvalue",
                "iPhone",
                "iOS",
                "Windows",
                "CalendarIdentifier",
                "CalendarUnit",
                "CalendarUnit",
                "ClusterInput",
                "FullScreenMovieControlMode",
                "FullScreenMovieScalingMode",
                "Handheld",
                "LocalNotification",
                "NotificationServices",
                "RemoteNotificationType",      
                "RemoteNotification",
                "SamsungTV",
                "TextureCompressionQuality",
                "TouchScreenKeyboardType",
                "TouchScreenKeyboard",
                "MovieTexture",
                "UnityEngineInternal",
                "Terrain",                            
                "Tree",
                "SplatPrototype",
                "DetailPrototype",
                "DetailRenderMode",
                "MeshSubsetCombineUtility",
                "AOT",
                "Social",
                "Enumerator",       
                "SendMouseEvents",               
                "Cursor",
                "Flash",
                "ActionScript",
                "OnRequestRebuild",
                "Ping",
                "ShaderVariantCollection",
                "SimpleJson.Reflection",
                "CoroutineTween",
                "GraphicRebuildTracker",
                "Advertisements",
                "UnityEditor",
			    "WSA",
			    "EventProvider",
			    "Apple",
			    "ClusterInput",
				"Motion",
                "UnityEngine.UI.ReflectionMethodsCache",
            };
        }
    }
}