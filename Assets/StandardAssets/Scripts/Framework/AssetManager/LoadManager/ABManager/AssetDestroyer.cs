using UnityEngine;
using System.Collections.Generic;

public class AssetDestroyer : MonoBehaviour
{
    public List<Object> m_Assets = new List<Object>();

    public void AddToDestroy(Object assets)
    {
        if (!m_Assets.Contains(assets))
        {
            m_Assets.Add(assets);
            enabled = true;
        }
    }

    public void DestroyRightNow()
    {
        if (m_Assets != null && m_Assets.Count > 0)
        {
            for (int i = 0; i < m_Assets.Count; ++i)
            {
                //Debug.Log("<color=yellow> LateDestroy: " + m_Assets[i].name + "\n" + "</color>");
                Object.DestroyImmediate(m_Assets[i], true);
            }
            m_Assets.Clear();
        }
        enabled = false;
    }

    void Update()
    {
        DestroyRightNow();
    }

    public void OnDestroy()
    {
        if (m_Assets != null)
        {
            m_Assets.Clear();
            m_Assets = null;
        }
    }
}
