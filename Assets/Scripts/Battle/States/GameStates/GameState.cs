public enum GAME_STATES
{
    NONE = 0,
    INIT,
    READY,

    WAVE_INFO,          //  다음 스테이지 진입시 잠시 대기 (다음 웨이브 번호 정보 정도를 보여줄 수 있는 시간)
    SPAWN,

    PLAYING,            //  전투 
    NEXT_WAVE,          //  다음 스테이지
    WAVE_RUN,

    PAUSE,
    GAME_OVER_WIN,
    GAME_OVER_LOSE,

    END
}

public class GameState<B, U>
{
    private GAME_STATES _transID = GAME_STATES.NONE;
    public GAME_STATES TransID
    {
        get { return _transID; }
        protected set { _transID = value; }
    }
    protected float Delta_Time;
    public GameState()
    {
    }

    public virtual void EnterState(B mng, U ui) { }
    public virtual void UpdateState(B mng, U ui) { }
    public virtual void ExitState(B mng, U ui) { }
    public virtual void FinallyState(B mng, U ui) { }
}
