using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolShared.Proto
{
    public class GameResultRequest
    {
        [Required]
        public ushort kill { get; set; }

        [Required] 
        public ushort death { get; set; }
    }

    public class GameResultResponse
    {
        public ushort score { get; set; }
    }
}
