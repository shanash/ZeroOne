using Cinemachine;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MemorialManager : MonoBehaviour
{
    [SerializeField, Tooltip("캐릭터 아이디")]
    int Player_Character_ID = 0;

    [SerializeField, Tooltip("메모리얼 타입")]
    MEMORIAL_TYPE Type = MEMORIAL_TYPE.NONE;

    [SerializeField, Tooltip("Memorial Container")]
    Transform Memorial_Container = null;

    [SerializeField]
    Image Cover = null;

    [SerializeField]
    CinemachineVirtualCamera VirtualCamera = null;

    List<MemorialSetNode> Used_Memorial_Set_Nodes = new List<MemorialSetNode>();

    private async void Start()
    {
        TouchCanvas.Instance.SetTouchEffectPrefabPath("VFX/Prefabs/Touch_Effect_Pink");
        InputCanvas.Instance.Enable = false;

        // TODO: 로딩 시간이 잠깐이라도 있을수 있기 때문에 로딩UI가 필요할 수 있다
        // TODO: 물론 캐릭터 id와 order 값도 어딘가에서 받아와야 한다
        await SpawnMemorialSetNode(Player_Character_ID);

        InputCanvas.Instance.Enable = true;
    }

    async Task SpawnMemorialSetNode(int player_character_id)
    {
        var node = await MemorialSetNode.Create(player_character_id, Type, Memorial_Container, Cover, VirtualCamera);
        Used_Memorial_Set_Nodes.Add(node);
    }

    public void OnClickBack()
    {
        SceneManager.LoadScene("home");
    }
}
