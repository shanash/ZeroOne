using FluffyDuck.Util;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarNode : MonoBehaviour, IPoolableComponent
{
    [SerializeField, Tooltip("Box")]
    RectTransform Box;

    [SerializeField, Tooltip("Life Back Bar")]
    Image Life_Red_Back_Bar;

    /// <summary>
    /// 일시적 추가 체력 및 보호막 추가 기능
    /// </summary>
    [SerializeField, Tooltip("Life Shield Bar")]
    Image Life_Shield_Bar;

    [SerializeField, Tooltip("Life Bar")]
    Image Life_Bar;

    [SerializeField, Tooltip("Duration Icon Container")]
    RectTransform Duration_Icon_Container;

    RectTransform This_Rect;

    Transform Target_Transform;

    Coroutine Flush_Coroutine;

    Coroutine Show_Coroutine;

    //public void SetBarColor(TEAM_TYPE ttype)
    //{
    //    if (ttype == TEAM_TYPE.LEFT)
    //    {
    //        Life_Bar.color = "00FF00".ToRGBFromHex();
    //    }
    //    else
    //    {
    //        Life_Bar.color = "FFAA00".ToRGBFromHex();
    //    }

    //    if (Show_Coroutine != null)
    //    {
    //        StopCoroutine(Show_Coroutine);
    //    }
    //    Show_Coroutine = StartCoroutine(StartShowLifeBar(5f));
    //}

    public void ShowLifeBar(float duration)
    {
        if (Show_Coroutine != null)
        {
            StopCoroutine(Show_Coroutine);
        }
        Show_Coroutine = StartCoroutine(StartShowLifeBar(duration));
    }

    public void SetTargetTransform(Transform t)
    {
        Target_Transform = t;
        UpdatePosition();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {
        if (Target_Transform != null)
        {
            Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, Target_Transform.position);
            //this.transform.position = pos;
            //(this.transform as RectTransform).anchoredPosition3D = pos;
            This_Rect.anchoredPosition3D = pos;
        }
    }

    public void SetLifePercent(float per)
    {
        float p = Mathf.Clamp01(per);
        Life_Bar.fillAmount = p;
        FlushBackBar();
        if (Show_Coroutine != null)
        {
            StopCoroutine(Show_Coroutine);
        }
        Show_Coroutine = StartCoroutine(StartShowLifeBar(2f));
    }

    public void SetShieldPercent(float per)
    {
        float p = Mathf.Clamp01(per);
        Life_Shield_Bar.fillAmount = p;
    }

    public void SetLifeAndShieldPercent(float life_per, float shield_per)
    {
        // 체력 + 보호막의 비율은 1을 초과할 수 없음. 1초과시 보호막의 수치를 낮춤
        float max_per = life_per + shield_per;
        if (max_per > 1f)
        {
            float diff = max_per - 1f;
            shield_per -= diff;
            if (shield_per < 0f)
            {
                shield_per = 0f;
            }
        }

        SetLifePercent(life_per);
        SetShieldPercent(life_per + shield_per);
    }

    /// <summary>
    /// 체력 및 보호막의 수치가 적용되고 바로 호출되는 것이 아닌,
    /// 해당 액션이 완전히 종료된 후 호출되는 함수.
    /// 그래야 Back Bar가 체력게이지를 따라가는 연출을 볼 수 있음.
    /// </summary>
    public void FlushBackBar()
    {
        if (Flush_Coroutine != null)
        {
            StopCoroutine(Flush_Coroutine);
        }

        Flush_Coroutine = StartCoroutine(BackBarFlush());
    }

    IEnumerator BackBarFlush()
    {
        float life_rate = Life_Bar.fillAmount;
        float shield_rate = Life_Shield_Bar.fillAmount;

        float target_point = 0f;
        if (shield_rate > life_rate)
        {
            target_point = shield_rate;
        }
        else
        {
            target_point = life_rate;
        }
        float start_point = Life_Red_Back_Bar.fillAmount;
        float time = 0f;
        const float duration = 0.3f;
        while (time < duration)
        {
            time += Time.deltaTime;
            Life_Red_Back_Bar.fillAmount = Mathf.Lerp(start_point, target_point, time / duration);
            yield return null;
        }
        Flush_Coroutine = null;
    }

    IEnumerator StartShowLifeBar(float delay)
    {
        Box.gameObject.SetActive(true);
        yield return new WaitForSeconds(delay);
        Box.gameObject.SetActive(false);
        Show_Coroutine = null;
    }

    public void HideLifeBar()
    {
        if (Show_Coroutine != null)
        {
            StopCoroutine(Show_Coroutine);
        }
        Show_Coroutine = null;
        Box.gameObject.SetActive(false);
    }

    public void Spawned()
    {
        Life_Bar.fillAmount = 1f;
        Life_Red_Back_Bar.fillAmount = 1f;
        Life_Shield_Bar.fillAmount = 1f;
        if (This_Rect == null)
        {
            This_Rect = GetComponent<RectTransform>();
        }
    }

    public void Despawned()
    {
        Target_Transform = null;
        if (Flush_Coroutine != null)
        {
            StopCoroutine(Flush_Coroutine);
        }
        Flush_Coroutine = null;


        if (Show_Coroutine != null)
        {
            StopCoroutine(Show_Coroutine);
        }
        Show_Coroutine = null;
    }



}
