///	<summary>
///	캐릭터가 속한 종족 타입
///	</summary>
public enum TRIBE_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>인간종족</summary>
	HUMAN = 1,
	/// <summary>엘프족</summary>
	ELF = 2,
	/// <summary>수인족</summary>
	WEREBEAST = 3,
	/// <summary>안드로이드</summary>
	ANDROID = 4,
	/// <summary>악마</summary>
	DEVIL = 5,
	/// <summary>천사</summary>
	ANGEL = 6,
}

///	<summary>
///	유저가 캐릭터 사거리를 대략적으로 쉽게 파악할 수 있게 편의성 용도로 제공할 표현
///	</summary>
public enum POSITION_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>전열 배치</summary>
	FRONT = 1,
	/// <summary>중열 배치</summary>
	MIDDLE = 2,
	/// <summary>후열 배치</summary>
	BACK = 3,
}

///	<summary>
///	npc 타입 
///	</summary>
public enum NPC_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>우호 NPC</summary>
	NPC = 1,
	/// <summary>일반 몬스터</summary>
	NORMAL = 2,
	/// <summary>엘리트 몬스터</summary>
	ELITE = 3,
	/// <summary>보스 몬스터</summary>
	BOSS = 4,
}

///	<summary>
///	타겟 룰 타입
///	다양한 방식의 타겟 룰을 지정하여 사용할 수 있다.
///	</summary>
public enum TARGET_RULE_TYPE
{
	/// <summary>임의 타겟 선택</summary>
	RANDOM = 0,
	/// <summary>자신 선택</summary>
	SELF = 1,
	/// <summary>전체 선택</summary>
	ALL = 2,
	/// <summary>자신을 제외한 아군 전체 선택</summary>
	ALL_WITHOUT_ME = 3,
	/// <summary>자신을 제외한 가장 가까운 아군 선택</summary>
	ALLY_WITHOUT_ME_NEAREST = 4,
	/// <summary>자신을 제외한 가장 먼 아군 선택</summary>
	ALLY_WITHOUT_ME_FURTHEST = 5,
	/// <summary>가장 가까운 적 선택 (순번 컬럼을 연동하여 앞에서부터의 순서대로)</summary>
	NEAREST = 6,
	/// <summary>가장 거리가 먼 적 선택 (순번 컬럼을 연동하여 뒤에서부터의 순서대로)</summary>
	FURTHEST = 7,
	/// <summary>가장 가까운 적 우선 선택(순번 컬럼 연동하여 순서대로) 후 일정 영역내의 뒤에있는 타겟의 추가 선택</summary>
	NEAREST_ADD_BACK_RANGE = 8,
	/// <summary>가장 거리가 먼 적 우선 선택 (순번 컬럼을 연동하여 뒤에서부터의 순서대로) 후 일정 영역내의 앞에 있는 타겟 추가 선택</summary>
	FURTHEST_ADD_FRONT_RANGE = 9,
	/// <summary>가장 가까운 적 우선 선택(순번 컬럼 연동하여 순서대로) 후 일정 영역내의(주변) 타겟의 추가 선택</summary>
	NEAREST_ADD_ARROUND_RANGE = 10,
	/// <summary>가장 거리가 먼 적 우선 선택 (순번 컬럼을 연동하여 뒤에서부터의 순서대로) 후 일정 영역내의 (주변) 타겟 추가 선택</summary>
	FURTHEST_ADD_ARROUND_RANGE = 11,
	/// <summary>남은 체력이 가장 낮은 타겟 선택</summary>
	LOWEST_LIFE_VALUE = 3001,
	/// <summary>공격력이 가장 낮은 타겟 선택</summary>
	LOWEST_ATTACK = 3003,
	/// <summary>방어력이 가장 낮은 타겟 선택</summary>
	LOWEST_DEFENSE = 3004,
	/// <summary>공격력이 가장 높은 타겟 선택</summary>
	HIGHEST_ATTACK = 2003,
	/// <summary>방어력이 가장 높은 타겟 선택</summary>
	HIGHEST_DEFENSE = 2004,
	/// <summary>가장 가까운 상대 타겟 선택(화면상에서 적에게 접근하기 위한 용도)</summary>
	APPROACH = 9999,
	/// <summary>리더 선택</summary>
	LEADER_HIGH_PRIORITY = 1001,
	/// <summary>남은 체력이 가장 많은 타겟 선택</summary>
	HIGHEST_LIFE_VALUE = 2001,
	/// <summary>남은 체력 비율이 가장 높은 타겟 선택</summary>
	HIGHEST_LIFE_RATE = 2002,
	/// <summary>명중률이 가장 높은 타겟 선택 (명중률 속성이 있다면)</summary>
	HIGHEST_ACCURACY = 2006,
	/// <summary>회피율이 가장 높은 타겟 선택 (회피율 속성이 있다면)</summary>
	HIGHEST_EVASION = 2007,
	/// <summary>남은 체력이 가장 높은 인간 종족 선택</summary>
	HIGHEST_LIFE_VALUE_HUMAN = 2008,
	/// <summary>남은 체력이 가장 높은 엘프 종족 선택</summary>
	HIGHEST_LIFE_VALUE_ELF = 2009,
	/// <summary>남은 체력이 가장 높은 수인 종족 선택</summary>
	HIGHEST_LIFE_VALUE_WEREBEAST = 2010,
	/// <summary>남은 체력이 가장 높은 안드로이드 선택</summary>
	HIGHEST_LIFE_VALUE_ANDROID = 2011,
	/// <summary>남은 체력이 가장 높은 악마 선택</summary>
	HIGHEST_LIFE_VALUE_DEVIL = 2012,
	/// <summary>남은 체력이 가장 높은 천사 선택</summary>
	HIGHEST_LIFE_VALUE_ANGEL = 2013,
	/// <summary>자신을 제외한 체력이 가장 높은 타겟 선택</summary>
	HIGHEST_LIFE_VALUE_WITHOUT_ME = 2014,
	/// <summary>자신을 제외한 체력 비율이 가장 높은 타겟 선택</summary>
	HIGHEST_LIFE_RATE_WITHOUT_ME = 2015,
	/// <summary>자신을 제외한 공격력이 가장 높은 타겟 선택</summary>
	HIGHEST_ATTACK_WITHOUT_ME = 2016,
	/// <summary>자신을 제외한 방어력이 가장 높은 타겟 선택</summary>
	HIGHEST_DEFENSE_WITHOUT_ME = 2017,
	/// <summary>자신을 제외한 공속이 가장 높은 타겟 선택</summary>
	HIGHEST_RAPIDITY_WITHOUT_ME = 2018,
	/// <summary>자신을 제외한 명중률이 가장 높은 타겟 선택 (명중률 속성이 있다면)</summary>
	HIGHEST_ACCURACY_WITHOUT_ME = 2019,
	/// <summary>자신을 제외한 회피율이 가장 높은 타겟 선택 (회피율 속성이 있다면)</summary>
	HIGHEST_EVASION_WITHOUT_ME = 2020,
	/// <summary>남은 체력 비율이 가장 낮은 타겟 선택</summary>
	LOWEST_LIFE_RATE = 3002,
	/// <summary>속도게이지가 가장 낮은 타겟 선택</summary>
	LOWEST_ACCUM_RAPIDITY_POINT = 3005,
	/// <summary>명중률이 가장 낮은 타겟 선택</summary>
	LOWEST_ACCURACY = 3006,
	/// <summary>회피율이 가장 낮은 타겟 선택</summary>
	LOWEST_EVASION = 3007,
	/// <summary>남은 체력이 가장 낮은 인간 종족 선택</summary>
	LOWEST_LIFE_VALUE_HUMAN = 3008,
	/// <summary>남은 체력이 가장 낮은 엘프 종족 선택</summary>
	LOWEST_LIFE_VALUE_ELF = 3009,
	/// <summary>남은 체력이 가장 낮은 수인 종족 선택</summary>
	LOWEST_LIFE_VALUE_WEREBEAST = 3010,
	/// <summary>남은 체력이 가장 낮은 안드로이드 선택</summary>
	LOWEST_LIFE_VALUE_ANDROID = 3011,
	/// <summary>남은 체력이 가장 낮은 악마 종족 선택</summary>
	LOWEST_LIFE_VALUE_DEVIL = 3012,
	/// <summary>남은 체력이 가장 낮은 천사 종족 선택</summary>
	LOWEST_LIFE_VALUE_ANGEL = 3013,
	/// <summary>자신을 제외한 남은 체력이 가장 낮은 타겟 선택</summary>
	LOWEST_LIFE_VALUE_WITHOUT_ME = 3014,
	/// <summary>자신을 제외한 남은 체력 비율이 가장 낮은 타겟 선택</summary>
	LOWEST_LIFE_RATE_WITHOUT_ME = 3015,
	/// <summary>자신을 제외한 공격력이 가장 낮은 타겟 선택</summary>
	LOWEST_ATTACK_WITHOUT_ME = 3016,
	/// <summary>자신을 제외한 방어력이 가장 낮은 타겟 선택</summary>
	LOWEST_DEFENSE_WITHOUT_ME = 3017,
	/// <summary>자신을 제외한 공속이 가장 낮은 타겟 선택</summary>
	LOWEST_RAPIDITY_WITHOUT_ME = 3018,
	/// <summary>자신을 제외한 명중률이 가장 낮은 타겟 선택</summary>
	LOWEST_ACCURACY_WITHOUT_ME = 3019,
	/// <summary>자신을 제외한 회피율이 가장 낮은 타겟 선택</summary>
	LOWEST_EVASION_WITHOUT_ME = 3020,
	/// <summary>자신을 포함한 남은 체력이 가장 낮은 타겟 선택</summary>
	LOWEST_LIFE_VALUE_WITH_ME = 3021,
	/// <summary>자신을 포함한 남은 체력 비율이 가장 낮은 타겟 선택</summary>
	LOWEST_LIFE_RATE_WITH_ME = 3022,
	/// <summary>자신을 포함한 공격력이 가장 낮은 타겟 선택</summary>
	LOWEST_ATTACK_WITH_ME = 3023,
	/// <summary>자신을 포함한 방어력이 가장 낮은 타겟 선택</summary>
	LOWEST_DEFENSE_WITH_ME = 3024,
	/// <summary>자신을 포함한 공속이 가장 낮은 타겟 선택</summary>
	LOWEST_RAPIDITY_WITH_ME = 3025,
	/// <summary>자신을 포함한 명중률이 가장 낮은 타겟 선택</summary>
	LOWEST_ACCURACY_WITH_ME = 3026,
	/// <summary>자신을 포함한 회피율이 가장 낮은 타겟 선택</summary>
	LOWEST_EVASION_WITH_ME = 3027,
	/// <summary>자신보다 약한 속성 타겟 선택 (상성 시스템이 있을 경우)</summary>
	WEAK_ELEMENT = 4001,
	/// <summary>자신보다 강한 속성 타겟 선택 (상성 시스템이 있을 경우)</summary>
	STRONG_ELEMENT = 4002,
	/// <summary>버프 효과가 있는 타겟 선택</summary>
	GAIN_BUFF_DURATION = 5001,
	/// <summary>버프 효과 중 공격력 증가 효과가 있는 타겟 선택</summary>
	GAIN_BUFF_DURATION_ATTACK_INC = 5002,
	/// <summary>버프 효과 중 방어력 증가 효과가 있는 타겟 선택</summary>
	GAIN_BUFF_DURATION_DEFENSE_INC = 5003,
	/// <summary>버프 효과 중 공속 증가 효과가 있는 타겟 선택</summary>
	GAIN_BUFF_DURATION_RAPIDITY_INC = 5004,
	/// <summary>버프 효과 중 회피율 증가 효과가 있는 타겟 선택</summary>
	GAIN_BUFF_DURATION_EVASION_INC = 5005,
	/// <summary>버프 효과 중 치명타 확률 증가 효과가 있는 타겟 선택</summary>
	GAIN_BUFF_DURATION_CRITICAL_RATE_INC = 5006,
	/// <summary>버프 효과 중 치명타 피해량 증가 효과가 있는 타겟 선택</summary>
	GAIN_BUFF_DURATION_CRITICAL_DAMAGE_INC = 5007,
	/// <summary>디버프 효과가 있는 타겟 선택</summary>
	GAIN_DEBUFF_DURATION = 6001,
	/// <summary>디버프 효과 중 공격력 감소 효과가 있는 타겟 선택</summary>
	GAIN_DEBUFF_DURATION_ATTACK_DEC = 6002,
	/// <summary>디버프 효과 중 방어력 감소 효과가 있는 타겟 선택</summary>
	GAIN_DEBUFF_DURATION_DEFENSE_DEC = 6003,
	/// <summary>디버프 효과 중 화상 효과가 있는 타겟 선택</summary>
	GAIN_DEBUFF_DURATION_BURN = 6004,
	/// <summary>디버프 효과 중 출혈 효과가 있는 타겟 선택</summary>
	GAIN_DEBUFF_DURATION_BLEEDING = 6005,
	/// <summary>디버프 효과 중 중독 효과가 있는 타겟 선택</summary>
	GAIN_DEBUFF_DURATION_POISON = 6006,
	/// <summary>디버프 효과 중 공속 감소 효과가 있는 타겟 선택</summary>
	GAIN_DEBUFF_DURATION_RAPIDITY_DEC = 6007,
	/// <summary>디버프 효과 중 수면 효과가 있는 타겟 선택</summary>
	GAIN_DEBUFF_DURATION_SLEEP = 6008,
	/// <summary>디버프 효과 중 치명상 효과가 있는 타겟 선택</summary>
	GAIN_DEBUFF_DURATION_FATAL_WOUNDS = 6009,
	/// <summary>디버프 효과 중 명중률 감소 효과가 있는 타겟 선택</summary>
	GAIN_DEBUFF_DURATION_ACCURACY_DEC = 6010,
}

