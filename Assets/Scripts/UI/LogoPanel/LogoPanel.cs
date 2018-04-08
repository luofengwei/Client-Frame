using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoPanel : MonoBehaviour
{
    public UITexture logoPic;
    public UITexture studioPic;
    private TweenAlpha tweenAplha = null;

    private void Awake()
    {
        if (logoPic != null && logoPic.mainTexture != null)
        {
            logoPic.alpha = 0;
        }
        if (studioPic != null && studioPic.mainTexture != null)
        {
            studioPic.alpha = 0;
        }
    }

    // Use this for initialization
    void Start()
    {
        //间隔1s到GameStart
        StartCoroutine(WaitForStartGame());
    }

    IEnumerator WaitForStartGame()
    {
        SetUITextureAlpha(logoPic,1.0f,1.0f);
        yield return new WaitForSeconds(1.0f);

        SetUITextureAlpha(logoPic,0.5f,0.0f);
        SetUITextureAlpha(studioPic,1.0f,1.0f);
        yield return new WaitForSeconds(2.0f);

        SetUITextureAlpha(studioPic,1.0f,0.0f);
        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene("GameStart");
    }

    private void SetUITextureAlpha(UITexture texture, float fTime, float fAlpha)
    {
        if (texture != null && texture.mainTexture != null)
        {
            tweenAplha = TweenAlpha.Begin(texture.gameObject,fTime,fAlpha);
        }
    }

    private void OnDestroy()
    {
        if(tweenAplha != null)
        {
            TweenAlpha.Destroy(tweenAplha);
        }
        if(logoPic != null)
        {
            logoPic.mainTexture = null;
            logoPic = null;
        }
        if(studioPic.mainTexture != null)
        {
            studioPic.mainTexture = null;
            studioPic = null;
        }
    }
}
