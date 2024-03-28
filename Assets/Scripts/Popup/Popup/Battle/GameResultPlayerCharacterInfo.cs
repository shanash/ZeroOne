using Cysharp.Text;
using FluffyDuck.Util;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameResultPlayerCharacterInfo : UIBase
{
    [SerializeField, Tooltip("Box")]
    RectTransform Box;

    [SerializeField, Tooltip("Level Up Animator")]
    Animation Lv_Up_Animator;

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

        SpawnSDHero();
        
        FixedUpdateInfo();
    }


    /// <summary>
    /// 경험치 추가 후 게이지 업데이트
    /// </summary>
    public void AfterAddExpHeroInfo(int char_exp, int destiny_exp)
    {
        StartCoroutine(StartLevelUp(char_exp));

        StartCoroutine(StartLoveLevelUp(destiny_exp));
    }

    IEnumerator StartLevelUp(int char_exp)
    {
        int before_lv = User_Data.GetLevel();

        var result_code = User_Data.AddExp(char_exp);
        if (!(result_code == RESPONSE_TYPE.SUCCESS || result_code == RESPONSE_TYPE.LEVEL_UP_SUCCESS))
        {
            yield break;
        }

        int after_lv = User_Data.GetLevel();
        float after_xp_percent = User_Data.GetExpPercetage();
        double after_remain_need_xp = User_Data.GetRemainNextExp();

        int fullcharge_count = after_lv - before_lv;
        if (fullcharge_count > 1)
        {
            fullcharge_count = 1;
        }
        float duration = 1f;
        float delta = 0f;
        var wait = new WaitForSeconds(0.01f);
        int loop_count = 0;
        //  풀 차지 횟수
        while (loop_count < fullcharge_count)
        {
            delta += Time.deltaTime * 10f;

            Level_Exp_Slider.value = Mathf.Lerp(Level_Exp_Slider.value, 1f, delta / duration);
            if (delta > duration)
            {
                delta = 0f;
                Level_Exp_Slider.value = 0f;
                ++loop_count;
                //Level_Text.text = (before_lv + loop_count).ToString();
                Level_Text.text = after_lv.ToString();
                if (loop_count >= fullcharge_count)
                {
                    Lv_Up_Animator?.Play();
                    break;
                }
            }
            else
            {
                yield return wait;
            }
        }
        //  need exp
        Level_Exp_Text.text = ZString.Format("앞으로 {0:N0}", after_remain_need_xp);


        //  남은 경험치 게이지 이동
        duration = 1f;
        delta = 0f;
        if (after_xp_percent > 0f)
        {
            while (delta < duration)
            {
                delta += Time.deltaTime * 5f;
                Level_Exp_Slider.value = Mathf.Lerp(Level_Exp_Slider.value, after_xp_percent, delta / duration);
                yield return wait;
            }
        }
    }
    IEnumerator StartLoveLevelUp(int destiny_exp)
    {
        int before_love_lv = User_Data.GetLoveLevel();

        var result_code = User_Data.AddLoveExp(destiny_exp);
        if (!(result_code == RESPONSE_TYPE.SUCCESS || result_code == RESPONSE_TYPE.LEVEL_UP_SUCCESS))
        {
            yield break;
        }

        int after_love_lv = User_Data.GetLoveLevel();
        float after_love_exp_per = User_Data.GetLoveExpPercetage();
        double after_remain_need_love_xp = User_Data.GetRemainNextLoveExp();

        int fullcharge_count = after_love_lv - before_love_lv;
        if (fullcharge_count > 1)
        {
            fullcharge_count = 1;
        }
        float duration = 1f;
        float delta = 0f;
        var wait = new WaitForSeconds(0.01f);
        int loop_count = 0;
        //  풀 챠치 횟수
        while (loop_count < fullcharge_count)
        {
            delta += Time.deltaTime * 10f;
            Love_Exp_Slider.value = Mathf.Lerp(Love_Exp_Slider.value, 1f, delta / duration);
            if (delta >= duration)
            {
                delta = 0f;
                Love_Exp_Slider.value = 0f;
                ++loop_count;
                //Love_Level_Text.text = (before_love_lv + loop_count).ToString();
                Love_Level_Text.text = after_love_lv.ToString();
                if (loop_count >= fullcharge_count)
                {
                    break;
                }
            }
            else
            {
                yield return wait;
            }
        }

        //  before love need exp
        Love_Exp_Text.text = ZString.Format("앞으로 {0:N0}", after_remain_need_love_xp);
        //  남은 경험치 게이지 이동
        duration = 1f;
        delta = 0f;
        if (after_love_exp_per > 0f)
        {
            while (delta < duration)
            {
                delta += Time.deltaTime * 5f;
                Love_Exp_Slider.value = Mathf.Lerp(Love_Exp_Slider.value, after_love_exp_per, delta / duration);
                yield return wait;
            }
        }
    }

    void FixedUpdateInfo()
    {
        int before_lv = User_Data.GetLevel();
        float before_xp_percent = User_Data.GetExpPercetage();
        double before_remain_need_xp = User_Data.GetRemainNextExp();

        int before_love_lv = User_Data.GetLoveLevel();
        float before_love_exp_per = User_Data.GetLoveExpPercetage();
        double before_remain_need_love_xp = User_Data.GetRemainNextLoveExp();

        //  before lv info
        Level_Text.text = before_lv.ToString();
        //  exp per
        Level_Exp_Slider.value = before_xp_percent;
        //  need exp
        Level_Exp_Text.text = ZString.Format("앞으로 {0:N0}", before_remain_need_xp);

        //  before love lv 
        Love_Level_Text.text = before_love_lv.ToString();
        //  before love exp
        Love_Exp_Slider.value = before_love_exp_per;
        //  before love need exp
        Love_Exp_Text.text = ZString.Format("앞으로 {0:N0}", before_remain_need_love_xp);

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
