using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolShared.Proto
{
    public class RewardGoodInfo
    {
        public REWARD_TYPE RewardType { get; set; } = REWARD_TYPE.NONE;
        public int RewardCount { get; set; } = 0;
        public Int64 AfterCount { get; set; } = 0;
    }

    public class RewardLevelInfo
    {
        public REWARD_TYPE RewardType { get; set; } = REWARD_TYPE.NONE;
        public Guid TargetGuid { get; set; } = Guid.Empty;
        public int Level { get; set; } = 0;
        public Int64 Experience { get; set; } = 0;
    }
    public class RewardResult
    {
        public RewardGoodInfo[] RewardGoods { get; set; } = new RewardGoodInfo[0];
        public RewardLevelInfo[] RewardLevels { get; set; } = new RewardLevelInfo[0];
        public ItemInfo[] RewardItems { get; set; } = new ItemInfo[0];
    }
    public class GameStartRequest
    {
        [Required]
        public ushort StageId { get; set; }
        // 스킵도 고려할것
        [Required]
        public Guid[] Characters { get; set; } = new Guid[0];
    }

    public class GameStartResponse
    {
        public short RemainStamina { get; set; }
    }

    public class GameEndRequest
    {
        [Required]
        public bool isWin { get; set; }
        [Required]
        public int ClearRank { get; set; }
    }

    public class GameEndResponse
    {
        public short remainStamina { get; set; } = 0;
        public RewardResult Reward { get; set; } = new RewardResult();
    }
}
