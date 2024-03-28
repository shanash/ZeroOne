using Cysharp.Text;
using Febucci.UI;
using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextBase : MonoBehaviour, IPoolableComponent
{
    protected DamageTextFactory Dmg_Factory;

    [SerializeField, Tooltip("Animator")]
    protected Animator Anim;

    [SerializeField, Tooltip("Damage Main Text")]
    protected TMP_Text Damage_Main_Text;

    [SerializeField, Tooltip("Damage White Text")]
    protected TMP_Text Damage_White_Text;

    [SerializeField, Tooltip("Type Writer")]
    protected TypewriterByCharacter Writer;

    /// <summary>
    /// 전투 배속에 따른 속도 배율
    /// </summary>
    protected float Effect_Speed_Multiple = 1f;

    public bool Is_Pause { get; protected set; }

    public virtual void ShowText(BATTLE_SEND_DATA send_data, DAMAGE_TYPE dtype, Vector3 target_pos, DAMAGE_DIRECTION dir)
    {
        bool is_physics = send_data.Onetime.GetOnetimeEffectType() == ONETIME_EFFECT_TYPE.PHYSICS_DAMAGE;
        double point = is_physics ? send_data.Physics_Attack_Point : send_data.Magic_Attack_Point;

        SetPosition(target_pos);
        StartMove(point, dtype);
    }
    public virtual void ShowText(double point, DAMAGE_TYPE dtype, Vector3 target_pos)
    {
        SetPosition(target_pos);
        StartMove(point, dtype);
    }
    /// <summary>
    /// 토탈 포인트 전용
    /// </summary>
    /// <param name="total"></param>
    /// <param name="target_pos"></param>
    public virtual void ShowText(Total_Damage_Data total, Vector3 target_pos)
    {
        double point = total.GetTotalDamage();
        SetPosition(target_pos);
        StartMove(point, DAMAGE_TYPE.TOTAL);
    }

    /// <summary>
    /// Miss 전용
    /// </summary>
    /// <param name="target_pos"></param>
    public virtual void ShowText(Vector3 target_pos)
    {
        double point = 0;
        SetPosition(target_pos);
        StartMove(point, DAMAGE_TYPE.NORMAL);
    }

    protected void SetPosition(Vector3 target_pos)
    {
        Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, target_pos);
        GetComponent<RectTransform>().anchoredPosition3D = pos;
    }

    protected void StartMove(double pt, DAMAGE_TYPE dtype)
    {
        var sb = ZString.CreateStringBuilder();
        if (pt == 0)
        {
            sb.Append("<sprite=11>");
        }
        else
        {
            string point_str = pt.ToString();
            int cnt = point_str.Length;
            for (int i = 0; i < cnt; i++)
            {
                sb.AppendFormat("<sprite={0}>", point_str[i]);
            }
        }

        Damage_Main_Text.text = sb.ToString();
        Damage_White_Text.text = sb.ToString();

        if (Writer != null)
        {
            //Writer.SetTypewriterSpeed(Effect_Speed_Multiple);
            Writer.ShowText(sb.ToString());
        }
        StartAnimationDamageType(dtype);
    }

    public void SetFactory(DamageTextFactory fac)
    {
        Dmg_Factory = fac;
    }

    public virtual void SetSpeedMultiple(float multiple)
    {
        Effect_Speed_Multiple = multiple;
        if (Anim != null)
        {
            Anim.speed = multiple;
        }
    }

    protected void StartAnimationDamageType(DAMAGE_TYPE dtype)
    {
        if (Anim == null)
        {
            return;
        }
        //Anim.speed = Effect_Speed_Multiple;
        switch (dtype)
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
        UnusedDamageText();
    }

    public virtual void UnusedDamageText()
    {
        Dmg_Factory.UnusedDamageTextBase(this);
        Is_Pause = false;
    }

    public virtual void OnPause() 
    {
        Is_Pause = true;
        if (Anim != null)
        {
            Anim.enabled = false;
        }
    }

    public virtual void OnResume() 
    {
        Is_Pause = false;
        if (Anim != null)
        {
            Anim.enabled = true;
        }
    }

    public void Spawned()
    {
        Damage_Main_Text.text = string.Empty;
        Damage_White_Text.text = string.Empty;
        //Point = 0;
        //Send_Data = null;
        //Move_Type = UIEaseBase.MOVE_TYPE.NONE;
        //Damage_Type = DAMAGE_TYPE.NORMAL;
    }
    public void Despawned()
    {
        
    }

    
}
