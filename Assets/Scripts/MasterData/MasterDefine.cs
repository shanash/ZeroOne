///	<summary>
///	캐릭터가 속한 종족 타입
///	</summary>
public enum TRIBE_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>1 인간종족</summary>
	HUMAN = 1,
	/// <summary>2 엘프족</summary>
	ELF = 2,
	/// <summary>3 수인족</summary>
	WEREBEAST = 3,
	/// <summary>4 안드로이드</summary>
	ANDROID = 4,
	/// <summary>5 악마</summary>
	DEVIL = 5,
	/// <summary>6 천사</summary>
	ANGEL = 6,
}

///	<summary>
///	스테이지 난이도
///	</summary>
public enum STAGE_DIFFICULTY_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>1 노말</summary>
	NORMAL = 1,
	/// <summary>2 어려움</summary>
	HARD = 2,
	/// <summary>3 매우 어려움</summary>
	VERY_HARD = 3,
}

///	<summary>
///	유저가 캐릭터 사거리를 대략적으로 쉽게 파악할 수 있게 편의성 용도로 제공할 표현
///	</summary>
public enum POSITION_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>1 전열 배치</summary>
	FRONT = 1,
	/// <summary>2 중열 배치</summary>
	MIDDLE = 2,
	/// <summary>3 후열 배치</summary>
	BACK = 3,
}

///	<summary>
///	역할군 표현
///	</summary>
public enum ROLE_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>1 탱커</summary>
	TANKER = 1,
	/// <summary>2 딜러</summary>
	DEARLER = 2,
	/// <summary>3 서포터</summary>
	SUPPORTER = 3,
	/// <summary>4 힐러</summary>
	HEALER = 4,
}

///	<summary>
///	npc 타입 
///	</summary>
public enum NPC_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>1 우호 NPC</summary>
	NPC = 1,
	/// <summary>2 일반 몬스터</summary>
	NORMAL = 2,
	/// <summary>3 엘리트 몬스터</summary>
	ELITE = 3,
	/// <summary>4 보스 몬스터</summary>
	BOSS = 4,
}

