///author       xuhan
///Data         2016.09.22
///Description

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutoDestroy : MonoBehaviour
{
    private bool bDestroy = false;

    public void SetDestroy()
    {
        bDestroy = true;
    }

    public void OnDestroy()
    {
        if (!bDestroy)
        {
            GameObjectUtils.DestoryResOnCompents(gameObject);
            bDestroy = true;
        }
    }
}
