using FluffyDuck.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// 브금 및 사운드 이펙트 관리
/// </summary>
public class AudioManager : MonoBehaviourSingleton<AudioManager>
{
    const int MAX_FX_COUNT = 100; // 최대로 동시 재생 가능한 FX 갯수
    const int VOICE_SAMPLE_COUNT = 256; // 목소리 크기를 측정하기 위한 샘플 갯수
    public const float VOICE_TERM_SECONDS = 0.07f;

    public enum Type
    {
        BGM,
        FX,
        Voice,
    }

    public enum BGMPlayOption
    {
        None = 0,
        CrossFade,
        FadeOutAndFadeIn,
        Delay,
    }

    /// <summary>
    /// 오디오 플레이시 상태 정보
    /// </summary>
    public enum AUDIO_STATES
    {
        NONE = 0,
        START,
        PLAYING,
        END,
    }

    #region Properties
    protected override bool _Is_DontDestroyOnLoad { get { return false; } }

    public bool IsInit
    {
        get { return _BGM_Src != null; }
    }

    /// <summary>
    /// 브금 OnOff
    /// </summary>
    public bool EnableBGM
    {
        set
        {
            _BGM_Enable = value;
            _BGM_Src.enabled = value;
            _Fadeout_Bgm_Src.enabled = value;
            //GameConfig.Instance.SetGameConfig<float>(GAME_CONFIG_KEY.SOUND_BGM_VOLUME, _BGM_Volume);
        }
        get
        {
            return _BGM_Enable;
        }
    }

    /// <summary>
    /// 사운드이펙트 OnOff
    /// </summary>
    public bool EnableFX
    {
        set
        {
            _FX_Enable = value;
            int cnt = _FX_Srcs.Count;
            for (int i = 0; i < cnt; ++i)
            {
                _FX_Srcs[i].enabled = _FX_Enable;
            }
            //GameConfig.Instance.SetGameConfig<float>(GAME_CONFIG_KEY.SOUND_FX_VOLUME, _FX_Volume);
        }

        get
        {
            return _FX_Enable;
        }
    }


    /// <summary>
    /// 보이스 오디오 소스는 비활성 처리하면 절대 안됨. 항상 플레이 가능하고 볼륨은 최대 상태여야 함
    /// 보이스 오디오의 volume & mute 조정은 audio mixer에서 해야 함
    /// </summary>
    public bool EnableVoice
    {
        set
        {
            _Voice_Enable = value;
            //_Voice_Src.enabled = value;
            if (Audio_MIxer != null)
            {
                if (_Voice_Enable)
                {
                    Audio_MIxer.SetFloat($"{VOICE_GROUP_NAME}_{VOLUME_END}", GetCalcVoiceVolume(VoiceVolume)); //  현재 볼륨으로 처리
                }
                else
                {
                    Audio_MIxer.SetFloat($"{VOICE_GROUP_NAME}_{VOLUME_END}", -80); //  무음처리
                }
            }
        }
        get
        {
            return _Voice_Enable;
        }
    }

    /// <summary>
    /// 브금 볼륨 조절
    /// </summary>
    public float BGMVolume
    {
        set
        {
            if (!EnableBGM) return;
            _BGM_Volume = value;
            if (_BGM_Src != null)
            {
                _BGM_Src.volume = value;
            }
            GameConfig.Instance.SetGameConfig<float>(GAME_CONFIG_KEY.SOUND_BGM_VOLUME, _BGM_Volume);
        }
        get
        {
            return _BGM_Volume;
        }
    }

