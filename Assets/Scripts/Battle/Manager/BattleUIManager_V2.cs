using Cysharp.Text;
using FluffyDuck.UI;
using FluffyDuck.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager_V2 : MonoBehaviour
{
    [SerializeField, Tooltip("Battle Mng")]
    BattleManager_V2 Battle_Mng;

    [SerializeField, Tooltip("HP Bar Container")]
    RectTransform HP_Bar_Container;

    [SerializeField, Tooltip("Damage Container")]
    RectTransform Damage_Container;

    [SerializeField, Tooltip("Box")]
    RectTransform Box_Rect;

    [Header("UI")]
    [SerializeField, Tooltip("Timer Animator")]
    Animator Timer_Anim;
    [SerializeField, Tooltip("Timer")]
    TMP_Text Timer_Text;

    [SerializeField, Tooltip("Current Wave")]
    TMP_Text Current_Wave;
    [SerializeField, Tooltip("Wave Max")]
    TMP_Text Wave_Max_Text;

    [Header("Buttons")]
    [SerializeField, Tooltip("Menu Btn")]
    Button Menu_Btn;
    [SerializeField, Tooltip("Auto Btn")]
    Button Auto_Btn;
    
    [SerializeField, Tooltip("Fast Btn")]
    Button Fast_Btn;

    [SerializeField, Tooltip("Fast Arrow List")]
    List<RectTransform> Fast_Arrow_List;

    bool Is_Alarm;

    /// <summary>
    /// 체력 게이지 
    /// </summary>
    List<LifeBarNode> Used_Life_Bar_List = new List<LifeBarNode>();

    private void Start()
    {
        UpdateFastSpeed();
        Is_Alarm = false;
        Timer_Anim.SetTrigger("Start");
    }
    public LifeBarNode AddLifeBar(HeroBase_V2 hero)
    {
        var target = hero.GetHPPositionTransform();
        string prefab_name = "Assets/AssetResources/Prefabs/UI/LifeBar/LeftTeam_LifeBar_V2";
        if (hero.Team_Type == TEAM_TYPE.RIGHT)
        {
            prefab_name = "Assets/AssetResources/Prefabs/UI/LifeBar/RightTeam_LifeBar_V2";
        }
        if (string.IsNullOrEmpty(prefab_name))
        {
            return null;
        }
        var pool = GameObjectPoolManager.Instance;
        var obj = pool.GetGameObject(prefab_name, HP_Bar_Container);
        var life = obj.GetComponent<LifeBarNode>();
        life.SetHeroBaseV2(hero);
        Used_Life_Bar_List.Add(life);

        return life;
    }

    public void UnusedLifeBarNode(LifeBarNode node)
    {
        if (node == null)
        {
            return;
        }
        Used_Life_Bar_List.Remove(node);
        GameObjectPoolManager.Instance.UnusedGameObject(node.gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (PopupManager.Instance.IsShow())
            {
                return;
            }

            if (Battle_Mng.IsPause())
            {
                Battle_Mng.RevertState();
            }
            else
            {
                ChangePause();
            }

        }
    }

    public RectTransform GetDamageContainer()
    {
        return Damage_Container;
    }

    void UpdateFastSpeed()
    {
        BATTLE_SPEED_TYPE speed_type = (BATTLE_SPEED_TYPE)GameConfig.Instance.GetGameConfigValue<int>(GAME_CONFIG_KEY.BATTLE_SPEED_TYPE, 0);
        int cnt = Fast_Arrow_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Fast_Arrow_List[i].gameObject.SetActive(i == (int)speed_type);
        }
    }

    /// <summary>
    /// Wave Text update
    /// </summary>
    public void UpdateWaveCount()
    {
        int max_wave = Battle_Mng.GetMaxWave();
        int cur_wave = Battle_Mng.GetCurrentWave() + 1;

        Current_Wave.text = cur_wave.ToString();
        Wave_Max_Text.text = ZString.Format("/{0}", max_wave);
    }

    /// <summary>
    /// UI 전체 숨기기
    /// </summary>
    /// <param name="show"></param>
    public void ShowBattleUI(bool show)
    {
        Box_Rect.gameObject.SetActive(show);
    }

    /// <summary>
    /// 던전 제한시간 업데이트
    /// </summary>
    /// <param name="limit_time"></param>
    public void UpdateTimeLimit(float limit_time)
    {
        var time_span = TimeSpan.FromSeconds(limit_time);
        Timer_Text.text = ZString.Format("{0:D2} : {1:D2}", time_span.Minutes, time_span.Seconds);
        if (!Is_Alarm && time_span.TotalSeconds < 30)
        {
            Is_Alarm = true;
            Timer_Anim.SetTrigger("Alarm");
        }
    }

    void ChangePause()
    {
        var state = Battle_Mng.GetCurrentState();
        if (state >= GAME_STATES.PLAYING && state <= GAME_STATES.ULTIMATE_SKILL)
        {
            Battle_Mng?.ChangeState(GAME_STATES.PAUSE);
        }
    }

    #region OnClick Funcs
    public void OnClickPause()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        ChangePause();
    }
    public void OnClickAutoPlay()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
    }
    public void OnClickFastPlay()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        int speed_type = GameConfig.Instance.GetGameConfigValue<int>(GAME_CONFIG_KEY.BATTLE_SPEED_TYPE, 0);
        speed_type += 1;
        if (speed_type > GameDefine.GAME_SPEEDS.Count -1)
        {
            speed_type = (int)BATTLE_SPEED_TYPE.NORMAL_TYPE;
        }
        GameConfig.Instance.SetGameConfig<int>(GAME_CONFIG_KEY.BATTLE_SPEED_TYPE, speed_type);
        UpdateFastSpeed();
        float speed = GameDefine.GAME_SPEEDS[(BATTLE_SPEED_TYPE)speed_type];
        Battle_Mng.SetBattleFastSpeed(speed);
    }
    #endregion
}
