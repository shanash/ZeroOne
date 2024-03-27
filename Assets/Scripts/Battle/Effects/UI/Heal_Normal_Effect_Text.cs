using Cysharp.Text;
using Febucci.UI;
using System;
using TMPro;
using UnityEngine;
/// <summary>
/// 사용 안함. 삭제 예정
/// </summary>
public class Heal_Normal_Effect_Text : EffectBase
{
    [SerializeField, Tooltip("Heal Text")]
    TMP_Text Heal_Text;

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
        double recovery_hp = Math.Truncate((double)data[0]);
        string hp_str = recovery_hp.ToString();
        var sb = ZString.CreateStringBuilder();
        int cnt = hp_str.Length;
        for (int i = 0; i < cnt; i++)
        {
            sb.AppendFormat("<sprite={0}>", hp_str[i]);
        }
        Show_Text = sb.ToString();
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
        Heal_Text.text = "";
    }
}
