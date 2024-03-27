using FluffyDuck.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBarNode : MonoBehaviour, IPoolableComponent, IUpdateComponent
{
    [SerializeField, Tooltip("Box")]
    protected RectTransform Box;

    [Space()]
    [Header("Slider Version")]
    [SerializeField, Tooltip("Main Life Bar")]
    protected Slider Main_Life_Bar;

    [SerializeField, Tooltip("Sub Life Bar")]
    protected Slider Sub_Life_Bar;

    [SerializeField, Tooltip("Main Life Bar Knob")]
    protected Image Main_Life_Bar_Knob;

    [Space()]
    [Header("지속성 아이콘")]
    [SerializeField, Tooltip("Duration Icon Container")]
    protected RectTransform Duration_Icon_Container;

    protected RectTransform This_Rect;

    protected Transform Target_Transform;

    //  image version
    protected Coroutine Show_Coroutine;

    //  slider version
    protected Coroutine Slider_Flush_Coroutine;


    protected bool Is_Boss_Gauge;


    protected HeroBase_V2 Hero;
    /// <summary>
    /// 지속성 효과 아이콘 리스트
    /// </summary>
    protected List<EnemyDurationSkillIconNode> Used_Duration_Skill_Icons = new List<EnemyDurationSkillIconNode>();

    public virtual void SetHeroBaseV2(HeroBase_V2 hero)
    {
        this.Hero = hero;
        if (this.Hero.Team_Type == TEAM_TYPE.RIGHT)
        {
            this.Hero.Slot_Events += SkillSlotEventCallback;
        }
        SetTargetTransform(this.Hero.GetHPPositionTransform());
        Is_Boss_Gauge = false;
        
    }

    public bool IsBossGauge()
    {
        return Is_Boss_Gauge;
    }

    protected void SkillSlotEventCallback(SKILL_SLOT_EVENT_TYPE etype)
    {
        if (etype == SKILL_SLOT_EVENT_TYPE.DURATION_SKILL_ICON_UPDATE)
        {
            UpdateDurationSkillIcons();
        }
    }

    /// <summary>
    /// 지속성 효과 아이콘 추가
    /// </summary>
    protected void UpdateDurationSkillIcons()
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

    protected void ClearDurationSkillIcons()
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

    

    protected virtual void UpdatePosition()
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

    public virtual void SetLifeSliderPercent(float percent, bool show)
    {
        float p = Mathf.Clamp01(percent);
        Main_Life_Bar.value = p;

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
    protected void HideFlushSubSlider()
    {
        Sub_Life_Bar.value = Main_Life_Bar.value;
   }

    public void FlushSubSlider()
    {
        if (Slider_Flush_Coroutine != null)
        {
            StopCoroutine(Slider_Flush_Coroutine);
        }
        if (this.isActiveAndEnabled)
        {
            Slider_Flush_Coroutine = StartCoroutine(SubSliderFlush());
        }
        else
        {
            HideFlushSubSlider();
        }
    }
    /// <summary>
    /// 서브 체력 슬라이더 플러쉬
    /// </summary>
    /// <returns></returns>
    protected IEnumerator SubSliderFlush()
    {
        Main_Life_Bar_Knob.gameObject.SetActive(true);
        float life_rate = Main_Life_Bar.value;

        float time = 0f;
        const float duration = 0.5f;
        while (time < duration)
        {
            time += Time.deltaTime;
            Sub_Life_Bar.value = Mathf.Lerp(Sub_Life_Bar.value, life_rate, time / duration);
            yield return null;
        }
        Slider_Flush_Coroutine = null;
        Main_Life_Bar_Knob.gameObject.SetActive(false);
        
    }

    public virtual void ShowLifeBar(float duration)
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

    public virtual void HideLifeBar()
    {
        if (Show_Coroutine != null)
        {
            StopCoroutine(Show_Coroutine);
        }
        Show_Coroutine = null;
        Box.gameObject.SetActive(false);
    }

    public virtual void Spawned()
    {
        Main_Life_Bar.value = 1f;
        Sub_Life_Bar.value = 1f;

        if (This_Rect == null)
        {
            This_Rect = GetComponent<RectTransform>();
        }
        HideLifeBar();

        CustomUpdateManager.Instance.RegistCustomUpdateComponent(this);
    }

    public virtual void Despawned()
    {
        ClearDurationSkillIcons();
        Target_Transform = null;
        if (Slider_Flush_Coroutine != null)
        {
            StopCoroutine(Slider_Flush_Coroutine);
        }
        Slider_Flush_Coroutine = null;
        HideLifeBar();

        CustomUpdateManager.Instance.DeregistCustomUpdateComponent(this);
    }

    public void OnUpdate(float dt)
    {
        UpdatePosition();
    }
}
