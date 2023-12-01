using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleUIManager : MonoBehaviour
{
    [SerializeField, Tooltip("Battle Manager")]
    BattleManager Battle_Mng;

    [SerializeField, Tooltip("HP Bar Container")]
    RectTransform HP_Bar_Container;

    /// <summary>
    /// 체력 게이지 
    /// </summary>
    List<LifeBarNode> Used_Life_Bar_List = new List<LifeBarNode>();
    /// <summary>
    /// 타겟 선택 화살표
    /// </summary>
    List<TargetArrowNode> Used_Target_Arrow_List = new List<TargetArrowNode>();


    float Test_Life_Value;

    private void Start()
    {
        Test_Life_Value = 1f;
        //_ = MasterDataManager.Instance;
        TouchCanvas.Instance.SetTouchEffectPrefabPath("VFX/Prefabs/Touch_Effect_Blue");
    }

    int Character_Index;
    public void OnClickChar01Focus()
    {
        var cine = Battle_Mng.GetVirtualCineManager();
        var list  = Battle_Mng.FindTeamManager(TEAM_TYPE.LEFT).GetMembers();
        
        var hero = list[Character_Index];
        cine.FocusCharacter(hero.gameObject);
        Character_Index++;
        if (Character_Index > list.Count - 1)
        {
            Character_Index = 0;
        }
    }

    public void OnClickFocusRelease()
    {
        Battle_Mng.GetVirtualCineManager().ReleaseCharacterCam();
    }
    

    public void OnClick60View()
    {
        Battle_Mng.GetVirtualCineManager().SetAngleView(60);
    }
    public void OnClick45View()
    {
        Battle_Mng.GetVirtualCineManager().SetAngleView(45);
    }
    public void OnClick30View()
    {
        Battle_Mng.GetVirtualCineManager().SetAngleView(30);
    }


    public void OnClick20View()
    {
        Battle_Mng.GetVirtualCineManager().SetAngleView(20);
    }



    public void OnClickGroup()
    {
        var cine = Battle_Mng.GetVirtualCineManager();
        var list = Battle_Mng.FindTeamManager(TEAM_TYPE.LEFT).GetMembers();

        for (int i = 0; i < list.Count; i++)
        {
            var h = list[i];
            cine.AddTarget(h.transform);
        }
        cine.SetGroupView();
    }

    public void OnClickGroupRelease()
    {
        Battle_Mng.GetVirtualCineManager().ReleaseGroupView();
    }

    public void OnClickOpenPopup()
    {
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/TestSlidePopup", (popup) =>
        {
            popup.ShowPopup();
        });
    }

    public void OnClickScaleOpenPopup()
    {
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/TestScalePopup", (popup) =>
        {
            popup.ShowPopup();
        });
    }
    

    public void OnClickGoMemorial()
    {

        SceneManager.LoadScene("memorial");

        //var m = MasterDataManager.Instance;
        //List<TestData> list = new List<TestData>();
        //m.Get_TestDataList(ref list);
        //int cnt = list.Count;
        //for (int i = 0; i < cnt; i++)
        //{
        //    var t = list[i];
        //    Debug.Log(t.ToString());
        //}
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
        life.SetTargetTransform(t);
        life.SetBarColor(ttype);

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
    /// <summary>
    /// 타겟 선택 화살표 추가
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public TargetArrowNode AddTargetArrowNode(Transform t)
    {
        var pool = GameObjectPoolManager.Instance;
        var obj = pool.GetGameObject("Assets/AssetResources/Prefabs/UI/TargetArrowNode", HP_Bar_Container);
        var arrow = obj.GetComponent<TargetArrowNode>();
        arrow.SetTargetTransform(t);
        return arrow;
    }
    /// <summary>
    /// 타겟 선택 화살표 제거
    /// </summary>
    /// <param name="arrow"></param>
    public void RemoveTargetArrowNode(TargetArrowNode arrow)
    {
        Used_Target_Arrow_List.Remove(arrow);
        GameObjectPoolManager.Instance.UnusedGameObject(arrow.gameObject);
    }

    
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.Minus))
        {
            Test_Life_Value -= 0.1f;
            if (Test_Life_Value < 0)
            {
                Test_Life_Value = 0;
            }
            UpdateLifeBars();
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.Plus))
        {
            Test_Life_Value += 0.1f;
            if (Test_Life_Value > 1)
            {
                Test_Life_Value = 1f;
            }
            UpdateLifeBars();
        }
        else if (Input.GetKeyUp(KeyCode.F) || Input.GetKeyUp(KeyCode.UpArrow))
        {
            FlushLifeBars();
        }
    }

    void UpdateLifeBars()
    {
        int cnt = Used_Life_Bar_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            var bar = Used_Life_Bar_List[i];
            bar.SetLifeAndShieldPercent(Test_Life_Value, 0.1f);
            
        }
    }

    void FlushLifeBars()
    {
        int cnt = Used_Life_Bar_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            Used_Life_Bar_List[i].FlushBackBar();
        }
    }


}
