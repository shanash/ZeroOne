using FluffyDuck.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameResultPlayerCharacterInfo : UIBase
{
    [SerializeField, Tooltip("Box")]
    RectTransform Box;

    [SerializeField, Tooltip("Level Text")]
    TMP_Text Level_Text;

    [SerializeField, Tooltip("Level Exp Slider")]
    Slider Level_Exp_Slider;
    [SerializeField, Tooltip("Level Exp Text")]
    TMP_Text Level_Exp_Text;

    [SerializeField, Tooltip("Love Level Text")]    //  인연 랭크 <color=#F883D3><size=28> 4</size></color>
    TMP_Text Love_Level_Text;
    [SerializeField, Tooltip("Love Exp Slider")]
    Slider Love_Exp_Slider;
    [SerializeField, Tooltip("Love Exp Text")]
    TMP_Text Love_Exp_Text;

    [SerializeField, Tooltip("SD Position")]
    RectTransform SD_Position;

    UIHeroBase SD_Hero;
    UserHeroData User_Data;

    public void SetUserHeroData(UserHeroData ud)
    {
        User_Data = ud;
        FixedUpdateInfo();
    }

    void FixedUpdateInfo()
    {
        SpawnSDHero();
    }
    void SpawnSDHero()
    {
        var obj = GameObjectPoolManager.Instance.GetGameObject(User_Data.GetPlayerCharacterData().sd_prefab_path, SD_Position);
        obj.transform.localPosition = Vector2.zero;
        SD_Hero = obj.GetComponent<UIHeroBase>();
        SD_Hero.PlayAnimation(HERO_PLAY_ANIMATION_TYPE.WIN_01);
    }




    public override void Despawned()
    {
        if (SD_Hero != null)
        {
            GameObjectPoolManager.Instance.UnusedGameObject(SD_Hero.gameObject);
        }
    }
}