///	<summary>
///	두번째 타겟룰 타입
///	처음 타겟팅 할때가 아닌, 이펙트 등이 타겟에 도달했을 때 추가적으로 타겟팅 해야할 경우에 사용
///	</summary>
public enum SECOND_TARGET_RULE_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>타겟을 중심으로 지정 반경내에 감지되는 타겟 검사</summary>
	AROUND_SPLASH = 1,
	/// <summary>타겟 뒤로 지정 반경내에 감지되는 타겟 검사</summary>
	BACK_SPLASH = 2,
}

///	<summary>
///	스킬 효과 타입 (사용하지 않을 예정)
///	</summary>
public enum EFFECT_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>대미지 만큼 현재 체력 감소</summary>
	DAMAGE = 1,
	/// <summary>다음 피해의 데미지를 감소</summary>
	NEXT_DAMAGE_REDUCT = 2,
	///	<summary>
	///	대상의 현재 HP를 회복. 
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
	/// <summary>공격력을 기준으로 절대값 계산을 하기 위한 수치</summary>
	ATTACK_VALUE = 100,
	/// <summary>공격력을 기준으로 배율 계산을 하기 위한 수치</summary>
	ATTACK = 101,
	/// <summary>방어력을 기준으로 계산을 하기 위한 수치</summary>
	DEFENSE = 200,
	/// <summary>최대 체력을 기준으로 계산을 하기 위한 수치</summary>
	MAX_LIFE = 300,
	/// <summary>현재 체력을 기준으로 계산을 하기 위한 수치</summary>
	LIFE = 400,
	/// <summary>크리티컬 확률을 기준으로 계산을 하기 위한 수치</summary>
	CRITICAL_RATE = 500,
	/// <summary>크리티컬 파워를 기준으로 계산을 하기 위한 수치</summary>
	CRITICAL_POWER = 600,
	/// <summary>명중률을 기준으로 계산을 하기 위한 수치</summary>
	ACCURACY = 700,
	/// <summary>회피율을 기준으로 계산을 하기 위한 수치</summary>
	EVASION = 800,
	/// <summary>피해량을 기준으로 계산을 하기 위한 수치</summary>
	DAMAGE = 900,
}

