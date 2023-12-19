using Cysharp.Text;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Heal_Normal_Effect_Text : EffectBase
{
    [SerializeField, Tooltip("Heal Text")]
    TMP_Text Heal_Text;

    readonly float VELOCITY = 100f;


    public override void SetData(params object[] data)
    {
        if (data.Length != 1)
        {
            return;
        }
        double recovery_hp = (double)data[0];
        Heal_Text.text = ZString.Format("+{0:0.##}", recovery_hp);
    }

    private void Update()
    {
        if (Is_Action)
        {
            Delta += Time.deltaTime;

            var pos = this.transform.localPosition;
            pos.y += VELOCITY * Time.deltaTime;
            this.transform.localPosition = pos;
            if (Delta > Duration)
            {
                Finish_Callback?.Invoke(this);
                UnusedEffect();
            }
        }
    }


    public override void Despawned()
    {
        Heal_Text.text = "";
    }
}
