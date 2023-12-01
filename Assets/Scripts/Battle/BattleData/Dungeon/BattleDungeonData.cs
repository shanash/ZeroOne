using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 모든 던전 데이터의 최상위 데이터
/// 스토리, 이벤트 던전, 레이드 던전 등 다양한 던전의 최상위 클래스
/// </summary>
public class BattleDungeonData : BattleDataBase
{
    public GAME_TYPE Game_Type { get; protected set; }

    /// <summary>
    /// 웨이브 진행 상황
    /// </summary>
    public int Wave { get; protected set; } = 0;

    public BattleDungeonData(GAME_TYPE gtype) {  Game_Type = gtype; }
    /// <summary>
    /// 던전 정보 세팅
    /// </summary>
    /// <param name="dungeon_id"></param>
    public virtual void SetDungeonID(int dungeon_id) { }
    /// <summary>
    /// 던전 데이터 반환
    /// </summary>
    /// <returns></returns>
    public virtual object GetDungeonData() { return null; }

    /// <summary>
    /// 웨이브 인덱스. 
    /// 라벨에 표시할땐 +1 해서 표시
    /// </summary>
    /// <returns></returns>
    public int GetWave()
    {
        return Wave;
    }

    public virtual void GetMonsterPrefabsPath(ref List<string> path) { }

    public virtual object GetWaveData() {  return null; }
}
