using FluffyDuck.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStartPopup : PopupBase
{
    [SerializeField, Tooltip("Normal Dungeon Start")]
    RectTransform Normal_Start;

    [SerializeField, Tooltip("Boss Dungeon Start")]
    RectTransform Boss_Start;

    [SerializeField, Tooltip("Time Out Start")]
    RectTransform Timeout_Start;

    float Delay_Time;
    bool Is_Action;

    /// <summary>
    /// 0 : normal start
    /// 1 : boss start
    /// 2 : time out start
    /// </summary>
    int Start_Type;
    protected override bool Initialize(object[] data)
    {
        if (data.Length != 1)
        {
            return false;
        }
        Start_Type = (int)data[0];
        Is_Action = false;
        SetEnableEscKeyExit(false);

        Is_Action = true;
        FixedUpdatePopup();
        return true;
    }

    protected override void FixedUpdatePopup()
    {
        if (Start_Type == 0)
        {
            Delay_Time = 1.2f;
            Normal_Start.gameObject.SetActive(true);
        }
        else if(Start_Type == 1)
        {
            Delay_Time = 1.2f;
            Boss_Start.gameObject.SetActive(true);
        }
        else if (Start_Type == 2)
        {
            Delay_Time = 1.2f;
            Timeout_Start.gameObject.SetActive(true);
        }
    }

    protected override void OnUpdatePopup()
    {
        if (Is_Action)
        {
            Delay_Time -= Time.deltaTime;
            if (Delay_Time < 0f)
            {
                HidePopup();
                Is_Action = false;
            }
        }
    }

    
    public override void Despawned()
    {
        base.Despawned();
        Normal_Start.gameObject.SetActive(false);
        Boss_Start.gameObject.SetActive(false);
        Timeout_Start.gameObject.SetActive(false);
    }
}
