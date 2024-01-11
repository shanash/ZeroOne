using UnityEngine;

public class HeroListItem : MonoBehaviour
{
    [SerializeField, Tooltip("Box")]
    RectTransform Box;

    [SerializeField, Tooltip("Hero Card")]
    HeroCardBase Card;

    System.Action<HeroListItem> Click_Hero_Callback;

    CHARACTER_SORT Filter_Type;

    public UserHeroData User_Data { get; private set; }

    public void SetUserHeroData(UserHeroData ud, CHARACTER_SORT filter = CHARACTER_SORT.NAME)
    {
        User_Data = ud;
        Filter_Type = filter;
        UpdateCellItem();
    }

    public void UpdateCellItem()
    {
        if (User_Data == null)
        {
            Box.gameObject.SetActive(false);
            return;
        }
        if (!Box.gameObject.activeSelf)
        {
            Box.gameObject.SetActive(true);
        }

        Card.SetHeroData(User_Data, Filter_Type);
    }

    public void SetClickHeroCallback(System.Action<HeroListItem> callback)
    {
        Click_Hero_Callback = callback;
    }

    private void OnEnable()
    {
        if (Card != null)
        {
            Card.AddTouchEventCallback(TouchEventCallback);
        }
    }

    private void OnDisable()
    {
        if (Card != null)
        {
            Card.RemoveTouchEventCallback(TouchEventCallback);
        }
    }

    void TouchEventCallback(TOUCH_RESULT_TYPE result)
    {
        if (result == TOUCH_RESULT_TYPE.CLICK)
        {
            if (User_Data == null)
            {
                return;
            }
            AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
            Click_Hero_Callback?.Invoke(this);
        }
    }
}