///	<summary>
///	팀 타입
///	</summary>
public enum TEAM_TYPE
{
	/// <summary>왼쪽 팀(아군)</summary>
	LEFT = 0,
	/// <summary>오른쪽 팀(적군)</summary>
	RIGHT = 1,
}

///	<summary>
///	신체 터치 부위 구분
///	</summary>
public enum TOUCH_BODY_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>몸체</summary>
	BODY = 1,
	/// <summary>가슴</summary>
	BREAST = 2,
	/// <summary>머리</summary>
	HEAD = 3,
	/// <summary>입</summary>
	MOUTH = 4,
	/// <summary>골반</summary>
	PELVIS = 5,
	/// <summary>다리</summary>
	LEG = 6,
	/// <summary>뿔</summary>
	HORN = 7,
	/// <summary>브라</summary>
	BRA = 8,
	/// <summary>팬티</summary>
	PANTY = 9,
	/// <summary>귀</summary>
	EAR = 10,
	/// <summary>얼굴</summary>
	FACE = 11,
	/// <summary>엉덩이</summary>
	HIP = 12,
	/// <summary>장난감</summary>
	TOY = 13,
}

///	<summary>
///	터치 종류 구분
///	</summary>
public enum TOUCH_GESTURE_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>누른 즉시</summary>
	DOWN = 1,
	/// <summary>뗀 즉시</summary>
	UP = 2,
	/// <summary>터치</summary>
	TOUCH = 3,
	/// <summary>더블터치</summary>
	DOUBLE_TOUCH = 4,
	/// <summary>드래그</summary>
	DRAG = 5,
	/// <summary>쓰다듬기</summary>
	NADE = 6,
}