///	<summary>
///	타겟 룰 타입
///	다양한 방식의 타겟 룰을 지정하여 사용할 수 있다.
///	</summary>
public enum TARGET_RULE_TYPE
{
	/// <summary>0 임의 타겟 선택</summary>
	RANDOM = 0,
	/// <summary>1 자신 선택</summary>
	SELF = 1,
	/// <summary>2 전체 선택</summary>
	ALL = 2,
	/// <summary>3 자신을 제외한 아군 전체 선택</summary>
	ALL_WITHOUT_ME = 3,
	/// <summary>4 자신을 제외한 가장 가까운 아군 선택</summary>
	ALLY_WITHOUT_ME_NEAREST = 4,
	/// <summary>5 자신을 제외한 가장 먼 아군 선택</summary>
	ALLY_WITHOUT_ME_FURTHEST = 5,
	/// <summary>6 가장 가까운 적 선택 (순번 컬럼을 연동하여 앞에서부터의 순서대로)</summary>
	NEAREST = 6,
	/// <summary>7 가장 거리가 먼 적 선택 (순번 컬럼을 연동하여 뒤에서부터의 순서대로)</summary>
	FURTHEST = 7,
	/// <summary>8 가장 가까운 적 우선 선택(순번 컬럼 연동하여 순서대로) 후 일정 영역내의 뒤에있는 타겟의 추가 선택</summary>
	NEAREST_ADD_BACK_RANGE = 8,
	/// <summary>9 가장 거리가 먼 적 우선 선택 (순번 컬럼을 연동하여 뒤에서부터의 순서대로) 후 일정 영역내의 앞에 있는 타겟 추가 선택</summary>
	FURTHEST_ADD_FRONT_RANGE = 9,
	/// <summary>10 가장 가까운 적 우선 선택(순번 컬럼 연동하여 순서대로) 후 일정 영역내의(주변) 타겟의 추가 선택</summary>
	NEAREST_ADD_ARROUND_RANGE = 10,
	/// <summary>11 가장 거리가 먼 적 우선 선택 (순번 컬럼을 연동하여 뒤에서부터의 순서대로) 후 일정 영역내의 (주변) 타겟 추가 선택</summary>
	FURTHEST_ADD_ARROUND_RANGE = 11,
	/// <summary>3001 남은 체력이 가장 낮은 타겟 선택</summary>
	LOWEST_LIFE_VALUE = 3001,
	/// <summary>3003 공격력이 가장 낮은 타겟 선택</summary>
	LOWEST_ATTACK = 3003,
	/// <summary>3004 방어력이 가장 낮은 타겟 선택</summary>
	LOWEST_DEFENSE = 3004,
	/// <summary>2003 공격력이 가장 높은 타겟 선택</summary>
	HIGHEST_ATTACK = 2003,
	/// <summary>2004 방어력이 가장 높은 타겟 선택</summary>
	HIGHEST_DEFENSE = 2004,
	/// <summary>9999 가장 가까운 상대 타겟 선택(화면상에서 적에게 접근하기 위한 용도)</summary>
	APPROACH = 9999,
	/// <summary>1001 리더 선택 (리더 없음)</summary>
	LEADER_HIGH_PRIORITY = 1001,
	/// <summary>2001 남은 체력이 가장 많은 타겟 선택</summary>
	HIGHEST_LIFE_VALUE = 2001,
	/// <summary>2002 남은 체력 비율이 가장 높은 타겟 선택</summary>
	HIGHEST_LIFE_RATE = 2002,
	/// <summary>2006 명중률이 가장 높은 타겟 선택 (명중률 속성이 있다면)</summary>
	HIGHEST_ACCURACY = 2006,
	/// <summary>2007 회피율이 가장 높은 타겟 선택 (회피율 속성이 있다면)</summary>
	HIGHEST_EVASION = 2007,
	/// <summary>2008 남은 체력이 가장 높은 인간 종족 선택</summary>
	HIGHEST_LIFE_VALUE_HUMAN = 2008,
	/// <summary>2009 남은 체력이 가장 높은 엘프 종족 선택</summary>
	HIGHEST_LIFE_VALUE_ELF = 2009,
	/// <summary>2010 남은 체력이 가장 높은 수인 종족 선택</summary>
	HIGHEST_LIFE_VALUE_WEREBEAST = 2010,
	/// <summary>2011 남은 체력이 가장 높은 안드로이드 선택</summary>
	HIGHEST_LIFE_VALUE_ANDROID = 2011,
	/// <summary>2012 남은 체력이 가장 높은 악마 선택</summary>
	HIGHEST_LIFE_VALUE_DEVIL = 2012,
	/// <summary>2013 남은 체력이 가장 높은 천사 선택</summary>
	HIGHEST_LIFE_VALUE_ANGEL = 2013,
	/// <summary>2014 자신을 제외한 체력이 가장 높은 타겟 선택</summary>
	HIGHEST_LIFE_VALUE_WITHOUT_ME = 2014,
	/// <summary>2015 자신을 제외한 체력 비율이 가장 높은 타겟 선택</summary>
	HIGHEST_LIFE_RATE_WITHOUT_ME = 2015,
	/// <summary>2016 자신을 제외한 공격력이 가장 높은 타겟 선택</summary>
	HIGHEST_ATTACK_WITHOUT_ME = 2016,
	/// <summary>2017 자신을 제외한 방어력이 가장 높은 타겟 선택</summary>
	HIGHEST_DEFENSE_WITHOUT_ME = 2017,
	/// <summary>2018 자신을 제외한 공속이 가장 높은 타겟 선택</summary>
	HIGHEST_RAPIDITY_WITHOUT_ME = 2018,
	/// <summary>2019 자신을 제외한 명중률이 가장 높은 타겟 선택 (명중률 속성이 있다면)</summary>
	HIGHEST_ACCURACY_WITHOUT_ME = 2019,
	/// <summary>2020 자신을 제외한 회피율이 가장 높은 타겟 선택 (회피율 속성이 있다면)</summary>
	HIGHEST_EVASION_WITHOUT_ME = 2020,
	/// <summary>3002 남은 체력 비율이 가장 낮은 타겟 선택</summary>
	LOWEST_LIFE_RATE = 3002,
	/// <summary>3005 속도게이지가 가장 낮은 타겟 선택</summary>
	LOWEST_ACCUM_RAPIDITY_POINT = 3005,
	/// <summary>3006 명중률이 가장 낮은 타겟 선택</summary>
	LOWEST_ACCURACY = 3006,
	/// <summary>3007 회피율이 가장 낮은 타겟 선택</summary>
	LOWEST_EVASION = 3007,
	/// <summary>3008 남은 체력이 가장 낮은 인간 종족 선택</summary>
	LOWEST_LIFE_VALUE_HUMAN = 3008,
	/// <summary>3009 남은 체력이 가장 낮은 엘프 종족 선택</summary>
	LOWEST_LIFE_VALUE_ELF = 3009,
	/// <summary>3010 남은 체력이 가장 낮은 수인 종족 선택</summary>
	LOWEST_LIFE_VALUE_WEREBEAST = 3010,
	/// <summary>3011 남은 체력이 가장 낮은 안드로이드 선택</summary>
	LOWEST_LIFE_VALUE_ANDROID = 3011,
	/// <summary>3012 남은 체력이 가장 낮은 악마 종족 선택</summary>
	LOWEST_LIFE_VALUE_DEVIL = 3012,
	/// <summary>3013 남은 체력이 가장 낮은 천사 종족 선택</summary>
	LOWEST_LIFE_VALUE_ANGEL = 3013,
	/// <summary>3014 자신을 제외한 남은 체력이 가장 낮은 타겟 선택</summary>
	LOWEST_LIFE_VALUE_WITHOUT_ME = 3014,
	/// <summary>3015 자신을 제외한 남은 체력 비율이 가장 낮은 타겟 선택</summary>
	LOWEST_LIFE_RATE_WITHOUT_ME = 3015,
	/// <summary>3016 자신을 제외한 공격력이 가장 낮은 타겟 선택</summary>
	LOWEST_ATTACK_WITHOUT_ME = 3016,
	/// <summary>3017 자신을 제외한 방어력이 가장 낮은 타겟 선택</summary>
	LOWEST_DEFENSE_WITHOUT_ME = 3017,
	/// <summary>3018 자신을 제외한 공속이 가장 낮은 타겟 선택</summary>
	LOWEST_RAPIDITY_WITHOUT_ME = 3018,
	/// <summary>3019 자신을 제외한 명중률이 가장 낮은 타겟 선택</summary>
	LOWEST_ACCURACY_WITHOUT_ME = 3019,
	/// <summary>3020 자신을 제외한 회피율이 가장 낮은 타겟 선택</summary>
	LOWEST_EVASION_WITHOUT_ME = 3020,
	/// <summary>3021 자신을 포함한 남은 체력이 가장 낮은 타겟 선택</summary>
	LOWEST_LIFE_VALUE_WITH_ME = 3021,
	/// <summary>3022 자신을 포함한 남은 체력 비율이 가장 낮은 타겟 선택</summary>
	LOWEST_LIFE_RATE_WITH_ME = 3022,
	/// <summary>3023 자신을 포함한 공격력이 가장 낮은 타겟 선택</summary>
	LOWEST_ATTACK_WITH_ME = 3023,
	/// <summary>3024 자신을 포함한 방어력이 가장 낮은 타겟 선택</summary>
	LOWEST_DEFENSE_WITH_ME = 3024,
	/// <summary>3025 자신을 포함한 공속이 가장 낮은 타겟 선택</summary>
	LOWEST_RAPIDITY_WITH_ME = 3025,
	/// <summary>3026 자신을 포함한 명중률이 가장 낮은 타겟 선택</summary>
	LOWEST_ACCURACY_WITH_ME = 3026,
	/// <summary>3027 자신을 포함한 회피율이 가장 낮은 타겟 선택</summary>
	LOWEST_EVASION_WITH_ME = 3027,
	/// <summary>4001 자신보다 약한 속성 타겟 선택 (상성 시스템이 있을 경우)</summary>
	WEAK_ELEMENT = 4001,
	/// <summary>4002 자신보다 강한 속성 타겟 선택 (상성 시스템이 있을 경우)</summary>
	STRONG_ELEMENT = 4002,
	/// <summary>5001 버프 효과가 있는 타겟 선택</summary>
	GAIN_BUFF_DURATION = 5001,
	/// <summary>5002 버프 효과 중 공격력 증가 효과가 있는 타겟 선택</summary>
	GAIN_BUFF_DURATION_ATTACK_INC = 5002,
	/// <summary>5003 버프 효과 중 방어력 증가 효과가 있는 타겟 선택</summary>
	GAIN_BUFF_DURATION_DEFENSE_INC = 5003,
	/// <summary>5004 버프 효과 중 공속 증가 효과가 있는 타겟 선택</summary>
	GAIN_BUFF_DURATION_RAPIDITY_INC = 5004,
	/// <summary>5005 버프 효과 중 회피율 증가 효과가 있는 타겟 선택</summary>
	GAIN_BUFF_DURATION_EVASION_INC = 5005,
	/// <summary>5006 버프 효과 중 치명타 확률 증가 효과가 있는 타겟 선택</summary>
	GAIN_BUFF_DURATION_CRITICAL_RATE_INC = 5006,
	/// <summary>5007 버프 효과 중 치명타 피해량 증가 효과가 있는 타겟 선택</summary>
	GAIN_BUFF_DURATION_CRITICAL_DAMAGE_INC = 5007,
	/// <summary>6001 디버프 효과가 있는 타겟 선택</summary>
	GAIN_DEBUFF_DURATION = 6001,
	/// <summary>6002 디버프 효과 중 공격력 감소 효과가 있는 타겟 선택</summary>
	GAIN_DEBUFF_DURATION_ATTACK_DEC = 6002,
	/// <summary>6003 디버프 효과 중 방어력 감소 효과가 있는 타겟 선택</summary>
	GAIN_DEBUFF_DURATION_DEFENSE_DEC = 6003,
	/// <summary>6004 디버프 효과 중 화상 효과가 있는 타겟 선택</summary>
	GAIN_DEBUFF_DURATION_BURN = 6004,
	/// <summary>6005 디버프 효과 중 출혈 효과가 있는 타겟 선택</summary>
	GAIN_DEBUFF_DURATION_BLEEDING = 6005,
	/// <summary>6006 디버프 효과 중 중독 효과가 있는 타겟 선택</summary>
	GAIN_DEBUFF_DURATION_POISON = 6006,
	/// <summary>6007 디버프 효과 중 공속 감소 효과가 있는 타겟 선택</summary>
	GAIN_DEBUFF_DURATION_RAPIDITY_DEC = 6007,
	/// <summary>6008 디버프 효과 중 수면 효과가 있는 타겟 선택</summary>
	GAIN_DEBUFF_DURATION_SLEEP = 6008,
	/// <summary>6009 디버프 효과 중 치명상 효과가 있는 타겟 선택</summary>
	GAIN_DEBUFF_DURATION_FATAL_WOUNDS = 6009,
	/// <summary>6010 디버프 효과 중 명중률 감소 효과가 있는 타겟 선택</summary>
	GAIN_DEBUFF_DURATION_ACCURACY_DEC = 6010,
	/// <summary>10001 현재 체력이 가장 낮은 대상</summary>
	LOW_HP_VALUE = 10001,
	/// <summary>10002 현재 체력이 가장 높은 대상</summary>
	HIGH_HP_VALUE = 10002,
	/// <summary>10003 남은 체력 비율이 가장 높은 대상</summary>
	HIGH_HP_RATE = 10003,
	/// <summary>10004 남은 체력 비율이 가장 낮은 대상</summary>
	LOW_HP_RATE = 10004,
	/// <summary>10003 최대 체력이 가장 낮은 대상</summary>
	LOW_MAX_HP = 10005,
	/// <summary>10004 최대 체력이 가장 높은 대상</summary>
	HIGH_MAX_HP = 10006,
	/// <summary>10005 물리 공격력이 가장 낮은 대상</summary>
	LOW_P_ATK = 10007,
	/// <summary>10006 물리 공격력이 가장 높은 대상</summary>
	HIGH_P_ATK = 10008,
	/// <summary>10007 마법 공격력이 가장 낮은 대상</summary>
	LOW_M_ATK = 10009,
	/// <summary>10008 마법 공격력이 가장 높은 대상</summary>
	HIGH_M_ATK = 10010,
	/// <summary>10009 물리/ 마법 공격력이 가장 낮은 대상</summary>
	LOW_PM_ATK = 10011,
	/// <summary>10010 물리/ 마법 공격력이 가장 높은 대상</summary>
	HIGH_PM_ATK = 10012,
	/// <summary>10011 물리 방어력이 가장 낮은 대상</summary>
	LOW_P_DEF = 10013,
	/// <summary>10012 물리 방어력이 가장 높은 대상</summary>
	HIGH_P_DEF = 10014,
	/// <summary>10013 마법 방어력이 가장 낮은 대상</summary>
	LOW_M_DEF = 10015,
	/// <summary>10014 마법 방어력이 가장 높은 대상</summary>
	HIGH_M_DEF = 10016,
	/// <summary>10015 물리/ 마법 방어력이 가장 낮은 대상</summary>
	LOW_PM_DEF = 10017,
	/// <summary>10016 물리/ 마법 방어력이 가장 높은 대상</summary>
	HIGH_PM_DEF = 10018,
	/// <summary>10017 물리 크리티컬 확률이 가장 낮은 대상</summary>
	LOW_P_CRI_INC = 10019,
	/// <summary>10018 물리 크리티컬 확률이 가장 높은 대상</summary>
	HIGH_P_CRI_INC = 10020,
	/// <summary>10019 마법 크리티컬 확률이 가장 낮은 대상</summary>
	LOW_M_CRI_INC = 10021,
	/// <summary>10020 마법 크리티컬 확률이 가장 높은 대상</summary>
	HIGH_M_CRI_INC = 10022,
	/// <summary>10021 물리 크리티컬 추가 대미지가 가장 낮은 대상</summary>
	LOW_P_CRI_ADD = 10023,
	/// <summary>10022 물리 크리티컬 추가 대미지가 가장 높은 대상</summary>
	HIGH_P_CRI_ADD = 10024,
	/// <summary>10023 마법 크리티컬 추가 대미지가 가장 낮은 대상</summary>
	LOW_M_CRI_ADD = 10025,
	/// <summary>10024 마법 크리티컬 추가 대미지가 가장 높은 대상</summary>
	HIGH_M_CRI_ADD = 10026,
	/// <summary>10025 명중이 가장 낮은 대상</summary>
	LOW_ACCURACY = 10027,
	/// <summary>10026 명중이 가장 높은 대상</summary>
	HIGH_ACCURACY = 10028,
	/// <summary>10027 회피가 가장 낮은 대상</summary>
	LOW_EVASION = 10029,
	/// <summary>10028 회피가 가장 높은 대상</summary>
	HIGH_EVASION = 10030,
	/// <summary>10029 타격 시 회복량이 가장 낮은 대상</summary>
	LOW_ATK_RECOVERY = 10031,
	/// <summary>10030 타격 시 회복량이 가장 높은 대상</summary>
	HIGH_ATK_RECOVERY = 10032,
	/// <summary>10031 회복량이 가장 낮은 대상</summary>
	LOW_HEAL = 10033,
	/// <summary>10032 회복량이 가장 높은 대상</summary>
	HIGH_HEAL = 10034,
	/// <summary>10033 강인함이 가장 낮은 대상</summary>
	LOW_RESIST = 10035,
	/// <summary>10034 강인함이 가장 높은 대상</summary>
	HIGH_RESIST = 10036,
	/// <summary>20001 상태이상 중인 대상</summary>
	CC_ALL_PROGRESS = 20001,
	/// <summary>20002 기절 중인 대상</summary>
	CC_STUN_PROGRESS = 20002,
	/// <summary>20003 결박 중인 대상</summary>
	CC_BIND_PROGRESS = 20003,
	/// <summary>20004 침묵 중인 대상</summary>
	CC_SILENCE_PROGRESS = 20004,
	/// <summary>20005 빙결 중인 대상</summary>
	CC_FREEZ_PROGRESS = 20005,
	/// <summary>20101 버프 상태 중인 대상</summary>
	BUFF_ALL_PROGRESS = 20101,
	/// <summary>20201 디버프 상태 중인 대상</summary>
	DEBUFF_ALL_PROGRESS = 20201,
}

