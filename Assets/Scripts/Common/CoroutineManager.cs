using FluffyDuck.Util;

/// <summary>
/// 일반 클래스에서도 코루틴을 사용할 수 있게 도와주는 역할...인데
/// 딱히 관리를 빡세게 할 필요를 아직 못 느끼겠다
/// </summary>
public class CoroutineManager : MonoSingleton<CoroutineManager>
{
    protected override bool Is_DontDestroyOnLoad => true;

    protected override void Initialize()
    {
    }
}
