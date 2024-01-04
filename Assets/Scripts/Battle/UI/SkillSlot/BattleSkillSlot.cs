using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleSkillSlot : UIBase, IUpdateComponent
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

    Vector2 Press_Scale = new Vector2(0.96f, 0.96f);

    UserHeroData User_Data;
    HeroBase_V2 Hero;

    public void SetPlayerCharacterData(int player_character_id, int player_character_num)
    {
        User_Data = GameData.Instance.GetUserHeroDataManager().FindUserHeroData(player_character_id, player_character_num);

        UpdateSkillSlot();
    }

    public void SetHeroBase(HeroBase_V2 hero)
    {
        Hero = hero;
    }


    /// <summary>
    /// 캐릭터 ID/Num 비교
    /// </summary>
    /// <param name="pc_id"></param>
    /// <param name="pc_num"></param>
    /// <returns></returns>
    public bool IsEqualPlayerCharacter(int pc_id, int pc_num)
    {
        if (User_Data != null)
        {
            return User_Data.GetPlayerCharacterID() == pc_id && User_Data.Player_Character_Num == pc_num;
        }
        return false;
    }

    void UpdateSkillSlot()
    {
        Card.SetHeroDataID(User_Data.GetPlayerCharacterID());
    }



    void TouchDownCallback(PointerEventData evt)
    {
        Card.transform.localScale = Press_Scale;
    }

    void TouchUpCallback(PointerEventData evt)
    {
        Card.transform.localScale = Vector2.one;
    }
    void ClickCallback(PointerEventData evt)
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
    }

    private void OnEnable()
    {
        if (Card != null)
        {
            Card.TouchDown += TouchDownCallback;
            Card.TouchUp += TouchUpCallback;
            Card.Click += ClickCallback;
        }
    }

    private void OnDisable()
    {
        if (Card != null)
        {
            Card.TouchDown -= TouchDownCallback;
            Card.TouchUp -= TouchUpCallback;
            Card.Click -= ClickCallback;
        }
    }

    public override void Spawned()
    {
        base.Spawned();
        CustomUpdateManager.Instance.RegistCustomUpdateComponent(this);
    }

    public override void Despawned()
    {
        base.Despawned();
        
        CustomUpdateManager.Instance.DeregistCustomUpdateComponent(this);
        Hero = null;
    }

    public void OnUpdate(float dt)
    {
        if (Hero == null)
        {
            return;
        }
        
    }
}
