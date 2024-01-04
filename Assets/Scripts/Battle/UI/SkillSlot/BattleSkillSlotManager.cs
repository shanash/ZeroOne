using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleSkillSlotManager : MonoBehaviour
{
    List<BattleSkillSlot> Used_Battle_Skill_Slots = new List<BattleSkillSlot>();

    GAME_TYPE Game_Type = GAME_TYPE.NONE;


    public void SetGameType( GAME_TYPE gtype)
    {
        Game_Type = gtype;
        InitSkillSlots();
    }

    void InitSkillSlots()
    {
        ClearSkillSlots();

        var pool = GameObjectPoolManager.Instance;
        var deck_mng = GameData.Instance.GetUserHeroDeckMountDataManager();
        var deck = deck_mng.FindSelectedDeck(Game_Type);
        var hero_list = deck.GetDeckHeroes();

        int cnt = hero_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var hero = hero_list[i];
            var obj = pool.GetGameObject("Assets/AssetResources/Prefabs/UI/Battle/BattleSkillSlot", this.transform);
            var slot = obj.GetComponent<BattleSkillSlot>();
            slot.SetPlayerCharacterData(hero.Player_Character_ID, hero.Player_Character_Num);

            Used_Battle_Skill_Slots.Add(slot);
        }

    }


    /// <summary>
    /// 슬롯 지우기
    /// </summary>
    void ClearSkillSlots()
    {
        var pool = GameObjectPoolManager.Instance;
        int cnt = Used_Battle_Skill_Slots.Count;
        for (int i = 0; i < cnt; i++)
        {
            pool.UnusedGameObject(Used_Battle_Skill_Slots[i].gameObject);
        }
        Used_Battle_Skill_Slots.Clear();
    }

    /// <summary>
    /// 슬롯은 이미 생성했고<br/>
    /// 이제 스파인과 연동해서 실시간으로 체크해가며 슬롯을 업데이트 해줘야 하기 때문에<br/>
    /// 스파인(HeroBase_V2)를 참조해야 한다
    /// </summary>
    /// <param name="members"></param>
    public void SetHeroMembers(List<HeroBase_V2> members)
    {
        int cnt = members.Count;
        for (int i = 0; i < cnt; i++)
        {
            var hero = members[i];
            var slot = Used_Battle_Skill_Slots.Find(x => x.IsEqualPlayerCharacter(hero.GetBattleUnitData().GetUnitID(), hero.GetBattleUnitData().GetUnitNum()));
            if (slot != null)
            {
                slot.SetHeroBase(hero);
            }
        }
    }
}
