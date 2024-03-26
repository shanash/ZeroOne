using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AudioSFXDelayAndPichController : MonoBehaviour
{
    private AudioSource haveAudioSource;

    [Tooltip("이 변수는 컴포넌트로 등록된 AudioClip이 없으면 사용되는 클립입니다.")]
    public AudioClip addingClip;

    public float delayTime;

    private void OnEnable()
    {
        //가지고 있는 오디오 소스 컴포넌트를 가져온다.
        haveAudioSource = GetComponent<AudioSource>();

        //안가지고 있으면 오디오 소스 컴포넌트를 생성합니다.
        if (haveAudioSource == null)
        {
            haveAudioSource = gameObject.AddComponent<AudioSource>();
        }

        AudioClip haveAudioClip;
        haveAudioClip = haveAudioSource.clip;

        //오디오 클립이 없다면 addingClip 변수에 담긴 오디오 클립을 넣어준다.
        if (haveAudioClip == null)
        {
            haveAudioSource.clip = addingClip;
        }

        if (delayTime > 0)
        {
            float finalDelayTime;

            //현재 배속 체크해서 딜레이 타임 조절
            BATTLE_SPEED_TYPE speed_type = (BATTLE_SPEED_TYPE)GameConfig.Instance.GetGameConfigValue<int>(GAME_CONFIG_KEY.BATTLE_SPEED_TYPE, 0);
            if (speed_type == BATTLE_SPEED_TYPE.FAST_SPEED_X2)
            {
                finalDelayTime = delayTime / 2;
                haveAudioSource.pitch = 0.5f;
            }
            else if (speed_type == BATTLE_SPEED_TYPE.FAST_SPEED_X3)
            {
                finalDelayTime = delayTime / 3;
                haveAudioSource.pitch = 0.33f;
            }
            else
            {
                finalDelayTime = delayTime;
            }
            Invoke("PlaySFXAudio", delayTime);
        }
        else
        {
            PlaySFXAudio();
        }
        
    }

    private void PlaySFXAudio()
    {
        haveAudioSource.Play();
    }
}
