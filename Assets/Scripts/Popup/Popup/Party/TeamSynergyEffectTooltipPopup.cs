using Cysharp.Text;
using FluffyDuck.UI;
using FluffyDuck.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TeamSynergyEffectTooltipPopup : PopupBase
{
    [SerializeField, Tooltip("Container")]
    RectTransform Container;

    GAME_TYPE Game_Type = GAME_TYPE.NONE;
    int Deck_Index;
    Vector2 Target_Position = Vector2.zero;

    List<TMP_Text> Used_Team_Synergy_Info_Text_List = new List<TMP_Text>();

    UserDeckData Deck_Data;

    protected override bool Initialize(object[] data)
    {
        if (data.Length != 3)
        {
            return false;
        }
        Game_Type = (GAME_TYPE)data[0];
        Deck_Index = (int)data[1];
        Target_Position = (Vector2)data[2];

        Deck_Data = GameData.Instance.GetUserHeroDeckMountDataManager().FindDeck(Game_Type, Deck_Index);

        InitAssets();
        return true;
    }

    void InitAssets()
    {
        List<string> asset_list = new List<string>();
        asset_list.Add("Assets/AssetResources/Prefabs/Popup/Popup/Party/Team_Synergy_Tooltip_Info_Text");

        GameObjectPoolManager.Instance.PreloadGameObjectPrefabsAsync(asset_list, PreloadCallback);
    }

    void PreloadCallback(int load_cnt, int total_cnt)
    {
        if (load_cnt == total_cnt)
        {
            FixedUpdatePopup();
            return;
        }
    }

    protected override void FixedUpdatePopup()
    {
        if (!Container.gameObject.activeSelf)
        {
            Container.gameObject.SetActive(true);
        }
        ClearTeamSynergyInfoList();
        //  팀 시너지
        var team_synergy_list = Deck_Data.GetTeamSynergyList();
        int cnt = team_synergy_list.Count;
        var pool = GameObjectPoolManager.Instance;
        if (cnt > 0)
        {
            for (int i = 0; i < cnt; i++)
            {
                var synergy = team_synergy_list[i];
                var obj = pool.GetGameObject("Assets/AssetResources/Prefabs/Popup/Popup/Party/Team_Synergy_Tooltip_Info_Text", Container);
                var synergy_info = obj.GetComponent<TMP_Text>();
                synergy_info.text = ZString.Format(GameDefine.GetLocalizeString(synergy.attribute_info), Math.Truncate(synergy.add_damage_per * 100));
                Used_Team_Synergy_Info_Text_List.Add(synergy_info);
            }
        }
        else
        {
            var obj = pool.GetGameObject("Assets/AssetResources/Prefabs/Popup/Popup/Party/Team_Synergy_Tooltip_Info_Text", Container);
            var synergy_info = obj.GetComponent<TMP_Text>();
            synergy_info.text = GameDefine.GetLocalizeString("system_attribute_synergy_none");
            Used_Team_Synergy_Info_Text_List.Add(synergy_info);
        }

        //  좌표 계산
        //float rect_half_width = Container.rect.width * 0.5f;
        var pos = Target_Position;
        //if (pos.x - rect_half_width < 0f)
        //{
        //    pos.x += (rect_half_width - pos.x) + 40f;
        //}
        pos.y += 30f;
        Container.transform.position = pos;
    }

    void ClearTeamSynergyInfoList()
    {
        var pool = GameObjectPoolManager.Instance;
        int cnt = Used_Team_Synergy_Info_Text_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            pool.UnusedGameObject(Used_Team_Synergy_Info_Text_List[i].gameObject);
        }
        Used_Team_Synergy_Info_Text_List.Clear();
    }

    public void OnClickDim()
    {
        if (Ease_Base != null && Ease_Base.IsPlaying())
        {
            return;
        }
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        HidePopup();
    }

    public override void Despawned()
    {
        base.Despawned();
        Container.gameObject.SetActive(false);
    }
}
