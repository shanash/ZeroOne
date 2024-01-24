using Cysharp.Text;
using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    [SerializeField, Tooltip("Timer Box")]
    RectTransform Timer_Box;
    [SerializeField, Tooltip("Timer")]
    TMP_Text Timer_Text;

    [SerializeField, Tooltip("Current Wave")]
    TMP_Text Current_Wave;
    [SerializeField, Tooltip("Wave Max")]
    TMP_Text Wave_Max_Text;

    [Header("Buttons")]
    [SerializeField, Tooltip("Menu Btn")]
    UIButtonBase Menu_Btn;
    [SerializeField, Tooltip("Auto Btn")]
    UIButtonBase Auto_Btn;
    
    [SerializeField, Tooltip("Fast Btn")]
    UIButtonBase Fast_Btn;
    [SerializeField, Tooltip("Fast Speed")]
    TMP_Text Fast_Speed;

    /// <summary>
    /// 체력 게이지 
    /// </summary>
    List<LifeBarNode> Used_Life_Bar_List = new List<LifeBarNode>();

    private void Start()
    {
        UpdateFastSpeed();
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
                Battle_Mng.ChangeState(GAME_STATES.PAUSE);
            }

        }
    }

    void UpdateFastSpeed()
    {
        BATTLE_SPEED_TYPE speed_type = (BATTLE_SPEED_TYPE)GameConfig.Instance.GetGameConfigValue<int>(GAME_CONFIG_KEY.BATTLE_SPEED_TYPE, 0);
        Fast_Speed.gameObject.SetActive(speed_type != BATTLE_SPEED_TYPE.NORMAL_TYPE);
        if (speed_type == BATTLE_SPEED_TYPE.FAST_SPEED_1_5)
        {
            Fast_Speed.text = "x1.5";
        }
        else if (speed_type == BATTLE_SPEED_TYPE.FAST_SPEED_2)
        {
            Fast_Speed.text = "x2";
        }
    }

    /// <summary>
    /// 체력 게이지 추가
    /// </summary>
    /// <param name="t"></param>
    /// <param name="ttype"></param>
    /// <returns></returns>
    public LifeBarNode AddLifeBarNode(Transform t, TEAM_TYPE ttype)
    {
        var pool = GameObjectPoolManager.Instance;
        var obj = pool.GetGameObject("Assets/AssetResources/Prefabs/UI/LifeBarNode", HP_Bar_Container);
        var life = obj.GetComponent<LifeBarNode>();
        life.SetBarColor(ttype);
        life.SetTargetTransform(t);


        Used_Life_Bar_List.Add(life);
        return life;
    }
    /// <summary>
    /// 체력 게이지 제거
    /// </summary>
    /// <param name="bar"></param>
    public void RemoveLifeBarNode(LifeBarNode bar)
    {
        Used_Life_Bar_List.Remove(bar);
        GameObjectPoolManager.Instance.UnusedGameObject(bar.gameObject);
    }

    public RectTransform GetDamageContainer()
    {
        return Damage_Container;
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

    #region OnClick Funcs
    public void OnClickPause()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        Battle_Mng?.ChangeState(GAME_STATES.PAUSE);
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
        if (speed_type > (int)BATTLE_SPEED_TYPE.FAST_SPEED_2)
        {
            speed_type = (int)BATTLE_SPEED_TYPE.NORMAL_TYPE;
        }
        GameConfig.Instance.SetGameConfig<int>(GAME_CONFIG_KEY.BATTLE_SPEED_TYPE, speed_type);
        UpdateFastSpeed();
        float speed = GameDefine.GAME_SPEED_DEFAULT;
        if (speed_type == (int)BATTLE_SPEED_TYPE.FAST_SPEED_1_5)
        {
            speed = GameDefine.GAME_SPEED_1_5;
        }
        else if (speed_type == (int)BATTLE_SPEED_TYPE.FAST_SPEED_2)
        {
            speed = GameDefine.GAME_SPEED_2;
        }
        Battle_Mng.SetBattleFastSpeed(speed);
    }
    #endregion
}
