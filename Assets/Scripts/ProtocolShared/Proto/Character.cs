using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable

namespace ProtocolShared.Proto
{
    public enum SkillSlotType
    {
        ACTIVE_SKILL_1,
        ACTIVE_SKILL_2,
        ULTIMATE_SKILL,
    }
    public class CharacterInfo
    {
        public Guid CharacterGuid { get; set; }
        public int CharacterDesignId { get; set; }
        public short Level { get; set; } = 1;
        public int Experience { get; set; } = 0;
        // 성(별)
        public byte Rank { get; set; } = 1;
        public int Piece { get; set; } = 0;
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

    public class UseCharacterExpItemRequest
    {
        public Guid CharacterGuid { get; set; }
        public UseItemInfo[] UseItems { get; set; }
    }

    public class UseCharacterExpItemResponse
    {
        public Guid CharacterGuid { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        // 사용된 아이템 변경된 정보들..(수량이 줄었거나 지워졌거나...)
        public ItemInfo[] Items { get; set; }
    }

    public class UseCharacterSkillItemRequest
    {
        public Guid CharacterGuid { get; set; }
        public SkillSlotType SkillSlotType { get; set; }
        public UseItemInfo[] UseItems { get; set; }
    }

    public class UseCharacterSkillItemResponse
    {
        public Guid CharacterGuid { get; set; }
        public SkillSlotType SkillSlotType { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        // 사용된 아이템 변경된 정보들..(수량이 줄었거나 지워졌거나...)
        public ItemInfo[] Items { get; set; }
    }
}
