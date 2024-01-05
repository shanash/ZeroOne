using Cysharp.Text;
using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PartySelectInfo : MonoBehaviour
{
    [SerializeField, Tooltip("Box")]
    RectTransform Box;
    [SerializeField, Tooltip("Hero Card")]
    HeroCardBase Card;

    [SerializeField, Tooltip("Star Points")]
    List<Image> Star_Points;

    [SerializeField, Tooltip("Level Text")]
    TMP_Text Level_Text;
    [SerializeField, Tooltip("Name Text")]
    TMP_Text Name_Text;

    [SerializeField, Tooltip("Position Tag")]
    Image Position_Tag;

    [SerializeField, Tooltip("Role Tag BG")]
    Image Role_Tag_BG;
    [SerializeField, Tooltip("Role Tag Icon")]
    Image Role_Icon;
    [SerializeField, Tooltip("Role Name")]
    TMP_Text Role_Name;

    //  todo [skill list]

    UserHeroData User_Data;
    
    public void SetPlayerCharacterID(int pc_id, int pc_num)
    {
        User_Data = GameData.Instance.GetUserHeroDataManager().FindUserHeroData(pc_id, pc_num);
        UpdateSelectInfo();
    }

    void UpdateSelectInfo()
    {
        if (User_Data == null)
        {
            UpdateEmptyInfo();
            return;
        }
        ShowInfoBox(true);

        var m = MasterDataManager.Instance;
        //  card
        Card.SetHeroDataID(User_Data.GetPlayerCharacterID());
        //  star
        int star_cnt = User_Data.GetStarGrade();
        for (int i = 0; i < Star_Points.Count; i++)
        {
            var star = Star_Points[i];
            star.gameObject.SetActive(i < star_cnt);
        }
        //  lv
        Level_Text.text = ZString.Format("LV.{0}", User_Data.GetLevel());

        //  name
        Name_Text.text = User_Data.GetPlayerCharacterData().name_kr;

        //  position
        var pos_data = m.Get_PositionIconData(User_Data.GetPositionType());
        //Position_Tag.sprite
        CommonUtils.GetResourceFromAddressableAsset<Sprite>(pos_data.icon, (spr) =>
        {
            Position_Tag.sprite = spr;
        });

        //  role type
        var role_data = m.Get_RoleIconData(User_Data.GetPlayerCharacterData().role_type);

        //  role tag
        CommonUtils.GetResourceFromAddressableAsset<Sprite>(role_data.tag_bg_path, (spr) =>
        {
            Role_Tag_BG.sprite = spr;
        });

        //  role icon
        CommonUtils.GetResourceFromAddressableAsset<Sprite>(role_data.icon, (spr) =>
        {
            Role_Icon.sprite = spr;
        });
        Role_Name.text = role_data.name_kr;
    }
    void UpdateEmptyInfo()
    {
        ShowInfoBox(false);
    }

    public void ShowInfoBox(bool show)
    {
        if (Box.gameObject.activeSelf != show)
        {
            Box.gameObject.SetActive(show);
        }
    }
}