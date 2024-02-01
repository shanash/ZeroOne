using FluffyDuck.UI;
using UnityEngine;

public class SkillInfoTooltipPopup : PopupBase
{
    [SerializeField, Tooltip("Container")]
    RectTransform Container;

    Player_Character_Skill_Data Skill_Data;
    Vector2 Target_Position = Vector2.zero;
    public override void ShowPopup(params object[] data)
    {
        if (data.Length != 2)
        {
            HidePopup();
            return;
        }

        int skill_id = (int)data[0];
        Target_Position = (Vector2)data[1];
        if (skill_id != 0)
        {
            Skill_Data = MasterDataManager.Instance.Get_PlayerCharacterSkillData(skill_id);
        }

        base.ShowPopup(data);

        FixedUpdatePopup();
    }

    protected override void FixedUpdatePopup()
    {
        //Vector2 size = Container.rect.size;
        //bool a = false;
        Container.transform.position = Target_Position;
    }
}
