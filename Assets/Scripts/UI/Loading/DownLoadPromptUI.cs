using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 不同的确认框类型
/// </summary>
public enum ConfirmType
{
    Cancel_OK_Btn, // 取消和确认按钮
    Single_OK_Btn, // 确认按钮
}

/// <summary>
/// 刚开始游戏的提示UI，提示玩家需要下载多少
/// LUA还没有启动，或者只是单纯的想在C#中调用提示UI
/// 下载或者网络连接不上失败的提示等
/// </summary>
public class DownLoadPromptUI : IDisposable
{
    const string PromptUIPath = "ConstFolder/UI/PreLoad/DownloadTips";

    // 允许和拒绝的回调
    public Action CancelCallBack;
    public Action AllowCallBack;
    public Action CenterAllowCallBack;

    // Objects
    GameObject PromptUIObj = null;
    UILabel ContentLabelTxt = null;
    UILabel CancelLabelTxt = null;
    UILabel AllowLabelTxt = null;
    UILabel CenterLabelTxt = null;

    UIEventListener CancelBtnListen = null;
    UIEventListener AllowBtnListen = null;
    UIEventListener CenterAllowBtnListen = null;

    /// <summary>
    /// 设置显示的提示
    /// </summary>
    public string Tips
    {
        set
        {
            if (ContentLabelTxt != null)
                ContentLabelTxt.text = value;
        }
    }

    /// <summary>
    /// 2个按钮时-左边取消按钮的提示
    /// </summary>
    public string CancelText
    {
        set
        {
            if (CancelLabelTxt != null)
                CancelLabelTxt.text = value;
        }
    }

    /// <summary>
    /// 2个按钮时-右边允许按钮的提示
    /// </summary>
    public string AllowText
    {
        set
        {
            if (AllowLabelTxt != null)
                AllowLabelTxt.text = value;
        }
    }

    /// <summary>
    /// 1个按钮时-中间按钮的提示
    /// </summary>
    public string CenterText
    {
        set
        {
            if (CenterLabelTxt != null)
                CenterLabelTxt.text = value;
        }
    }

    // 创建UI
    public DownLoadPromptUI(ConfirmType type = ConfirmType.Cancel_OK_Btn)
    {
        CreateUI(type);
        AddEvts();
    }

    // 初始化UI和获取UI的GameObjects,Components
    void CreateUI(ConfirmType type = ConfirmType.Cancel_OK_Btn)
    {
        PromptUIObj = GameObject.Instantiate(Resources.Load(PromptUIPath)) as GameObject;

        GameObject contentLabel = GameObjectUtils.DirectFind("ConfirmBox/ContentLabel",PromptUIObj);
        GameObject cancelBtn = GameObjectUtils.DirectFind("ConfirmBox/CancelBtn",PromptUIObj);
        GameObject cancelLabel = GameObjectUtils.DirectFind("ConfirmBox/CancelBtn/Label", PromptUIObj);
        GameObject allowBtn = GameObjectUtils.DirectFind("ConfirmBox/ConfirmBtn",PromptUIObj);
        GameObject allowLabel = GameObjectUtils.DirectFind("ConfirmBox/ConfirmBtn/Label", PromptUIObj);
        GameObject centerBtn = GameObjectUtils.DirectFind("ConfirmBox/CenterBtn",PromptUIObj);
        GameObject centerLabel = GameObjectUtils.DirectFind("ConfirmBox/CenterBtn/Label", PromptUIObj);

        ContentLabelTxt = contentLabel != null ? contentLabel.GetComponent<UILabel>() : null;
        CancelLabelTxt = cancelLabel != null ? cancelLabel.GetComponent<UILabel>() : null;
        AllowLabelTxt = allowLabel != null ? allowLabel.GetComponent<UILabel>() : null;
        CenterLabelTxt = centerLabel != null ? centerLabel.GetComponent<UILabel>() : null;

        CancelBtnListen = cancelBtn != null ? cancelBtn.GetComponent<UIEventListener>() : null;
        AllowBtnListen = allowBtn != null ? allowBtn.GetComponent<UIEventListener>() : null;
        CenterAllowBtnListen = centerBtn != null ? centerBtn.GetComponent<UIEventListener>() : null;

        // 这里初始化不同的确认框类型
        // 这里其实可以写成 cancelBtn.SetActive(type == ConfirmType.Cancel_OK_Btn)
        if (type == ConfirmType.Cancel_OK_Btn)
        {
            cancelBtn.SetActive(true);
            allowBtn.SetActive(true);
            centerBtn.SetActive(false);
        }
        else if (type == ConfirmType.Single_OK_Btn)
        {
            cancelBtn.SetActive(false);
            allowBtn.SetActive(false);
            centerBtn.SetActive(true);
        }
        else
        {
            // none
            cancelBtn.SetActive(true);
            allowBtn.SetActive(true);
            centerBtn.SetActive(false);
        }
    }

