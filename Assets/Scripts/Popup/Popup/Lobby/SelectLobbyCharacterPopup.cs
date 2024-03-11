using Cysharp.Text;
using FluffyDuck.UI;
using Gpm.Ui;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectLobbyCharacterPopup : PopupBase
{
    [SerializeField, Tooltip("Popup Title")]
    TMP_Text Popup_Title;

    [SerializeField, Tooltip("Memorial Own Check")]
    Toggle Memorial_Own_Check;

    [SerializeField, Tooltip("Select Count")]
    TMP_Text Select_Count;

    [SerializeField, Tooltip("Character List View")]
    InfiniteScroll Character_List_View;

    List<UserL2dData>  Character_List;
    int Current_Choice_Count = 0;

    protected override void ShowPopupAniEndCallback()
    {
        FixedUpdatePopup();
        UpdatePopup();
    }

    protected override void HidePopupAniEndCallback()
    {
        Character_List_View.Clear();
        base.HidePopupAniEndCallback();
    }

    protected override void FixedUpdatePopup()
    {
        Character_List_View.Clear();

        const int column_count = 5;

        var l2d_mng = GameData.I.GetUserL2DDataManager();
        Character_List = l2d_mng.GetCloneUserL2dDataList();

        //  선택된 순서에 따라 우선 보여준다.
        // Character_List = Character_List.OrderBy(x => x.Skin_Id).OrderBy(x => x.Lobby_Choice_Number).OrderByDescending(x => x.Is_Choice_Lobby).ToList();

        //  리스트 호출을 위한 데이터 분배
        int start = 0;
        int row = Character_List.Count / column_count;
        if (Character_List.Count % column_count > 0)
        {
            row += 1;
        }
        for (int r = 0; r < row; r++)
        {
            start = r * column_count;

            var new_data = new LobbyCharacterListData();
            new_data.Click_Char_Callback = SelectCharacterCallback;
            if (start + column_count < Character_List.Count)
            {
                new_data.SetUserL2dDataList(Character_List.GetRange(start, column_count));
            }
            else
            {
                new_data.SetUserL2dDataList(Character_List.GetRange(start, Character_List.Count - start));
            }
            Character_List_View.InsertData(new_data);
        }
    }

    public override void UpdatePopup()
    {
        Current_Choice_Count = Character_List.OrderByDescending(x => x.Lobby_Choice_Number).Select(x => x.Lobby_Choice_Number).ToList()[0];
        Select_Count.text = ZString.Format("선택 중   <color=#67afff>{0} / {1}</color>", Current_Choice_Count, GameDefine.MAX_LOBBY_CHARACTER_COUNT);
    }

    /// <summary>
    /// 메모리얼 캐릭터 선택시 콜백
    /// </summary>
    /// <param name="ud"></param>
    void SelectCharacterCallback(UserL2dData ud)
    {
        if (ud.Is_Choice_Lobby)
        {
            if (Current_Choice_Count == GameDefine.MIN_LOBBY_CHARACTER_COUNT)
            {
                PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.NOTI_TYPE, (popup) =>
                {
                    popup.ShowPopup(3f, ConstString.Message.MIN);
                });
                return;
            }


            int canceled_lobby_num = ud.Lobby_Choice_Number;
            ud.ReleaseLobbyChoice();

            foreach (var data in Character_List)
            {
                if (data.Lobby_Choice_Number > canceled_lobby_num)
                {
                    data.SetLobbyChoiceNumber(data.Lobby_Choice_Number - 1);
                }
            }
        }
        else
        {
            if (Current_Choice_Count == GameDefine.MAX_LOBBY_CHARACTER_COUNT)
            {
                PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.NOTI_TYPE, (popup) =>
                {
                    popup.ShowPopup(3f, ConstString.Message.MAX);
                });
                return;
            }

            ud.SetLobbyChoiceNumber(Current_Choice_Count + 1);
        }

        Character_List_View.UpdateAllData();
        UpdatePopup();
    }

    public override void HidePopup(System.Action cb = null)
    {
        if (cb == null)
        {
            base.HidePopup(RollbackChoiceData);
        }
        else
        {
            base.HidePopup(cb);
        }

    }

    /// <summary>
    /// 로비 메모리얼 순서 롤백(취소)
    /// </summary>
    void RollbackChoiceData()
    {
    }

    /// <summary>
    /// 로비 메모리얼 순서 확정
    /// </summary>
    void ConfirmChoiceData()
    {
        var selected_list = Character_List.FindAll(x => x.Is_Choice_Lobby).OrderBy(x => x.Lobby_Choice_Number).ToList();

        GameData.I.GetUserL2DDataManager().SetLobbyChoiceOrder(selected_list);
        GameData.I.GetUserL2DDataManager().Save();

        Closed_Delegate?.Invoke("SelectLobbyCharacterPopup", selected_list);
    }

    /// <summary>
    /// 취소 버튼 이벤트
    /// </summary>
    public void OnClickCancel()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        //  rollback todo
        HidePopup(RollbackChoiceData);
    }
    /// <summary>
    /// 확인 버튼 이벤트
    /// </summary>
    public void OnClickConfirm()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        //  confirm todo
        ConfirmChoiceData();
        HidePopup();
    }

    public void OnClickToggleCheck()
    {

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
}
