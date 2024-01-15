public static class ConstString
{
    public class Hero
    {
        public static string FormatHeroAge(int age) => $"{age}세";
        public static string FormatHeroBirthday(int[] birthdate) => $"{birthdate[0]}월 {birthdate[1]}일";
        public static string FormatHeroHeight(int height) => $"{height}cm";

        public static readonly string[] TRIBES = { "NONE", "인간", "엘프", "수인", "안드로이드", "악마", "천사" };
        public static readonly string[] ROLE = { "NONE", "탱커", "딜러", "서포터", "힐러" };
        public static readonly string[] SORT_FILLTER = { "이름", "레벨", "성급", "인연 레벨", "스킬 레벨", "궁극 스킬 레벨", "공격력", "방어력", "사정거리", "호감도" };

        public const string COMBAT_POWER = "전투력";
        public const string LIFE_POINT = "체력";
        public const string ATTACK_DAMAGE = "물리 공격력";
        public const string MAGIC_DAMAGE = "마법 공격력";
        public const string ATTACK_DEFENSE = "물리 방어력";
        public const string MAGIC_DEFENSE = "마법 방어력";
        public const string APPROACH_DISTANCE = "접근 사거리";
        public const string ATTACK_RECOVERY = "흡혈";
        public const string EVASION_POINT = "회피";
        public const string ACCURACY_POINT = "명중";
        public const string AUTO_RECORVERY = "자동 회복";
    }

    public static class HeroListUI
    {
        public const string TITLE = "캐릭터";
    }

    public static class HeroInfoUI
    {
        public const string TITLE = "캐릭터";

        public static readonly string[] TAB_NAMES = { "기본 정보", "레벨 업", "승급" };

        public const string ADVANCE_SKILL = "스킬 강화";
        public const string ADVANCE_EQUIPMENT = "장비 강화";
        public const string ADVANCE_WEAPON = "무기 강화";
    }

    public static class ProfilePopup
    {
        public const string AGE = "나이";
        public const string BIRTHDAY = "생일";
        public const string HEIGHT = "키";
        public const string HOBBY = "취미";
    }

    public static class StatusPopup
    {
        public const string TITLE = "능력치 상세정보";
    }

    public static class Message
    {
        public const string NOT_YET = "준비중 입니다.";
    }
}
