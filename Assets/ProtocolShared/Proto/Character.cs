using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable

namespace ProtocolShared.Proto
{
    public class CharacterInfo
    {
        public Guid Id { get; set; }
        public int CharacterId { get; set; }
        public byte Level { get; set; } = 1;
        public ushort Experience { get; set; } = 0;
        // 성(별)
        public byte Rank { get; set; } = 1;
        public ushort Piece { get; set; } = 0;
        // 스킬
        public byte UltimateSkillLevel { get; set; } = 1;
        public uint UltimateSkillExp { get; set; } = 0;
        public byte ActiveSkillLevel_1 { get; set; } = 1;
        public uint ActiveSkillExp_1 { get; set; } = 0;
        public byte ActiveSkillLevel_2 { get; set; } = 1;
        public uint ActiveSkillExp_2 { get; set; } = 0;
        public byte PassiveSkillLevel { get; set; } = 1;
        public uint PassiveSkillExp { get; set; } = 0;
        // 호감도
        public byte LoveLevel { get; set; } = 1;
        public uint LoveExp { get; set; } = 0;
        // 장비 슬롯
        //public Guid ItemSlot_1 { get; set; }
        //public Guid ItemSlot_2 { get; set; }
        //public Guid ItemSlot_3 { get; set; }
        //public Guid ItemSlot_4 { get; set; }
        //public Guid ItemSlot_5 { get; set; }
        //public Guid ItemSlot_6 { get; set; }
        // 액세서리 슬롯
        public Guid AccSlot_1 { get; set; }
        public Guid AccSlot_2 { get; set; }
        public Guid AccSlot_3 { get; set; }
    }

    public class CharacterListResponse
    {
        public List<CharacterInfo> CharacterList { get; set; }
    }

    public class DeckInfo
    {
        public uint No { get; set; }
        public Guid Slot1 { get; set; }
        public Guid? Slot2 { get; set; }
        public Guid? Slot3 { get; set; }
        public Guid? Slot4 { get; set; }
        public Guid? Slot5 { get; set; }
    }

    public class DeckListResponse
    {
        public List<DeckInfo> DeckList { get; set; } = new List<DeckInfo>();
    }

    public class SetDeckRequest
    {
        public DeckInfo Deck { get; set; } 
    }
}
