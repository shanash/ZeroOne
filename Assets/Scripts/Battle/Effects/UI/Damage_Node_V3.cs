using Cysharp.Text;
using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Damage_Node_V3 : EffectBase
{

    [SerializeField, Tooltip("Damage Orange Text")]
    TMP_Text Damage_Orange_Text;

    [SerializeField, Tooltip("Damage White Text")]
    TMP_Text Damage_White_Text;

    [SerializeField, Tooltip("Animator")]
    Animator Anim;

    BATTLE_SEND_DATA? Send_Data;
    [SerializeField, Tooltip("Damage Type")]
    DAMAGE_TYPE Damage_Type = DAMAGE_TYPE.NORMAL;

    public override void StartParticle(float duration, bool loop = false)
    {
        if (Send_Data == null)
        {
            UnusedEffect();
            return;
        }
        bool is_physics = Send_Data.Value.Onetime.GetOnetimeEffectType() == ONETIME_EFFECT_TYPE.PHYSICS_DAMAGE;
        double damage = is_physics ? Send_Data.Value.Physics_Attack_Point : Send_Data.Value.Magic_Attack_Point;
        StartMove(damage, Damage_Type);

        base.StartParticle(duration, loop);
    }
    public void StartMove(double dmg, DAMAGE_TYPE dmg_type)
    {
        string dmg_str = dmg.ToString();
        int cnt = dmg_str.Length;
        var sb = ZString.CreateStringBuilder();

        //  크리티컬
        if (dmg_type == DAMAGE_TYPE.WEAK_CRITICAL || dmg_type == DAMAGE_TYPE.CRITICAL)
        {
            sb.Append("<sprite=10>");
        }

        for (int i = 0; i < cnt; i++)
        {
            sb.AppendFormat("<sprite={0}>", dmg_str[i]);
        }

        Damage_Orange_Text.text = sb.ToString();
        Damage_White_Text.text = sb.ToString();

        StartAnimation(dmg_type);
    }

    public override void SetData(params object[] data)
    {
        if (data.Length != 3)
        {
            return;
        }
        Send_Data = (BATTLE_SEND_DATA)data[0];
        Damage_Type = (DAMAGE_TYPE)data[1];

        var target_position = (Vector3)data[2];
        Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, target_position);
        GetComponent<RectTransform>().anchoredPosition3D = pos;
        
    }

    void StartAnimation(DAMAGE_TYPE dmg_type)
    {
        switch (dmg_type)
        {
            case DAMAGE_TYPE.NORMAL:
                Anim.SetTrigger("Normal");
                break;
            case DAMAGE_TYPE.WEAK:
                Anim.SetTrigger("Weak");
                break;
            case DAMAGE_TYPE.CRITICAL:
                Anim.SetTrigger("Cri");
                break;
            case DAMAGE_TYPE.WEAK_CRITICAL:
                Anim.SetTrigger("Weak_Cri");
                break;
            case DAMAGE_TYPE.TOTAL:
                Anim.SetTrigger("Total");
                break;
            default:
                Anim.SetTrigger("Normal");
                break;
        }
    }

    public void AnimationEndCallback()
    {
        Finish_Callback?.Invoke(this);
        UnusedEffect();
    }

    public override void Spawned()
    {
        Damage_Orange_Text.text = string.Empty;
        Damage_White_Text.text = string.Empty;

    }
}
