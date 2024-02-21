using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolShared.Proto
{

    public class RewardResult
    {

    }
    public class GameStartRequest
    {
        [Required]
        public ushort stageId { get; set; }
        // 스킵도 고려할것
        [Required]
        public Guid charSlot_1 { get; set; }
        public Guid charSlot_2 { get; set; }
        public Guid charSlot_3 { get; set; }
        public Guid charSlot_4 { get; set; }
        public Guid charSlot_5 { get; set; }
    }

    public class GameStartResponse
    {
        public short remainActionPoint { get; set; }
    }

    public class GameEndRequest
    {
        [Required]
        public bool isWin { get; set; }
    }

    public class GameEndResponse
    {
        public ushort score { get; set; }
    }
}
