using UnityEngine;
using UnityEngine.UI;

public class LobbyMainButton : UIButtonBase
{
    [SerializeField, Tooltip("Icon Group")]
    CanvasGroup Group;

    [SerializeField, Tooltip("Badge")]
    Image Badge;

    [SerializeField, Tooltip("Lock Icon")]
    Image Lock_Icon;

    [SerializeField, Tooltip("Temp Open")]
    bool Is_Temp_Open;

    protected override void Start()
    {
        UpdateButtonUI();
    }

    void UpdateButtonUI()
    {
        Lock_Icon.gameObject.SetActive(!Is_Temp_Open);
        Badge.gameObject.SetActive(false);

        Group.alpha = Is_Temp_Open ? 1f : 0.5f;
    }
}
