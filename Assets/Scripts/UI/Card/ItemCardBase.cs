using FluffyDuck.Util;
using UnityEngine;
using UnityEngine.UI;

public class ItemCardBase : UIBase
{
    [SerializeField, Tooltip("Item Icon")]
    protected Image Item_Icon;


    protected ItemGoodsDataBase Item_Goods_Data;
    protected ItemDataBase Item_Data;

    public void SetItem(ITEM_TYPE itype, int item_id)
    {
        Item_Data = CreateItemData(itype, item_id);
        UpdateItemIcon();
    }

    public void SetGoods(GOODS_TYPE gtype, int item_id)
    {

    }

    protected void UpdateItemIcon()
    {
        if (Item_Data == null)
        {
            return;
        }

        CommonUtils.GetResourceFromAddressableAsset<Sprite>(Item_Data.GetItemIconPath(), (spr) =>
        {
            Item_Icon.sprite = spr;
        });
    }


    ItemDataBase CreateItemData(ITEM_TYPE itype, int item_id)
    {
        ItemDataBase item = null;
        switch (itype)
        {
            case ITEM_TYPE.GOLD:
                item = new ItemDataBase();
                break;
            case ITEM_TYPE.DIA:
                item = new ItemDataBase();
                break;
            case ITEM_TYPE.DUNGEON_TICKET:
                item = new ItemDataBase();
                break;
            case ITEM_TYPE.CHARACTER_PIECE:
                item = new ItemCharacterPieceData();
                break;
            case ITEM_TYPE.EXP_POTION:
                item = new ItemExpPotionData();
                break;
            case ITEM_TYPE.STA_POTION:
                item = new ItemStaminaPotionData();
                break;
            case ITEM_TYPE.MEMORIAL_ITEM:
                Debug.Assert(false);
                break;
            case ITEM_TYPE.CHARACTER:
                Debug.Assert(false);
                break;
            case ITEM_TYPE.EXPENDABLE_ITEM:
                item = new ItemExpendableData();
                break;
            case ITEM_TYPE.EQUIPMENT_ITEM:
                Debug.Assert(false);
                break;
            case ITEM_TYPE.FAVORITE_ITEM:
                item = new ItemFavoriteData();
                break;
            case ITEM_TYPE.STAMINA:
                Debug.Assert(false);
                break;
            default:
                Debug.Assert(false);
                break;
        }
        item?.SetItem(itype, item_id);
        return item;
    }

}
