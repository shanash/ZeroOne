using FluffyDuck.UI;
using UnityEngine;

public class SkillInfoTooltipPopup : PopupBase
{
    [SerializeField, Tooltip("Container")]
    RectTransform Container;

    Player_Character_Skill_Data Skill_Data;
    Vector2 Target_Position = Vector2.zero;
    protected override bool Initialize(object[] data)
    {
        if (data.Length != 2)
        {
            return false;
        }

        int skill_id = (int)data[0];
        Target_Position = (Vector2)data[1];
        if (skill_id != 0)
        {
            Skill_Data = MasterDataManager.Instance.Get_PlayerCharacterSkillData(skill_id);
        }

        FixedUpdatePopup();

        return true;
    }

    protected override void FixedUpdatePopup()
    {
        float rect_half_width = Container.rect.width * 0.5f;
        var pos = Target_Position;
        if (pos.x - rect_half_width < 0f)
        {
            pos.x += (rect_half_width - pos.x) + 20f;
            //pos.x = rect_half_width + 20f;
        }
        Container.transform.position = pos;
    }
}
