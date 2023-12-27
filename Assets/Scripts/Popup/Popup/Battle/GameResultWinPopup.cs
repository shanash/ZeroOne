using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResultWinPopup : PopupBase
{
    [Header("Result Popup Vars")]
    [Space()]
    [SerializeField, Tooltip("Backlight Ease Alpha")]
    UIEaseCanvasGroupAlpha Backlight_Ease;

    [SerializeField, Tooltip("Win Text Ease Scale")]
    UIEaseScale WinText_Ease;

    [SerializeField, Tooltip("Result Info Container")]
    UIEaseCanvasGroupAlpha Result_Info_Container;

    [Space()]
    [SerializeField, Tooltip("Stars Container Ease Slide")]
    UIEaseSlide Stars_Container_Ease_Slide;

    [SerializeField, Tooltip("Star Container Ease Alpha")]
    UIEaseCanvasGroupAlpha Star_Container_Ease_Alpha;

    [SerializeField, Tooltip("Complete Scale Stars")]
    List<UIEaseScale> Complete_Mission_Star_Ease_Scale_List;

    [SerializeField, Tooltip("Complete Alpha Stars")]
    List<UIEaseCanvasGroupAlpha> Complete_Mission_Star_Ease_Alpha_List;

    [Space()]
    [SerializeField, Tooltip("Player Info Ease Slide")]
    UIEaseSlide Player_Info_Ease_Slide;
    [SerializeField, Tooltip("Player Info Ease Alpha")]
    UIEaseCanvasGroupAlpha Player_Info_Ease_Alpha;
    

    [Space()]
    [SerializeField, Tooltip("Character Info Ease Alpha")]
    UIEaseCanvasGroupAlpha Character_Info_Ease_Alpha;
    [SerializeField, Tooltip("Character Info Ease Slide")]
    UIEaseSlide Character_Info_Ease_Slide;

    [SerializeField, Tooltip("Character Info Container")]
    RectTransform Character_Info_Container;

    [Space()]
    [SerializeField, Tooltip("Next Btn Ease Alpha")]
    UIEaseCanvasGroupAlpha Next_Btn_Ease_Alpha;
    [SerializeField, Tooltip("Next Btn Ease Slide")]
    UIEaseSlide Next_Btn_Ease_Slide;

    [Space()]
    [SerializeField, Tooltip("Reward Info Container")]
    RectTransform Reward_Info_Container;

    [Space()]
    [SerializeField, Tooltip("Reward Box Ease Alpha")]
    UIEaseCanvasGroupAlpha Reward_Box_Ease_Alpha;
    [SerializeField, Tooltip("Reward Box Ease Scale")]
    UIEaseScale Reward_Box_Ease_Scale;

    [Space()]
    [SerializeField, Tooltip("Reward Btn Container Ease Slide")]
    UIEaseSlide Reward_Btn_Container_Ease_Slide;


    List<GameResultPlayerCharacterInfo> Used_Player_Character_Info_List = new List<GameResultPlayerCharacterInfo>();


    public override void ShowPopup(params object[] data)
    {
        base.ShowPopup(data);
        SetEnableEscKeyExit(false);
        InitAssets();
        
    }

    void InitAssets()
    {
        List<string> asset_list = new List<string>();
        asset_list.Add("Assets/AssetResources/Prefabs/Popup/Popup/Battle/GameResultPlayerCharacterInfo");

        var deck_mng = GameData.Instance.GetUserHeroDeckMountDataManager();
        var deck = deck_mng.FindSelectedDeck(GAME_TYPE.STORY_MODE);
        var hero_list = deck.GetDeckHeroes();

        for (int i = 0; i < hero_list.Count; i++)
        {
            var hero = hero_list[i];
            if (!asset_list.Contains(hero.GetUserHeroData().GetPlayerCharacterData().sd_prefab_path))
            {
                asset_list.Add(hero.GetUserHeroData().GetPlayerCharacterData().sd_prefab_path);
            }
        }

        GameObjectPoolManager.Instance.PreloadGameObjectPrefabsAsync(asset_list, PreloadCallback);
    }

    void PreloadCallback(int load_cnt, int total_cnt)
    {
        if (load_cnt == total_cnt)
        {
            FixedUpdatePopup();
            return;
        }
    }

    protected override void FixedUpdatePopup()
    {
        //  player info


        //  character info
        
        AddPlayerInfoNodes();

        //  backlight 밝아지기
        Backlight_Ease.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
        //  승리 텍스트 등장
        WinText_Ease.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN, WinTextEaseComplete);
    }

    /// <summary>
    /// 캐릭터 정보 추가<br/>
    /// 덱에 포함되어 있는 캐릭터들의 정보를 보여준다
    /// </summary>
    void AddPlayerInfoNodes()
    {
        var pool = GameObjectPoolManager.Instance;
        var deck_mng = GameData.Instance.GetUserHeroDeckMountDataManager();
        var deck = deck_mng.FindSelectedDeck(GAME_TYPE.STORY_MODE);
        var hero_list = deck.GetDeckHeroes();
        int cnt = hero_list.Count;
        for (int i = 0; i < cnt; i++)
        {
            var hero_data = hero_list[i];
            var obj = pool.GetGameObject("Assets/AssetResources/Prefabs/Popup/Popup/Battle/GameResultPlayerCharacterInfo", Character_Info_Container);
            var character_info = obj.GetComponent<GameResultPlayerCharacterInfo>();
            character_info.SetUserHeroData(hero_data.GetUserHeroData());
            Used_Player_Character_Info_List.Add(character_info);
        }
    }

    void ClearCharacterInfos()
    {
        var pool = GameObjectPoolManager.Instance;
        int cnt = Used_Player_Character_Info_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            pool.UnusedGameObject(Used_Player_Character_Info_List[i].gameObject);
        }
        Used_Player_Character_Info_List.Clear();
    }

    /// <summary>
    /// Win 텍스트 등장 완료 후, 별등급/플레이어 정보 박스 등장
    /// </summary>
    void WinTextEaseComplete()
    {
        Stars_Container_Ease_Slide.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN, StarContainerEaseComplete);
        Star_Container_Ease_Alpha.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
    }

    void StarContainerEaseComplete()
    {
        StartCoroutine(ShowStars(2));
    }

    void ShowStarComplete()
    {
        Player_Info_Ease_Slide.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
        Player_Info_Ease_Alpha.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
        Character_Info_Ease_Alpha.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN, CharacterInfoEaseComplete);
        Character_Info_Ease_Slide.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
    }

    void CharacterInfoEaseComplete()
    {
        Next_Btn_Ease_Slide.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
        Next_Btn_Ease_Alpha.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
    }

    IEnumerator ShowStars(int cnt)
    {
        if (cnt > 3)
        {
            cnt = 3;
        }
        for (int i = 0; i < cnt; i++)
        {
            var scale = Complete_Mission_Star_Ease_Scale_List[i];
            scale.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
            var alpha = Complete_Mission_Star_Ease_Alpha_List[i];
            alpha.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
            yield return new WaitForSeconds(0.2f);
        }
        ShowStarComplete();
    }


    public void OnClickGameResultOK()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        //  todo go home / next stage
        //HidePopup();

        Result_Info_Container.StartMove(UIEaseBase.MOVE_TYPE.MOVE_OUT, ResultInfoContainerCallback);
    }


    public void OnClickGameRewardNext()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        HidePopup();
    }
    public void OnClickGameRewardHome()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        HidePopup();

    }

    void ResultInfoContainerCallback()
    {
        Reward_Info_Container.gameObject.SetActive(true);
        Reward_Box_Ease_Alpha.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN, RewardBoxEaseComplete);
        Reward_Box_Ease_Scale.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
    }

    void RewardBoxEaseComplete()
    {
        Reward_Btn_Container_Ease_Slide.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
    }

    public override void Despawned()
    {
        //  backlight alpha 초기화
        Backlight_Ease.ResetEase(0f);
        //  wintextease init
        WinText_Ease.ResetEase(new Vector2(0.5f, 0.5f));

        //  star ease init

        Vector2 star_pos = Stars_Container_Ease_Slide.transform.localPosition;
        star_pos.y = 174;
        Stars_Container_Ease_Slide.ResetEase(star_pos);

        Star_Container_Ease_Alpha.ResetEase(0f);

        int cnt = Complete_Mission_Star_Ease_Scale_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            var scale = Complete_Mission_Star_Ease_Scale_List[i];
            scale.ResetEase(new Vector2(5f, 5f));

            var alpha = Complete_Mission_Star_Ease_Alpha_List[i];
            alpha.ResetEase(0f);
        }

        //  player info init
        Vector2 player_info_pos = Player_Info_Ease_Slide.transform.localPosition;
        player_info_pos.y = 81;
        Player_Info_Ease_Slide.ResetEase(player_info_pos);

        Player_Info_Ease_Alpha.ResetEase(0f);

        //  character info init
        Character_Info_Ease_Alpha.ResetEase(0f);

        Vector2 char_pos = Character_Info_Ease_Slide.transform.localPosition;
        char_pos.y = -384;
        Character_Info_Ease_Slide.ResetEase(char_pos);

        //  next btn
        Next_Btn_Ease_Alpha.ResetEase(0f);

        Vector2 next_pos = Next_Btn_Ease_Slide.transform.localPosition;
        next_pos.y = -80;
        Next_Btn_Ease_Slide.ResetEase(next_pos);

        //  result info
        Result_Info_Container.ResetEase(1f);


        Reward_Info_Container.gameObject.SetActive(false);

        Reward_Box_Ease_Alpha.ResetEase(0f);
        Reward_Box_Ease_Scale.ResetEase(Vector2.zero);

        Vector2 reward_btn_pos = Reward_Btn_Container_Ease_Slide.transform.localPosition;
        reward_btn_pos.y = -160;
        Reward_Btn_Container_Ease_Slide.ResetEase(reward_btn_pos);

        ClearCharacterInfos();
    }



}
