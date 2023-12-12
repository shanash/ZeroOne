using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUIManager_V2 : MonoBehaviour
{
    [SerializeField, Tooltip("Battle Mng")]
    BattleManager_V2 Battle_Mng;

    [SerializeField, Tooltip("HP Bar Container")]
    RectTransform HP_Bar_Container;

    [SerializeField, Tooltip("Damage Container")]
    RectTransform Damage_Container;

    /// <summary>
    /// 체력 게이지 
    /// </summary>
    List<LifeBarNode> Used_Life_Bar_List = new List<LifeBarNode>();

    public void OnClickPause()
    {
        Battle_Mng?.ChangeState(GAME_STATES.PAUSE);
    }


    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (PopupManager.Instance.IsShow())
            {
                return;
            }

            if (Battle_Mng.IsPause())
            {
                Battle_Mng.RevertState();
            }
            else
            {
                Battle_Mng.ChangeState(GAME_STATES.PAUSE);
            }
            
        }
    }

    /// <summary>
    /// 체력 게이지 추가
    /// </summary>
    /// <param name="t"></param>
    /// <param name="ttype"></param>
    /// <returns></returns>
    public LifeBarNode AddLifeBarNode(Transform t, TEAM_TYPE ttype)
    {
        var pool = GameObjectPoolManager.Instance;
        var obj = pool.GetGameObject("Assets/AssetResources/Prefabs/UI/LifeBarNode", HP_Bar_Container);
        var life = obj.GetComponent<LifeBarNode>();
        life.SetBarColor(ttype);
        life.SetTargetTransform(t);
        

        Used_Life_Bar_List.Add(life);
        return life;
    }
    /// <summary>
    /// 체력 게이지 제거
    /// </summary>
    /// <param name="bar"></param>
    public void RemoveLifeBarNode(LifeBarNode bar)
    {
        Used_Life_Bar_List.Remove(bar);
        GameObjectPoolManager.Instance.UnusedGameObject(bar.gameObject);
    }

    public RectTransform GetDamageContainer()
    {
        return Damage_Container;
    }
}
