using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SkillLevelPopup
{
    public class SkillExpItem : MonoBehaviour
    {
        static readonly int[] SPRITE_IDX_BY_ITEM_IDs = new int[] { 20, 19, 18, 17, 16 };

        [SerializeField, Tooltip("경험치 아이템 갯수 텍스트")]
        TMP_Text Exist_Count_UI = null;

        [SerializeField, Tooltip("사용한 아이템 갯수 텍스트")]
        TMP_Text Use_Count_UI = null;

        [SerializeField, Tooltip("선택 UI")]
        GameObject Select = null;

        [SerializeField, Tooltip("경험치 아이템별 배경 UI")]
        Image BackGround = null;

        [SerializeField, Tooltip("경험치 아이템별로 다르게 배치할 스프라이트 리소스")]
        Sprite[] BG_Sprites = null;

        UserItem_NormalItemData Data = null;

        public delegate ERROR_CODE OnChangedUseCount(int item_id, int count);
        OnChangedUseCount On_Changed_Use_Count = null;

        int Use_Count { get; set; } = 0;
        int Exist_Count { get { return (Data == null) ? 0 : (int)Data.GetCount(); } }

        public void Initialize(UserItem_NormalItemData data, OnChangedUseCount callback_changed_use_item)
        {
            InitializeField();

            Data = data;
            On_Changed_Use_Count = callback_changed_use_item;
            int index = Array.FindIndex(SPRITE_IDX_BY_ITEM_IDs, (item) => item == Data.Data.item_id);
            BackGround.sprite = BG_Sprites[index];
            UpdateUI();
        }

        public void OnClickAdd()
        {
            SetUseCount(Use_Count + 1);
        }

        public void OnClickRemove()
        {
            SetUseCount(Use_Count - 1);
        }

        private void InitializeField()
        {
            Data = null;
            On_Changed_Use_Count = null;
            Use_Count = 0;
        }

        void UpdateUI()
        {
            Select.SetActive(Use_Count > 0);
            Exist_Count_UI.text = Exist_Count.ToString("N0");
            Use_Count_UI.text = $"{Use_Count.ToString("N0")}/{Exist_Count.ToString("N0")}";
        }

        public bool SetUseCount(int count)
        {
            if (!CheckAvailableCount(count))
            {
                return false;
            }

            var error_code = On_Changed_Use_Count?.Invoke(Data.Data.item_id, count);
            if (error_code == ERROR_CODE.SUCCESS || error_code == ERROR_CODE.LEVEL_UP_SUCCESS)
            {
                Use_Count = count;
            }
            UpdateUI();

            return true;
        }

        bool CheckAvailableCount(int count)
        {
            return 0 <= count && count <= Exist_Count;
        }
    }
}
