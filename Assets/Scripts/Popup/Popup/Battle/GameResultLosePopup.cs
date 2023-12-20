using FluffyDuck.UI;
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

    [Space()]
    [SerializeField, Tooltip("Home Btn Ease Alpha")]
    UIEaseCanvasGroupAlpha Home_Btn_Ease_Alpha;
    [SerializeField, Tooltip("Home Btn Ease Slide")]
    UIEaseSlide Home_Btn_Ease_Slide;

    public override void ShowPopup(params object[] data)
    {
        base.ShowPopup(data);
        FixedUpdatePopup();
    }

    protected override void FixedUpdatePopup()
    {
        Lose_Text_Ease.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN, LoseTextEaseComplete);
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
    }
}
