using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LobbyCharacterCellItem : MonoBehaviour
{
    [SerializeField, Tooltip("Box")]
    RectTransform Box;

    [SerializeField, Tooltip("Character Cell Base")]
    CharacterCellBase Character_Base;

    [SerializeField, Tooltip("Memorial Info Box")]
    RectTransform Memorial_Info_Box;
    [SerializeField, Tooltip("Memorial Box BG")]        //  (보유) #ffffff / (미보유) #e0e0e0
    Image Memorial_Box_Bg;
    [SerializeField, Tooltip("Memorial Icon")]          //  #5D7DA1 / #969696
    Image Memorial_Icon;
    [SerializeField, Tooltip("Memorial Count")]         //  #5D7DA1 / #969696
    TMP_Text Memorial_Count;

    [SerializeField, Tooltip("Memorial Select Frame")]
    Image Memorial_Select_Frame;
    [SerializeField, Tooltip("Memorial Select Order Number")]
    TMP_Text Memorial_Select_Order;

    System.Action<UserHeroData> Click_Callback;
    UserHeroData User_Data;

    public void SetUserHeroData(UserHeroData data)
    {
        User_Data = data;
        UpdateCellItem();
        
    }

    void UpdateCellItem()
    {
        if (User_Data == null)
        {
            Box.gameObject.SetActive(false);
            return;
        }
        if (!Box.gameObject.activeSelf)
        {
            Box.gameObject.SetActive(true);
        }
        //  character base
        Character_Base.SetPlayerCharacterID(User_Data.GetPlayerCharacterID());




        //  select frame
        Memorial_Select_Frame.gameObject.SetActive(User_Data.Is_Choice_Lobby);

    }

    public void SetClickCallback(System.Action<UserHeroData> cb)
    {
        Click_Callback = cb;
    }

    public void OnClickCharacterItem()
    {
        if (User_Data == null)
        {
            return;
        }
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        Click_Callback?.Invoke(User_Data);
    }
}
