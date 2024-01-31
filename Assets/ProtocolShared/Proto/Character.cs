using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable

namespace ProtocolShared.Proto
{
    public class ItemData
    {
        public Guid Id { get; set; }
        public int ItemId { get; set; }
        public int Count { get; set; } = 0;
    }
    public class InventoryResponse
    {
        public List<ItemData> ItemList { get; set; }
    }

    public class CreateItemRequest
    {
        public int ItemId { get; set; }
    }

    public class CreateItemResponse
    {
        public ItemData Item { get; set; }
    } 
}