///	<summary>
///	캐릭터 정렬 
///	</summary>
public enum CHARACTER_SORT
{
	/// <summary>0 캐릭터 이름</summary>
	NAME = 0,
	/// <summary>1 캐릭터 레벨</summary>
	LEVEL_CHARACTER = 1,
	/// <summary>2 성급</summary>
	STAR = 2,
	/// <summary>3 인연 랭크</summary>
	DESTINY = 3,
	/// <summary>4 스킬 레벨</summary>
	SKILL_LEVEL = 4,
	/// <summary>5 궁극기 스킬 레벨</summary>
	EX_SKILL_LEVEL = 5,
	/// <summary>6 공격력</summary>
	ATTACK = 6,
	/// <summary>7 방어력</summary>
	DEFEND = 7,
	/// <summary>8 사거리</summary>
	RANGE = 8,
	/// <summary>9 호감도</summary>
	LIKEABILITY = 9,
}

///	<summary>
///	두번째 타겟룰 타입
///	처음 타겟팅 할때가 아닌, 이펙트 등이 타겟에 도달했을 때 추가적으로 타겟팅 해야할 경우에 사용
///	</summary>
public enum SECOND_TARGET_RULE_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>1 타겟을 중심으로 지정 반경내에 감지되는 타겟 검사</summary>
	AROUND_SPLASH = 1,
	/// <summary>2 타겟 뒤로 지정 반경내에 감지되는 타겟 검사</summary>
	BACK_SPLASH = 2,
}

