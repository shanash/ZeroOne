using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCellBase : MonoBehaviour
{
    [SerializeField, Tooltip("Cell BG")]
    protected Image Cell_BG;

    [SerializeField, Tooltip("Hero Icon")]
    protected Image Hero_Icon;

    [SerializeField, Tooltip("Frame")]
    protected Image Frame_Image;


    protected Player_Character_Data Data;

    protected Player_Character_Battle_Data Battle_Data;

    public void SetPlayerCharacterID(int player_character_id)
    {
        var m = MasterDataManager.Instance;
        Data = m.Get_PlayerCharacterData(player_character_id);
        Battle_Data = m.Get_PlayerCharacterBattleData(Data.battle_info_id);

        UpdateIcon();
    }

    public Player_Character_Data GetPlayerCharacterData()
    {
        return Data;
    }
    public Player_Character_Battle_Data GetPlayerCharacterBattleData()
    {
        return Battle_Data;
    }

    protected void UpdateIcon()
    {
        CommonUtils.GetResourceFromAddressableAsset<Sprite>(Data.icon_path, (spr) => 
        {
            Hero_Icon.sprite = spr;
        });
    }
}
