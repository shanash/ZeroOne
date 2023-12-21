using FluffyDuck.UI;
using FluffyDuck.Util;
using Gpm.Ui;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SelectLobbyCharacterPopup : PopupBase
{
    [SerializeField, Tooltip("Popup Title")]
    TMP_Text Popup_Title;

    [SerializeField, Tooltip("Select Count")]
    TMP_Text Select_Count;

    [SerializeField, Tooltip("Character List View")]
    InfiniteScroll Character_List_View;

    public override void ShowPopup(params object[] data)
    {
        base.ShowPopup(data);

        //List<string> prefab_list = new List<string>();
        //prefab_list.Add("Assets/AssetResources/Prefabs/Popup/Popup/Lobby/LobbyCharacterCell");
        //GameObjectPoolManager.Instance.PreloadGameObjectPrefabsAsync(prefab_list, null);
    }

    protected override void ShowPopupAniEndCallback()
    {
        FixedUpdatePopup();
    }

    protected override void FixedUpdatePopup()
    {
        Character_List_View.Clear();

        const int column_count = 5;

        var hero_mng = GameData.Instance.GetUserHeroDataManager();
        List<UserHeroData> hero_list = new List<UserHeroData>();
        List<UserHeroData> temp_list = new List<UserHeroData>();
        hero_mng.GetUserHeroDataList(ref hero_list);

        hero_list.Sort((a, b) => b.Is_Choice_Lobby.CompareTo(a.Is_Choice_Lobby));
        hero_list.Sort((a, b) => a.Lobby_Choice_Num.CompareTo(b.Lobby_Choice_Num));

        int start = 0;
        int row = hero_list.Count / column_count;
        if (hero_list.Count % column_count > 0)
        {
            row += 1;
        }
        for (int r = 0; r < row; r++)
        {
            start = r * column_count;

            var new_data = new LobbyCharacterListData();
            new_data.SetClickCallback(SelectCharacterCallback);
            if (start + column_count < hero_list.Count)
            {
                new_data.SetUserHeroDataList(hero_list.GetRange(start, column_count));
            }
            else
            {
                new_data.SetUserHeroDataList(hero_list.GetRange(start, hero_list.Count - start));
            }
            Character_List_View.InsertData(new_data);
        }

    }

    void SelectCharacterCallback(UserHeroData ud)
    {
        Debug.Log(ud.ToString());
    }


    public void OnClickCancel()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        //  rollback todo
        HidePopup();
    }

    public void OnClickConfirm()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        //  confirm todo
        HidePopup();
    }
}
