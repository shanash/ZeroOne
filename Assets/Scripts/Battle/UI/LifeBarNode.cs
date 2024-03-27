using FluffyDuck.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarNode : MonoBehaviour, IPoolableComponent
{
    [SerializeField, Tooltip("Box")]
    RectTransform Box;

    [Space()]
    [Header("Slider Version")]
    [SerializeField, Tooltip("Main Life Bar")]
    Slider Main_Life_Bar;

    [SerializeField, Tooltip("Sub Life Bar")]
    Slider Sub_Life_Bar;

    [SerializeField, Tooltip("Main Life Bar Knob")]
    Image Main_Life_Bar_Knob;

    [Space()]
    [Header("Image Fillamount Version")]
    [SerializeField, Tooltip("Main Life Image Bar")]
    Image Main_Life_Image_Bar;

    [SerializeField, Tooltip("Sub Life Image Bar")]
    Image Sub_Life_Image_Bar;

    [SerializeField, Tooltip("Main Life Image Bar Knob")]
    Image Main_Life_Image_Bar_Knob;


    [SerializeField, Tooltip("Duration Icon Container")]
    RectTransform Duration_Icon_Container;

    RectTransform This_Rect;

    Transform Target_Transform;

    //  image version
    Coroutine Show_Coroutine;

    //  slider version
    Coroutine Slider_Flush_Coroutine;

    bool Use_Slider = true;


    HeroBase_V2 Hero;
    /// <summary>
    /// 지속성 효과 아이콘 리스트
    /// </summary>
    List<EnemyDurationSkillIconNode> Used_Duration_Skill_Icons = new List<EnemyDurationSkillIconNode>();

    RectTransform Canvas_Rect;
    public void SetCanvasRect(RectTransform canvas_rect)
    {
        this.Canvas_Rect = canvas_rect;
    }

    public void SetHeroBaseV2(HeroBase_V2 hero)
    {
        this.Hero = hero;
        if (this.Hero.Team_Type == TEAM_TYPE.RIGHT)
        {
            this.Hero.Slot_Events += SkillSlotEventCallback;
        }
        SetTargetTransform(this.Hero.GetHPPositionTransform());
    }

    void SkillSlotEventCallback(SKILL_SLOT_EVENT_TYPE etype)
    {
        if (etype == SKILL_SLOT_EVENT_TYPE.DURATION_SKILL_ICON_UPDATE)
        {
            UpdateDurationSkillIcons();
        }
    }

    /// <summary>
    /// 지속성 효과 아이콘 추가
    /// </summary>
    void UpdateDurationSkillIcons()
    {
        if (Hero == null)
        {
            return;
        }
        if (Duration_Icon_Container == null)
        {
            return;
        }
        ClearDurationSkillIcons();
        float interval = 0.7f;

        var pool = GameObjectPoolManager.Instance;
        var dur_list = Hero.GetSkillManager().GetDurationSkillDataList();
        int cnt = dur_list.Count;
        int col = 0;
        int row = 0;
        float x, y;
        for (int i = 0; i < cnt; i++)
        {
            col = i % 5;
            row = i / 5;
            x = col * interval;
            y = row * interval;

            var obj = pool.GetGameObject("Assets/AssetResources/Prefabs/UI/Battle/EnemyDurationSkillIconNode", Duration_Icon_Container);
            obj.transform.localPosition = new Vector3(x, y, 0);
            var node = obj.GetComponent<EnemyDurationSkillIconNode>();
            node.SetBattleDurationSkillData(dur_list[i]);
            Used_Duration_Skill_Icons.Add(node);
        }
    }

    void ClearDurationSkillIcons()
    {
        var pool = GameObjectPoolManager.Instance;
        int cnt = Used_Duration_Skill_Icons.Count;
        for (int i = 0; i < cnt; i++)
        {
            pool.UnusedGameObject(Used_Duration_Skill_Icons[i].gameObject);
        }
        Used_Duration_Skill_Icons.Clear();
    }

    public void SetTargetTransform(Transform t)
    {
        Target_Transform = t;
        UpdatePosition();
    }

    void Update()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {
        if (Slider_Flush_Coroutine != null)
        {
            return;
        }
        if (Target_Transform != null)
        {
            Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, Target_Transform.position);
            This_Rect.anchoredPosition3D = pos;
        }
    }

    public void SetLifeSliderPercent(float percent, bool show)
    {
        float p = Mathf.Clamp01(percent);
        if (Use_Slider)
        {
            Main_Life_Bar.value = p;
        }
        else
        {
            Main_Life_Image_Bar.fillAmount = p;
            float bar_width = 168;
            var pos = Main_Life_Image_Bar_Knob.transform.localPosition;
            pos.x = bar_width * p;
            Main_Life_Image_Bar_Knob.transform.localPosition = pos;
        }

        if (show)
        {
            ShowLifeBar(2f);
            FlushSubSlider();
        }
        else
        {
            HideLifeBar();
            HideFlushSubSlider();
        }
    }

    

    /// <summary>
    /// 숨겨진 상태에서는 코루틴이 안도니깐, 그냥 업데이트
    /// </summary>
    void HideFlushSubSlider()
    {
        if (Use_Slider)
        {
            Sub_Life_Bar.value = Main_Life_Bar.value;
        }
        else
        {
            Sub_Life_Image_Bar.fillAmount = Main_Life_Image_Bar.fillAmount;
        }
   }

    public void FlushSubSlider()
    {
        if (Slider_Flush_Coroutine != null)
        {
            StopCoroutine(Slider_Flush_Coroutine);
        }
        Slider_Flush_Coroutine = StartCoroutine(SubSliderFlush());
    }
    /// <summary>
    /// 서브 체력 슬라이더 플러쉬
    /// </summary>
    /// <returns></returns>
    IEnumerator SubSliderFlush()
    {
        if (Use_Slider)
        {
            Main_Life_Bar_Knob.gameObject.SetActive(true);
            float life_rate = Main_Life_Bar.value;

            float time = 0f;
            const float duration = 1f;
            while (time < duration)
            {
                time += Time.deltaTime;
                Sub_Life_Bar.value = Mathf.Lerp(Sub_Life_Bar.value, life_rate, time / duration);
                yield return null;
            }
            Slider_Flush_Coroutine = null;
            Main_Life_Bar_Knob.gameObject.SetActive(false);
        }
        else
        {
            Main_Life_Image_Bar_Knob.gameObject.SetActive(true);
            float life_rate = Main_Life_Image_Bar.fillAmount;

            float time = 0f;
            const float duration = 1f;
            while (time < duration)
            {
                time += Time.deltaTime;
                Sub_Life_Image_Bar.fillAmount = Mathf.Lerp(Sub_Life_Image_Bar.fillAmount, life_rate, time / duration);
                yield return null;
            }
            Slider_Flush_Coroutine = null;
            Main_Life_Image_Bar_Knob.gameObject.SetActive(false);
        }
        
    }

   

    public void ShowLifeBar(float duration)
    {
        if (Show_Coroutine != null)
        {
            StopCoroutine(Show_Coroutine);
        }
        Show_Coroutine = StartCoroutine(StartShowLifeBar(duration));
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
        if (Use_Slider)
        {
            Main_Life_Bar.value = 1f;
            Sub_Life_Bar.value = 1f;
        }
        else
        {
            Main_Life_Image_Bar.fillAmount = 1f;
            Sub_Life_Image_Bar.fillAmount = 1f;
            var pos = Main_Life_Image_Bar_Knob.transform.localPosition;
            pos.x = 168;
            Main_Life_Image_Bar_Knob.transform.localPosition = pos;
        }

        if (This_Rect == null)
        {
            This_Rect = GetComponent<RectTransform>();
        }
        HideLifeBar();
    }

    public void Despawned()
    {
        ClearDurationSkillIcons();
        Target_Transform = null;
        if (Slider_Flush_Coroutine != null)
        {
            StopCoroutine(Slider_Flush_Coroutine);
        }
        Slider_Flush_Coroutine = null;
        HideLifeBar();
    }



}
