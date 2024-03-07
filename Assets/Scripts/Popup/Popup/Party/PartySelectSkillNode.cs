using Cysharp.Text;
using FluffyDuck.UI;
using FluffyDuck.Util;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartySelectSkillNode : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField, Tooltip("Box")]
    RectTransform Box;

    [SerializeField, Tooltip("Skill Type")]
    SKILL_TYPE Skill_Type = SKILL_TYPE.NONE;

    [SerializeField, Tooltip("Skill Type Text")]
    TMP_Text Skill_Type_Text;

    [SerializeField, Tooltip("Skill Icon")]
    Image Skill_Icon;

    [SerializeField, Tooltip("Skill Level")]
    TMP_Text Skill_Level;

    Vector2 Press_Scale = new Vector2(0.96f, 0.96f);
    public Action<Rect, UserHeroSkillData> OnStartLongPress;
    public Action OnFinishLongPress;
    Coroutine CheckForLongPress = null;

    UserHeroData User_Data;
    UserHeroSkillData Skill_Data;

    void Start()
    {
        UpdateUI();
    }

    public void Initialize(UserHeroSkillData data)
    {
        Skill_Data = data;
        Skill_Type = Skill_Data.GetSkillType();
        UpdateSkillCard();
        UpdateUI();
    }

    public void SetPlayerCharacterID(int pc_id, int pc_num)
    {
        var gd = GameData.Instance;
        User_Data = gd.GetUserHeroDataManager().FindUserHeroData(pc_id, pc_num);

        var skill_list = User_Data.GetPlayerCharacterBattleData().skill_pattern;
        int skill_group_id = 0;
        if (Skill_Type == SKILL_TYPE.SKILL_01)
        {
            skill_group_id = skill_list[1];
        }
        else if (Skill_Type == SKILL_TYPE.SKILL_02)
        {
            skill_group_id = skill_list[2];
        }
        else if (Skill_Type == SKILL_TYPE.SPECIAL_SKILL)
        {
            skill_group_id = User_Data.GetPlayerCharacterBattleData().special_skill_group_id;
        }
        else
        {
            Debug.Assert(false);
        }

        if (skill_group_id != 0)
        {
            Skill_Data = gd.GetUserHeroSkillDataManager().FindUserHeroSkillData(pc_id, pc_num, skill_group_id);
            UpdateSkillCard();
        }
    }

    void UpdateUI()
    {
        // text
        if (Skill_Data != null)
        {
            Skill_Type_Text.text = Skill_Data.GetSkillTypeText();
        }
        else
        {
            switch (Skill_Type)
            {
                case SKILL_TYPE.SKILL_01:
                    Skill_Type_Text.text = "액티브 스킬 1";
                    break;
                case SKILL_TYPE.SKILL_02:
                    Skill_Type_Text.text = "액티브 스킬 2";
                    break;
                case SKILL_TYPE.SPECIAL_SKILL:
                    Skill_Type_Text.text = "궁극기";
                    break;
            }
        }
    }

    void UpdateSkillCard()
    {
        //  icon
        CommonUtils.GetResourceFromAddressableAsset<Sprite>(Skill_Data.GetSkillGroupData().icon, (spr) =>
        {
            Skill_Icon.sprite = spr;
        });
        //  level
        Skill_Level.text = ZString.Format("Lv.{0}", Skill_Data.GetLevel());
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (OnStartLongPress != null)
        {
            Box.localScale = Press_Scale;
            StopCoroutine(ref CheckForLongPress);
            CheckForLongPress = StartCoroutine(CoCheckForLongPress());
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (OnFinishLongPress != null)
        {
            Box.localScale = Vector2.one;

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
        OnStartLongPress?.Invoke(GameObjectUtils.GetScreenRect(rt), Skill_Data);
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
