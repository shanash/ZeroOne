using Cysharp.Text;
using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BehaviorNode : UnitBase
{
    /// <summary>
    /// 프레임 이미지.
    /// 아군 : 흰색, 적군 : 빨강
    /// </summary>
    [SerializeField, Tooltip("Frame Image")]
    Image Frame_Image;

    /// <summary>
    /// 포지션 번호
    /// </summary>
    [SerializeField, Tooltip("Icon Name")]
    TMP_Text Icon_Name;

    HeroBase Hero;

    Vector2 Target_Position = Vector2.zero;
    float Move_Delta;
    const float Duration = 0.5f;

    protected override void InitStates()
    {
        FSM = new UnitStateSystem<UnitBase, BattleManager, BattleUIManager>();

        FSM.AddTransition(new UnitStateIdle());
        FSM.AddTransition(new UnitStateMove());
        FSM.AddTransition(new UnitStateEnd());

        FSM.Lazy_Init_Setting(this, null, null, UNIT_STATES.IDLE);
    }

    public void SetHeroBase(HeroBase h)
    {
        Hero = h;

        SetIconName(Hero);
        SetIconFrame(Hero);
    }

    public HeroBase GetHeroBase()
    {
        return Hero;
    }
    void SetIconName(HeroBase h)
    {
        Icon_Name.text = ZString.Format("{0}", (int)Hero.Position_Type);
    }
    void SetIconFrame(HeroBase h)
    {
        if (Hero.Team_Type == TEAM_TYPE.LEFT)
        {
            Frame_Image.color = Color.white;
        }
        else
        {
            Frame_Image.color = Color.red;
        }
    }

    public double GetHeroAccumRapidityPoint()
    {
        return Hero.Accum_Rapidity_Value;
    }

    public void SetMovePositionY(float y)
    {
        Target_Position = this.transform.localPosition;
        Target_Position.y = y;
        ChangeState(UNIT_STATES.MOVE);
    }

    #region States
    
    public override void UnitStateIdle()
    {
        
    }

    public override void UnitStateMoveBegin()
    {
        Move_Delta = 0f;
    }
    public override void UnitStateMove()
    {
        Move_Delta += Time.smoothDeltaTime;
        if (Move_Delta > Duration)
        {
            Move_Delta = Duration;
            ChangeState(UNIT_STATES.IDLE);
        }
        this.transform.localPosition = Vector2.Lerp(this.transform.localPosition, Target_Position, Move_Delta / Duration);
    }
    public override void UnitStateMoveExit()
    {
        
    }

    public override void UnitStateEndBegin()
    {
        
    }

    #endregion


    public override void Despawned()
    {
        Hero = null;
    }


    [ContextMenu("ToStringBehaviorNode")]
    public void ToStringBehaviorNode()
    {
        var sb = ZString.CreateStringBuilder();
        sb.AppendLine("== ToStringBehaviorNode ==");
        sb.AppendLine(Hero.ToString());
        Debug.Log(sb.ToString());
    }
}
