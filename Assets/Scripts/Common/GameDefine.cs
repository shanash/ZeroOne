using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 게임 타입 
/// </summary>
public enum GAME_TYPE
{
    NONE = 0,
    EDITOR_SKILL_PREVIEW_MODE,          //  에디터에서 스킬 프리뷰 모드
    EDITOR_SKILL_EDIT_MODE,             //  에디터에서 스킬 에디트 모드(아직 미개발)

    STORY_MODE,                         //  스토리 모드
    BOSS_DUNGEON_MODE,                  //  보스 던전 모드
}
/// <summary>
/// 전투 배속 타입 <b>(주석에서 사용할 태그 테스트)</b><br/>
/// <b>Bold</b><br/>
/// <i>Italic</i><br/>
/// <u>Underline</u><br/>
/// <br>Line Break</br><br/>
/// </summary>
public enum BATTLE_SPEED_TYPE
{
    NORMAL_TYPE = 0,                //  x1
    FAST_SPEED_X2,                 //  x2
    FAST_SPEED_X3,                   //  x3
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

    IDLE_01,
    IDLE_02,

    RUN_01,

    DAMAGE_01,
    DAMAGE_02,
    DAMAGE_03,

    STUN,

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

    /// <summary>
    /// 로비 캐릭터 최대 선택 가능 수
    /// </summary>
    public static readonly int MAX_LOBBY_CHARACTER_COUNT = 10;

    /// <summary>
    /// 전투 배속 정보
    /// </summary>
    public static readonly Dictionary<BATTLE_SPEED_TYPE, float> GAME_SPEEDS = new Dictionary<BATTLE_SPEED_TYPE, float>
    {
        { BATTLE_SPEED_TYPE.NORMAL_TYPE, 1f },
        { BATTLE_SPEED_TYPE.FAST_SPEED_X2, 2f },
        { BATTLE_SPEED_TYPE.FAST_SPEED_X3, 3f },
    };


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
    /// <summary>
    /// 테이블 데이터에 지정되어 있는 문자열 ID를 이용하여 각 언어 타입별로 구분하여 문자열 반환
    /// </summary>
    /// <param name="str_id"></param>
    /// <returns></returns>
    public static string GetLocalizeString(string str_id)
    {
        var lang = Application.systemLanguage;
        var splits = str_id.Split("_");
        if (splits.Length == 0)
            return string.Empty;
        string table_name = splits[0].ToLower();

        string find_string = string.Empty;
        var m = MasterDataManager.Instance;
        if (table_name.Equals("system"))
        {
            var lang_data = m.Get_SystemLangData(str_id);
            if (lang == SystemLanguage.Korean)
            {
                find_string = lang_data.kor;
            }
            else if (lang == SystemLanguage.Japanese)
            {
                find_string = lang_data.jpn;
            }
            else
            {
                find_string = lang_data.eng;
            }
        }
        else if (table_name.Equals("character") || table_name.Equals("monster"))
        {
            var lang_data = m.Get_CharacterLangData(str_id);
            if (lang == SystemLanguage.Korean)
            {
                find_string = lang_data.kor;
            }
            else if (lang == SystemLanguage.Japanese)
            {
                find_string = lang_data.jpn;
            }
            else
            {
                find_string = lang_data.eng;
            }
        }
        else if (table_name.Equals("skill"))
        {
            var lang_data = m.Get_SkillLangData(str_id);
            if (lang == SystemLanguage.Korean)
            {
                find_string = lang_data.kor;
            }
            else if (lang == SystemLanguage.Japanese)
            {
                find_string = lang_data.jpn;
            }
            else
            {
                find_string = lang_data.eng;
            }
        }
        else if (table_name.Equals("item"))
        {
            var lang_data = m.Get_ItemLangData(str_id);
            if (lang == SystemLanguage.Korean)
            {
                find_string = lang_data.kor;
            }
            else if (lang == SystemLanguage.Japanese)
            {
                find_string = lang_data.jpn;
            }
            else
            {
                find_string = lang_data.eng;
            }
        }
        else if (table_name.Equals("dialog"))
        {
            var lang_data = m.Get_DialogLangData(str_id);
            if (lang == SystemLanguage.Korean)
            {
                find_string = lang_data.kor;
            }
            else if (lang == SystemLanguage.Japanese)
            {
                find_string = lang_data.jpn;
            }
            else
            {
                find_string = lang_data.eng;
            }
        }
        else if (table_name.Equals("story"))
        {
            var lang_data = m.Get_StoryLangData(str_id);
            if (lang == SystemLanguage.Korean)
            {
                find_string = lang_data.kor;
            }
            else if (lang == SystemLanguage.Japanese)
            {
                find_string = lang_data.jpn;
            }
            else
            {
                find_string = lang_data.eng;
            }
        }
        else
        {
            Debug.Assert(false);
        }

        return find_string;
    }
}
