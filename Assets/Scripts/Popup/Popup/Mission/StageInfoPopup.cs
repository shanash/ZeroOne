using Cysharp.Text;
using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageInfoPopup : PopupBase
{
    [Space()]
    [SerializeField, Tooltip("Stage Number")]
    TMP_Text Stage_Number;

    [SerializeField, Tooltip("Stage Name")]
    TMP_Text Stage_Name;

    [SerializeField, Tooltip("Star Points")]
    List<Image> Stage_Star_Points;

    [SerializeField, Tooltip("Action Point Icon")]
    Image Action_Point_Icon;

    [SerializeField, Tooltip("Aciton Point Current")]
    TMP_Text Action_Point_Current;

    [SerializeField, Tooltip("Action Point Used")]
    TMP_Text Action_Point_Used;

    [SerializeField, Tooltip("Entrance Btn")]
    UIButtonBase Entrance_Btn;

    [SerializeField, Tooltip("Npc List View")]
    ScrollRect Npc_List_View;

    [SerializeField, Tooltip("First Reward List View")]
    ScrollRect First_Reward_List_View;

    [SerializeField, Tooltip("Repeat Reward List View")]
    ScrollRect Repeat_Reward_List_View;

    [SerializeField, Tooltip("Npc Card Prefab")]
    GameObject Npc_Card_Prefab;

    [SerializeField, Tooltip("Reward Item Card Prefab")]
    GameObject Reward_Item_Card_Prefab;


    Stage_Data Stage;
    UserStoryStageData User_Data;


    List<NpcCardBase> Used_Npc_List = new List<NpcCardBase>();
    List<RewardItemCard> Used_Reward_Item_List = new List<RewardItemCard>();

    public override void ShowPopup(params object[] data)
    {
        if (data.Length != 1)
        {
            HidePopup();
            return;
        }
        
        Stage = (Stage_Data)data[0];

        User_Data = GameData.Instance.GetUserStoryStageDataManager().FindUserStoryStageData(Stage.stage_id);

        base.ShowPopup(data);
    }



    protected override void ShowPopupAniEndCallback()
    {
        FixedUpdatePopup();
    }

    protected override void HidePopupAniEndCallback()
    {
        base.HidePopupAniEndCallback();
    }


    protected override void FixedUpdatePopup()
    {
        var m = MasterDataManager.Instance;
        var zone = m.Get_ZoneData(Stage.zone_id);

        Stage_Number.text = ZString.Format("{0}-{1}", zone.zone_ordering, Stage.stage_ordering);

        Stage_Name.text = Stage.stage_name;


        //  npc list


        //  first reward list
        List<First_Reward_Data> first_reward_data_list = new List<First_Reward_Data>();
        

        //  star reward list



        //  repeat reward list


        if (User_Data != null)
        {
            MarkStarPoint(User_Data.GetStarPoint());
        }
        else
        {
            MarkStarPoint(0);
        }
    }

    void MarkStarPoint(int pt)
    {
        int cnt = Stage_Star_Points.Count;
        for (int i = 0; i < cnt; i++)
        {
            Stage_Star_Points[i].gameObject.SetActive(false);
        }
        if (pt >= cnt)
        {
            pt = cnt;
        }
        for (int i = 0; i < pt; i++)
        {
            Stage_Star_Points[i].gameObject.SetActive(true);
        }
    }

    public void OnClickCancel()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        HidePopup();
    }

    public void OnClickEntrance()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
    }

    void ClearNpcList()
    {
        var pool = GameObjectPoolManager.Instance;
        int cnt = Used_Npc_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            pool.UnusedGameObject(Used_Npc_List[i].gameObject);
        }
        Used_Npc_List.Clear();
    }
    void ClearRewardItemList()
    {
        var pool = GameObjectPoolManager.Instance;
        int cnt = Used_Reward_Item_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            pool.UnusedGameObject(Used_Reward_Item_List[i].gameObject);
        }
        Used_Reward_Item_List.Clear();
    }

    public override void Spawned()
    {
        base.Spawned();

        if (Ease_Base != null)
        {
            Ease_Base.transform.localScale = new Vector2(0f, 0f);
        }

    }

    public override void Despawned()
    {
        Stage_Number.text = "";
        Stage_Name.text = "";
        MarkStarPoint(0);

        ClearNpcList();
        ClearRewardItemList();
    }
}