    /// <summary>
    /// 사운드이펙트 볼륨 조절
    /// </summary>
    public float FXVolume
    {
        set
        {
            if (!EnableFX) return;
            _FX_Volume = value;
            foreach (AudioSource asrc in _FX_Srcs)
            {
                asrc.volume = _FX_Volume;
            }
            GameConfig.Instance.SetGameConfig<float>(GAME_CONFIG_KEY.SOUND_FX_VOLUME, _FX_Volume);
        }
        get
        {
            return _FX_Volume;
        }
    }
    /// <summary>
    /// 보이스 볼륨 조절
    /// 보이스의 볼륨은 오디오 소스의 볼륨이 아닌 오디오 믹서의 볼륨을 조절해야 함
    /// </summary>
    public float VoiceVolume
    {
        set
        {
            _Voice_Volume = value;
            if (Audio_MIxer != null)
            {
                Audio_MIxer.SetFloat($"{VOICE_GROUP_NAME}_{VOLUME_END}", GetCalcVoiceVolume(value));
            }
            GameConfig.Instance.SetGameConfig<float>(GAME_CONFIG_KEY.SOUND_VOICE_VOLUME, _Voice_Volume);
        }

        get
        {
            return _Voice_Volume;
        }
    }

    public float FXTimeStretch
    {
        get => _FXTimeStretch;
        set
        {
            _FXTimeStretch = value;
            foreach (var src in _FX_Srcs)
            {
                src.pitch = _FXTimeStretch;
                Audio_MIxer.SetFloat($"{FX_GROUP_NAME}_{PITCH_END}", 1f / _FXTimeStretch);
            }
        }
    }
    float _FXTimeStretch = 1;
    #endregion

    #region Variables

    readonly string BGM_GROUP_NAME = "BGM";
    readonly string FX_GROUP_NAME = "FX";
    readonly string VOICE_GROUP_NAME = "VOICE";

    readonly string VOLUME_END = "Volume";
    readonly string PITCH_END = "Pitch";

    bool _BGM_Enable = true;
    bool _FX_Enable = true;
    bool _Voice_Enable = true;

    float _BGM_Volume = 1f;
    float _FX_Volume = 1f;
    float _Voice_Volume = 1f;

    BGMPlayOption _Option = BGMPlayOption.None;
    float _Option_Time = 1.0f; // 브금 옵션에 걸리는 시간
    Coroutine _Option_Progress = null;

    Dictionary<string, AudioClipController> _Audio_Clips = new Dictionary<string, AudioClipController>();
    #endregion

    #region Hierarchy Variables
    AudioSource _BGM_Src = null;
    AudioSource _Fadeout_Bgm_Src = null;
    List<AudioSource> _FX_Srcs = new List<AudioSource>();
    //  voice variables
    AudioSource _Voice_Src = null;
    Coroutine _Voice_Play_Coroutine = null;
    float[] _Voice_Sample_Data = new float[VOICE_SAMPLE_COUNT];
    AudioMixer Audio_MIxer = null;
    Dictionary<string, AudioMixerGroup> AudioMixerGroup = new Dictionary<string, AudioMixerGroup>();
    #endregion

    #region Methods
    /// <summary>
    /// 음소거 및 음량 설정 로드
    /// </summary>
    void LoadConfig()
    {
        GameConfig I = GameConfig.Instance;

        float bgmVolume = I.GetGameConfigValue<float>(GAME_CONFIG_KEY.SOUND_BGM_VOLUME, 1.0f);
        float fxVolume = I.GetGameConfigValue<float>(GAME_CONFIG_KEY.SOUND_FX_VOLUME, 1.0f);
        float voiceVolume = I.GetGameConfigValue<float>(GAME_CONFIG_KEY.SOUND_VOICE_VOLUME, 1.0f);

        BGMVolume = bgmVolume;
        FXVolume = fxVolume;
        VoiceVolume = voiceVolume;
        // 차후 사용하게 된다면 사용합시다...
        //_BGM_Enable = I.GetGameConfigValue<bool>(GAME_CONFIG_KEY.SOUDN_BGM_ENABLE, true);
        //_FX_Enable = I.GetGameConfigValue<bool>(GAME_CONFIG_KEY.SOUDN_FX_ENABLE, true);
    }

