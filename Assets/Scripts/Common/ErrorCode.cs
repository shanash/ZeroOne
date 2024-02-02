public enum ERROR_CODE
{
    //TODO: RESULT_CODE가 더 낫지 않을까 합니다..
    SUCCESS = 100,
    LEVEL_UP_SUCCESS = 110,

    NOT_EFFECT_SUCCESS = 130,
    FAILED = 400,


    ALL_CLEAR_STAGE = 1001,

    NOT_ENOUGH_ITEM = 20001,                                                        //  아이템 부족
    NOT_ENOUGH_GOLD,                                                                //  금화 부족
    NOT_ENOUGH_SOUL_STONE,                                                          //  다이아 부족
    NOT_ENOUGH_SKILL_CARD,
    NOT_ENOUGH_PARTNER,
    NOT_ENOUGH_PET,
    NOT_ENOUGH_WEAPON,
    NOT_ENOUGH_SHIELD,
    NOT_ENOUGH_RING,
    NOT_ENOUGH_ARMOR,
    NOT_ENOUGH_SKILL_STONE,
    NOT_ENOUGH_ARMOR_MATERIALS,
    NOT_ENOUGH_SMITH_COUNT,
    NOT_ENOUGH_ALCHEMIST_COUNT,
    NOT_ENOUGH_SLOT,
    NOT_ENOUGH_BINGO_POINT,
    NOT_ENOUGH_ALL,


    NOT_WORK = 30001,

    OVER_MAX_ITEM_BOUND = 40001,                                                    //  아이템 최대 보유 개수 초과
    NOT_OPEN_CONTENT,
    NOT_CONDITION_MET,
    NOT_READY_ADS_MOVIE,                                                            //  아직 광고가 준비되지 않았습니다.
    ALREADY_MAX_LEVEL,                                                              //  이미 최고 레벨임
    DUPLICATE_NICKNAME,                                                             //  중복된 닉네임
    SERVER_MAINTERNANCE,                                                            //  점검 중
    PASSWORD_ERROR,
    BLOCKED_DEVICE,                                                                 //  차단당한 디바이스
    USER_WITHDRAW_ING,                                                              //  탈퇴진행중
    NEED_FORCE_UPDATE,                                                              //  강제 업데이트 필요
    NEED_REFRESH_TOKEN,                                                             //  토큰 리프레쉬 필요
    ALREADY_RECV_REWARD,                                                            //  이미 받은 보상
    ALREADY_OPEN_SKILL,                                                             //  이미 오픈된 스킬 카드
    ALREADY_OPEN_ARMOR,                                                             //  이미 오픈된 갑옷 카드
    REMAIN_TIME,                                                                    //  시간이 남아 있음
    INVALID_DATETIME,                                                               //  허용 시간이 아님
    NOT_ENABLE_WORK,                                                                //  작업 불가
    NOT_EXIST_EMPTY_SLOT,                                                           //  빈 슬롯이 없음
    ALREADY_COMPLETE,
    NOT_CONNECT_NETWORK = 50001,
}
