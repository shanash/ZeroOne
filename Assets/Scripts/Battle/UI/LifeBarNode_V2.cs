using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBarNode_V2 : MonoBehaviour, IPoolableComponent
{
    [SerializeField, Tooltip("Bar BG")]
    Transform Bar_BG;

    [SerializeField, Tooltip("Red Bar")]
    SpriteRenderer Life_Shield_Bar;

    [SerializeField, Tooltip("Blue Bar(Life Bar)")]
    SpriteRenderer Life_Bar;

    const float Fill_Width = 3f;


    Coroutine Flush_Coroutine;

    Coroutine Show_Coroutine;


    public void ShowLifeBar(float duration)
    {
        if (Show_Coroutine != null)
        {
            StopCoroutine(Show_Coroutine);
        }
        Show_Coroutine = StartCoroutine(StartShowLifeBar(duration));
    }

    public void HideLifeBar()
    {
        if (Show_Coroutine != null)
        {
            StopCoroutine(Show_Coroutine);
        }
        Show_Coroutine = null;
        Bar_BG.gameObject.SetActive(false);
    }


    IEnumerator StartShowLifeBar(float delay)
    {
        Bar_BG.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        Bar_BG.gameObject.SetActive(false);
        Show_Coroutine = null;
    }

    /// <summary>
    /// 체력 게이지 비율
    /// </summary>
    /// <param name="per"></param>
    public void SetLifePercent(float per)
    {
        SetBarPercent(Life_Bar, per);
        ShowLifeBar(2f);
    }

    /// <summary>
    /// 체력 보호막 게이지 비율
    /// </summary>
    /// <param name="per"></param>
    public void SetLifeShieldPercent(float per)
    {
        SetBarPercent(Life_Shield_Bar, per);
        ShowLifeBar(2f);
    }

    /// <summary>
    /// 지징된 바 게이지의 비율 계산하여 업데이트
    /// </summary>
    /// <param name="bar"></param>
    /// <param name="per"></param>
    void SetBarPercent(SpriteRenderer bar, float per)
    {
        float p = Mathf.Clamp01(per) * Fill_Width;
        bar.size = new Vector2(p, bar.size.y);
    }


    public void Spawned()
    {
        SetBarPercent(Life_Shield_Bar, 0f);
        SetBarPercent(Life_Bar, 1f);
        
        HideLifeBar();
    }
    public void Despawned()
    {
        if (Flush_Coroutine != null)
        {
            StopCoroutine(Flush_Coroutine);
        }
        Flush_Coroutine = null;
        HideLifeBar();
    }

    
}
