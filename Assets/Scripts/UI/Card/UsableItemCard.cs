using Cysharp.Text;
using FluffyDuck.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UsableItemCard : UIBase
{
    [SerializeField, Tooltip("Box")]
    RectTransform Box;

    [SerializeField, Tooltip("Item Icon")]
    Image Item_Icon;

    [SerializeField, Tooltip("사용한 아이템 갯수")]
    TMP_Text Use_Count_UI = null;

    [SerializeField, Tooltip("선택 UI")]
    RectTransform Select = null;

    [SerializeField, Tooltip("Minus Btn")]
    UIButtonBase Minus_Btn;

    [SerializeField, Tooltip("경험치 아이템 배경 UI")]
    Image BackGround = null;

    UserItemData User_Data = null;
    Item_Data Data;

    USABLE_ITEM_DATA Use_Item = new USABLE_ITEM_DATA();

    System.Action<bool, System.Action<bool>> Usable_Item_Count_Change_Callback;


    public void SetUserItemData(ITEM_TYPE_V2 itype, int item_id, System.Action<bool, System.Action<bool>> change_cb)
    {
        User_Data = GameData.Instance.GetUserItemDataManager().FindUserItem(itype, item_id);
        Data = MasterDataManager.Instance.Get_ItemData(itype, item_id);
        Usable_Item_Count_Change_Callback = change_cb;

        Use_Item.Item_Type = itype;
        Use_Item.Item_ID = item_id;
        Use_Item.Use_Count = 0;

        UpdateItemCard();
    }

    public void ResetUsableCount()
    {
        Use_Item.Use_Count = 0;
        UpdateItemCard();
    }

    public USABLE_ITEM_DATA GetUsableItemData()
    {
        return Use_Item;
    }

    public int GetUseCount()
    {
        return Use_Item.Use_Count;
    }

    public void UpdateItemCard()
    {
        if (User_Data == null || User_Data.GetCount() == 0)
        {
            //  보유 아이템 수
            //Exist_Count_UI.text = "0";
            Use_Count_UI.text = "0";
            Minus_Btn.gameObject.SetActive(false);
            //  아이템 아이콘
            CommonUtils.GetResourceFromAddressableAsset<Sprite>(Data.icon_path, (spr) =>
            {
                Item_Icon.sprite = spr;
            });
            return;
        }

        //  아이템 아이콘
        CommonUtils.GetResourceFromAddressableAsset<Sprite>(User_Data.GetIconPath(), (spr) =>
        {
            Item_Icon.sprite = spr;
        });

        //  선택 
        Minus_Btn.gameObject.SetActive(Use_Item.Use_Count > 0);
        Use_Count_UI.text = ZString.Format("{0}/{1}", Use_Item.Use_Count, User_Data.GetCount());

    }

    

    public void OnClickPlus()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        if (User_Data == null || User_Data.GetCount() == 0)
        {
            return;
        }

        Use_Item.Use_Count += 1;
        int own_cnt = (int)User_Data.GetCount();
        if (Use_Item.Use_Count > own_cnt)
        {
            Use_Item.Use_Count = own_cnt;
        }
        Usable_Item_Count_Change_Callback?.Invoke(true, (success) =>
        {
            if (!success) 
            {
                Use_Item.Use_Count -= 1;
            }
            UpdateItemCard();
        });
        
    }
    public void OnClickMinus()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        if (User_Data == null || User_Data.GetCount() == 0)
        {
            return;
        }
        Use_Item.Use_Count -= 1;
        if (Use_Item.Use_Count < 0)
        {
            Use_Item.Use_Count = 0;
        }
        Usable_Item_Count_Change_Callback?.Invoke(false, (success) =>
        {
            UpdateItemCard();
        });
        

    }
}
