using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class LocalizedTMPText : MonoBehaviour
{
    [SerializeField, Tooltip("String Table 키")]
    string Key_Of_StringTable = string.Empty;

    void Start()
    {
        var tmp = this.GetComponent<TMP_Text>();
        tmp.text = GameDefine.GetLocalizeString(Key_Of_StringTable);
    }
}
