public static class Essence_Status_Data_Define
{
    public static string ToStringForUI(this Essence_Status_Data value)
    {
        string result = string.Empty;
        AbilityToString(ref result, ConstString.Hero.ATTACK_DAMAGE, value.add_atk);
        AbilityToString(ref result, ConstString.Hero.ATTACK_DEFENSE, value.add_matk);
        AbilityToString(ref result, ConstString.Hero.MAGIC_DAMAGE, value.add_def);
        AbilityToString(ref result, ConstString.Hero.MAGIC_DEFENSE, value.add_mdef);
        AbilityToString(ref result, ConstString.Hero.LIFE_POINT, value.add_hp);

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

            result += $"{name} {ConstString.FormatPlus(value)}";
        }
    }
}
