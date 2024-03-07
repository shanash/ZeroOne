using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

/// <summary>
/// 타임라인이 ShakeCameraClip 구간에 들어와서 플레이될때 실제 가동되는 부분
/// </summary>
[Serializable]
public class ShakeCameraBehaviour : PlayableBehaviour
{
    const float MinTickRange = -10f;
    const float MaxTickRange = 10f;

    [SerializeField, Range(0, 100), Tooltip("카메라가 흔들리는 최대치")]
    float _Magnitude = 0.0f;

    [SerializeField, Range(0, 100), Tooltip("높을수록 격렬하게 흔들고, 낮을수록 부드럽게 흔듬")]
    float _Roughness = 0.0f;

    [SerializeField, Tooltip("세로로 흔듬")]
    bool _Vertical = true;

    [SerializeField, Tooltip("가로로 흔듬")]
    bool _Horizontal = true;

    VirtualCineManager _VirtualCineManager = null;
    bool _BecameActiveThisFrame = false;
    float _PerlinNoiseSeed = 0.0f;
    float _ShakeSeconds = 0.0f;

    public TimelineClip Clip { get; set; }

    /// <summary>
    /// 현재 PlayableBehaviour가 플레이 될때 호출
    /// </summary>
    /// <param name="playable"></param>
    /// <param name="info"></param>
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        _BecameActiveThisFrame = true; // 플레이 후 첫프레임인지 확인하기 위해서 켜줍니다
        base.OnBehaviourPlay(playable, info);
    }

    /// <summary>
    /// 플레이가 중단되었을때 호출
    /// </summary>
    /// <param name="playable"></param>
    /// <param name="info"></param>
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (_VirtualCineManager != null)
        {
            _VirtualCineManager.ResetPos(); // 흔들림이 끝났으면 VirtualCineManager의 원래 좌표를 잡아줍니다
            _VirtualCineManager = null;
        }
        base.OnBehaviourPause(playable, info);
    }

    /// <summary>
    /// 플레이 중일때 각 프레임마다 호출
    /// </summary>
    /// <param name="playable"></param>
    /// <param name="info"></param>
    /// <param name="playerData">track 앞쪽에 입력되어 있는 data</param>
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        VirtualCineManager virtualCineManager = playerData as VirtualCineManager;

        if (!virtualCineManager)
            return;

        if (_BecameActiveThisFrame) // 클립 플레이 후 첫 프레임만 실행
        {
            _PerlinNoiseSeed = UnityEngine.Random.Range(MinTickRange, MaxTickRange);
            _BecameActiveThisFrame = false;
            _ShakeSeconds = 0;
            _VirtualCineManager = null;
        }

        float inputWeight = info.weight;
        // 클립이 플레이중이면 0이상
        // 플레이 비중이 100퍼센트면 1, 50퍼센트면 0.5
        if (inputWeight > 0f)
        {
            if (_VirtualCineManager != virtualCineManager)
            {
                _VirtualCineManager = virtualCineManager;
            }

            // 펄린노이즈는 난수를 생성하는 함수이지만
            // 다른 난수 생성 함수와는 달리 주어지는 연속된 값에 따라
            // 비교적 부드러운 곡선의 난수를 생성한다
            // 그래서 지형 생성에 주로 사용하는 편이라
            // 여기의 함수도 x, y값을 별도로 받고 있네요
            _ShakeSeconds += info.deltaTime;
            float perlinInput = _ShakeSeconds * _Roughness + _PerlinNoiseSeed;
            float rand = UnityEngine.Random.Range(0f, 1f);
            Vector3 PositionCorrection = new Vector3(
            _Horizontal ? Mathf.PerlinNoise(perlinInput, rand) - .5f : 0, // PerlinNoise의 결과값은 0~1이니 0.5를 빼서 -0.5 ~ 0.5를 만들어줘야 음수방향으로도 흔들어줘서 균형이 맞습니다.
            _Vertical ? Mathf.PerlinNoise(rand, perlinInput) - .5f : 0,
            0f) * _Magnitude * inputWeight;

            // VirtualCineManager를 직접 흔들어준다
            _VirtualCineManager.ShakeFromOriginalPositionBy(PositionCorrection);
        }

        base.ProcessFrame(playable, info, playerData);
    }
}
