using FluffyDuck.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PartySelectSkillNode : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    [SerializeField, Tooltip("Box")]
    RectTransform Box;

    [SerializeField, Tooltip("Skill Type")]
    SKILL_TYPE Skill_Type = SKILL_TYPE.NONE;

    [SerializeField, Tooltip("Skill Type Text")]
    TMP_Text Skill_Type_Text;

    [SerializeField, Tooltip("Skill Icon")]
    Image Skill_Icon;

    [SerializeField, Tooltip("Skill Level")]
    TMP_Text Skill_Level;

    Vector2 Press_Scale = new Vector2(0.96f, 0.96f);

    private void Start()
    {
        switch (Skill_Type)
        {
            case SKILL_TYPE.SKILL_01:
                Skill_Type_Text.text = "액티브 스킬 1";
                break;
            case SKILL_TYPE.SKILL_02:
                Skill_Type_Text.text = "액티브 스킬 2";
                break;
            case SKILL_TYPE.SPECIAL_SKILL:
                Skill_Type_Text.text = "궁극기";
                break;
        }
    }

    public void SetPlayerCharacterID(int pc_id, int pc_num)
    {

    }


    public void OnPointerDown(PointerEventData eventData)
    {
        Box.localScale = Press_Scale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Box.localScale = Vector2.one;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Party/SkillInfoTooltipPopup", (popup) =>
        {
            popup.ShowPopup(0, (Vector2)transform.position);
        });
    }

}
