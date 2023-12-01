using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AudioTest : MonoBehaviour
{
    private static readonly string DEFAULT_PATH = "Assets/AssetResources/Audio/";
    public int PlayFXNum = 1;
    private int FX_ID = 0;
    private string Prepare_BGM_Path = "";
    private string Prepare_FX_Path = "";

    private void Awake()
    {
        // 오디오 리소스 준비-임시입니다..ㅠ
        List<string> list = new List<string>();
        for (int i = 0; i < 10; i++)
        {
            list.Add($"{DEFAULT_PATH}BGM/{(i + 1).ToString("D2")}");
        }

        for (int i = 0; i < 50; i++)
        {
            list.Add($"Assets/AssetResources/Audio/FX/DM-CGS-{(i + 1).ToString("D2")}");
        }

        list.Add($"{DEFAULT_PATH}Voice/CutePout");
        list.Add($"{DEFAULT_PATH}Voice/PleaseFindThis");
        AudioManager.Instance.PreloadAudioClipsAsync(list, PreloadCallback);

        OnValueChangeSelectBGM(0);
        OnValueChangeSelectFX(0);
    }

    private void PreloadCallback(int load_cnt, int total_cnt)
    {
        Debug.LogFormat("Load Count [{0}], Total Count [{1}]", load_cnt, total_cnt);
        if (load_cnt == total_cnt)
        {
            Debug.Log("<color=#ff0000>Load Complete!!</color>");
        }
    }

    public void OnValueChangeSelectBGM(int dropdown_Value)
    {
        Prepare_BGM_Path = $"Assets/AssetResources/Audio/BGM/{(dropdown_Value + 1).ToString("D2")}";
    }

    public void PlayBGM()
    {
        if (!string.IsNullOrEmpty(Prepare_BGM_Path))
        {
            AudioManager.Instance.PlayFX(Prepare_BGM_Path);
        }
    }

    public void PlayVoice()
    {
        AudioManager.Instance.PlayVoice($"{DEFAULT_PATH}Voice/CutePout", false, (name, state, vol) => {
            Debug.Log($"name : {name}");
        });
    }

    public void OnValueChangeSelectFX(int dropdown_Value)
    {
        FX_ID = dropdown_Value;
        Prepare_FX_Path = $"Assets/AssetResources/Audio/FX/DM-CGS-{(dropdown_Value + 1).ToString("D2")}";
    }

    public void PlayFX()
    {
        if (!string.IsNullOrEmpty(Prepare_FX_Path))
        {
            for (int i = 0; i < PlayFXNum; i++)
            {
                AudioManager.Instance.PlayFX(Prepare_FX_Path);
            }
        }
        FX_ID++;
        if (FX_ID >= 50)
        {
            FX_ID = 0;
        }
        Prepare_FX_Path = $"Assets/AssetResources/Audio/FX/DM-CGS-{(FX_ID + 1).ToString("D2")}";
    }

    public void OnValueChangeBGMVolume(float volume)
    {
        Debug.Log($"OnValueChangeBGMVolume :: {volume}");
        AudioManager.Instance.BGMVolume = volume;
    }

    public void OnValueChangeFXVolume(float volume)
    {
        Debug.Log($"OnValueChangeFXVolume :: {volume}");
        AudioManager.Instance.FXVolume = volume;
    }

    public void ToggleBGM()
    {
        AudioManager.Instance.EnableBGM = !AudioManager.Instance.EnableBGM;
    }

    public void ToggleFX()
    {
        AudioManager.Instance.EnableFX = !AudioManager.Instance.EnableFX;
    }

    public void AllStop()
    {
        AudioManager.Instance.StopAllFX();
    }   
}
