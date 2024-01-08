using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleSkillSlotManager : MonoBehaviour
{
    List<BattleSkillSlot> Used_Battle_Skill_Slots = new List<BattleSkillSlot>();


    /// <summary>
    /// 슬롯 생성<br/>
    /// 스파인과 연동해서 실시간으로 체크해가며 슬롯을 업데이트 해줘야 하기 때문에<br/>
    /// 스파인(HeroBase_V2)를 참조해야 한다
    /// </summary>
    /// <param name="members"></param>
    public void SetHeroMembers(List<HeroBase_V2> members)
    {
        ClearSkillSlots();
        var pool = GameObjectPoolManager.Instance;
        int cnt = members.Count;
        for (int i = 0; i < cnt; i++)
        {
            var hero = members[i];
            var obj = pool.GetGameObject("Assets/AssetResources/Prefabs/UI/Battle/BattleSkillSlot", this.transform);
            var slot = obj.GetComponent<BattleSkillSlot>();
            slot.SetHeroBase(hero);

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

   
}
