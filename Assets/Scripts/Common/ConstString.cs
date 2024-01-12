public static class ConstString
{
    public static string FormatHeroAge(int age) => $"{age}세";
    public static string FormatHeroBirthdate(int[] birthdate) => $"{birthdate[0]}월 {birthdate[1]}일";
    public static string FormatHeroHeight(int height) => $"{height}cm";

    public static readonly string[] Hero_Tribes = { "NONE", "인간", "엘프", "퍼리", "안드로이드", "악마", "천사" };
    public static readonly string[] Hero_Roles = { "NONE", "탱커", "딜러", "서포터", "힐러" };
}
