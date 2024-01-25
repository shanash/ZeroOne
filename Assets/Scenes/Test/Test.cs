using Spine;
using Spine.Unity;
using System.IO;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    private void Awake()
    {
        Debug.Log($"Canvas : {canvas.additionalShaderChannels}");
    }
}
