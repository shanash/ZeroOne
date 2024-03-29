using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleResultAnimEventCallback : MonoBehaviour
{
    public enum BATTLE_RESULT_ANIM_CALLBACK_TYPE
    {
        VICTORY_END = 0,
        DEFEAT_END,
        STAR_END,
        EXP_END,
        REWARD_OPEN,
        REWARD_CLOSE,
    }

    System.Action<BATTLE_RESULT_ANIM_CALLBACK_TYPE> Anim_End_Callback;

    public void SetAnimEncCallback(System.Action<BATTLE_RESULT_ANIM_CALLBACK_TYPE> callback)
    {
        Anim_End_Callback = callback;
    }

    public void AnimationEndCallback(BATTLE_RESULT_ANIM_CALLBACK_TYPE callback_type)
    {
        Anim_End_Callback?.Invoke(callback_type);
    }
}
