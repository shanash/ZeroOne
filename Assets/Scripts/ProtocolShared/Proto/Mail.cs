using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolShared.Proto
{
    public class MailInfo
    {
        public Guid MailGuid { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int[] RewardGroupIds { get; set; } = new int[0];
        public bool IsRead { get; set; } = false;
    }
    public class GetMailListRequest
    {
    }

    public class GetMailListResponse
    {
        public MailInfo[] MailList = new MailInfo[0];
    }

    public class ReadMailRequest
    {
        public Guid[] MailGuidList = new Guid[0];
    }

    public class ReadMailResponse
    {
        public RewardResult? Reward {  get; set; }   
    }
}
