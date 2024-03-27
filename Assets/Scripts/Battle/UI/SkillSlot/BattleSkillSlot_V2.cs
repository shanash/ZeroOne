using FluffyDuck.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SKILL_SLOT_EVENT_TYPE
{
    NONE = 0,

    HITTED,                     //  피격
    LIFE_UPDATE,                //  체력 게이지 업데이트
    DURATION_SKILL_ICON_UPDATE, //  지속성 효과 아이콘 업데이트
    COOLTIME_INIT,              //  쿨타임 최초 업데이트

    DEATH,                      //  죽었을 때

}


public class BattleSkillSlot_V2 : UIBase, IUpdateComponent
{
    [SerializeField, Tooltip("Box")]
    RectTransform Box;

    [Space()]
    [Header("Character")]
    [SerializeField, Tooltip("Hero Card")]
    UIInteractiveButton Hero_Slot_Btn;

    [SerializeField, Tooltip("Hero Icon")]
    Image Hero_Icon;


    [Space()]
    [Header("Skill")]
    [SerializeField, Tooltip("Skill Ready")]
    Animator Skill_Ready;

    [SerializeField, Tooltip("Cooltime Gauge")]
    Image Cooltime_Gauge;
    [SerializeField, Tooltip("Cooltime Count")]
    TMP_Text Cooltime_Count;

    [Space()]
    [Header("Life Bar")]
    [SerializeField, Tooltip("Main Life Bar")]
    Slider Main_Life_Bar;

    [SerializeField, Tooltip("Sub Life Bar")]
    Slider Sub_Life_Bar;

    [SerializeField, Tooltip("Main Life Bar Knob")]
    Image Main_Life_Bar_Knob;


    [Space()]
    [Header("Duration Icon")]
    [SerializeField, Tooltip("Duration Icon Container")]
    RectTransform Duration_Icon_Container;

    [Space()]
    [Header("Shake")]
    [SerializeField, Tooltip("Shaker")]
    Shake2D Shaker;

    HeroBase_V2 Hero;
    List<BattleDurationSkillIconNode> Used_Duration_Skill_Icons = new List<BattleDurationSkillIconNode>();

    Coroutine Flush_Coroutine;

    public void SetHeroBase(HeroBase_V2 hero)
    {
        this.Hero = hero;
        this.Hero.Slot_Events += SkillSlotEventCallback;

        var skill_group = hero.GetSkillManager().GetSpecialSkillGroup();
        if (skill_group == null)
        {
            return;
        }
        if (Hero_Slot_Btn != null)
        {
            Hero_Slot_Btn.Tooltip_Data = (skill_group as BattlePcSkillGroup).GetUserHeroSkillData();
        }
        InitUpdateSkillSlot();
    }
    
    void InitUpdateSkillSlot()
    {
        //  icon
        CommonUtils.GetResourceFromAddressableAsset<Sprite>(Hero.GetBattleUnitData().GetIconPath(), (spr) =>
        {
            Hero_Icon.sprite = spr;
        });
        //  life bar init
        Main_Life_Bar.value = 1f;
        Sub_Life_Bar.value = 1f;
        Main_Life_Bar_Knob.gameObject.SetActive(false);
    }

    void SkillSlotEventCallback(SKILL_SLOT_EVENT_TYPE etype)
    {
        switch (etype)
        {
            case SKILL_SLOT_EVENT_TYPE.HITTED:
                UpdateHitted();
                break;
            case SKILL_SLOT_EVENT_TYPE.LIFE_UPDATE:
                UpdateLifeBar();
                break;
            case SKILL_SLOT_EVENT_TYPE.DURATION_SKILL_ICON_UPDATE:
                UpdateDurationSkillIcons();
                break;
            case SKILL_SLOT_EVENT_TYPE.COOLTIME_INIT:
                UpdateCooltime();
                break;
            case SKILL_SLOT_EVENT_TYPE.DEATH:
                Hero_Slot_Btn.interactable = false;
                HideSkillReadyAndCooltime();
                UpdateLifeBar();
                UpdateDurationSkillIcons();
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }
    
    void UpdateHitted() 
    {
        Shaker.Shake(0.3f, 10f, null);
    }
    void UpdateLifeBar()
    {
        if (Hero == null)
        {
            return;
        }
        float per = (float)(Hero.Life / Hero.Max_Life);
        Main_Life_Bar.value = Mathf.Clamp01(per);
        FlushSubLifeBar();
    }

    /// <summary>
    /// 서브바 플러쉬
    /// </summary>
    void FlushSubLifeBar()
    {
        if (Flush_Coroutine != null)
        {
            StopCoroutine(Flush_Coroutine);
        }

        if (this.isActiveAndEnabled)
        {
            Flush_Coroutine = StartCoroutine(SubSliderFlush());
        }
        else
        {
            Sub_Life_Bar.value = Main_Life_Bar.value;
        }
    }

    IEnumerator SubSliderFlush()
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

        Flush_Coroutine = null;
        Main_Life_Bar_Knob.gameObject.SetActive(false);
    }