///	<summary>
///	스킬 효과 타입 (사용하지 않을 예정)
///	</summary>
public enum EFFECT_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>1 대미지 만큼 현재 체력 감소</summary>
	DAMAGE = 1,
	/// <summary>2 다음 피해의 데미지를 감소</summary>
	NEXT_DAMAGE_REDUCT = 2,
	///	<summary>
	///	3 대상의 현재 HP를 회복. 
	///	MAX HP를 초과할 수 없다
	///	</summary>
	HP_RECOVERY = 3,
}

///	<summary>
///	스탯 멀티플 타입
///	</summary>
public enum STAT_MULTIPLE_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>100 공격력 절대값 계산</summary>
	ATTACK_VALUE = 100,
	/// <summary>101 방어력 절대값 계산</summary>
	DEFENSE_VALUE = 101,
	/// <summary>102 최대 체력 절대 값</summary>
	MAX_LIFE = 102,
	/// <summary>103 현재 체력 절대 값</summary>
	LIFE = 103,
	/// <summary>104 크리티컬 확률 절대값</summary>
	CRITICAL_CHANCE = 104,
	/// <summary>105 크리티컬 파워 절대 값</summary>
	CRITICAL_POWER_ADD = 105,
	/// <summary>106 명중률 절대 값</summary>
	ACCURACY_VALUE = 106,
	/// <summary>107 회피율 절대 값</summary>
	EVASION_VALUE = 107,
	/// <summary>108 회복량 절대 값</summary>
	HEAL_VALUE = 108,
	/// <summary>201 공격력 배율 계산</summary>
	ATTACK_RATE = 201,
	/// <summary>202 방어력 배율 계산</summary>
	DEFENSE_RATE = 202,
	/// <summary>203 최대 체력 배율 값</summary>
	MAX_LIFE_RATE = 203,
	/// <summary>204 현재 체력 배율 계산</summary>
	LIFE_RATE = 204,
	/// <summary>205 크리티컬 확률 배율 계산</summary>
	CRITICAL_CHANCE_RATE = 205,
	/// <summary>206 크리티컬 파워 배율 계산</summary>
	CRITICAL_POWER_ADD_RATE = 206,
	/// <summary>207 명중률 배율 계산</summary>
	ACCURACY_RATE = 207,
	/// <summary>208 회피율 배율 계산</summary>
	EVASION_RATE = 208,
	/// <summary>209 회복량 배율 계산</summary>
	HEAL_RATE = 209,
	/// <summary>210 피해량 배율 계산</summary>
	DAMAGE = 210,
}

