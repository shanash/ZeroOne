using Cysharp.Text;
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

    int Before_Lv;
    float Before_Exp_Percent;
    double Before_Remain_Need_Exp;

    int After_Lv;
    float After_Exp_Percent;
    double After_Remain_Need_Remain;

    public void SetUserHeroData(UserHeroData ud)
    {
        User_Data = ud;

        Before_Lv = User_Data.GetLevel();
        Before_Exp_Percent = User_Data.GetExpPercetage();
        Before_Remain_Need_Exp = User_Data.GetRemainNextExp();

        SpawnSDHero();

        FixedUpdateInfo();
    }

    /// <summary>
    /// 경험치 추가 후 게이지 업데이트
    /// </summary>
    public void AfterAddExpHeroInfo(int char_xp, int destiny_xp)
    {

    }

    void FixedUpdateInfo()
    {
        //  before lv info
        Level_Text.text = Before_Lv.ToString();
        //  exp per
        Level_Exp_Slider.value = Before_Exp_Percent;
        //  need exp
        Level_Exp_Text.text = ZString.Format("앞으로 {0:N0}", Before_Remain_Need_Exp);
    }

    /// <summary>
    /// UI용 SD 캐릭터 불러오기
    /// </summary>
    void SpawnSDHero()
    {
        var obj = GameObjectPoolManager.Instance.GetGameObject(User_Data.GetPlayerCharacterData().sd_prefab_path, SD_Position);
        obj.transform.localPosition = Vector2.zero;
        SD_Hero = obj.GetComponent<UIHeroBase>();
        MainThreadDispatcher.Instance.AddAction(() =>
        {
            SD_Hero.PlayAnimation(HERO_PLAY_ANIMATION_TYPE.WIN_01);
        });
    }


    public override void Despawned()
    {
        if (SD_Hero != null)
        {
            GameObjectPoolManager.Instance.UnusedGameObject(SD_Hero.gameObject);
        }
        SD_Hero = null;
    }
}