    void UpdateDurationSkillIcons()
    {
        if (Hero == null)
        {
            return;
        }
        ClearDurationSkillIcons();
        var pool = GameObjectPoolManager.Instance;
        var dur_list = Hero.GetSkillManager().GetDurationSkillDataList();
        int cnt = dur_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var obj = pool.GetGameObject("Assets/AssetResources/Prefabs/UI/Battle/BattleDurationSkillIconNode", Duration_Icon_Container);
            obj.transform.localPosition = Vector3.zero;
            var node = obj.GetComponent<BattleDurationSkillIconNode>();
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

    void UpdateCooltime()
    {
        if (Hero == null)
        {
            HideSkillReadyAndCooltime();
            return;
        }
        var skill_group = Hero.GetSkillManager().GetSpecialSkillGroup();
        if (skill_group == null)
        {
            return;
        }

        if (skill_group.IsPrepareCooltime())
        {
            if (!Skill_Ready.gameObject.activeSelf)
            {
                Skill_Ready.gameObject.SetActive(true);
            }
            if (Cooltime_Gauge.gameObject.activeSelf)
            {
                Cooltime_Gauge.gameObject.SetActive(false);
            }
            return;
        }
        if (Skill_Ready.gameObject.activeSelf)
        {
            Skill_Ready.gameObject.SetActive(false);
        }
        if (!Cooltime_Gauge.gameObject.activeSelf)
        {
            Cooltime_Gauge.gameObject.SetActive(true);
        }
        float cooltime = (float)skill_group.GetCooltime();
        float remain_time = (float)skill_group.GetRemainCooltime();
        float per = Mathf.Clamp01(remain_time / cooltime);
        Cooltime_Gauge.fillAmount = (float)per;
        Cooltime_Count.text = remain_time.ToString("N0");
    }

    void HideSkillReadyAndCooltime()
    {
        Skill_Ready.gameObject.SetActive(false);
        Cooltime_Gauge.gameObject.SetActive(false);
    }

    public void TouchEventCallback(TOUCH_RESULT_TYPE result, Func<bool, Rect> hole, object data)
    {
        switch (result)
        {
            case TOUCH_RESULT_TYPE.CLICK:
                AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
                if (!Hero.IsAlive())
                {
                    return;
                }
                var skill_group = Hero.GetSkillManager().GetSpecialSkillGroup();
                if (skill_group == null)
                {
                    return;
                }
                if (skill_group.IsPrepareCooltime())
                {
                    Hero?.UltimateSkillExec();
                }
                break;
            case TOUCH_RESULT_TYPE.LONG_PRESS:
                AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
                UserHeroSkillData skill_data = null;
                if (data == null || data is not UserHeroSkillData)
                {
                    Debug.Assert(false);
                }
                else
                {
                    skill_data = data as UserHeroSkillData;
                }
                TooltipManager.I.Add("Assets/AssetResources/Prefabs/UI/SkillTooltip", hole(false), skill_data);
                break;
        }
    }

    public void OnUpdate(float dt)
    {
        if (Hero == null)
        {
            return;
        }
        if (!Hero.IsAlive())
        {
            return;
        }
        var state = Hero.GetCurrentState();
        if (state < UNIT_STATES.IDLE)
        {
            return;
        }
        UpdateCooltime();
    }

    public override void Spawned()
    {
        Main_Life_Bar.value = 1f;
        Sub_Life_Bar.value = 1f;
        Main_Life_Bar_Knob.gameObject.SetActive(false);

        Skill_Ready.gameObject.SetActive(false);
        Cooltime_Gauge.gameObject.SetActive(false);

        Hero_Slot_Btn.Touch_Tooltip_Callback.RemoveAllListeners();
        Hero_Slot_Btn.Touch_Tooltip_Callback.AddListener(TouchEventCallback);

        CustomUpdateManager.Instance.RegistCustomUpdateComponent(this);
    }

    public override void Despawned()
    {
        ClearDurationSkillIcons();
        if (Flush_Coroutine != null)
        {
            StopCoroutine(Flush_Coroutine);
        }
        Flush_Coroutine = null;
        Main_Life_Bar_Knob.gameObject.SetActive(false);

        CustomUpdateManager.Instance.DeregistCustomUpdateComponent(this);
    }

    
}
