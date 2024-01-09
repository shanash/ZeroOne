using Spine;
using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoiceSampleTester : MonoBehaviour
{

    [SerializeField, Tooltip("Sample Spine")]
    SkeletonAnimation Sample_Anim;

    [SerializeField, Tooltip("Volume")]
    Slider Volume_Slider;

    [SerializeField, Tooltip("Audio Clip Paths")]
    List<string> Voice_Clip_Paths;


    readonly int LAUGH_TRACK = 16;


    private void Start()
    {
        PlayAnimation(LAUGH_TRACK, "16_mouth_laugh", false);
        var track = FindTrack(LAUGH_TRACK);
        if (track != null)
        {
            track.Alpha = 0f;
        }

        InitVoiceAudio();
    }

    void InitVoiceAudio()
    {
        AudioManager.Instance.PreloadAudioClipsAsync(Voice_Clip_Paths, (load_cnt, total_cnt) =>
        {
            Debug.LogFormat("Lood Audio [{0}/{1}]", load_cnt, total_cnt);
            if (load_cnt == total_cnt)
            {
                Debug.Log("<color=#ffff00>Voice Load Complete!!</color>");
            }
        });
    }

    void PlayAnimation(int track, string anim_name, bool is_loop)
    {
        Sample_Anim.AnimationState.SetAnimation(track, anim_name, is_loop);
    }

    public void OnSliderChangeValue()
    {
        //float volume = Mathf.Log10(Volume_Slider.value) * 20f;
        //Debug.Log($"Slider [{Volume_Slider.value}] [{volume}]");
        //Voice_Mixer.SetFloat("VOICE", volume);
        AudioManager.Instance.VoiceVolume = Volume_Slider.value;
    }


    public void OnClickVoiceTest(int vidx)
    {
        string clip_key = Voice_Clip_Paths[vidx];
        AudioManager.Instance.PlayVoice(clip_key, false, (name, state, volume) =>
        {
            //Debug.LogFormat("Clip Name [{0}] => [{1}]", name, state);
            var track = FindTrack(LAUGH_TRACK);
            if (track != null)
            {
                switch (state)
                {
                    case AudioManager.AUDIO_STATES.START:
                        track.Alpha = 0f;
                        break;
                    case AudioManager.AUDIO_STATES.PLAYING:
                        track.Alpha = volume;
                        break;
                    case AudioManager.AUDIO_STATES.END:
                        track.Alpha = 0f;
                        break;
                }

            }
        });
    }

    //IEnumerator StartAudioPlay(int vidx)
    //{
    //    var clip = Voice_Clips[vidx];
    //    Audio.clip = clip;
    //    Audio.Play();
    //    yield return null;
    //    var wait = new WaitForSeconds(0.07f);

    //    while (Audio.isPlaying)
    //    {
    //        var track = FindTrack(LAUGH_TRACK);
    //        if (track != null)
    //        {
    //            float volume = GetVolume(0);
    //            float alpha = volume * 20f;
    //            track.Alpha = Mathf.Clamp01(alpha);

    //            yield return wait;
    //        }
    //    }
    //    Audio_Coroutine = null;
    //}




    TrackEntry FindTrack(int track)
    {
        if (Sample_Anim != null)
        {
            return Sample_Anim.AnimationState.GetCurrent(track);
        }
        return null;
    }

    //float GetVolume(int channel)
    //{
    //    int qsamples = 1024;
    //    Audio.GetOutputData(sample_data, channel);

    //    float sum = 0;
    //    for (int i = 0; i < sample_data.Length; i++)
    //    {
    //        float s = sample_data[i];
    //        sum += s * s;
    //    }

    //    return (float)Math.Round(Mathf.Sqrt(sum / qsamples), 2);
    //}
}