///	<summary>
///	타겟 팀 타입
///	</summary>
public enum TARGET_TYPE
{
	/// <summary>아군</summary>
	MY_TEAM = 0,
	/// <summary>적군</summary>
	ENEMY_TEAM = 1,
}

///	<summary>
///	일회성 효과 타입
///	</summary>
public enum ONETIME_EFFECT_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>데미지를 준다</summary>
	DAMAGE = 1,
	/// <summary>체력 회복</summary>
	LIFE_RECOVERY = 2,
}

///	<summary>
///	지속성 효과 타입
///	</summary>
public enum DURATION_EFFECT_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>피해 감소</summary>
	DAMAGE_REDUCE = 1,
	/// <summary>중독</summary>
	POISON = 101,
	/// <summary>기절</summary>
	STUN = 102,
	/// <summary>침묵</summary>
	SILENCE = 103,
	/// <summary>결박</summary>
	BIND = 104,
	/// <summary>빙결</summary>
	FREEZE = 105,
	/// <summary>공격력 증가</summary>
	ATK_UP = 106,
	/// <summary>방어력 증가</summary>
	DEF_UP = 107,
	/// <summary>공격력 감소</summary>
	ATK_DOWN = 108,
	/// <summary>방어력 감소</summary>
	DEF_DOWN = 109,
}

