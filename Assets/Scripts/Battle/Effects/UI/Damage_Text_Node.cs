using Cysharp.Text;
using Febucci.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Damage_Text_Node : EffectBase
{
    [SerializeField, Tooltip("Box")]
    Transform Container;

    [SerializeField, Tooltip("Damage Text")]
    TMP_Text Damage_Text;

    [SerializeField, Tooltip("Weak")]
    SpriteRenderer Weak_Icon;

    [SerializeField, Tooltip("Damage Duration")]
    float Damage_Duration;


    [SerializeField, Tooltip("Easing Effect List")]
    List<EffectEasingBase> Effect_Easing_List;



    public override void StartParticle(float duration, bool loop = false)
    {
        //base.StartParticle(duration, loop);
        EasingExec(EffectEasingBase.EASING_MOVE_TYPE.IN, EasingInCallback);
    }

    void EasingInCallback()
    {
        base.StartParticle(Damage_Duration);
    }



    void EasingExec(EffectEasingBase.EASING_MOVE_TYPE move_type, System.Action cb = null)
    {
        for (int i = 0; i < Effect_Easing_List.Count; i++)
        {
            if (i == 0)
            {
                Effect_Easing_List[i].StartMove(move_type, cb);
            }
            else
            {
                Effect_Easing_List[i].StartMove(move_type);
            }
        }
    }

    public override void SetData(params object[] data)
    {
        if (data.Length != 1)
        {
            return;
        }

        if (data[0] is BATTLE_SEND_DATA)
        {
            var send_data = (BATTLE_SEND_DATA)data[0];
            ONETIME_EFFECT_TYPE etype = send_data.Onetime.GetOnetimeEffectType();
            bool is_critical = etype == ONETIME_EFFECT_TYPE.MAGIC_DAMAGE ? send_data.Is_Magic_Critical : send_data.Is_Physics_Critical;
            double dmg = etype == ONETIME_EFFECT_TYPE.MAGIC_DAMAGE ? Math.Truncate(send_data.Magic_Attack_Point) : Math.Truncate(send_data.Physics_Attack_Point);
            string dmg_str = dmg.ToString();
            int cnt = dmg_str.Length;
            if (cnt > 0)
            {
                var sb = ZString.CreateStringBuilder();
                //  크리티컬 폰트 추가
                if (is_critical)
                {
                    sb.Append("<sprite=10>");
                }
                for (int i = 0; i < cnt; i++)
                {
                    sb.AppendFormat("<sprite={0}>", dmg_str[i]);
                }

                Damage_Text.text = sb.ToString();

                Weak_Icon.gameObject.SetActive(send_data.Is_Weak);
            }
        }
    }

    private void Update()
    {
        if (Is_Action && !Is_Pause)
        {
            Delta += Time.deltaTime * Effect_Speed_Multiple;

            if (Delta > Duration)
            {
                EasingExec(EffectEasingBase.EASING_MOVE_TYPE.OUT, EasingOutCallback);
                Is_Action = false;
            }
        }
    }

    void EasingOutCallback()
    {
        Finish_Callback?.Invoke(this);
        UnusedEffect();
    }

    public override void Show(bool show)
    {
        Container.gameObject.SetActive(show);
    }

    public override void Despawned()
    {
        Damage_Text.text = string.Empty;
        
    }


}
