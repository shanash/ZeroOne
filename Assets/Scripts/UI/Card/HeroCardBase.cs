using FluffyDuck.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroCardBase : MonoBehaviour, IPoolableComponent
{
    [SerializeField, Tooltip("Card BG")]
    protected Image Card_BG;

    [SerializeField, Tooltip("Hero Icon Image")]
    protected Image Hero_Icon_Image;

    [SerializeField, Tooltip("Card Frame")]
    protected Image Card_Frame;

    [SerializeField, Tooltip("Position Icon")]
    protected Image Position_Icon_Image;

    [SerializeField, Tooltip("Distance Text")]
    protected TMP_Text Distance_Text;

    protected Player_Character_Data Data;
    protected Player_Character_Battle_Data Battle_Data;

    public void SetHeroDataID(int hero_data_id)
    {
        var m = MasterDataManager.Instance;
        Data = m.Get_PlayerCharacterData(hero_data_id);
        Battle_Data = m.Get_PlayerCharacterBattleData(Data.battle_info_id);
        UpdateCardBase();
    }

    void UpdateCardBase()
    {
        if (Data == null)
        {
            return;
        }

        //  icon
        CommonUtils.GetResourceFromAddressableAsset<Sprite>(Data.icon_path, (spr) => 
        { 
            Hero_Icon_Image.sprite = spr;
        });

        //  position icon
        var pos_data = MasterDataManager.Instance.Get_PositionIconData(Battle_Data.position_type);
        CommonUtils.GetResourceFromAddressableAsset<Sprite>(pos_data.icon, (spr) =>
        {
            Position_Icon_Image.sprite = spr;
        });

        //  distance
        Distance_Text.text = Battle_Data.distance.ToString();

    }

    public virtual void Despawned()
    {

    }

    public virtual void Spawned()
    {

    }
}
