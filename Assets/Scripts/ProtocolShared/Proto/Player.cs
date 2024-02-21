using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable

namespace ProtocolShared.Proto
{
    public class CreatePlayerRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }

    public class PlayerInfoResponse
    {
        public Guid PlayerGuid { get; set; }
        public string Name { get; set; } = string.Empty;
        public short Level { get; set; } = 0;
        public long Experience { get; set; } = 0;
        public string AideSelect { get; set; }
    }

    public class UsePlayerExpItemRequest
    {
        public UseItemInfo[] UseItems { get; set; }
    }

    public class UsePlayerExpItemResponse
    {
        public short Level { get; set; } = 0;
        public long Experience { get; set; } = 0;
        public ItemInfo[] Items { get; set; }
    }
}
