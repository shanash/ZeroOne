using UnityEngine;

public enum GAME_TYPE
{
    NONE = 0,
    STORY_MODE,
}

public enum TEAM_POSITION_TYPE
{
    NONE = -1,

    FRONT_TOP,
    FRONT_MIDDLE,
    FRONT_BOTTOM,

    CENTER_TOP,
    CENTER_MIDDLE,
    CENTER_BOTTOM,

    REAR_TOP,
    REAR_MIDDLE,
    REAR_BOTTOM,

}

/// <summary>
/// 지속성 효과의 시간 계산에 따른 결과 타입(횟수 타입은 적용되지 않음)
/// </summary>
public enum DURATION_CALC_RESULT_TYPE
{
    NONE = 0,
    REPEAT_INTERVAL,            //  반복 주기에 걸리는 상황
    FINISH,                     //  지속성 효과 종료
    REPEAT_AND_FINISH,          //  반복 주기 및 종료(두가지가 동시에 걸릴 경우)
}


public enum HERO_PLAY_ANIMATION_TYPE
{
    NONE = 0,
    
    PREPARE_01,
    PREPARE_02,
    
    IDLE_01,
    IDLE_02,

    READY_01,
    READY_02,

    JUMP_01,
    JUMP_02,

    RUN_01,
    RUN_02,
    RUN_03,

    WALK_01,

    DAMAGE_01,
    DAMAGE_02,
    DAMAGE_03,

    ATTACK_01,
    ATTACK_02,
    ATTACK_03,

    SKILL_01,
    SKILL_02,
    SKILL_03,

    DEATH_01,

    WIN_01,

}


/// <summary>
/// 본 클래스는 다양한 enum  및 static 한 변수를 선언하기 위한 함수.
/// 일부 게임 내에서 공용으로 사용할만한 static 함수도 정의하여 사용 가능
/// </summary>
public class GameDefine : MonoBehaviour
{
    public static readonly bool USE_DEBUG_MODE = true;

    public static readonly string SCENE_LOAD = "load";
    public static readonly string SCENE_HOME = "home";

    /// <summary>
    /// 년월일시 포멧
    /// </summary>
    public static readonly string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss";
    /// <summary>
    /// 년/월/일 포멧
    /// </summary>
    public static readonly string DATE_FORMAT = "yyyy-MM-dd";

    public static readonly int SCREEN_BASE_WIDTH = 1440;
    public static readonly int SCREEN_BASE_HEIGHT = 1080;
    public static readonly Vector2 SCREEN_BASE_SIZE = new Vector2(SCREEN_BASE_WIDTH, SCREEN_BASE_HEIGHT);

    public static readonly string TAG_HERO = "Hero";
    public static readonly string TAG_MONSTER = "Monster";


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void SetUp()
    {
        _ = new GameObject("GameDefine").AddComponent<GameDefine>();
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void Awake()
    {
        gameObject.hideFlags = HideFlags.HideAndDontSave;
        DontDestroyOnLoad(gameObject);
    }
}
