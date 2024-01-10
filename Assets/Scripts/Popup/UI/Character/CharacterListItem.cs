using UnityEngine;

public class CharacterListItem : MonoBehaviour
{
    [SerializeField, Tooltip("Box")]
    RectTransform Box;

    [SerializeField, Tooltip("Hero Card")]
    HeroCardBase Card;

    UserHeroData User_Data;
    CHARACTER_SORT Filter_Type;

    public void SetClickHeroCallback(System.Action<CharacterListItem> callback)
    {
        //Click_Hero_Callback = callback;
    }

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
}
