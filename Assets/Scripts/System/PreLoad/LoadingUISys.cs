using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingUISys : CGameSystem
{
    /// <summary>
    /// 单例化预加载的读取界面
    /// </summary>
    private static LoadingUISys _Instance = null;
    public static LoadingUISys Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = CGameRoot.GetGameSystem<LoadingUISys>();
            }
            return _Instance;
        }
    }

    private const string cUIPrefabPath = "LoginConst/PreLoad/PreLoading";
    private const string cUIBikeGirl = "LoginConst/PreLoad/BikeGirl";

    private const string cUIProgressRoot = "ProgressMain";
    private const string cUIProgressTips = "TipsLabel";
    private const string cUIProgressSlider = "Slider";
    private const string cUIProgressThumb = "Thumb";
    private const string cUISpine2DRoot = "SpineRoot";
    private const string cUISpine2DBg = "BlackBg";
    private const string cUIBackGround = "Background";
    private const string cUIShadow = "Shadow";


    private Object mPreLoadObj = null;
    private GameObject mPreLoadUI = null;

    private Object mBikeGirlObject = null;
    private GameObject mBikeGirlUI = null;

    private GameObject mProgressRoot = null;
    private UILabel mTipsLabel = null;
    private GameObject mSliderObj = null;
    private UISlider mSlider = null;
    private GameObject mProgressThumb = null;
    private GameObject mSpine2DRoot = null;
    private GameObject mSpine2DBg = null;
    private GameObject mUIBackGround = null;
    private GameObject mUIShadow = null;


    private void Awake()
    {
        InitUI();
    }

    /// <summary>
    /// 初始持有loading界面的一些控件，带有一个spine2D动画
    /// </summary>
    private void InitUI()
    {
        if (mPreLoadUI == null)
        {
            mPreLoadObj = Resources.Load(cUIPrefabPath);
            mPreLoadUI = GameObject.Instantiate(mPreLoadObj) as GameObject;

            mProgressRoot = GameObjectUtils.DeepFind(mPreLoadUI, cUIProgressRoot);
            mTipsLabel = GameObjectUtils.DeepFind(mPreLoadUI, cUIProgressTips).GetComponent<UILabel>();
            mSliderObj = GameObjectUtils.DeepFind(mProgressRoot, cUIProgressSlider);
            mSlider = mSliderObj.GetComponent<UISlider>();
            mProgressThumb = GameObjectUtils.DeepFind(mProgressRoot, cUIProgressThumb);
            mSpine2DRoot = GameObjectUtils.DeepFind(mProgressRoot, cUIProgressRoot);
            mSpine2DBg = GameObjectUtils.DeepFind(mProgressRoot, cUISpine2DBg);
            mUIBackGround = GameObjectUtils.DeepFind(mPreLoadUI, cUIBackGround);
            mUIShadow = GameObjectUtils.DeepFind(mPreLoadUI, cUIShadow);
            if(mSpine2DRoot != null && mSpine2DBg != null)
            {
                mBikeGirlObject = Resources.Load(cUIBikeGirl);
                mBikeGirlUI = GameObject.Instantiate(mBikeGirlObject) as GameObject;

                GameObjectUtils.AddSpine2D(mBikeGirlUI, mSpine2DRoot, mSpine2DBg, 0, 0, 1);
                GameObjectUtils.ChangeLayersRecursively(mBikeGirlUI, mSpine2DRoot.layer);
            }

            mSliderObj.SetActive(true);
            mSlider.value = 0;
            SetBackgroundSuitForDifferentDpi();
            
            GameObjectUtils.KeepObjectInEdge(mSpine2DRoot, mProgressThumb);
        }
        mPreLoadUI.SetActive(true);
    }

    private void SetBackgroundSuitForDifferentDpi()
    {
        if (mPreLoadUI == null)
        {
            return;
        }
        UIRoot mUIRoot = mPreLoadUI.GetComponent<UIRoot>();
        if (mUIRoot == null)
        {
            return;
        }
        float s = mUIRoot.activeHeight / (float)Screen.height;
        float actualWidth = Screen.width * s;
        float actualHeight = Screen.height * s;
        UITexture BGUITexture = mUIBackGround.GetComponent<UITexture>();
        if (BGUITexture == null)
        {
            return;
        }
        if (actualWidth < 100 || actualHeight < 100)
        {
            actualHeight = 720;
            actualWidth = 1280;
        }
        UISprite BGUIProSprite = mUIShadow.GetComponent<UISprite>();
        if (BGUIProSprite == null)
        {
            return;
        }
        BGUIProSprite.width = BGUITexture.width;
        BGUIProSprite.transform.localPosition = new Vector3(0, -actualHeight / 2 - 2, 0);
    }

    public void FixedUpdate()
    {
        if(mSlider != null)
        {

        }
    }

    public void LoadUI()
    {
        if(mPreLoadUI != null && !mPreLoadUI.activeInHierarchy)
        {
            mPreLoadUI.SetActive(true);

            mSliderObj.SetActive(true);
        }        
    }

    public void ShowUI()
    {
        if(mPreLoadUI != null && !mPreLoadUI.activeSelf)
        {
            mPreLoadUI.SetActive(true);
        }

        if(mSliderObj != null)
        {
            mSliderObj.SetActive(true);
        }
    }

    public void CloseUI()
    {
        if(mPreLoadUI != null && mPreLoadUI.activeSelf)
        {
            mTipsLabel.text = string.Empty;
            mPreLoadUI.SetActive(false);
        }
        UnLoad();
    }

    public void UnLoad()
    {
        if(mTipsLabel != null)
        {
            mTipsLabel.text = string.Empty;
            mTipsLabel = null;
        }
        if(mPreLoadUI != null)
        {
            GameObject.Destroy(mPreLoadUI);
            mPreLoadUI = null;
        }

        mProgressRoot = null;
        mSliderObj = null;
        mSlider = null;
        mTipsLabel = null;

        mPreLoadObj = null;
        _Instance = null;
    }

    #region 对进度条的控制

    private float _low = 0f;
    private float _high = 0f;

    public float LowProgress
    {
        get { return _low; }
        set
        {
            _low = float.IsNaN(value)?0f:value;
        }
    }

    public float HighProgress
    {
        get { return _high; }
        set
        {
            _high = float.IsNaN(value) ? 0f : value;
        }
    }

    public void SetProgress(float low = 0f,float high = 1f)
    {
        LowProgress = low;
        HighProgress = high;
    }

    public void setTips(string tips)
    {
        if (mSlider == null || mTipsLabel == null)
            return;

        mTipsLabel.text = tips;
    }

    #endregion
}
