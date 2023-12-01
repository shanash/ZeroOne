using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;


[Serializable]
public class ScreenFaderBehaviour : PlayableBehaviour
{
    [SerializeField, Tooltip("Fade 되는 색상")]
    Color color = Color.black;

    Color Origin_Color = Color.black;
    bool _BecameActiveThisFrame = false;
    bool _BecameInactiveThisFrame = false;

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
        _BecameInactiveThisFrame = true;
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
        Image fade_image = playerData as Image;

        if (!fade_image)
            return;

        if (_BecameActiveThisFrame) // 클립 플레이 후 첫 프레임만 실행
        {
            Origin_Color = fade_image.color;
            _BecameActiveThisFrame = false;
        }
        else if (_BecameInactiveThisFrame)
        {
            fade_image.color = Origin_Color;
            _BecameInactiveThisFrame = false;
        }

        float inputWeight = info.weight;
        // 클립이 플레이중이면 0이상
        // 플레이 비중이 100퍼센트면 1, 50퍼센트면 0.5
        if (inputWeight > 0f)
        {
            Color _color = color;
            _color.a *= inputWeight;
            fade_image.color = _color;
        }

        base.ProcessFrame(playable, info, playerData);
    }
}
