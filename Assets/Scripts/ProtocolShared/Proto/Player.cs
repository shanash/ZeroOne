using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable

namespace ProtocolShared.Proto
{
    public class PlayerSimpleInfo
    {
        public Guid PlayerGuid { get; set; } = Guid.Empty;
        public string PlayerName { get; set; } = string.Empty;
        public short Level { get; set; } = 1;
    }
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

    public class ChargeInfo
    {
        public REWARD_TYPE RewardType { get; set; } = REWARD_TYPE.NONE;
        public int ChargeCount { get; set; } = 0;   
        public int AfterCount { get; set; } = 0;
        public DateTime LastChargeTime { get; set; } = DateTime.UtcNow;
    }
    public class ChargeCheckRequest
    {
        [Required]
        public REWARD_TYPE[] CheckRewardTypes { get; set; } = new REWARD_TYPE[0];
    }

    public class ChargeCheckResponse
    {
        public ChargeInfo[] Charges { get; set; } = new ChargeInfo[0];
    }

    public class UsePlayerExpItemRequest
    {
        [Required]
        public UseItemInfo[] UseItems { get; set; }
    }

    public class UsePlayerExpItemResponse
    {
        public short Level { get; set; } = 0;
        public long Experience { get; set; } = 0;
        public ItemInfo[] Items { get; set; }
    }
}
