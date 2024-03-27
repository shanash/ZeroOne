using Cysharp.Text;
using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossLifeBarNode : LifeBarNode
{
    [Space()]
    [Header("Boss Info")]
    [SerializeField, Tooltip("Boss Name")]
    TMP_Text Boss_Name;

    [SerializeField, Tooltip("Boss Life")]
    TMP_Text Boss_Life;

    public override void SetHeroBaseV2(HeroBase_V2 hero)
    {
        this.Hero = hero;
        if (this.Hero.Team_Type == TEAM_TYPE.RIGHT)
        {
            this.Hero.Slot_Events += SkillSlotEventCallback;
        }
        Is_Boss_Gauge = true;

        Boss_Name.text = this.Hero.GetBattleUnitData().GetUnitName();

        SetLifeSliderPercent(1f, true);
    }

    public override void SetLifeSliderPercent(float percent, bool show)
    {
        base.SetLifeSliderPercent(percent, true);

        //  boss life text
        Boss_Life.text = ZString.Format("{0}/{1}", Hero.Life, Hero.Max_Life);
    }

    public override void ShowLifeBar(float duration)
    {
        Box.gameObject.SetActive(true);
    }
    public override void HideLifeBar()
    {
        Box.gameObject.SetActive(false);
    }

    protected override void UpdatePosition()
    {
        //  nothing. 상위 UpdatePosition을 호출하면 안됨
    }

    public override void Spawned()
    {
        Main_Life_Bar.value = 1f;
        Sub_Life_Bar.value = 1f;

        CustomUpdateManager.Instance.RegistCustomUpdateComponent(this);
    }

    public override void Despawned()
    {
        ClearDurationSkillIcons();
        if (Slider_Flush_Coroutine != null)
        {
            StopCoroutine(Slider_Flush_Coroutine);
        }
        Slider_Flush_Coroutine = null;

        CustomUpdateManager.Instance.DeregistCustomUpdateComponent(this);
    }
}
