using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Fs.Config
{
    [CustomLuaClassAttribute]
    public static class FSLayerConfig
    {
        [DoNotToLua]
        public static int DynamicLayer = LayerMask.NameToLayer("Dynamic");
        [DoNotToLua]
        public static int GroundLayer = LayerMask.NameToLayer("Ground");
        [DoNotToLua]
        public static int AvatarLayer = LayerMask.NameToLayer("Avatar");
        [DoNotToLua]
        public static int DefaultLayer = LayerMask.NameToLayer("Default");
        [DoNotToLua]
        public static int UILayer = LayerMask.NameToLayer("UI");
        [DoNotToLua]
        public static int UI3DLayer = LayerMask.NameToLayer("3DUI");
        [DoNotToLua]
        public static int PupUpLayer = LayerMask.NameToLayer("PupUpLayer");
        [DoNotToLua]
        public static int GlowLayer = LayerMask.NameToLayer("GlowLayer");
        [DoNotToLua]
        public static int ClipLayer = LayerMask.NameToLayer("ClipLayer");

        public static void SetObjectLayer(this GameObject obj, int layer)
        {
            obj.layer = layer;
            Transform[] trans = obj.GetComponentsInChildren<Transform>();
            for (int i = 0; i < trans.Length; ++i)
            {
                trans[i].gameObject.layer = layer;
            }
            //foreach (Transform tran in obj.GetComponentsInChildren<Transform>())
            //{
            //    tran.gameObject.layer = layer;
            //}
        }
    }
}