///	<summary>
///	팀 타입
///	</summary>
public enum TEAM_TYPE
{
	/// <summary>0 왼쪽 팀(아군)</summary>
	LEFT = 0,
	/// <summary>1 오른쪽 팀(적군)</summary>
	RIGHT = 1,
}

///	<summary>
///	신체 터치 부위 구분
///	</summary>
public enum TOUCH_BODY_TYPE
{
	/// <summary>캐릭터 클릭이지만, 퍼즐 클릭이 아님</summary>
	NONE = 0,
	/// <summary>1 몸체</summary>
	BODY = 1,
	/// <summary>2 가슴</summary>
	BREAST = 2,
	/// <summary>3 머리</summary>
	HEAD = 3,
	/// <summary>4 입</summary>
	MOUTH = 4,
	/// <summary>5 골반</summary>
	PELVIS = 5,
	/// <summary>6 다리</summary>
	LEG = 6,
	/// <summary>7 눈</summary>
	EYE = 7,
	/// <summary>8 코</summary>
	NOSE = 8,
	/// <summary>9 귀</summary>
	EAR = 9,
	/// <summary>10 볼</summary>
	CHEEK = 10,
	/// <summary>11 턱</summary>
	JAW = 11,
	/// <summary>12 이마</summary>
	BROW = 12,
	/// <summary>13 얼굴</summary>
	FACE = 13,
	/// <summary>14 엉덩이</summary>
	HIP = 14,
	/// <summary>15 브라</summary>
	BRA = 15,
	/// <summary>16 팬티</summary>
	PANTY = 16,
	/// <summary>17 뿔</summary>
	HORN = 17,
	/// <summary>18 꼬리</summary>
	TOY = 18,
	/// <summary>19 기타 소품 및 의상 1</summary>
	PROP_1 = 19,
	/// <summary>20 기타 소품 및 의상 2</summary>
	PROP_2 = 20,
	/// <summary>21 기타 소품 및 의상 3</summary>
	PROP_3 = 21,
	/// <summary>22 기타 소품 및 의상 4</summary>
	PROP_4 = 22,
}

