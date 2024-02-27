using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolShared.Proto
{
    public class ItemInfo
    {
        public Guid ItemGuid { get; set; }
        public int DesignItemId { get; set; }
        public int Count { get; set; } = 0;
        public bool IsDeleted { get; set; }
        public DateTime ExpireDate { get; set; }
    }

    public class UseItemInfo
    {
        public Guid ItemGuid { get; set; }
        public int DesignItemId { get; set; }
        public int Count { get; set; } = 0;
    }
    public class InventoryResponse
    {
        public List<ItemInfo>? ItemList { get; set; }
    }

    public class CreateItemRequest
    {
        public int DesignItemId { get; set; }
    }

    public class CreateItemResponse
    {
        public ItemInfo? Item { get; set; }
    }

    //public class UseItemRequest
    //{
    //    public UseItemInfo[]? useItems { get; set; }
    //}

    //public class UseItemResponse
    //{
    //    public ItemInfo[] UseItems { get; set; }
    //    /// <summary>
    //    /// 아이템 사용으로 변화된 값
    //    /// </summary>
    //    public UseItemResult[] UseItemResults { get; set; }
    //}
}