    /// <summary>
    /// 크로스페이드옵션 진행
    /// </summary>
    /// <param name="fadein">점점 커질 오디오소스</param>
    /// <param name="fadeout">점점 사라질 오디오소스</param>
    /// <returns></returns>
    IEnumerator CoPlayCrossFade(AudioSource fadein, AudioSource fadeout)
    {
        fadeout.volume = _BGM_Volume;
        bool fadeinCheck = (null != fadein);

        if (fadeinCheck)
        {
            fadein.volume = 0.0f;
        }

        float elapsedTime = 0.0f;

        while (_Option_Time > elapsedTime)
        {
            yield return null;
            elapsedTime += Time.deltaTime;

            if (fadeinCheck)
            {
                fadein.volume = (elapsedTime / _Option_Time) * _BGM_Volume;
                if (fadein.volume > _BGM_Volume)
                    fadein.volume = _BGM_Volume;
            }

            fadeout.volume = _BGM_Volume - (elapsedTime / _Option_Time) * _BGM_Volume;

            if (fadeout.volume < 0)
                fadeout.volume = 0;
        }

        if (fadeinCheck)
        {
            fadein.volume = _BGM_Volume;
        }
        fadeout.Stop();

        _Option_Progress = null;
    }

    IEnumerator CoPlayFadeOutAndFadeIn(AudioSource fadein, AudioSource fadeout)
    {
        // TODO: 나중에 구성
        yield return null;
    }

    /// <summary>
    /// 풀에 없는 오디오 클립을 로드
    /// </summary>
    /// <param name="key">클립 경로</param>
    /// <returns>쓰레드id</returns>
    async Task<int> PreloadAudioAsync(string key)
    {
        if (GetAudioClipController(key) == null)
        {
            var contoller = await AudioClipController.Create(key);

            if (contoller == null)
            {
                Debug.Assert(contoller != null, $"Failed Load AudioClip from LoadAssetAsync :: path : {key}");
                return Thread.CurrentThread.ManagedThreadId;
            }

            _Audio_Clips.Add(key, contoller);
        }

        return Thread.CurrentThread.ManagedThreadId;
    }

    /// <summary>
    /// 오디오 클립 가져오기
    /// </summary>
    /// <param name="key">클립 경로</param>
    /// <returns></returns>
    AudioClipController GetAudioClipController(string key)
    {
        if (_Audio_Clips.ContainsKey(key))
        {
            return _Audio_Clips[key];
        }

        return null;
    }

    /// <summary>
    /// 오디오 소스 재생
    /// </summary>
    /// <param name="src">오디오소스</param>
    /// <param name="clip">오디오 클립</param>
    /// <param name="loop">재생이후 반복</param>
    void PlayAudioSource(AudioSource src, AudioClip clip, bool loop)
    {
        SetAudioSource(src, clip, loop);
        src.Play();
    }

    /// <summary>
    /// 재생 전 오디오소스 세팅
    /// </summary>
    /// <param name="src">오디오소스</param>
    /// <param name="clip">오디오 클립</param>
    /// <param name="loop">재생이후 반복</param>
    void SetAudioSource(AudioSource src, AudioClip clip, bool loop)
    {
        src.clip = clip;
        src.loop = loop;
    }

    /// <summary>
    /// 오디오소스 만들기
    /// </summary>
    /// <param name="volume">초기화 볼륨</param>
    /// <returns>만들어진 오디오소스</returns>
    AudioSource CreateAudioSource(float volume = 1.0f, string audio_mixer_group_name = default)
    {
        AudioSource result = gameObject.AddComponent<AudioSource>();
        result.playOnAwake = false;
        result.volume = volume;

        if (!string.IsNullOrEmpty(audio_mixer_group_name) && AudioMixerGroup.ContainsKey(audio_mixer_group_name))
        {
            result.outputAudioMixerGroup = AudioMixerGroup[audio_mixer_group_name];
        }

        return result;
    }
    /// <summary>
    /// 보이스용 오디오 소스 만들기
    /// Mixer를 사용해야 하기 때문에 추가 옵션이 필요함
    /// </summary>
    /// <param name="volume"></param>
    /// <returns></returns>
    AudioSource CreateVoiceSource(float volume = 1f)
    {
        var result = CreateAudioSource(1, VOICE_GROUP_NAME);
        Audio_MIxer.SetFloat(VOICE_GROUP_NAME, GetCalcVoiceVolume(volume));
        return result;
    }

