public static class GameCalc
{
    public static double GetCombatPoint(double hp, double attack_dmg, double attack_def, double auto_recovery_life, double evation, double attack_recovery, double accuracy, int sum_skills_level)
    {
        var cp =
            hp * 0.1f
            + (attack_dmg + attack_def) * 4.5f
            + auto_recovery_life * 0.1f
            + evation * 6f
            + attack_recovery * 4.5f
            + accuracy * 2f
            + sum_skills_level * 10f;
        return cp;
    }
}
