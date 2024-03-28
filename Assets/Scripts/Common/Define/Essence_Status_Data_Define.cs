using Cysharp.Text;

public static class Essence_Status_Data_Define
{
    public static string ToStringForUI(this Essence_Status_Data value)
    {
        string result = string.Empty;
        AbilityToString(ref result, GameDefine.GetLocalizeString("system_stat_physics_attack"), value.add_atk);
        AbilityToString(ref result, GameDefine.GetLocalizeString("system_stat_physics_defence"), value.add_matk);
        AbilityToString(ref result, GameDefine.GetLocalizeString("system_stat_magic_attack"), value.add_def);
        AbilityToString(ref result, GameDefine.GetLocalizeString("system_stat_magic_defence"), value.add_mdef);
        AbilityToString(ref result, GameDefine.GetLocalizeString("system_stat_life"), value.add_hp);

        return result;
    }

    static void AbilityToString(ref string result, string name, int value)
    {
        if (value > 0)
        {
            if (!string.IsNullOrEmpty(result))
            {
                result += ", ";
            }

            result += $"{name} {ZString.Format(GameDefine.GetLocalizeString("system_plus_format"), value)}";
        }
    }
}