    /// <summary>
    /// FX 오디오소스 가져오기
    /// </summary>
    /// <returns>오디오소스</returns>
    AudioSource GetFXAudioSource()
    {
        AudioSource src = null;
        int cnt = _FX_Srcs.Count;
        for (int i = 0; i < cnt; ++i)
        {
            if (!_FX_Srcs[i].isPlaying)
            {
                src = _FX_Srcs[i];
                break;
            }
        }

        // 찾아봤는데 남는 오디오소스가 없으면 만든다
        if (src == null)
        {
            Debug.Assert(MAX_FX_COUNT > _FX_Srcs.Count, $"FX MAX Count(={MAX_FX_COUNT}) Over!!");

            src = CreateAudioSource(_FX_Volume, FX_GROUP_NAME);
            _FX_Srcs.Add(src);
        }

        return src;
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// 브금 플레이
    /// </summary>
    /// <param name="key">재생할 브금 경로</param>
    /// <param name="loop">반복재생유무</param>
    /// <param name="callback">재생 끝났을때 호출받을 콜백함수</param>
    public void PlayBGM(string key, bool loop = true, Action<string> callback = null)
    {
        if (!IsInit)
        {
            Debug.LogWarning("Plz Initialize Before Call PlayBGM");
            OnInit();
        }

        var clip_Control = GetAudioClipController(key);

        if (clip_Control == null)
        {
            Debug.Assert(clip_Control != null, "미리 로딩하지 않고 불러오려 하면 첫번째 호출에서는 재생되지 않습니다.");
            _ = PreloadAudioAsync(key);

            return;
        }

        if (clip_Control == null) return;
        if (_BGM_Src.clip == clip_Control.Clip) return;

        switch (_Option)
        {
            case BGMPlayOption.None:
                if (_BGM_Src.isPlaying) StopBGM();

                PlayAudioSource(_BGM_Src, clip_Control.Clip, loop);
                break;
            case BGMPlayOption.CrossFade:
                if (_Option_Progress != null)
                {
                    _Fadeout_Bgm_Src.Stop();
                    StopCoroutine(_Option_Progress);
                    _Option_Progress = null;
                }

                if (_BGM_Src.isPlaying)
                {
                    _Fadeout_Bgm_Src.Stop();
                    AudioSource temp = _Fadeout_Bgm_Src;
                    _Fadeout_Bgm_Src = _BGM_Src;
                    _BGM_Src = temp;
                }

                if (_BGM_Src != null)
                {
                    PlayAudioSource(_BGM_Src, clip_Control.Clip, loop);
                }

                _Option_Progress = StartCoroutine(CoPlayCrossFade(_BGM_Src, _Fadeout_Bgm_Src));
                break;
            case BGMPlayOption.FadeOutAndFadeIn:

                SetAudioSource(_BGM_Src, clip_Control.Clip, loop);
                _Option_Progress = StartCoroutine(CoPlayFadeOutAndFadeIn(_BGM_Src, _Fadeout_Bgm_Src));
                break;
        }

        if (callback != null)
        {
            StartCoroutine(WaitForCallback(_BGM_Src, clip_Control, callback));
        }
    }

    /// <summary>
    /// 브금 재생 일시중지
    /// </summary>
    public void PauseBGM()
    {
        if (!EnableBGM) return;

        if (_BGM_Src.isPlaying)
            _BGM_Src.Pause();
    }

    /// <summary>
    /// 브금 일시중지된 곳부터 다시 재생
    /// </summary>
    public void ResumeBGM()
    {
        if (!EnableBGM) return;

        if (!_BGM_Src.isPlaying && _BGM_Src.clip != null)
            _BGM_Src.Play();
    }

    /// <summary>
    /// 브금 재생 중지
    /// </summary>
    public void StopBGM()
    {
        if (!EnableBGM) return;

        if (!_BGM_Src.isPlaying) return;

        if (_Option == BGMPlayOption.CrossFade)
        {
            if (_Option_Progress != null)
            {
                _Fadeout_Bgm_Src.Stop();
                StopCoroutine(_Option_Progress);
                _Option_Progress = null;
            }

            AudioSource temp = _Fadeout_Bgm_Src;
            _Fadeout_Bgm_Src = _BGM_Src;
            _BGM_Src = temp;

            _Option_Progress = StartCoroutine(CoPlayCrossFade(null, _Fadeout_Bgm_Src));
        }

        _BGM_Src.Stop();
        _BGM_Src.clip = null;
    }

    /// <summary>
    /// 브금 옵션 세팅
    /// </summary>
    /// <param name="value">브금옵션</param>
    /// <param name="seconds">옵션이 적용될 초</param>
    public void SetBGMOption(BGMPlayOption value, float seconds = 0)
    {
        _Option = value;
        if (seconds < 0)
            seconds = 0;
        _Option_Time = seconds;
    }

    /// <summary>
    /// 사운드이펙트 재생
    /// </summary>
    /// <param name="key">재생 음원 경로</param>
    /// <param name="loop">반복재생 유무</param>
    /// <param name="callback">호출받을 콜백함수</param>
    /// <returns></returns>
    public bool PlayFX(string key, bool loop = false, Action<string> callback = null)
    {
        if (!IsInit)
        {
            Debug.LogWarning("Plz Initialize Before Call PlayBGM");
            OnInit();
        }

        if (!EnableFX) return false;

        var clip_Control = GetAudioClipController(key);
        if (clip_Control == null)
        {
            Debug.Assert(clip_Control != null, "미리 로딩하지 않고 불러오려 하면 첫번째 호출에서는 재생되지 않습니다.");
            _ = PreloadAudioAsync(key);

            return false;
        }

        AudioSource fxPlayer = GetFXAudioSource();
        PlayAudioSource(fxPlayer, clip_Control.Clip, loop);

        if (callback != null)
        {
            StartCoroutine(WaitForCallback(fxPlayer, clip_Control, callback));
        }

        return true;
    }

    /// <summary>
    /// 콜백 호출하기 위한 함수
    /// </summary>
    /// <param name="src">오디오소스</param>
    /// <param name="callback">호출할 콜백</param>
    /// <returns></returns>
    IEnumerator WaitForCallback(AudioSource src, AudioClipController controller, Action<string> callback)
    {
        yield return new WaitUntil(() => !src.isPlaying);

        callback?.Invoke($"{controller.Key}");
    }

    /// <summary>
    /// 사운드 이펙트 재생 중지
    /// </summary>
    /// <param name="key">정지할 path 경로</param>
    public void StopFX(string key)
    {
        if (!EnableFX) return;

        Debug.AssertFormat(_Audio_Clips.ContainsKey(key), "Error : Inserted Wrong FX path. please Confirm FX path {0}", key);

        var founds = _FX_Srcs.FindAll(src => src.clip == _Audio_Clips[key].Clip);
        int cnt = founds.Count;
        for (int i = 0; i < cnt; ++i)
        {
            founds[i].Stop();
        }
    }

    /// <summary>
    /// 모든 사운드 이펙트 재생 중지
    /// </summary>
    public void StopAllFX()
    {
        if (!EnableFX) return;

        int cnt = _FX_Srcs.Count;
        for (int i = 0; i < cnt; ++i)
        {
            _FX_Srcs[i].Stop();
        }
    }

    /// <summary>
    /// 사운드 이펙트 재생 확인
    /// </summary>
    /// <param name="key">재생할 FX의 키</param>
    public bool IsPlayingFX(string key)
    {
        Debug.AssertFormat(_Audio_Clips.ContainsKey(key), "Error : Inserted Wrong FX path. please Confirm FX path {0}", key);

        var src = _FX_Srcs.Find(src => src.clip == _Audio_Clips[key].Clip);
        return src != null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public void PlayVoice(string key, bool is_loop, Action<string, AUDIO_STATES, float> callback = null)
    {
        if (!IsInit)
        {
            Debug.LogWarning("Plz Initialize Before Call PlayBGM");
            OnInit();
        }
        //  보이스를 플레이 중이라면 그냥 무시
        // TODO: 였는데 일단 강제로 멈추고 플레이시킴
        // 입모양 콜백쪽에 문제가 있을수 있으니 차후 확인
        if (_Voice_Play_Coroutine != null)
        {
            StopCoroutine( _Voice_Play_Coroutine );
            _Voice_Play_Coroutine = null;
            StopVoice();
        }

        var clip_control = GetAudioClipController(key);

        if (clip_control == null)
        {
            Debug.Assert(clip_control != null, "미리 로딩하지 않고 불러오려 하면 첫번째 호출에서는 재생되지 않습니다.");
            _ = PreloadAudioAsync(key);

            return;
        }

        if (clip_control == null) return;

        //  이전에 플레이하던 보이스를 강제로 중지시켜 버리면 콜백으로 종료 결과를 넘겨줄 수가 없음. 그냥 중첩되지 않도록 해야할 듯.
        //if (_Voice_Src.isPlaying)
        //{
        //    if (_Voice_Src.clip == clip_Control.Clip) return;
        //    StopVoice();
        //}

        //PlayAudioSource(_Voice_Src, clip_Control.Clip, false);

        if (callback != null)
        {
            _Voice_Play_Coroutine = StartCoroutine(WaitForVoiceCallback(_Voice_Src, clip_control, is_loop, callback));
        }
    }

    /// <summary>
    /// 보이스를 플레이해주는 코루틴
    /// 보이스 플레이의 상태값 반환 및 해당 시점의 볼륨 레이드 정보를 콜백으로 주기적으로 반환해준다.
    /// </summary>
    /// <param name="voice_src"></param>
    /// <param name="clip_ctrl"></param>
    /// <param name="is_loop"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    IEnumerator WaitForVoiceCallback(AudioSource voice_src, AudioClipController clip_ctrl, bool is_loop, Action<string, AUDIO_STATES, float> callback)
    {
        PlayAudioSource(voice_src, clip_ctrl.Clip, is_loop);
        yield return null;
        callback?.Invoke(clip_ctrl.Key, AUDIO_STATES.START, 0f);
        var wait = new WaitForSeconds(VOICE_TERM_SECONDS);
        float rms = 0f;

        while (IsPlayingVoice())
        {
            rms = GetAudioVolumeRMS(voice_src);
            callback?.Invoke(clip_ctrl.Key, AUDIO_STATES.PLAYING, rms);
            yield return null;
        }
        _Voice_Play_Coroutine = null;
        callback?.Invoke(clip_ctrl.Key, AUDIO_STATES.END, 0f);
        StopVoice();
    }

    /// <summary>
    /// 현재 플레이 중인 오디오의 볼륨 정보를 받아온다.
    /// 볼륨정보란 오디오 볼륨이 아닌, 현재 재생되고 있는 사운드의 강약 정도로 판단하면 될것 같음.
    /// 재생중 소리가 없는 부분에서는 0에 수렴하는 값이 반환되고, 큰 소리로 소리치는 경우 1에 수렴하는 값이 반환되는 구조
    /// </summary>
    /// <param name="channel"></param>
    /// <returns></returns>
    float GetAudioVolumeRMS(AudioSource src)
    {
        int channels = src.clip.channels;
        float volume = 0f;

        for (int i = 0; i < channels; i++)
        {
            src.GetOutputData(_Voice_Sample_Data, channels);
            float sum = 0f;
            int len = _Voice_Sample_Data.Length;
            for (int j = 0; j < len; j++)
            {
                float s = _Voice_Sample_Data[j];
                sum += s * s;
            }

            volume += Mathf.Sqrt(sum / VOICE_SAMPLE_COUNT);
        }

        return volume;
    }

    /// <summary>
    /// 실제 보이스의 볼륨 데이터 저장은 0.0001 ~ 1까지의 값을 저장한다. (0은 사용하면 안됨. 최소값은 0.0001이어야 함)
    /// 그럼 실제 mixer group에서 사용하는 db의 볼륨 값으로 변환해주는 함수
    /// </summary>
    /// <param name="vol"></param>
    /// <returns></returns>
    float GetCalcVoiceVolume(float vol)
    {
        float volume = Mathf.Clamp01(vol);
        if (volume < 0.0001f)
            volume = 0.0001f;
        return Mathf.Log10(volume) * 20f;
    }

    public bool IsPlayingVoice()
    {
        return _Voice_Src.isPlaying;
    }

    /// <summary>
    /// 브금 재생 중지
    /// </summary>
    public void StopVoice()
    {
        if (!EnableVoice) return;

        if (!_Voice_Src.isPlaying) return;

        _Voice_Src.Stop();
        _Voice_Src.clip = null;
    }

    public float GetClipLength(string clip_key)
    {
        var clip_control = GetAudioClipController(clip_key);
        return clip_control.Clip.length;
    }

    /// <summary>
    /// 재생할 음원들 미리 로드
    /// </summary>
    /// <param name="audio_Pathes">음원 경로 리스트</param>
    /// <param name="callback">음원 로딩 완료시 받을 콜백</param>
    public async void PreloadAudioClipsAsync(List<string> audio_Pathes, Action<int, int> callback)
    {
        if (!IsInit)
        {
            Debug.LogWarning("Plz Initialize Before Call AsyncPreloadAudioList");
            OnInit();
        }

        var all_tasks = new List<Task<int>>();

        if (audio_Pathes == null)
        {
            return;
        }

        int total_cnt = audio_Pathes.Count;
        for (int i = 0; i < total_cnt; i++)
        {
            all_tasks.Add(PreloadAudioAsync(audio_Pathes[i]));
        }

        callback?.Invoke(total_cnt - all_tasks.Count, total_cnt);

        while (all_tasks.Count > 0)
        {
            var finish_task = await Task.WhenAny(all_tasks);
            if (all_tasks.Contains(finish_task))
            {
                all_tasks.Remove(finish_task);
                callback?.Invoke(total_cnt - all_tasks.Count, total_cnt);
            }
        }
    }

    /// <summary>
    /// 특정 AudioClip 리소스를 해제
    /// </summary>
    /// <param name="key">해제할 AudioClip의 key</param>
    public void UnloadAudioClip(string key)
    {
        var controller = GetAudioClipController(key);
        if (controller != null)
        {
            controller.Dispose();
            _Audio_Clips.Remove(key);
        }
    }

    /// <summary>
    /// 특정 AudioClip 리소스들을 해제
    /// </summary>
    /// <param name="audio_Pathes">해제할 AudioClip의 경로 리스트</param>
    public void UnloadAudioClip(List<string> audio_Pathes)
    {
        if (!IsInit)
        {
            Debug.LogError("Plz Initialize Before Call AsyncUnloadAudioClips");
            OnInit();
            return;
        }

        Debug.Assert(audio_Pathes != null, "");

        int total_cnt = audio_Pathes.Count;
        for (int i = 0; i < total_cnt; i++)
        {
            UnloadAudioClip(audio_Pathes[i]);
        }
    }

    /// <summary>
    /// 가지고 있는 모든 AudioClip 리소스들을 해제해줍니다.
    /// </summary>
    public void UnloadAllAudioClips()
    {
        if (!IsInit)
        {
            Debug.LogError("Plz Initialize Before Call AsyncUnloadAudioClips");
            OnInit();
            return;
        }

        foreach (AudioClipController controller in _Audio_Clips.Values)
        {
            controller.Dispose();
        }

        _Audio_Clips.Clear();
    }

    /// <summary>
    /// 초기화는 실행시 반드시 한번은 해야 합니다
    /// </summary>
    public void OnInit()
    {
        LoadConfig();
        Audio_MIxer = Resources.Load<AudioMixer>("AudioMixer/PRMixer");
        var mixers = Audio_MIxer.FindMatchingGroups(FX_GROUP_NAME);
        if (mixers.Length > 0)
        {
            AudioMixerGroup.Add(VOICE_GROUP_NAME, mixers[0]);
        }

        _BGM_Src = CreateAudioSource(_BGM_Volume, BGM_GROUP_NAME);
        _Fadeout_Bgm_Src = CreateAudioSource(0, BGM_GROUP_NAME);
        _Voice_Src = CreateVoiceSource(VoiceVolume);
    }

    protected override void OnAwake()
    {
        base.OnAwake();
        OnInit();
    }

    #endregion
}
