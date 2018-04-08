using UnityEngine;
using System.Collections;

public class RenderQueueModifier : MonoBehaviour {

    ///必须设置 或者 由代码设置
    public UIPanel m_panel;
    public UIWidget m_target;
    public bool isForSpine = true;

    public Renderer renderer;
    public SkeletonRenderer skeletonRender;
    void Awake()
    {
        if (isForSpine)
        {
            skeletonRender = GetComponent<SkeletonRenderer>();
        }
        else
        {
            renderer = GetComponent<Renderer>();
        }
    }


    void OnEnable()
    {
        AddToPanel();
    }
    public void Set(UIPanel m_panel, UIWidget m_target, bool isForSpine)
    {
        this.m_panel = m_panel;
        this.m_target = m_target;
        this.isForSpine = isForSpine;
        AddToPanel();
    }

    void AddToPanel()
    {
        if (m_panel != null) m_panel.renderQueueModifiers.Add(this);
    }

    void OnDisable()
    {

        m_panel.renderQueueModifiers.Remove(this);
    }

    int lasetQueue = int.MinValue;
    public void setQueue(int queue)
    {
        if (this.lasetQueue != queue)
        {
            this.lasetQueue = queue;

            if (isForSpine)
            {
                skeletonRender.needControllerRenderQueue = true;
                skeletonRender.renderQueue = this.lasetQueue;
            }
            else
            {
                renderer.material.renderQueue = this.lasetQueue;
            }
        }
    }
}