///	<summary>
///	터치 종류 구분
///	</summary>
public enum TOUCH_GESTURE_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>1 OnClick</summary>
	CLICK = 1,
	/// <summary>2 DoubleClick</summary>
	DOUBLE_CLICK = 2,
	/// <summary>3 PushDown(누른 채로 1초 이상)</summary>
	PUSH_DOWN = 3,
	/// <summary>4 Drag</summary>
	DRAG = 4,
}

///	<summary>
///	타겟 팀 타입
///	</summary>
public enum TARGET_TYPE
{
	/// <summary>0 아군</summary>
	MY_TEAM = 0,
	/// <summary>1 적군</summary>
	ENEMY_TEAM = 1,
}

///	<summary>
///	일회성 효과 타입
///	</summary>
public enum ONETIME_EFFECT_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>1 물리 대미지</summary>
	PHYSICS_DAMAGE = 1,
	/// <summary>2 마법 대미지</summary>
	MAGIC_DAMAGE = 2,
	/// <summary>3 체력 회복</summary>
	LIFE_RECOVERY = 3,
	/// <summary>4 빈 이펙트</summary>
	NONE_EFFECT = 4,
}

///	<summary>
///	지속성 효과 타입
///	</summary>
public enum DURATION_EFFECT_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>1 피해 감소</summary>
	DAMAGE_REDUCE = 1,
	/// <summary>101 중독</summary>
	POISON = 101,
	/// <summary>102 기절</summary>
	STUN = 102,
	/// <summary>103 침묵</summary>
	SILENCE = 103,
	/// <summary>104 결박</summary>
	BIND = 104,
	/// <summary>105 빙결</summary>
	FREEZE = 105,
	/// <summary>106 물리 공격력 증가</summary>
	PHYSICS_ATTACK_UP = 106,
	/// <summary>107 마법 공격력 증가</summary>
	MAGIC_ATTACK_UP = 107,
	/// <summary>108 물리 방어력 증가</summary>
	PHYSICS_DEFEND_UP = 108,
	/// <summary>109 마법 방어력 증가</summary>
	MAGIC_DEFEND_UP = 109,
	/// <summary>110 마법 공격력 감소</summary>
	PHYSICS_ATTACK_DOWN = 110,
	/// <summary>111 마법 공격력 감소</summary>
	MAGIC_ATTACK_DOWN = 111,
	/// <summary>112 물리 방어력 감소</summary>
	PHYSICS_DEFEND_DOWN = 112,
	/// <summary>113 마법 방어력 감소</summary>
	MAGIC_DEFEND_DOWN = 113,
	/// <summary>114 물리 공격력 물리 방어력 스탯 교환</summary>
	EXCHANGE_PHYSICS_ATTACK_DEFEND = 114,
	/// <summary>115 마법 공격력 마법 방어력 스탯 교환</summary>
	EXCHANGE_MAGIC_ATTACK_DEFEND = 115,
	/// <summary>116 물리/마법 공격력 물리/ 마법 방어력 스탯 교환</summary>
	EXCHANGE_ATTACK_DEFEND_ALL = 116,
	/// <summary>117 물리 크리티컬 확률 증가</summary>
	PHYSICS_CRITICAL_CHANCE_UP = 117,
	/// <summary>118 마법 크리티컬 확률 증가</summary>
	MAGIC_CRITICAL_CHANCE_UP = 118,
	/// <summary>119 물리 크리티컬 추가 대미지 증가</summary>
	PHYSICS_CRITICAL_POWER_ADD_UP = 119,
	/// <summary>120 마법 크리티컬 추가 대미지 증가</summary>
	MAGIC_CRITICAL_POWER_ADD_UP = 120,
	/// <summary>121 타격 시 회복량 증가</summary>
	ATTACK_LIFE_RECOVERY_UP = 121,
	/// <summary>122 회피 증가</summary>
	EVASION_UP = 122,
	/// <summary>123 명중 증가</summary>
	ACCURACY_UP = 123,
	/// <summary>124 회복량 증가</summary>
	HEAL_UP = 124,
	/// <summary>125 물리 크리티컬 확률 감소</summary>
	PHYSICS_CRITICAL_CHANCE_DOWN = 125,
	/// <summary>126 마법 크리티컬 확률 감소</summary>
	MAGIC_CRITICAL_CHANCE_DOWN = 126,
	/// <summary>127 물리 크리티컬 추가 대미지 감소</summary>
	PHYSICS_CRITICAL_POWER_ADD_DOWN = 127,
	/// <summary>128 마법 크리티컬 추가 대미지 감소</summary>
	MAGIC_CRITICAL_POWER_ADD_DOWN = 128,
	/// <summary>129 타격 시 회복량 감소</summary>
	ATTACK_LIFE_RECOVERY_DOWN = 129,
	/// <summary>130 회피 감소</summary>
	EVASION_DOWN = 130,
	/// <summary>131 명중 감소</summary>
	ACCURACY_DOWN = 131,
	/// <summary>132 회복량 감소</summary>
	HEAL_DOWN = 132,
}

///	<summary>
///	지속성 방식 타입
///	</summary>
public enum PERSISTENCE_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>1 시간 지속</summary>
	TIME = 1,
	/// <summary>2 피격 횟수 제한</summary>
	HITTED = 2,
	/// <summary>3 공격 횟수 제한</summary>
	ATTACK = 3,
}

