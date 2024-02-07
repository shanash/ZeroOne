using FluffyDuck.Util;
using Gpm.Ui;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SkillLevelPopup
{
    public class SkillInfoTab : MonoBehaviour
    {
        [SerializeField]
        string Default_Name = string.Empty;

        [SerializeField]
        CanvasGroup skill_contents_group = null;

        [SerializeField]
        GameObject lock_icon = null;

        [SerializeField]
        Image Icon = null;

        [SerializeField]
        TMP_Text Name = null;

        public void Set(UserHeroSkillData data)
        {
            if (data == null)
            {
                Icon.sprite = null;
                Name.text = Default_Name;
                return;
            }

            CommonUtils.GetResourceFromAddressableAsset<Sprite>(data.Group_Data.icon, (spr) =>
            {
                Icon.sprite = spr;
            });

            Name.text = data.Group_Data.name_kr;
        }

        public void OnChangeValue(bool is_on)
        {
            skill_contents_group.alpha = is_on ? 1f : 0.3f;
        }

        public void OnChangeBlock(bool is_blocked)
        {
            lock_icon.SetActive(is_blocked);
        }
    }
}
