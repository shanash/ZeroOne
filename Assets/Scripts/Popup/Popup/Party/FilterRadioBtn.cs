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
        Filter_Text.text = GameDefine.GetFilterString(Filter_Type);
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
