using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResultLosePopup : PopupBase
{
    [Header("Result Popup Vars")]
    [Space()]
    [SerializeField, Tooltip("Lose Text Ease Scale")]
    UIEaseScale Lose_Text_Ease;

    [Space()]
    [SerializeField, Tooltip("Player Info Ease Alpha")]
    UIEaseCanvasGroupAlpha Player_Info_Ease_Alpha;
    [SerializeField, Tooltip("Player Info Ease Slide")]
    UIEaseSlide Player_Info_Ease_Slide;

    [Space()]
    [SerializeField, Tooltip("Character Info Ease Alpha")]
    UIEaseCanvasGroupAlpha Character_Info_Ease_Alpha;
    [SerializeField, Tooltip("Character Info Ease Slide")]
    UIEaseSlide Character_Info_Ease_Slide;

    [SerializeField, Tooltip("Character Info Container")]
    RectTransform Character_Info_Container;


    [Space()]
    [SerializeField, Tooltip("Home Btn Ease Alpha")]
    UIEaseCanvasGroupAlpha Home_Btn_Ease_Alpha;
    [SerializeField, Tooltip("Home Btn Ease Slide")]
    UIEaseSlide Home_Btn_Ease_Slide;

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
        AddCharacterInfoNodes();

        Lose_Text_Ease.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN, LoseTextEaseComplete);
    }

    /// <summary>
    /// 캐릭터 정보 추가하기
    /// </summary>
    void AddCharacterInfoNodes()
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
    /// <summary>
    /// 추가된 캐릭터 정보 정리
    /// </summary>
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


    void LoseTextEaseComplete()
    {
        Player_Info_Ease_Alpha.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN, PlayerInfoEaseComplete);
        Player_Info_Ease_Slide.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);

        Character_Info_Ease_Alpha.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
        Character_Info_Ease_Slide.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
    }

    void PlayerInfoEaseComplete()
    {
        Home_Btn_Ease_Alpha.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
        Home_Btn_Ease_Slide.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
    }

    public void OnClickHome()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        HidePopup();

    }

    public override void Despawned()
    {
        //  lose text reset
        Lose_Text_Ease.ResetEase(new Vector2(0.5f, 0.5f));

        //  player info reset
        Player_Info_Ease_Alpha.ResetEase(0f);
        Vector2 player_info_pos = Player_Info_Ease_Slide.transform.localPosition;
        player_info_pos.y = 81;
        Player_Info_Ease_Slide.ResetEase(player_info_pos);

        //  character info reset
        Character_Info_Ease_Alpha.ResetEase(0f);
        Vector2 character_info_pos = Character_Info_Ease_Slide.transform.localPosition;
        character_info_pos.y = -384;
        Character_Info_Ease_Slide.ResetEase(character_info_pos);

        //  home btn
        Home_Btn_Ease_Alpha.ResetEase(0f);
        Vector2 home_btn_pos = Home_Btn_Ease_Slide.transform.localPosition;
        home_btn_pos.y = -160;
        Home_Btn_Ease_Slide.ResetEase(home_btn_pos);

        ClearCharacterInfos();
    }
}
