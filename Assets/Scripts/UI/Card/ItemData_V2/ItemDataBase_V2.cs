using System;
using System.Diagnostics;

public abstract class ItemDataBase_V2 : IDisposable
{
    protected bool disposed = false;

    public ITEM_TYPE_V2 Item_Type { get; protected set; } = ITEM_TYPE_V2.NONE;

    public int Item_ID { get; protected set; } = 0;
    public int Item_Count { get; protected set; } = 0;

    public virtual string ItemName
    {
        get
        {
            string result = "EMPTY";
            try
            {
                string name_id;
                if (Item_Type == ITEM_TYPE_V2.PIECE_ITEM)
                {
                    name_id = MasterDataManager.Instance.Get_ItemPieceData(Item_ID).name_id;
                }
                else if (Item_Type == ITEM_TYPE_V2.EQUIPMENT)
                {
                    name_id = MasterDataManager.Instance.Get_EquipmentData(Item_ID).name_id;
                }
                else
                {
                    name_id = MasterDataManager.Instance.Get_ItemData(Item_ID).name_id;
                }

                result = GameDefine.GetLocalizeString(name_id);
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogWarning(ex);
            }

            return result;
        }
    }
    public virtual string ItemDesc
    {
        get
        {
            string result = "EMPTY";
            try
            {
                string dsec_id;
                Item_Data item_data = null;
                if (Item_Type == ITEM_TYPE_V2.PIECE_ITEM)
                {
                    dsec_id = MasterDataManager.Instance.Get_ItemPieceData(Item_ID).desc_id;
                }
                else if (Item_Type == ITEM_TYPE_V2.EQUIPMENT)
                {
                    dsec_id = MasterDataManager.Instance.Get_EquipmentData(Item_ID).desc_id;
                }
                else
                {
                    item_data = MasterDataManager.Instance.Get_ItemData(Item_ID);
                    dsec_id = item_data.desc_id;
                }

                result = GameDefine.GetLocalizeString(dsec_id);

                // Item_Data 시트일때 desc에 {0} 같은게 있으면 int_var1을 적용해줘야 한다
                // 값 100 미만은 Item_Data시트겠죠? 아니면 버그날거니까 바꿔주세요
                if ((int)Item_Type/100 == 0)
                {
                    result = string.Format(result, item_data.int_var1, item_data.int_var2);
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogWarning(ex);
            }

            return result;
        }
    }

    public ItemDataBase_V2()
    {
        Init();
    }


    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                //  관리되는 자원 해제
                Destroy();
            }
            disposed = true;
        }
    }

    protected virtual void Init() 
    {
        Item_Type = ITEM_TYPE_V2.NONE;
        Item_ID = 0;
        Item_Count = 0;
    }

    protected virtual void Destroy() { }

    public virtual void SetItem(ITEM_TYPE_V2 gtype, int item_id)
    {
        Item_Type = gtype;
        Item_ID = item_id;
    }

    public void AddItemCount(int cnt) { Item_Count += cnt; }

    public virtual string GetItemIconPath() { return string.Empty; }

    public virtual object GetItemData() { return null; }

    public virtual PIECE_TYPE GetPieceType() {  return PIECE_TYPE.NONE; }

    public virtual EQUIPMENT_TYPE GetEquipmentType() { return EQUIPMENT_TYPE.NONE; }
}
