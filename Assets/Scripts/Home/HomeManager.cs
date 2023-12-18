using FluffyDuck.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
    private void Start()
    {
        var audio = AudioManager.Instance;

        List<string> audio_clip_list = new List<string>();
        audio_clip_list.Add("Assets/AssetResources/Audio/FX/click_01");

        audio.PreloadAudioClipsAsync(audio_clip_list, null);

        //double a = 12.020893;
        //Debug.LogFormat("{0:0.##}", a);

        
    }

    public void OnClickBattleStart()
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
        if (!MasterDataManager.Instance.IsLoaded)
        {
            return;
        }

        PopupManager.Instance.Add("Assets/AssetResources/Prefabs/Popup/UI/Deck/PCDeckSettingUI", (popup) =>
        {
            popup.ShowPopup();
        });
    }

    public void OnClickMemorialStart()
    {
        SceneManager.LoadScene("memorial");
    }


}
