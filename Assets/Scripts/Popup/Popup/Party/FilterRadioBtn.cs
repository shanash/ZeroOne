using System.Collections;
using System.Collections.Generic;
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
        switch (Filter_Type)
        {
            case CHARACTER_SORT.NAME:
                Filter_Text.text = "이름";
                break;
            case CHARACTER_SORT.LEVEL_CHARACTER:
                Filter_Text.text = "레벨";
                break;
            case CHARACTER_SORT.STAR:
                Filter_Text.text = "성급";
                break;
            case CHARACTER_SORT.DESTINY:
                Filter_Text.text = "인연 랭크";
                break;
            case CHARACTER_SORT.SKILL_LEVEL:
                Filter_Text.text = "스킬 레벨";
                break;
            case CHARACTER_SORT.EX_SKILL_LEVEL:
                Filter_Text.text = "궁극기 스킬 레벨";
                break;
            case CHARACTER_SORT.ATTACK:
                Filter_Text.text = "공격력";
                break;
            case CHARACTER_SORT.DEFEND:
                Filter_Text.text = "방어력";
                break;
            case CHARACTER_SORT.RANGE:
                Filter_Text.text = "사거리";
                break;
            case CHARACTER_SORT.LIKEABILITY:
                Filter_Text.text = "호감도";
                break;
        }
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
