public class ActorLucia : ActorBase
{
    public override void ActorStateReactBegin()
    {
        base.ActorStateReactBegin();

        // 챗모션 1200002015과 1200002016는 플레이 이후 그 상태를 유지하도록 합니다
        // 루시아가 브라 잡고 있을때 브라끈을 풀면 풀린상태 유지
        if (Current_Chat_Motion_Id == 1200002015 || Current_Chat_Motion_Id == 1200002016)
        {
            react_track_entries.Clear();
        }

        // 맨가슴 보여서 부끄러워지는 상태로 이동하는 리액션일때 왼쪽 브라끈 애니메이션을 비워줘야
        // 루시아가 자연스럽게 브라끈을 끌어당깁니다.
        if (Current_Chat_Motion_Id == 1200002018)
        {
            Skeleton.AnimationState.SetEmptyAnimation(21, 0);
        }

        // 본래 상태로 돌아오는 리액션을 할 때 아이들 애니메이션을 자연스럽게 본래 상태로 되돌립니다.
        if (Current_Chat_Motion_Id == 1200002021)
        {
            Skeleton.AnimationState.SetAnimation(0, "00_idle_01", true).MixDuration = 1.0f;
        }
    }
}
