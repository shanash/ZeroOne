using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolShared.Proto
{
    public class GameStartRequest
    {
        [Required]
        public ushort stage { get; set; }
    }

    public class GameStartResponse
    {
        public ushort useActionPoint { get; set; }
    }


    public class GameEndRequest
    {
        [Required]
        public bool isSuccess { get; set; }
        [Required]
        public ushort kill { get; set; }
        [Required] 
        public ushort death { get; set; }
    }

    public class GameEndResponse
    {
        public ushort score { get; set; }
    }
}
