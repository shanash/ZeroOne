using FluffyDuck.UI;
using FluffyDuck.Util;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HeroInfoBoxAdvance : MonoBehaviour
{
    [SerializeField, Tooltip("캐릭터의 현재 성급 이미지")]
    GameObject[] Current_Star_Img = null;

    [SerializeField, Tooltip("캐릭터 성급 진화 후 성급 이미지")]
    GameObject[] Next_Star_Img = null;

    [SerializeField, Tooltip("캐릭터 조각 소유/필요 갯수 UI 텍스트")]
    TMP_Text Need_Piece_Text = null;

    [SerializeField, Tooltip("골드 필요 갯수 UI 텍스트")]
    TMP_Text Need_Gold_Text = null;

    [SerializeField, Tooltip("성급 진화 버튼")]
    UIButtonBase Advance_Button = null;

    [SerializeField, Tooltip("진화 버튼 활성화 스프라이트")]
    Sprite Enable_Button_Spr = null;

    [SerializeField, Tooltip("진화 버튼 비활성화 스프라이트")]
    Sprite Disable_Button_Spr = null;

    [SerializeField, Tooltip("진화Max시 알림 텍스트 오브젝트")]
    TMP_Text Cannot_Advance_Text = null;

    [SerializeField, Tooltip("진화Max시 비활성화 할 버튼 위 재화표시")]
    GameObject Need_Gold_Object = null;

    [SerializeField, Tooltip("진화Max시 비활성화 할 버튼 밑의 패널")]
    GameObject Need_Gold_BG_Panel = null;

    [SerializeField, Tooltip("진화Max시 비활성화 할 진화 등급 변경 화살표")]
    GameObject Next_StarGrade_Arrow = null;

    [SerializeField, Tooltip("진화Max시 비활성화 할 진화 후 변경될 별 이미지 묶음")]
    GameObject Next_StarGrade = null;

    BattlePcData Battle_PC_Data = null;

    void Awake()
    {
        Debug.Assert(Current_Star_Img != null, "HeroInfoBoxAdvance.Current_Star_Img == null");
        Debug.Assert(Next_Star_Img != null, "HeroInfoBoxAdvance.Next_Star_Img == null");
    }

    public void SetHeroData(BattlePcData data)
    {
        Battle_PC_Data = data;
        FixedUpdateUI();
    }

    public void FixedUpdateUI()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (Battle_PC_Data == null)
        {
            return;
        }

        int cur_grade = Battle_PC_Data.GetStarGrade();
        int next_grade = cur_grade+1;

        var error_code = Battle_PC_Data.CheckAdvanceStarGrade();

        bool is_success = error_code == ERROR_CODE.SUCCESS;
        bool is_already_max_level = error_code == ERROR_CODE.ALREADY_MAX_LEVEL;

        Advance_Button.image.sprite = is_success ? Enable_Button_Spr : Disable_Button_Spr;
        Advance_Button.enabled = is_success;
        Cannot_Advance_Text.gameObject.SetActive(is_already_max_level);
        Need_Gold_Object.SetActive(!is_already_max_level);
        Need_Gold_BG_Panel.SetActive(!is_already_max_level);
        Next_StarGrade_Arrow.SetActive(!is_already_max_level);
        Next_StarGrade.SetActive(!is_already_max_level);

        string piece_color_code = "#67afff";
        string gold_color_code = "#5C606A";

        var piece_data = GameData.Instance.GetUserItemDataManager().FindUserItem(ITEM_TYPE_V2.PIECE_CHARACTER, Battle_PC_Data.Data.player_character_id);
        string cur_piece_count = piece_data.GetCount().ToString();
        string need_piece_count = Battle_PC_Data.User_Data.GetNeedPiece().ToString();

        if (error_code != ERROR_CODE.SUCCESS)
        {
            switch(error_code)
            {
                case ERROR_CODE.NOT_ENOUGH_GOLD:
                    gold_color_code = "#ff0000";
                    break;
                case ERROR_CODE.NOT_ENOUGH_ITEM:
                    piece_color_code = "#ff0000";
                    break;
                case ERROR_CODE.NOT_ENOUGH_ALL:
                    gold_color_code = "#ff0000";
                    piece_color_code = "#ff0000";
                    break;
                case ERROR_CODE.ALREADY_MAX_LEVEL:
                    piece_color_code = "#BFC9D1";
                    cur_piece_count = "0";
                    next_grade = cur_grade;
                    break;
            }
        }

        Need_Piece_Text.text = $"{cur_piece_count.WithColorTag(piece_color_code)}/{need_piece_count}";
        Need_Gold_Text.text = Battle_PC_Data.User_Data.GetNeedGold().ToString("N0").WithColorTag(gold_color_code);

        for (int i = 0; i < Current_Star_Img.Length; i++)
        {
            Current_Star_Img[i].SetActive(i < cur_grade);
        }

        for (int i = 0; i < Next_Star_Img.Length; i++)
        {
            Next_Star_Img[i].SetActive(i < next_grade);
        }
    }

    /// <summary>
    /// 성급 진화 버튼
    /// </summary>
    public void OnClickAdvanceBtn()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Hero/AdvanceStarGradePopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
        {
            popup.ShowPopup(Battle_PC_Data);
            popup.AddClosedCallbackDelegate(OnConfirmAdvance);
        });
    }

    public void OnConfirmAdvance(params object[] data)
    {
        if (ERROR_CODE.SUCCESS != Battle_PC_Data.AdvanceStarGrade())
        {
            Debug.Assert(false, "성급진화 실패???");
            return;
        }

        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Common/LevelUpAniPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
        {
            popup.ShowPopup();
        });

        Refresh();
    }

    public void OnSelectedTab(Gpm.Ui.Tab tab)
    {
        Refresh();
    }
}
