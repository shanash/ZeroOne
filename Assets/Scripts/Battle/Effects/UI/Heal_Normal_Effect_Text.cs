using Cysharp.Text;
using TMPro;
using UnityEngine;

public class Heal_Normal_Effect_Text : EffectBase
{
    [SerializeField, Tooltip("Heal Text")]
    TMP_Text Heal_Text;

    readonly float VELOCITY = 3.5f;


    public override void SetData(params object[] data)
    {
        if (data.Length != 1)
        {
            return;
        }
        double recovery_hp = (double)data[0];
        Heal_Text.text = ZString.Format("{0}", recovery_hp);
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
