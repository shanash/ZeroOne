using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleResultRewardItem : RewardCardBase
{
    [SerializeField, Tooltip("Count Text")]
    protected TMP_Text Count_Text;

    public void SetCount(int cnt)
    {
        Count_Text.text = cnt.ToString("N0");
    }
}
