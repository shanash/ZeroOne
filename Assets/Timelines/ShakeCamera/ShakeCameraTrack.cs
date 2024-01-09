using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(0.855f, 0.8623f, 0.87f)]
[TrackClipType(typeof(ShakeCameraClip))]
[TrackBindingType(typeof(VirtualCineManager))]
public class ShakeCameraTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        var clips = GetClips();
        foreach (var clip in clips)
        {
            var shakeCameraClip = clip.asset as ShakeCameraClip;
            shakeCameraClip.ClipPassthrough = clip;
        }

        return ScriptPlayable<ShakeCameraMixerBehaviour>.Create(graph, inputCount);
    }
}
