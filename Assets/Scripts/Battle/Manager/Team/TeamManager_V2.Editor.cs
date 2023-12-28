using FluffyDuck.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TeamManager_V2
{
    /// <summary>
    /// 에디터용 모든 멤버 삭제
    /// </summary>
    public void Editor_RemoveAllMembers()
    {
        var pool = GameObjectPoolManager.Instance;
        int cnt = Used_Members.Count;
        for (int i = 0; i < cnt; i++)
        {
            pool.UnusedGameObject(Used_Members[i].gameObject);
        }
        Used_Members.Clear();
    }

    /// <summary>
    /// 에디터용 - 유닛 추가
    /// </summary>
    /// <param name="unit"></param>
    public void Editor_AddBattleUnit(BattleUnitData unit)
    {
        Editor_RemoveAllMembers();

        GameObjectPoolManager.Instance.GetGameObject(unit.GetPrefabPath(), Unit_Container, (obj) =>
        {
            HeroBase_V2 hero = null;
            if (unit.Character_Type == CHARACTER_TYPE.PC)
            {
                hero = obj.GetComponent<HeroBase_V2>();
            }
            else
            {
                hero = obj.GetComponent<MonsterBase_V2>();
            }
            
            if (hero != null)
            {
                hero.SetTeamManager(this);
                hero.SetBattleUnitDataID(unit.GetUnitID(), unit.GetUnitNum());
                hero.SetDeckOrder(0);
                hero.Lazy_Init(Battle_Mng, UI_Mng, UNIT_STATES.INIT);
                AddMember(hero);
            }
          
        });
        EditorLeftTeamPosition();
    }

    /// <summary>
    /// 에디터 모드 몬스터 스폰
    /// </summary>
    void Editor_MonsterTeamSpawn()
    {
        var pool = GameObjectPoolManager.Instance;

        var wdata = (Editor_Wave_Data)Dungeon.GetWaveData();
        int len = wdata.enemy_appearance_info.Length;

        //  battle npc data
        for (int i = 0; i < len; i++)
        {
            var npc = new BattleNpcData();
            npc.SetUnitID(wdata.enemy_appearance_info[i]);

            var obj = pool.GetGameObject(npc.GetPrefabPath(), Unit_Container);
            MonsterBase_V2 monster = obj.GetComponent<MonsterBase_V2>();
            monster.SetTeamManager(this);

            monster.SetBattleUnitDataID(npc.GetUnitID());

            monster.SetFlipX(true);
            monster.SetDeckOrder(i);
            monster.Lazy_Init(Battle_Mng, UI_Mng, UNIT_STATES.INIT);
            AddMember(monster);
        }

        Editor_RightTeamPosition();
    }


    /// <summary>
    /// 에디터 모드에서 화면안의 아군 포지션 위치
    /// </summary>
    public void EditorLeftTeamPosition()
    {
        int cnt = Used_Members.Count;
        float offset_z = 0f;
        float offset_x = 5;
        float interval = 12f;
        float position_offset = 0f;

        for (int i = 0; i < cnt; i++)
        {
            var member = Used_Members[i];
            position_offset = (int)member.GetPositionType() * interval;

            float distance = member.GetApproachDistance();

            List<HeroBase_V2> same_positions = Used_Members.FindAll(x => x.GetPositionType() == member.GetPositionType());
            same_positions.Sort((a, b) => a.GetApproachDistance().CompareTo(b.GetApproachDistance()));
            Debug.Assert(same_positions.Count > 0);

            int find_index = same_positions.IndexOf(member);

            member.transform.localPosition = new Vector3(offset_x - position_offset, 0, offset_z + (find_index * 5));
        }

    }

    /// <summary>
    /// 실제 게임상이 아닌, 스킬 에디터상에서의 좌표.<br/>
    /// 화면 밖이 아닌, 화면 안의 적군 포지션에서 시작한다.
    /// </summary>
    public void Editor_RightTeamPosition()
    {
        int cnt = Used_Members.Count;

        float offset_z = 0f;
        float offset_x = 5;
        float interval = 12f;
        float position_offset = 0f;

        for (int i = 0; i < cnt; i++)
        {
            var member = Used_Members[i];
            position_offset = (int)member.GetPositionType() * interval;

            float distance = member.GetApproachDistance();

            List<HeroBase_V2> same_positions = Used_Members.FindAll(x => x.GetPositionType() == member.GetPositionType());
            same_positions.Sort((a, b) => a.GetApproachDistance().CompareTo(b.GetApproachDistance()));
            Debug.Assert(same_positions.Count > 0);

            int find_index = same_positions.IndexOf(member);

            member.transform.localPosition = new Vector3(offset_x + position_offset, 0, offset_z + (find_index * 5));
        }

    }
}
