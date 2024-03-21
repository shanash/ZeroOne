using Cysharp.Text;
using FluffyDuck.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AttributePowerCompatibilityPopup : PopupBase
{
    [SerializeField, Tooltip("Popup Title")]
    TMP_Text Popup_Title;

    [SerializeField, Tooltip("상성표")]
    TMP_Text Title_Content_01;

    [SerializeField, Tooltip("시너지 효과")]
    TMP_Text Title_Content_02;

    [SerializeField, Tooltip("Compatibility Info Desc")]
    TMP_Text Compatibility_Info_Desc;

    [SerializeField, Tooltip("Electric Name")]
    TMP_Text Electric_Name;
    [SerializeField, Tooltip("Advantage Electric")]
    TMP_Text Advantage_Electric;

    [SerializeField, Tooltip("Veritarium Name")]
    TMP_Text Veritarium_Name;
    [SerializeField, Tooltip("Advantage Veritarium")]
    TMP_Text Advantage_Veritarium;

    [SerializeField, Tooltip("Charm Name")]
    TMP_Text Charm_Name;
    [SerializeField, Tooltip("Advantage Charm")]
    TMP_Text Advantage_Charm;

    [SerializeField, Tooltip("Mana Name")]
    TMP_Text Mana_Name;
    [SerializeField, Tooltip("Advantage Mana")]
    TMP_Text Advantage_Mana;

    [SerializeField, Tooltip("Synergy Superior List")]
    List<TMP_Text> Synergy_Superior_List;

    protected override bool Initialize(object[] data)
    {
        FixedUpdatePopup();
        return true;
    }

    protected override void FixedUpdatePopup()
    {
        var m = MasterDataManager.Instance;

        Popup_Title.text = GameDefine.GetLocalizeString("system_information");

        //  상성표
        Title_Content_01.text = GameDefine.GetLocalizeString("system_title_attribute_Info");
        //  시너지 효과
        Title_Content_02.text = GameDefine.GetLocalizeString("system_title_synergy_Info");

        //  desc
        Compatibility_Info_Desc.text = GameDefine.GetLocalizeString("system_attribute_info");

        //  electric
        var elec_attr_super = m.Get_AttributeSuperiorityData(ATTRIBUTE_TYPE.ELECTRICITY);
        var elec_attr = m.Get_AttributeIconData(ATTRIBUTE_TYPE.ELECTRICITY);
        
        Electric_Name.text = GameDefine.GetLocalizeString(elec_attr.name_id);
        Advantage_Electric.text = ZString.Format("{0}%", Math.Truncate(elec_attr_super.final_damage_per * 100));

        //  veritarium
        var veri_attr_super = m.Get_AttributeSuperiorityData(ATTRIBUTE_TYPE.VEGETARIUM);
        var veri_attr = m.Get_AttributeIconData (ATTRIBUTE_TYPE.VEGETARIUM);

        Veritarium_Name.text = GameDefine.GetLocalizeString(veri_attr.name_id);
        Advantage_Veritarium.text = ZString.Format("{0}%", Math.Truncate(veri_attr_super.final_damage_per * 100));

        //  charm
        var charm_attr_super = m.Get_AttributeSuperiorityData(ATTRIBUTE_TYPE.CHARM);
        var charm_attr = m.Get_AttributeIconData(ATTRIBUTE_TYPE.CHARM);
        
        Charm_Name.text = GameDefine.GetLocalizeString (charm_attr.name_id);
        Advantage_Charm.text = ZString.Format("{0}%", Math.Truncate(charm_attr_super.final_damage_per * 100));

        //  mana
        var mana_attr_super = m.Get_AttributeSuperiorityData(ATTRIBUTE_TYPE.MANA);
        var mana_attr = m.Get_AttributeIconData(ATTRIBUTE_TYPE.MANA);
        
        Mana_Name.text = GameDefine.GetLocalizeString (mana_attr.name_id);
        Advantage_Mana.text = ZString.Format("{0}%", Math.Truncate(mana_attr_super.final_damage_per * 100));

        //  synergy superior
        List<int> same_count_list = new List<int>();
        same_count_list.Add(2);
        same_count_list.Add(3);
        same_count_list.Add(4);
        same_count_list.Add(5);

        for (int i = 0; i < Synergy_Superior_List.Count; i++)
        {
            TMP_Text syenrgy_text = Synergy_Superior_List[i];
            int same_cnt = same_count_list[i];
            var synergy_data = m.Get_AttributeSynergyData(ATTRIBUTE_TYPE.ELECTRICITY, same_cnt);
            if (synergy_data != null)
            {
                syenrgy_text.text = ZString.Format(GameDefine.GetLocalizeString("system_attribute_synergy_guide"), synergy_data.same_attribute_count, synergy_data.add_damage_per.ToString("P0"));
            }
        }
    }

    public void OnClickDim()
    {
        if (Ease_Base != null && Ease_Base.IsPlaying())
        {
            return;
        }
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        HidePopup();
    }
}
