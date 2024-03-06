using FluffyDuck.UI;
using FluffyDuck.Util;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZeroOne.Input;

public class EssenceController : SceneControllerBase
{
    [SerializeField]
    RawImage Chara_Image = null;

    [SerializeField]
    GameObject Effect_Node = null;

    [SerializeField]
    ParticleSystem Success_Effect = null;

    [SerializeField]
    SerifuBox Serifu_Box = null;

    List<UserL2dData> L2d_List = null;
    Producer pd = null;
    RenderTexture Chara_Texture = null;

    BattlePcData Battle_Pc_Data = null;
    LOVE_LEVEL_TYPE Selected_Relationship;

    protected override void Initialize()
    {
        List<string> asset_list = new List<string>();
        asset_list.Add("Assets/AssetResources/Prefabs/Popup/Popup/Common/LevelUpAniPopup");
        GameObjectPoolManager.Instance.PreloadGameObjectPrefabsAsync(asset_list, PreloadCallback);

        Screen.orientation = ScreenOrientation.Portrait;

        var audio = AudioManager.Instance;

        List<string> audio_clip_list = new List<string>();
        audio_clip_list.Add("Assets/AssetResources/Audio/FX/click_01");
        audio_clip_list.Add("Assets/AssetResources/Audio/FX/success");
        audio.PreloadAudioClipsAsync(audio_clip_list, null);

        L2d_List = GameData.I.GetUserL2DDataManager().GetUserL2dDataListByChoice();

        if (L2d_List.Count == 0)
        {
            string msg = "정상적으로 데이터가 설정되지 않았습니다.\nPersistent 데이터를 지워주세요";
            PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Noti/NotiTimerPopup", POPUP_TYPE.NOTI_TYPE, (popup) =>
            {
                popup.ShowPopup(3f, msg);
            });

            SCManager.Instance.SetCurrent(this);
            return;
        }

        /*
        TouchCanvas.Instance.EnableTouchEffect(false);

        ScreenEffectManager.I.SetShaderEffect("Shaders/Glow");
        ScreenEffectManager.I.Shader_Material.SetColor("_GlowColor", Color.red);
        ScreenEffectManager.I.Shader_Material.SetFloat("_GlowIntensity", 1.5f);
        ScreenEffectManager.I.Shader_Material.SetFloat("_Threshold", 0.85f);

        ScreenEffectManager.I.ShaderEffectActive(false);
        */
    }

    void PreloadCallback(int load_cnt, int total_cnt)
    {
        if (load_cnt == total_cnt)
        {
            SCManager.I.SetCurrent(this, "OnReceiveData");
            return;
        }
    }

    bool OnReceiveData(BattlePcData data, int index)
    {
        Battle_Pc_Data = data;
        Selected_Relationship = (LOVE_LEVEL_TYPE)(index + 1);// 인덱스 + 1 = 실제 호감도

        pd = Factory.Instantiate<Producer>(Battle_Pc_Data.Data.essence_id, Selected_Relationship, SPINE_CHARA_LOCATION_TYPE.TRANSFER_ESSENCE);
        pd.OnSuccessTransferEssence += OnSuccessTransfer;
        pd.OnCompleteTransferEssence += OnCompleteTransferEssence;
        pd.OnSendActorMessage += Serifu_Box.OnReceiveSpineMessage;

        GestureManager.Instance.Enable = true;

        return true;
    }

    public void OnSuccessTransfer(TOUCH_BODY_TYPE type)
    {
        //ScreenEffectManager.I.ShaderEffectActive(true);
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/success");
        //Effect_Node.SetActive(true);
        /*
        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/Popup/Common/LevelUpAniPopup", POPUP_TYPE.DIALOG_TYPE, (popup) =>
        {
            popup.ShowPopup();
        });
        */

        Success_Effect.Play(true);
        Battle_Pc_Data.User_Data.SetDataSendedEssence(type);
        GameData.Instance.GetUserHeroDataManager().Save();
    }

    public void OnCompleteTransferEssence()
    {
        //ScreenEffectManager.I.ShaderEffectActive(false);
    }

    public override void OnClick(UIButtonBase button)
    {
        base.OnClick(button);

        switch (button.name)
        {
            case "HomeBtn":
                SCManager.I.ChangeScene(SceneName.home);
                break;
            case "BackBtn":
                SCManager.I.ChangeScene(SceneName.home, Battle_Pc_Data);
                break;
        }

        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.orientation = ScreenOrientation.AutoRotation;

        TouchCanvas.Instance.EnableTouchEffect(true);
    }
}