///	<summary>
///	투사체 타입
///	</summary>
public enum PROJECTILE_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>1 투사체를 타겟의 발 밑에 던진다</summary>
	THROW_FOOT = 1,
	/// <summary>2 투사체를 타겟의 몸에 던진다</summary>
	THROW_BODY = 2,
	/// <summary>3 투사체를 타겟의 머리에 던진다</summary>
	THROW_HEAD = 3,
	/// <summary>11 타겟의 발 밑에서 즉시 효과 발동</summary>
	INSTANT_TARGET_FOOT = 11,
	/// <summary>12 타겟의 몸에서 즉시 효과 발동</summary>
	INSTANT_TARGET_BODY = 12,
	/// <summary>13 타겟의 머리에서 즉시 효과 발동</summary>
	INSTANT_TARGET_HEAD = 13,
	/// <summary>21 전체 선택(진영의 중앙)</summary>
	ALL_ROUND = 21,
}

///	<summary>
///	부등호 타입
///	</summary>
public enum INEQUALITY_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>1 같음</summary>
	EQUAL = 1,
	/// <summary>2 다름</summary>
	NOT_EQUAL = 2,
	/// <summary>3 초과</summary>
	GREATER = 3,
	/// <summary>4 미만</summary>
	LESS = 4,
	/// <summary>5 이상</summary>
	GREATER_EQUAL = 5,
	/// <summary>6 이하</summary>
	LESS_EQUAL = 6,
}

///	<summary>
///	SKII TYPE
///	</summary>
public enum SKILL_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>1 일반 공격</summary>
	NORMAL_ATTACK = 1,
	/// <summary>2 스킬 공격 1</summary>
	SKILL_01 = 2,
	/// <summary>3 스킬 공격 2</summary>
	SKILL_02 = 3,
	/// <summary>4 궁극기</summary>
	SPECIAL_SKILL = 4,
}

///	<summary>
///	해금 종류 타입
///	</summary>
public enum LIMIT_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>1 레벨 해금</summary>
	LEVEL = 1,
	/// <summary>2 스테이지 클리어 해금</summary>
	STAGE_CLEAR = 2,
}

///	<summary>
///	아이템 종류
///	</summary>
public enum ITEM_TYPE_V2
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>1 플레이어 경험치 물약</summary>
	EXP_POTION_P = 1,
	/// <summary>2 캐릭터 경험치 물약</summary>
	EXP_POTION_C = 2,
	/// <summary>3 스테미나 회복 물약</summary>
	STA_POTION = 3,
	/// <summary>4 호감도 아이템</summary>
	FAVORITE_ITEM = 4,
	/// <summary>5 스킬 경험치 아이템</summary>
	EXP_SKILL = 5,
	/// <summary>6 스테이지 스킵 티켓</summary>
	STAGE_SKIP = 6,
	/// <summary>7 던전 입장 티켓</summary>
	TICKET_DUNGEON = 7,
	/// <summary>8 정련석(장비 성장)</summary>
	EQ_GROWUP = 8,
	/// <summary>9 보상 선택 티켓(1개를 선택 획득)</summary>
	TICKET_REWARD_SELECT = 9,
	/// <summary>10 보상 랜덤 티켓(1개를 확률 획득)</summary>
	TICKET_REWARD_RANDOM = 10,
	/// <summary>11 보상 패키지 티켓(모든 보상 획득)</summary>
	TICKET_REWARD_ALL = 11,
	/// <summary>100 장비</summary>
	EQUIPMENT = 100,
	/// <summary>101 캐릭터</summary>
	CHARACTER = 101,
	/// <summary>1000 장비 조각</summary>
	PIECE_EQUIPMENT = 1000,
	/// <summary>1001 캐릭터 조각</summary>
	PIECE_CHARACTER = 1001,
	/// <summary>1002 아이템 조각</summary>
	PIECE_ITEM = 1002,
}

///	<summary>
///	재화 종류
///	</summary>
public enum GOODS_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>1 골드 (게임내 재화)</summary>
	GOLD = 1,
	/// <summary>2 다이아 (게임내 유료 재화)</summary>
	DIA = 2,
}

///	<summary>
///	조각 분류
///	</summary>
public enum PIECE_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>1 아이템</summary>
	ITEM = 1,
	/// <summary>2 장비</summary>
	EQUIPMENT = 2,
	/// <summary>3  캐릭터</summary>
	CHARACTER = 3,
}

///	<summary>
///	조각 분류
///	</summary>
public enum EQUIPMENT_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>1 무기</summary>
	WEAPON = 1,
	/// <summary>2 방어구</summary>
	ARMOR = 2,
	/// <summary>3 신발</summary>
	SHOES = 3,
	/// <summary>4 반지</summary>
	RING = 4,
	/// <summary>5 목걸이</summary>
	NECKLACE = 5,
}

///	<summary>
///	아이템 드랍 확률 적용유형
///	</summary>
public enum DROP_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>1 개별 드랍 확률로 체크</summary>
	DROP_EACH = 1,
	/// <summary>2 ID내에서의 드랍 비중으로 체크</summary>
	DROP_WEIGHT = 2,
}

///	<summary>
///	반복 주기 타입
///	</summary>
public enum REPEAT_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>1 지정된 분마다</summary>
	REPEAT_MIN = 1,
	/// <summary>2 지정된 일마다 (주기 적용의 시간 타이밍은 schedule 테이블의 time_open 칼럼 참조)</summary>
	REPEAT_DAY = 2,
	/// <summary>3 지정된 주마다 (주기 적용의 시간 타이밍은 schedule 테이블의 time_open 칼럼 참조)</summary>
	REPEAT_WEEK = 3,
	/// <summary>4 지정된 달마다 (주기 적용의 시간 타이밍은 schedule 테이블의 time_open 칼럼 참조)</summary>
	REPEAT_MONTH = 4,
	/// <summary>5 지정된 해마다 (주기 적용의 시간 타이밍은 schedule 테이블의 time_open 칼럼 참조)</summary>
	REPEAT_YEAR = 5,
}

