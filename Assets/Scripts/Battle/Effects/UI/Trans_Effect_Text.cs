using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Trans_Effect_Text : EffectBase
{
    [SerializeField, Tooltip("Trans Text")]
    TMP_Text Trans_Text;

    readonly float VELOCITY = 100f;



    public override void SetData(params object[] data)
    {
        if (data.Length != 1)
        {
            return;
        }
        DURATION_EFFECT_TYPE etype = (DURATION_EFFECT_TYPE)data[0];
        string trans_name = GetDurationEffectTypeTransName(etype);
        Trans_Text.text = trans_name;
    }

    string GetDurationEffectTypeTransName(DURATION_EFFECT_TYPE etype)
    {
        string trans_name = string.Empty;

        switch (etype)
        {
            case DURATION_EFFECT_TYPE.DAMAGE_REDUCE:
                trans_name = "<color=#ffff00>피해감소</color>";
                break;
            case DURATION_EFFECT_TYPE.POISON:
                trans_name = "<color=#CD00BE>중독</color>";
                break;
            case DURATION_EFFECT_TYPE.SILENCE:
                trans_name = "<color=#A9A9A9>침묵</color>";
                break;
            case DURATION_EFFECT_TYPE.FREEZE:
                trans_name = "<color=#4CAAFF>빙결</color>";
                break;
            case DURATION_EFFECT_TYPE.BIND:
                trans_name = "<color=#00AE02>결박</color>";
                break;
        }
        return trans_name;
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
        Trans_Text.text = string.Empty;
    }
}
