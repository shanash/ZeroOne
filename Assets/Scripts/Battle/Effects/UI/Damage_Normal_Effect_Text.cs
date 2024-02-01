using Cysharp.Text;
using TMPro;
using UnityEngine;

public class Damage_Normal_Effect_Text : EffectBase
{
    [SerializeField, Tooltip("Damage Text")]
    TMP_Text Damage_Text;

    readonly float VELOCITY = 1.5f;


    public override void SetData(params object[] data)
    {
        if (data.Length != 1)
        {
            return;
        }

        BATTLE_SEND_DATA send_data = (BATTLE_SEND_DATA)data[0];

        //double dmg = Math.Truncate(send_data.Damage);
        double dmg = send_data.Damage;

        if (send_data.Duration_Effect_Type == DURATION_EFFECT_TYPE.NONE)
        {
            Damage_Text.text = ZString.Format("-{0:0.##}", dmg);
        }
        else
        {
            string txt_color = GetDurationEffectColorText(send_data.Duration_Effect_Type);
            if (!string.IsNullOrEmpty(txt_color))
            {
                if (send_data.Duration_Effect_Type == DURATION_EFFECT_TYPE.DAMAGE_REDUCE)
                {
                    Damage_Text.text = ZString.Format("<color=#{0}><size=30>피해감소</size></color>\n-{1:0.##}", txt_color, dmg);
                }
                else
                {
                    Damage_Text.text = ZString.Format("<color=#{0}>-{1:0.##}</color>", txt_color, dmg);
                }
            }
            else
            {
                Damage_Text.text = ZString.Format("-{0:0.##}", dmg);
            }
        }
    }

    string GetDurationEffectColorText(DURATION_EFFECT_TYPE dtype)
    {
        string text_color = string.Empty;
        switch (dtype)
        {
            case DURATION_EFFECT_TYPE.DAMAGE_REDUCE:
                text_color = "ffff00";
                break;
            case DURATION_EFFECT_TYPE.POISON:
                text_color = "CD00BE";
                break;
            case DURATION_EFFECT_TYPE.STUN:
                break;
            case DURATION_EFFECT_TYPE.SILENCE:
                break;
            case DURATION_EFFECT_TYPE.BIND:
                break;
            case DURATION_EFFECT_TYPE.FREEZE:
                text_color = "4CAAFF";
                break;
        }
        return text_color;
    }

    private void Update()
    {
        if (Is_Action && !Is_Pause)
        {
            Delta += Time.deltaTime * Effect_Speed_Multiple;

            var pos = this.transform.position;
            pos.y += VELOCITY * Time.deltaTime;
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
    }



}
