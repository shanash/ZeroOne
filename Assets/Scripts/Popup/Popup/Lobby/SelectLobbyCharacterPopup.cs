using Cysharp.Text;
using FluffyDuck.UI;
using Gpm.Ui;
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
        var memorial_mng = GameData.Instance.GetUserMemorialDataManager();
        List<UserMemorialData> memorial_list = new List<UserMemorialData>();

        //  메모리얼 리스트에 임시 순서를 지정해준다.
        memorial_mng.ReadyTempLobbyChoiceNumber();
        //  메모리얼 리스트 가져오기
        memorial_mng.GetUserMemorialDataList(ref memorial_list);

        //  선택된 순서에 따라 우선 보여준다.
        memorial_list = memorial_list.OrderBy(x => x.Memorial_ID).OrderBy(x => x.Temp_Lobby_Choice_Number).OrderByDescending(x => x.Is_Temp_Choice).ToList();

        //  리스트 호출을 위한 데이터 분배
        int start = 0;
        int row = memorial_list.Count / column_count;
        if (memorial_list.Count % column_count > 0)
        {
            row += 1;
        }
        for (int r = 0; r < row; r++)
        {
            start = r * column_count;

            var new_data = new LobbyCharacterListData();
            new_data.Click_Memorial_Callback = SelectCharacterCallback;
            if (start + column_count < memorial_list.Count)
            {
                new_data.SetUserMemorialDataList(memorial_list.GetRange(start, column_count));
            }
            else
            {
                new_data.SetUserMemorialDataList(memorial_list.GetRange(start, memorial_list.Count - start));
            }
            Character_List_View.InsertData(new_data);
        }

    }

    public override void UpdatePopup()
    {
        var memorial_mng = GameData.Instance.GetUserMemorialDataManager();
        var choice_list = memorial_mng.GetUserMemorialDataListbyTempChoice();
        Select_Count.text = ZString.Format("선택 중   <color=#67afff>{0} / {1}</color>", choice_list.Count, GameDefine.MAX_LOBBY_CHARACTER_COUNT);
    }
    /// <summary>
    /// 메모리얼 캐릭터 선택시 콜백
    /// </summary>
    /// <param name="ud"></param>
    void SelectCharacterCallback(UserMemorialData ud)
    {
        var memorial_mng = GameData.Instance.GetUserMemorialDataManager();
        var result_code = memorial_mng.SelectTempMemorialOrder(ud.Memorial_ID, ud.Player_Character_ID);
        if (result_code == ERROR_CODE.SUCCESS)
        {
            Character_List_View.UpdateAllData();
            UpdatePopup();
        }
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
        var memorial_mng = GameData.Instance.GetUserMemorialDataManager();
        memorial_mng.RollbackLobbyChoiceOrder();
    }
    /// <summary>
    /// 로비 메모리얼 순서 확정
    /// </summary>
    void ConfirmChoiceData()
    {
        var memorial_mng = GameData.Instance.GetUserMemorialDataManager();
        memorial_mng.ConfirmLobbyChoiceOrder();
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
        HidePopup(ConfirmChoiceData);
    }

    public void OnClickToggleCheck()
    {

    }
}
