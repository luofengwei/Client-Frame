///author       xuhan
///Data         2017.10.09
///Description  设置角色材质的辅助函数

using UnityEngine;
using UnityEngine.Rendering;
using System;

public class MaterialUtils
{
    public static void ResetGameObjectMaterials(GameObject go)
    {        
#if UNITY_EDITOR
        if (go == null)
            return;
        Renderer[] renderers = go.GetComponentsInChildren<Renderer>();
        if (renderers != null)
        {
            for (int i = 0; i < renderers.Length; ++i)
            {
                Renderer renderer = renderers[i];
                if (renderer != null && renderer.materials != null)
                {
                    for (int j = 0; j < renderer.materials.Length; ++j)
                    {
                        if (renderer.materials[j] != null)
                            renderer.materials[j].shader = Shader.Find(renderer.materials[j].shader.name);
                    }
                }
            }
        }
#endif
    }
}