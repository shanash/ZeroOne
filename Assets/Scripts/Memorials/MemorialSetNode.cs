using Cinemachine;
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
    // 메모리얼 카메라 렌즈 설정
    const float MEMORIAL_CAMERA_VERTICAL_FOV = 56.7f;
    const float MEMORIAL_CAMERA_ORTHOGRAPHIC_SIZE = 10.0f;
    const float MEMORIAL_CAMERA_NEAR_CLIP = 0.1f;
    const float MEMORIAL_CAMERA_FAR_CLIP = 5000.0f;
    const float MEMORIAL_CAMERA_DUTCH = 0.0f;

    [SerializeField, Tooltip("Memorial Timeline Animation")]
    protected PlayableDirector Playable_Director;

    [SerializeField, Tooltip("Actress Container")]
    protected Transform Actress_Container;

    [SerializeField, Tooltip("Actress")]
    protected ActorBase Actor;

    [SerializeField, Tooltip("Far BG Container")]
    protected Transform Far_BG_Container;

    [SerializeField, Tooltip("Middle BG Container")]
    protected Transform Middle_BG_Container;

    [SerializeField, Tooltip("Near BG Container")]
    protected Transform Near_BG_Container;

    protected MEMORIAL_TYPE Memorial_Type = MEMORIAL_TYPE.NONE;

    SkeletonAnimation Actress_Skeleton_Animation
    {
        get { return Actor.GetComponent<SkeletonAnimation>(); }
    }

    public static async Task<MemorialSetNode> Create(int player_character_id, MEMORIAL_TYPE type, Transform parent, Image cover, CinemachineVirtualCamera virtual_camera)
    {
        // TODO: 현재는 스토리모드가 구현이 되어 있지 않아서 메인로비말고는 없는데
        // 일단 메인로비는 order = 0 으로 생각합시다...
        int order = type == MEMORIAL_TYPE.MAIN_LOBBY ? 0 : 1;

        Me_Resource_Data data = MasterDataManager.Instance.Get_MemorialData(player_character_id, order);
        if (data == null)
        {
            Debug.Assert(false, $"해당 메모리얼 데이터가 존재하지 않습니다 : pcid {player_character_id}, order {order}");
            return null;
        }

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

    async Task Init(MEMORIAL_TYPE type, Me_Resource_Data data, Image cover, CinemachineVirtualCamera virtual_camera)
    {
        SetMemorialType(type);
        //Actor.Init(data.player_character_id, data.state_id);

        virtual_camera.m_Lens = new LensSettings(MEMORIAL_CAMERA_VERTICAL_FOV, MEMORIAL_CAMERA_ORTHOGRAPHIC_SIZE, MEMORIAL_CAMERA_NEAR_CLIP, MEMORIAL_CAMERA_FAR_CLIP, MEMORIAL_CAMERA_DUTCH);

        transform.position = virtual_camera.State.CorrectedPosition + virtual_camera.State.CorrectedOrientation * Vector3.forward * 10;
        transform.rotation = virtual_camera.State.FinalOrientation;

        if (string.IsNullOrEmpty(data.intro_key))
        {
            return;
        }

        var virtual_camera_animator = virtual_camera.gameObject.AddComponent<Animator>();

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
                Playable_Director.SetGenericBinding(track, virtual_camera_animator);
            }
        }
        Playable_Director.Play();
    }
}
