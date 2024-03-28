using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;


/// <summary>
/// 루시아
/// </summary>
public class Hero_100001 : HeroBase_V2
{


    #region States

    public override void UnitStateSkillReady01Begin()
    {
        SetPlayableDirector();
    }
    public override void UnitStateSkillReady01()
    {
        ChangeState(UNIT_STATES.SKILL_1);
    }


    #endregion


    #region Etc Funcs

    protected override void SetPlayableDirector()
    {
        base.SetPlayableDirector();
        StartPlayableDirector();
    }

    protected override void UnsetPlayableDirector()
    {
        base.UnsetPlayableDirector();
    }

   

    //public override void TriggerEventListener(string trigger_id, EventTriggerValue evt_val)
    //{
    //    //  감추기
    //    if (trigger_id.Trim().Equals(TRIGGER_EVENT_IDS.chr_hide.ToString()))
    //    {
    //        HideCharacters(evt_val);
    //    }   //  보이기
    //    else if (trigger_id.Trim().Equals(TRIGGER_EVENT_IDS.chr_show.ToString()))
    //    {
    //        ShowCharacters(evt_val);
    //    }
    //}
    #endregion


}
