using System.Threading.Tasks;
using UnityEngine;

public class UITest : MonoBehaviour
{
    int atk_balloon_id = 0;
    int idle_balloon_id = 0;
    HeroTest _idle_hero = null;
    HeroTest _atk_hero = null;

    void Update()
    {
        if (atk_balloon_id == 0)
        {
            HeroTest[] heroes = FindObjectsOfType<HeroTest>();
            if (heroes == null) return;

            foreach (var hero in heroes)
            {
                if (hero.GetCurrentState() == UNIT_STATES.ATTACK_1 && _atk_hero != hero)
                {
                    /*
                    _ = SpeechBalloonManager.Instance.CreateBalloonAsync(
                        "공격하는 중이야",
                        hero.transform.position,
                        new Vector2(100, 100),
                        SpeechBalloon.BalloonSizeType.Flexible,
                        ((hero.Team_Type == TEAM_TYPE.LEFT) ? SpeechBalloon.Pivot.Left : SpeechBalloon.Pivot.Right) | SpeechBalloon.Pivot.Bottom,
                        null,
                        20,
                        3);
                    _atk_hero = hero;
                    */
                    break;
                }
            }
        }
    }

    public void OnClickDeleteAllButton()
    {
        SpeechBalloonManager.Instance.DeleteAllBalloon();
    }

    public void OnClickAppearButton()
    {
        AppearText();
    }

    void AppearText()
    {
        if (idle_balloon_id == 0)
        {
            HeroTest[] heroes = FindObjectsOfType<HeroTest>();
            if (heroes == null) return;

            foreach (var hero in heroes)
            {
                if (hero.GetCurrentState() == UNIT_STATES.IDLE)
                {
                    /*
                    idle_balloon_id = await SpeechBalloonManager.Instance.CreateBalloonAsync(
                        "아무것도 안하는중이야",
                        hero.transform.position,
                        new Vector2(100, 100),
                        SpeechBalloon.BalloonSizeType.Flexible,
                        ((hero.Team_Type == TEAM_TYPE.LEFT) ? SpeechBalloon.Pivot.Left : SpeechBalloon.Pivot.Right) | SpeechBalloon.Pivot.Bottom,
                        null,
                        20);
                    */
                    _idle_hero = hero;
                    break;
                }
            }
        }
        else
        {
            if (_idle_hero.GetCurrentState() == UNIT_STATES.IDLE)
            {
                SpeechBalloonManager.Instance.SetTextBalloon(idle_balloon_id, "계속 아무것도 안하는중이야!");
            }
            else
            {
                SpeechBalloonManager.Instance.DisappearBalloon(idle_balloon_id);
                idle_balloon_id = 0;
                _idle_hero = null;
            }
        }
    }
}
