using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FilterRadioBtn : MonoBehaviour
{
    [SerializeField, Tooltip("Filter Type")]
    CHARACTER_SORT Filter_Type = CHARACTER_SORT.NAME;

    [SerializeField, Tooltip("Toggle Btn")]
    Toggle Toggle_Btn;

    [SerializeField, Tooltip("Filter Text")]
    TMP_Text Filter_Text;

    private void Start()
    {
        Filter_Text.text = GetFilterString(Filter_Type);
    }

    string GetFilterString(CHARACTER_SORT ftype)
    {
        string filter = string.Empty;
        switch (ftype)
        {
            case CHARACTER_SORT.NAME:
                filter = GameDefine.GetLocalizeString("system_sorting_name_01");
                break;
            case CHARACTER_SORT.LEVEL_CHARACTER:
                filter = GameDefine.GetLocalizeString("system_sorting_name_02");
                break;
            case CHARACTER_SORT.STAR:
                filter = GameDefine.GetLocalizeString("system_sorting_name_03");
                break;
            case CHARACTER_SORT.DESTINY:
                break;
            case CHARACTER_SORT.SKILL_LEVEL:
                filter = GameDefine.GetLocalizeString("system_sorting_name_04");
                break;
            case CHARACTER_SORT.EX_SKILL_LEVEL:
                filter = GameDefine.GetLocalizeString("system_sorting_name_05");
                break;
            case CHARACTER_SORT.ATTACK:
                filter = GameDefine.GetLocalizeString("system_sorting_name_06");
                break;
            case CHARACTER_SORT.DEFEND:
                filter = GameDefine.GetLocalizeString("system_sorting_name_07");
                break;
            case CHARACTER_SORT.RANGE:
                filter = GameDefine.GetLocalizeString("system_sorting_name_08");
                break;
            case CHARACTER_SORT.LIKEABILITY:
                break;
            case CHARACTER_SORT.ATTRIBUTE:
                filter = GameDefine.GetLocalizeString("system_sorting_name_09");
                break;
            case CHARACTER_SORT.BATTLEPOWER:
                filter = GameDefine.GetLocalizeString("system_stat_battlepower");
                break;
            default:
                Debug.Assert(false);
                break;
        }

        return filter;
    }

    public CHARACTER_SORT GetCharacterFilterType()
    {
        return Filter_Type;
    }

    public void SetFilterOn()
    {
        Toggle_Btn.isOn = true;
    }

}
