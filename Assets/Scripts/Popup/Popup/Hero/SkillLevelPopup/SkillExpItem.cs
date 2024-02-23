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

        UserItemData Data = null;

        public delegate RESPONSE_TYPE OnChangedUseCount(int item_id, int count);
        OnChangedUseCount On_Changed_Use_Count = null;
        /// <summary>
        /// 사용 개수(선택 개수)
        /// </summary>
        int Use_Count { get; set; } = 0;
        /// <summary>
        /// 실제 보유 수량
        /// </summary>
        int Exist_Count { get { return (Data == null) ? 0 : (int)Data.GetCount(); } }

        public void Initialize(UserItemData data, OnChangedUseCount callback_changed_use_item)
        {
            InitializeField();

            Data = data;
            On_Changed_Use_Count = callback_changed_use_item;
            //  스킬 경험치 아이템의 배경이미지는 교체가 가능하지만, 캐릭터 경험치의 배경 이미지는 교체할 수 없는 로직임.
            //  각 아이템의 등급이 별도로 있지 않다고 함. 이 기능은 추후 사용하지 않을 수도 있음. 추후 체크 필요. [ForestJ]
            int index = Array.FindIndex(SPRITE_IDX_BY_ITEM_IDs, (item) => item == Data.Item_ID);
            if (index != -1)
            {
                BackGround.sprite = BG_Sprites[index];
            }

            UpdateUI();
        }

        public void OnClickAdd()
        {
            AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
            SetUseCount(Use_Count + 1);
        }

        public void OnClickRemove()
        {
            AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
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

            var error_code = On_Changed_Use_Count?.Invoke(Data.Item_ID, count);
            if (error_code == RESPONSE_TYPE.SUCCESS || error_code == RESPONSE_TYPE.LEVEL_UP_SUCCESS)
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
