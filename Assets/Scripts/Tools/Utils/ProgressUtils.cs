///author       xuhan
///Data         2017.12.04
///Description
///

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void UpdateProgress(float value, bool bSend);

public class ProgressUtils
{
    public static UpdateProgress updateProgress = null; 
    
    private static float lowProgress = 0.0f;
    private static float highProgress = 0.0f;

    public static float LowProgress
    {
        get { return lowProgress; }
        set
        {
            if (float.IsNaN(value))
                lowProgress = 0.0f;
            else lowProgress = value;
        }
    }

    public static float HighProgress
    {
        get { return highProgress; }
        set
        {
            if (float.IsNaN(value))
                highProgress = 0.0f;
            else highProgress = value;
        }
    }

    public static void SetProgress(float low = 0.0f, float high = 1.0f)
    {
        LowProgress = low;
        HighProgress = high;
    }

    public static float GetCurrentProgress(float fProgress)
    {
        float fPercent = fProgress > 1.0f ? 1.0f : fProgress;
        fPercent = LowProgress + (HighProgress - LowProgress) * fProgress;
        fPercent = fPercent < 0.0f || float.IsNaN(fPercent) ? 0.0f : fPercent;
        return fPercent;
    }

    public static void SyncProgress(float fProgress, bool bSendToServer)
    {
        if (updateProgress != null)
        {
            float fPercent = ProgressUtils.GetCurrentProgress(fProgress);          
            updateProgress(fPercent, bSendToServer);
            //Debug.Log("<color=yellow>SyncProgress: " + fPercent + "</color>");
        }  
    }   
}