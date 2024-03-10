using TMPro;
using UnityEngine;

public class CommonTooltip : TooltipBase
{
    [SerializeField]
    TMP_Text NumberText = null;

    public void Initialize(Rect hole, RewardDataBase reward_data, bool is_screen_modify = true)
    {
        Initialize(hole, reward_data.RewardName, reward_data.Desc, is_screen_modify);

        NumberText.text = string.Format($"수량 : {0}", reward_data.Count);
    }
}
