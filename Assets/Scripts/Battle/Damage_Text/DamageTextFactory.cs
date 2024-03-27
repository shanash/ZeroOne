using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextFactory : MonoBehaviour
{
    List<DamageTextBase> Used_Damage_Text_List = new List<DamageTextBase>();

    float Effect_Speed_Multiple = GameDefine.GAME_SPEEDS[BATTLE_SPEED_TYPE.NORMAL_TYPE];

    public DamageTextBase Create(string path)
    {
        DamageTextBase dmg = null;

        var pool = GameObjectPoolManager.Instance;
        var obj = pool.GetGameObject(path, this.transform);
        dmg = obj.GetComponent<DamageTextBase>();
        dmg.SetFactory(this);
        dmg.SetSpeedMultiple(Effect_Speed_Multiple);

        Used_Damage_Text_List.Add(dmg);
        return dmg;
    }

    public void SetBattleSpeedMultiple(float multiple)
    {
        Effect_Speed_Multiple = multiple;
        int cnt = Used_Damage_Text_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Used_Damage_Text_List[i].SetSpeedMultiple(multiple);
        }
    }
    /// <summary>
    /// 모든 데미지 텍스트 삭제
    /// </summary>
    public void ClearAllTexts()
    {
        var pool = GameObjectPoolManager.Instance;
        int cnt = Used_Damage_Text_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            pool.UnusedGameObject(Used_Damage_Text_List[i].gameObject);
        }
        Used_Damage_Text_List.Clear();
    }

    public void UnusedDamageTextBase(DamageTextBase obj)
    {
        Used_Damage_Text_List.Remove(obj);
        GameObjectPoolManager.Instance.UnusedGameObject(obj.gameObject);
    }
    public void OnPause()
    {
        int cnt = Used_Damage_Text_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Used_Damage_Text_List[i].OnPause();
        }
    }
    public void OnResume()
    {
        int cnt = Used_Damage_Text_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Used_Damage_Text_List[i].OnResume();
        }
    }
}
