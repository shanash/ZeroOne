using Cysharp.Text;
using Febucci.UI;
using System;
using TMPro;
using UnityEngine;


/// <summary>
/// 데미지 포인트 표시<br/>
/// 0~9 까지 이미지 폰트 사용. 사용방법. 각 숫자 텍스트를 <sprite=0>방식으로 적용 필요.<br/>
/// 크리티컬 <sprite=10><br/>
/// Miss(회피) <sprite=11><br/>
/// </summary>
public class Damage_Normal_Effect_Text : EffectBase
{
    [SerializeField, Tooltip("Damage Text")]
    TMP_Text Damage_Text;

    [SerializeField, Tooltip("Typewriter")]
    TypewriterByCharacter Writer;

    readonly float VELOCITY = 2f;

    string Show_Text = string.Empty;

    public override void StartParticle(float duration, bool loop = false)
    {
        base.StartParticle(duration, loop);
        Writer.SetTypewriterSpeed(Effect_Speed_Multiple);
        Writer.ShowText(Show_Text);
    }

    public override void SetData(params object[] data)
    {
        if (data.Length != 1)
        {
            return;
        }

        BATTLE_SEND_DATA send_data = (BATTLE_SEND_DATA)data[0];

        ONETIME_EFFECT_TYPE etype = send_data.Onetime.GetOnetimeEffectType();

        double dmg = etype == ONETIME_EFFECT_TYPE.MAGIC_DAMAGE ? Math.Truncate(send_data.Magic_Attack_Point) : Math.Truncate(send_data.Physics_Attack_Point);
        if (dmg <= 0)
        {
            bool a = false;
        }
        string dmg_str = dmg.ToString();
        bool use_sprite_font = true;
        if (use_sprite_font)
        {
            int cnt = dmg_str.Length;
            var sb = ZString.CreateStringBuilder();
            for (int i = 0; i < cnt; i++)
            {
                sb.AppendFormat("<sprite={0}>", dmg_str[i]);
            }
            Show_Text = sb.ToString();
        }
        else
        {
            Show_Text = dmg.ToString();
        }
       
    }


    private void Update()
    {
        if (Is_Action && !Is_Pause)
        {
            Delta += Time.deltaTime * Effect_Speed_Multiple;

            var pos = this.transform.position;
            pos.y += VELOCITY * Time.deltaTime * Effect_Speed_Multiple;
            this.transform.position = pos;
            if (Delta > Duration)
            {
                Finish_Callback?.Invoke(this);
                UnusedEffect();
            }
        }
    }

    public override void Show(bool show)
    {
        this.gameObject.SetActive(show);
    }

    public override void Despawned()
    {
        Damage_Text.text = "";
        Show_Text = string.Empty;
    }



}
