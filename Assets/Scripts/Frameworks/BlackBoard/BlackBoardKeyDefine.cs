namespace FluffyDuck.Util
{
    /// <summary>
    /// 이 값은 사용자 정의 값이다.
    /// BlackBoard에서 해당 이넘 타입을 사용하고 있음
    /// </summary>
    public enum BLACK_BOARD_KEY
    {
        NONE = 0,
        TEST_KEY_ID,
        GAME_TYPE,

        DUNGEON_ID,
        EDITOR_STAGE_ID,

        SELECTED_LOVE_LEVEL,

        OPEN_STORY_STAGE_DUNGEON_ID,    //  스토리 스테이지 던전 오픈


        //  test
        PLAYER_ATTACK_INC_MULTIPLE          = 100001,       //  플레이어 캐릭터 공격력 증가
        PLAYER_DEFENSE_INC_MULTIPLE,                       //  플레이어 캐릭터 방어력 증가
        PLAYER_CRITICAL_CHANCE_INC_MULTIPLE,                //  플레이어 캐릭터 크리티컬 확률 증가


        ENEMY_DEFENSE_INC_MULTIPLE,                         //  적 캐릭터 방어력 증가
    }
}