///	<summary>
///	차징 타입
///	</summary>
public enum CHARGE_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>Final Max(Base max + add max)값까지 차징</summary>
	FINALMAX = 1,
	/// <summary>Base max값만큼만 차징, Final Max 값을 넘지 못함</summary>
	BASEMAX_CUT = 2,
	/// <summary>Base max값만큼만 차징, Final Max 값을 넘겨서 차징 가능</summary>
	BASEMAX_OVER = 3,
	/// <summary>Final max값만큼만 차징, Final Max 값을 넘겨서 차징 가능</summary>
	FINALMAX_OVER = 4,
	/// <summary>지정 수치만큼 차징, Final max값 이상은 차징되지 않음</summary>
	VALUE_CUT = 5,
	/// <summary>지정 수치만큼 차징, Final max값 이상 차징 가능</summary>
	VALUE_OVER = 6,
}

///	<summary>
///	재화 및 아이템 종류
///	</summary>
public enum REWARD_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>1 금화(게임내 사용되는 재화)</summary>
	GOLD = 1,
	/// <summary>2 보석(게임내 사용되는 유료 재화)</summary>
	DIA = 2,
	/// <summary>3 스태미나</summary>
	STAMINA = 3,
	/// <summary>4 호감도</summary>
	FAVORITE = 4,
	/// <summary>5 플레이어 경험치</summary>
	EXP_PLAYER = 5,
	/// <summary>6 캐릭터 경험치</summary>
	EXP_CHARACTER = 6,
	/// <summary>7 캐릭터</summary>
	CHARACTER = 7,
	/// <summary>8 장비</summary>
	EQUIPMENT = 8,
	/// <summary>9 근원 전달 횟수(플레이어 보유)</summary>
	SEND_ESSENCE = 9,
	/// <summary>10 근원 받을 수 있는 횟수(캐릭터 공용 설정)</summary>
	GET_ESSENCE = 10,
	/// <summary>101 플레이어 경험치 물약</summary>
	EXP_POTION_P = 101,
	/// <summary>102 캐릭터 경험치 물약</summary>
	EXP_POTION_C = 102,
	/// <summary>103 스테미나 회복 물약</summary>
	STA_POTION = 103,
	/// <summary>104 호감도 아이템</summary>
	FAVORITE_ITEM = 104,
	/// <summary>105 스테이지 스킵 티켓</summary>
	STAGE_SKIP = 105,
	/// <summary>106 던전 입장 티켓</summary>
	TICKET_DUNGEON = 106,
	/// <summary>107 정련석(장비 성장)</summary>
	EQ_GROWUP = 107,
	/// <summary>109 보상 선택 티켓(1개를 선택 획득)</summary>
	TICKET_REWARD_SELECT = 108,
	/// <summary>100 보상 랜덤 티켓(1개를 확률 획득)</summary>
	TICKET_REWARD_RANDOM = 109,
	/// <summary>110 보상 패키지 티켓(모든 보상 획득)</summary>
	TICKET_REWARD_ALL = 110,
	/// <summary>111 장비 조각</summary>
	PIECE_EQUIPMENT = 111,
	/// <summary>112 캐릭터 조각</summary>
	PIECE_CHARACTER = 112,
	/// <summary>113 아이템 조각</summary>
	PIECE_ITEM = 113,
	/// <summary>114 스킬 경험치 아이템</summary>
	EXP_SKILL = 114,
}

///	<summary>
///	이펙트의 싱글/멀티플 타입
///	</summary>
public enum EFFECT_COUNT_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>전체 공격 등 대표 이펙트 하나만 발현되는 이펙트</summary>
	SINGLE_EFFECT = 1,
	/// <summary>각 타겟에 개별적으로 발현되는 이펙트</summary>
	EACH_TARGET_EFFECT = 2,
}

///	<summary>
///	캐릭터 상태 타입
///	</summary>
public enum LOVE_LEVEL_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>1 어색함</summary>
	NORMAL = 1,
	/// <summary>2 친근함</summary>
	FRIENDLINESS = 2,
	/// <summary>3 절친함</summary>
	CLOSENESS = 3,
	/// <summary>4 좋아함</summary>
	LIKE = 4,
	/// <summary>5 사랑함</summary>
	LOVE = 5,
}

///	<summary>
///	공격 타입
///	</summary>
public enum ATTRIBUTE_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>전기</summary>
	ELECTRICITY = 1,
	/// <summary>베리타리움</summary>
	VEGETARIUM = 2,
	/// <summary>요력</summary>
	CHARM = 3,
	/// <summary>마력</summary>
	MANA = 4,
}

///	<summary>
///	PC or NPC TYPE
///	</summary>
public enum CHARACTER_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>1 PLAYER CHARACTER</summary>
	PC = 1,
	/// <summary>2 NPC</summary>
	NPC = 2,
}

///	<summary>
///	Asc or Desc
///	</summary>
public enum SORT_ORDER_TYPE
{
	/// <summary>오름 차순</summary>
	ASC = 0,
	/// <summary>내림 차순</summary>
	DESC = 1,
}

