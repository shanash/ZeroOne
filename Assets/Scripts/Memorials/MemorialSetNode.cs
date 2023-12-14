using FluffyDuck.Util;
using Spine.Unity;
using Spine.Unity.Playables;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

/// <summary>
/// 메모리얼 세팅 노드
/// </summary>
[RequireComponent(typeof(PlayableDirector))]
public class MemorialSetNode : MonoBehaviour, IPoolableComponent
{
    [SerializeField, Tooltip("Memorial Timeline Animation")]
    protected PlayableDirector Playable_Director;

    [SerializeField, Tooltip("Actress Container")]
    protected Transform Actress_Container;

    [SerializeField, Tooltip("Actress")]
    protected ActressBase Actress;

    [SerializeField, Tooltip("Far BG Container")]
    protected Transform Far_BG_Container;

    [SerializeField, Tooltip("Middle BG Container")]
    protected Transform Middle_BG_Container;

    [SerializeField, Tooltip("Near BG Container")]
    protected Transform Near_BG_Container;

    protected MEMORIAL_TYPE Memorial_Type = MEMORIAL_TYPE.NONE;

    SkeletonAnimation Actress_Skeleton_Animation
    {
        get { return Actress.GetComponent<SkeletonAnimation>(); }
    }

    public static async Task<MemorialSetNode> Create(Me_Resource_Data data, Transform parent, MEMORIAL_TYPE type, Image cover, Animator virtual_camera)
    {
        var obj = await GameObjectPoolManager.Instance.GetGameObjectAsync(data.prefab_key, parent);
        var node = obj.GetComponent<MemorialSetNode>();
        await node.Init(type, data, cover, virtual_camera);

        return node;
    }

    private void Awake()
    {
        CheckScreenAsfectRatio();
    }

    /// <summary>
    /// 스크린 사이즈 비율 체크
    /// </summary>
    protected virtual void CheckScreenAsfectRatio()
    {
        const float width_ratio = 4f;
        const float height_ratio = 3f;

        float ortho_size = (Screen.height / (Screen.width / width_ratio)) / height_ratio;
        float asfect_scale = 1f / ortho_size;

        ScalingContainer(Far_BG_Container, asfect_scale);
        ScalingContainer(Middle_BG_Container, asfect_scale);
        ScalingContainer(Near_BG_Container, asfect_scale);
        ScalingContainer(Actress_Container, asfect_scale);
    }

    void ScalingContainer(Transform tr, float scale)
    {
        var cscale = tr.localScale;
        cscale *= scale;
        tr.localScale = cscale;
    }

    public void SetMemorialType(MEMORIAL_TYPE type)
    {
        Memorial_Type = type;
    }

    public void Spawned()
    {
        transform.localPosition = Vector3.zero;
    }

    public void Despawned()
    {

    }

    async Task Init(MEMORIAL_TYPE type, Me_Resource_Data data, Image cover, Animator virtual_camera)
    {
        SetMemorialType(type);
        Actress.Init(data.player_character_id, data.state_id);
        if (string.IsNullOrEmpty(data.intro_key))
        {
            return;
        }

        var intro_ta = await CommonUtils.GetResourceFromAddressableAsset<TimelineAsset>(data.intro_key);
        var intro_tracks = intro_ta.GetOutputTracks();
        Playable_Director.playableAsset = intro_ta;

        foreach (var track in intro_tracks)
        {
            if (track is SpineAnimationStateTrack)
            {
                Playable_Director.SetGenericBinding(track, Actress_Skeleton_Animation);
            }
            else if (track is ScreenFaderTrack)
            {
                Playable_Director.SetGenericBinding(track, cover);
            }
            else if (track is AnimationTrack)
            {
                Playable_Director.SetGenericBinding(track, virtual_camera);
            }
        }
        Playable_Director.Play();
    }
}
