using UnityEngine;

public class HeroInfoBoxLevelUp : MonoBehaviour
{
    //BattleUnitData Unit_Data;

    public void SetHeroData(BattleUnitData data)
    {
        _ = data;
        //Unit_Data = data;

        Refresh();
    }

    void Refresh()
    {
    }

    public void OnSelectedTab(Gpm.Ui.Tab tab)
    {
        Refresh();
    }
}
