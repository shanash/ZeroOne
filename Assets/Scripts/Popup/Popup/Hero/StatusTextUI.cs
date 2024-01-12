using TMPro;
using UnityEngine;

public class StatusTextUI : MonoBehaviour
{
    [SerializeField]
    TMP_Text _Subject;
    [SerializeField]
    TMP_Text _Value;

    public TMP_Text Subject => _Subject;
    public TMP_Text Value => _Value;
}
