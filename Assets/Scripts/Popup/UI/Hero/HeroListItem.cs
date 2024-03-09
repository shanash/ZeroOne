using UnityEngine;

public class HeroListItem : MonoBehaviour
{
    [SerializeField, Tooltip("Box")]
    RectTransform Box;

    [SerializeField, Tooltip("Hero Card")]
    HeroCardBase Card;

    System.Action<HeroListItem> Click_Hero_Callback;

    CHARACTER_SORT Filter_Type;

    public BattlePcData BattlePcData { get; private set; }

    public void SetUserHeroData(BattlePcData ud, CHARACTER_SORT filter = CHARACTER_SORT.NAME)
    {
        BattlePcData = ud;
        Filter_Type = filter;
        UpdateCellItem();
    }

    public void UpdateCellItem()
    {
        if (BattlePcData == null)
        {
            Box.gameObject.SetActive(false);
            return;
        }
        if (!Box.gameObject.activeSelf)
        {
            Box.gameObject.SetActive(true);
        }

        Card.SetHeroData(BattlePcData, Filter_Type);
    }

    public void SetClickHeroCallback(System.Action<HeroListItem> callback)
    {
        Click_Hero_Callback = callback;
    }

    public void TouchEventCallback(TOUCH_RESULT_TYPE result)
    {
        if (result == TOUCH_RESULT_TYPE.CLICK)
        {
            if (BattlePcData == null)
            {
                return;
            }
            AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
            Click_Hero_Callback?.Invoke(this);
        }
    }
}
