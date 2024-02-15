using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolShared.Proto
{
    public class CreatePlayerRequest
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }

    public class PlayerInfoResponse
    {
        public Guid PlayerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public ushort Level { get; set; } = 0;
        public UInt64 Experience { get; set; } = 0;
        public string? AideSelect { get; set; }
        
    }
}
