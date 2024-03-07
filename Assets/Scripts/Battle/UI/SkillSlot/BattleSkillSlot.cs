using FluffyDuck.UI;
using FluffyDuck.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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

public class BattleSkillSlot : UIBase, IUpdateComponent, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField, Tooltip("Box")]
    RectTransform Box_Rect;

    [SerializeField, Tooltip("Hero Card")]
    HeroCardBase Card;

    [SerializeField, Tooltip("Buff Icon Container")]
    RectTransform Buff_Icon_Container;

    [SerializeField, Tooltip("Skill Ready")]
    RectTransform Skill_Ready;

    [SerializeField, Tooltip("Life Bar")]
    Image Life_Bar_Gauge;

    [SerializeField, Tooltip("Cooltime Gauge")]
    Image Cooltime_Gauge;

    [SerializeField, Tooltip("Cooltime Text")]
    TMP_Text Cooltime_Text;

    [SerializeField, Tooltip("Death")]
    RectTransform Death_Box;

    [SerializeField, Tooltip("Shaker")]
    Shake2D Shake;

    HeroBase_V2 Hero;

    List<BattleDurationSkillIconNode> Used_Duration_Skill_Icons = new List<BattleDurationSkillIconNode>();

    Vector2 Tooltip_Upper_Pos = new Vector2(0, 280f);

    Vector2 Press_Scale = new Vector2(0.96f, 0.96f);
    public Action<Rect, UserHeroSkillData> OnStartLongPress;
    public Action OnFinishLongPress;
    Coroutine CheckForLongPress = null;

    public void SetHeroBase(HeroBase_V2 hero)
    {
        Hero = hero;
        Hero.Slot_Events += SkillSlotEventCallback;

        UpdateSkillSlot();
    }

    void UpdateSkillSlot()
    {
        //Card.SetHeroDataID(User_Data.GetPlayerCharacterID());
        Card.SetHeroDataID(Hero.GetBattleUnitData().GetUnitID());

        UpdateLifeBar();
    }


    /// <summary>
    /// 아이콘 터치 이벤트.<br/>
    /// Click / Long Touch 이벤트 사용
    /// </summary>
    /// <param name="result"></param>
    void TouchEventCallback(TOUCH_RESULT_TYPE result)
    {
        if (result == TOUCH_RESULT_TYPE.CLICK)
        {
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
        }
        /*
        else if (result == TOUCH_RESULT_TYPE.LONG_PRESS)
        {
            AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
            //  show skill tooltip
            Vector2 pos = this.transform.position;
            pos += Tooltip_Upper_Pos;

            PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Party/SkillInfoTooltipPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
            {
                popup.ShowPopup(0, pos);
            });
        }
        */
    }

    /// <summary>
    /// 영웅 SD에서 스킬 슬롯에 이벤트를 전달할 경우 사용
    /// </summary>
    /// <param name="etype"></param>
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
            case SKILL_SLOT_EVENT_TYPE.DEATH:
                UpdateDeath();
                break;
            case SKILL_SLOT_EVENT_TYPE.COOLTIME_INIT:
                UpdateCooltime();
                break;
            default:
                Debug.Assert(false);
                break;
        }
    }

    #region Slot Event Funcs

    void UpdateHitted()
    {
        Shake.Shake(0.3f, 10f, null);
    }
    void UpdateLifeBar()
    {
        if (Hero == null)
        {
            return;
        }
        float per = (float)(Hero.Life / Hero.Max_Life);
        Life_Bar_Gauge.fillAmount = Mathf.Clamp01(per);
    }
    void UpdateDurationSkillIcons()
    {
        if (Hero == null)
        {
            return;
        }
        //  todo
        ClearDurationSkillIcons();
        var pool = GameObjectPoolManager.Instance;
        var dur_list = Hero.GetSkillManager().GetDurationSkillDataList();
        int cnt = dur_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var obj = pool.GetGameObject("Assets/AssetResources/Prefabs/UI/Battle/BattleDurationSkillIconNode", Buff_Icon_Container);
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

    void UpdateDeath()
    {
        Death_Box.gameObject.SetActive(true);
        Cooltime_Gauge.gameObject.SetActive(false);
        Skill_Ready.gameObject.SetActive(false);
        UpdateLifeBar();
    }

    void UpdateCooltime()
    {
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
        Cooltime_Text.text = remain_time.ToString("N0");
    }

    #endregion

    private void OnEnable()
    {
        if (Card != null)
        {
            Card.AddTouchEventCallback(TouchEventCallback);
        }
    }

    private void OnDisable()
    {
        if (Card != null)
        {
            Card.RemoveTouchEventCallback(TouchEventCallback);
        }
        if (Hero != null)
        {
            Hero.Slot_Events -= SkillSlotEventCallback;
        }
    }

    public override void Spawned()
    {
        base.Spawned();
        Death_Box.gameObject.SetActive(false);
        Skill_Ready.gameObject.SetActive(false);
        Cooltime_Gauge.gameObject.SetActive(false);

        CustomUpdateManager.Instance.RegistCustomUpdateComponent(this);
    }

    public override void Despawned()
    {
        base.Despawned();
        CustomUpdateManager.Instance.DeregistCustomUpdateComponent(this);
    }

    public void OnUpdate(float dt)
    {
        if (Hero == null)
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

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
        if (OnStartLongPress != null)
        {
            Box_Rect.localScale = Press_Scale;
            StopCoroutine(ref CheckForLongPress);
            CheckForLongPress = StartCoroutine(CoCheckForLongPress());
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp");
        if (OnFinishLongPress != null)
        {
            Box_Rect.localScale = Vector2.one;

            bool is_stopped = StopCoroutine(ref CheckForLongPress);

            // 제대로 OnStartLongPress가 실행되었어야지
            // (CheckForLongPress == null) (is_stopped == false)
            // OnFinishLongPress을 호출
            if (!is_stopped)
            {
                OnFinishLongPress?.Invoke();
            }
        }
    }

    IEnumerator CoCheckForLongPress()
    {
        float elapsed_time = 0;
        while (Tooltip.PRESS_TIME_FOR_SHOW > elapsed_time)
        {
            yield return null;
            elapsed_time += Time.deltaTime;
        }

        var rt = this.GetComponent<RectTransform>();
        var skill_group = Hero.GetSkillManager().GetSpecialSkillGroup() as BattlePcSkillGroup;
        OnStartLongPress?.Invoke(GameObjectUtils.GetScreenRect(rt), skill_group.GetUserHeroSkillData());
        CheckForLongPress = null;
    }

    bool StopCoroutine(ref Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
            return true;
        }

        return false;
    }
}
