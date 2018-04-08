using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CustomLuaClassAttribute]
public class GameSysManager
{
    public static void ClosePreLoadingUI()
    {
        LoadingUISys.Instance.CloseUI();
    }
}
