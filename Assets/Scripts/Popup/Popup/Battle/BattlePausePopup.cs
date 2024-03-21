using Cysharp.Text;
using FluffyDuck.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattlePausePopup : PopupBase
{

    [SerializeField, Tooltip("Popup Title")]
    TMP_Text Popup_Title;

    [SerializeField, Tooltip("Zone Number")]
    TMP_Text Zone_Number;

    [SerializeField, Tooltip("Zone Stage Name")]
    TMP_Text Zone_Stage_Name;

    [SerializeField, Tooltip("이어하기 버튼 텍스트")]
    TMP_Text Continue_Btn_Text;
    [SerializeField, Tooltip("포기하기 버튼 텍스트")]
    TMP_Text Exit_Btn_Text;

    BattleDungeonData Dungeon;

    protected override bool Initialize(object[] data)
    {
        if (data.Length != 1)
        {
            return false;
        }
        Dungeon = (BattleDungeonData)data[0];
        FixedUpdatePopup();
        return true;
    }
    protected override void FixedUpdatePopup()
    {
        var m = MasterDataManager.Instance;
        //  메뉴

        if (Dungeon.Game_Type == GAME_TYPE.STORY_MODE)
        {
            var story_data = (Stage_Data)Dungeon.GetDungeonData();
            var zone_data = m.Get_ZoneDataByStageGroupID(story_data.stage_group_id);
            Zone_Number.text = ZString.Format("{0}-{1}", zone_data.zone_ordering, story_data.stage_ordering);
            //  zone stage name
            Zone_Stage_Name.text = ZString.Format("{0} {1}", GameDefine.GetLocalizeString(zone_data.zone_name_id), GameDefine.GetLocalizeString(story_data.stage_name_id));
        }  
        else if(Dungeon.Game_Type == GAME_TYPE.BOSS_DUNGEON_MODE)
        {
            Zone_Number.text = string.Empty;

            //  
            Boss_Stage_Data stage_data = (Boss_Stage_Data)Dungeon.GetDungeonData();
            Boss_Data boss = m.Get_BossDataByBossStageGroupID(stage_data.boss_stage_group_id);
            Zone_Stage_Name.text = ZString.Format("보스 던전 {0}", GameDefine.GetLocalizeString(boss.boss_name));

        }

        //  이어하기 버튼 텍스트

        //  포기하기 버튼 텍스트

    }

    /// <summary>
    /// 이어 하기
    /// </summary>
    public void OnClickContinue()
    {
        if (Ease_Base != null && Ease_Base.IsPlaying())
        {
            return;
        }
        HidePopup();
    }

    /// <summary>
    /// 전투 종료. 홈으로 이동
    /// </summary>
    public void OnClickExit()
    {
        SCManager.Instance.ChangeScene(SceneName.home);
    }

    public override void Despawned()
    {
        base.Despawned();
        Dungeon = null;
    }
}
