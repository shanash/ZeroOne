using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolShared.Proto
{
    public class ItemInfo
    {
        public Guid Id { get; set; }
        public int ItemId { get; set; }
        public int Count { get; set; } = 0;
        public byte Level { get; set; } = 1;
        public uint Experience { get; set; } = 0;

    }
    public class InventoryResponse
    {
        public List<ItemInfo>? ItemList { get; set; }
    }

    public class CreateItemRequest
    {
        public int ItemId { get; set; }
    }

    public class CreateItemResponse
    {
        public ItemInfo? Item { get; set; }
    }
}
