﻿///	<summary>
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
	/// <summary>1001 리더 선택</summary>
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
	/// <summary>0 타겟을 중심으로 지정 반경내에 감지되는 타겟 검사</summary>
	AROUND_SPLASH = 1,
	/// <summary>1 타겟 뒤로 지정 반경내에 감지되는 타겟 검사</summary>
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
	/// <summary>100 공격력을 기준으로 절대값 계산을 하기 위한 수치</summary>
	ATTACK_VALUE = 100,
	/// <summary>101 공격력을 기준으로 배율 계산을 하기 위한 수치</summary>
	ATTACK = 101,
	/// <summary>200 방어력을 기준으로 계산을 하기 위한 수치</summary>
	DEFENSE = 200,
	/// <summary>300 최대 체력을 기준으로 계산을 하기 위한 수치</summary>
	MAX_LIFE = 300,
	/// <summary>400 현재 체력을 기준으로 계산을 하기 위한 수치</summary>
	LIFE = 400,
	/// <summary>500 크리티컬 확률을 기준으로 계산을 하기 위한 수치</summary>
	CRITICAL_RATE = 500,
	/// <summary>600 크리티컬 파워를 기준으로 계산을 하기 위한 수치</summary>
	CRITICAL_POWER = 600,
	/// <summary>700 명중률을 기준으로 계산을 하기 위한 수치</summary>
	ACCURACY = 700,
	/// <summary>800 회피율을 기준으로 계산을 하기 위한 수치</summary>
	EVASION = 800,
	/// <summary>900 피해량을 기준으로 계산을 하기 위한 수치</summary>
	DAMAGE = 900,
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
	/// <summary>NONE</summary>
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
	/// <summary>7 뿔</summary>
	HORN = 7,
	/// <summary>8 브라</summary>
	BRA = 8,
	/// <summary>9 팬티</summary>
	PANTY = 9,
	/// <summary>10 귀</summary>
	EAR = 10,
	/// <summary>11 얼굴</summary>
	FACE = 11,
	/// <summary>12 엉덩이</summary>
	HIP = 12,
	/// <summary>13 장난감</summary>
	TOY = 13,
}

///	<summary>
///	터치 종류 구분
///	</summary>
public enum TOUCH_GESTURE_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>1 누른 즉시</summary>
	DOWN = 1,
	/// <summary>2 뗀 즉시</summary>
	UP = 2,
	/// <summary>3 터치</summary>
	TOUCH = 3,
	/// <summary>4 더블터치</summary>
	DOUBLE_TOUCH = 4,
	/// <summary>5 드래그</summary>
	DRAG = 5,
	/// <summary>6 쓰다듬기</summary>
	NADE = 6,
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
	/// <summary>1 데미지를 준다</summary>
	DAMAGE = 1,
	/// <summary>2 체력 회복</summary>
	LIFE_RECOVERY = 2,
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
	/// <summary>106 공격력 증가</summary>
	ATK_UP = 106,
	/// <summary>107 방어력 증가</summary>
	DEF_UP = 107,
	/// <summary>108 공격력 감소</summary>
	ATK_DOWN = 108,
	/// <summary>109 방어력 감소</summary>
	DEF_DOWN = 109,
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
///	아이템의 종류 타입
///	</summary>
public enum ITEM_TYPE
{
	/// <summary>NONE</summary>
	NONE = 0,
	/// <summary>1 금화(게임내 사용되는 재화)</summary>
	GOLD = 1,
	/// <summary>2 보석(게임내 사용되는 유료 재화)</summary>
	DIA = 2,
	/// <summary>3 던전 입장 티켓</summary>
	DUNGEON_TICKET = 3,
	/// <summary>4 캐릭터 조각</summary>
	CHARACTER_PIECE = 4,
	/// <summary>5 경험치 물약</summary>
	EXP_POTION = 5,
	/// <summary>6 스테미나 회복 물약</summary>
	STA_POTION = 6,
	/// <summary>7 사용하지 않음</summary>
	MEMORIAL_ITEM = 7,
	/// <summary>8 캐릭터 완전체</summary>
	CHARACTER = 8,
	/// <summary>9 각종 소모용 아이템</summary>
	EXPENDABLE_ITEM = 9,
	/// <summary>10 장비</summary>
	EQUIPMENT_ITEM = 10,
	/// <summary>11 호감도 아이템</summary>
	FAVORITE_ITEM = 11,
	/// <summary>12 스태미나</summary>
	STAMINA = 12,
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
///	재화 및 아이템 종류
///	</summary>
public enum GOODS_TYPE
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
	/// <summary>1000 장비 조각</summary>
	PIECE_EQUIPMENT = 1000,
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
	/// <summary>6 장비</summary>
	EQUIPMENT = 8,
	/// <summary>근원 전달 횟수(플레이어 보유)</summary>
	SEND_ESSENCE = 9,
	/// <summary>근원 받을 수 있는 횟수(캐릭터 공용 설정)</summary>
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
	/// <summary>113 스킬 경험치 아이템</summary>
	EXP_SKILL = 113,
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