///	<summary>
///	지속성 방식 타입
///	</summary>
public enum PERSISTENCE_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>시간 지속</summary>
	TIME = 1,
	/// <summary>피격 횟수 제한</summary>
	HITTED = 2,
	/// <summary>공격 횟수 제한</summary>
	ATTACK = 3,
}

///	<summary>
///	투사체 타입
///	</summary>
public enum PROJECTILE_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>투사체를 타겟의 발 밑에 던진다</summary>
	THROW_FOOT = 1,
	/// <summary>투사체를 타겟의 몸에 던진다</summary>
	THROW_BODY = 2,
	/// <summary>투사체를 타겟의 머리에 던진다</summary>
	THROW_HEAD = 3,
	/// <summary>타겟의 발 밑에서 즉시 효과 발동</summary>
	INSTANT_TARGET_FOOT = 11,
	/// <summary>타겟의 몸에서 즉시 효과 발동</summary>
	INSTANT_TARGET_BODY = 12,
	/// <summary>타겟의 머리에서 즉시 효과 발동</summary>
	INSTANT_TARGET_HEAD = 13,
	/// <summary>전체 선택(진영의 중앙)</summary>
	ALL_ROUND = 21,
}

///	<summary>
///	부등호 타입
///	</summary>
public enum INEQUALITY_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>같음</summary>
	EQUAL = 1,
	/// <summary>다름</summary>
	NOT_EQUAL = 2,
	/// <summary>초과</summary>
	GREATER = 3,
	/// <summary>미만</summary>
	LESS = 4,
	/// <summary>이상</summary>
	GREATER_EQUAL = 5,
	/// <summary>이하</summary>
	LESS_EQUAL = 6,
}

///	<summary>
///	SKII TYPE
///	</summary>
public enum SKILL_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>일반 공격</summary>
	NORMAL_ATTACK = 1,
	/// <summary>스킬 공격 1</summary>
	SKILL_01 = 2,
	/// <summary>스킬 공격 2</summary>
	SKILL_02 = 3,
	/// <summary>궁극기</summary>
	SPECIAL_SKILL = 4,
}

///	<summary>
///	아이템의 종류 타입
///	</summary>
public enum ITEM_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>금화(게임내 사용되는 재화)</summary>
	GOLD = 1,
	/// <summary>보석(게임내 사용되는 유료 재화)</summary>
	DIA = 2,
	/// <summary>던전 입장 티켓</summary>
	DUNGEON_TICKET = 3,
	/// <summary>캐릭터 조각</summary>
	CHARACTER_PIECE = 4,
	/// <summary>경험치 물약</summary>
	EXP_POTION = 5,
	/// <summary>스테미나 회복 물약</summary>
	STA_POTION = 6,
	/// <summary>메모리얼에서 사용될 아이템</summary>
	MEMORIAL_ITEM = 7,
	/// <summary>캐릭터 완전체</summary>
	CHARACTER = 8,
	/// <summary>각종 소모용 아이템</summary>
	EXPENDABLE_ITEM = 9,
	/// <summary>장비</summary>
	EQUIPMENT_ITEM = 10,
}

///	<summary>
///	PC or NPC TYPE
///	</summary>
public enum CHARACTER_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>PLAYER CHARACTER</summary>
	PC = 1,
	/// <summary>NPC</summary>
	NPC = 2,
}