    // 注册UI的事件处理
    void AddEvts()
    {
        if (CancelBtnListen != null)
            CancelBtnListen.onClick = go => { if (CancelCallBack != null) CancelCallBack(); };
        if (AllowBtnListen != null)
            AllowBtnListen.onClick = go => { if (AllowCallBack != null) AllowCallBack(); };
        if (CenterAllowBtnListen != null)
            CenterAllowBtnListen.onClick = go => { if (CenterAllowCallBack != null) CenterAllowCallBack(); }; 
    }


    // 销毁Gameobject
    public void Dispose()
    {
        Debug.Log("PromptUI Dispose");
        CancelCallBack = null;
        AllowCallBack = null;
        CenterAllowCallBack = null;
        ContentLabelTxt = null;
        CancelLabelTxt = null;
        AllowLabelTxt = null;
        CenterLabelTxt = null;
        CancelBtnListen = null;
        AllowBtnListen = null;
        CenterAllowBtnListen = null;
        if (PromptUIObj != null)
        {
            GameObject.DestroyImmediate(PromptUIObj);
            PromptUIObj = null;
        }
    }
}

public static class PromptUIUtil
{

    /// <summary>
    /// 用于检测网络连接失败的提示
    /// </summary>
    /// <returns></returns>
    public static IEnumerator CheckNetWork_DownLoadFail()
    {
        using (DownLoadPromptUI ui = new DownLoadPromptUI(ConfirmType.Cancel_OK_Btn))
        {
            bool isAllow = false;

#if UNITY_IOS || UNITY_IPHONE
            ui.CancelCallBack = () =>
            {
                isAllow = true;
            };
            ui.CancelText = "继续";
            ui.AllowCallBack = () =>
            {
                isAllow = true;
                XDSDK.ShowNetWordAlert();
                Application.Quit();
            };
            ui.AllowText = "检查权限";
#else
            ui.CancelCallBack = () =>
            {
                isAllow = true;
            };
            ui.CancelText = "继续";
            ui.AllowCallBack = () =>
            {
                isAllow = true;
                Application.Quit();
            };
            ui.AllowText = "离开";
#endif

            ui.Tips = "无法下载资源，请检查您的网络状态及游戏的网络权限设置。若无法解决问题请联系官方人员（官方QQ群549996364）。";

            while (!isAllow)
                yield return null;
        }
    }

    public static IEnumerator DownLoadChannelFail()
    {
        using (DownLoadPromptUI ui = new DownLoadPromptUI(ConfirmType.Single_OK_Btn))
        {
            bool isAllow = false;

#if UNITY_IOS || UNITY_IPHONE
            ui.CenterAllowCallBack = () =>
            {
                isAllow = true;
                XDSDK.ShowNetWordAlert();
                Application.Quit();
            };
            ui.CenterText = "权限设置";
#else
            ui.CenterAllowCallBack = () =>
            {
                isAllow = true;
                Application.Quit();
            };
            ui.CenterText = "离开";
#endif

            ui.Tips = "无法下载资源，请检查您的网络状态及游戏的网络权限设置。若无法解决问题请联系官方人员（官方QQ群549996364）。";

            while (!isAllow)
                yield return null;
        }
    }

    /// <summary>
    /// 用于创建提示不同的提示框
    /// </summary>
    /// <param name="type">使用哪一个提示类型</param>
    /// <param name="AllowCallBack">确定之后的回调</param>
    /// <param name="CancelCallBack">取消之后的回调</param>
    public static IEnumerator PromptUI(ConfirmType type = ConfirmType.Cancel_OK_Btn)
    {
        using (DownLoadPromptUI ui = new DownLoadPromptUI(type))
        {
            bool isAllow = false;
            if (type == ConfirmType.Cancel_OK_Btn)
            {
                ui.AllowCallBack = () => { isAllow = true; };
                ui.CancelCallBack = () => { 
#if !UNITY_EDITOR
                    Application.Quit();
#endif
                };
            }
            else
            {
                ui.CenterAllowCallBack = () => { 
#if !UNITY_EDITOR
                    Application.Quit();
#endif
                };
            }

            while (!isAllow)
                yield return null;
        }
    }

    // 单纯的提示UI
    public static IEnumerator PromptUI(string tips)
    {
        using (DownLoadPromptUI ui = new DownLoadPromptUI(ConfirmType.Single_OK_Btn))
        {
            bool isAllow = false;
            ui.CenterAllowCallBack = () => { isAllow = true; };

            ui.Tips = tips;

            while (!isAllow)
                yield return null;
        }
    }
}
