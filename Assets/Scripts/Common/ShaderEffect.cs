using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ShaderEffect : MonoBehaviour
{
    public Material Effect_Material = null; // 포스트 프로세싱 효과를 적용할 Material
    public bool Effect_Enable = false;

    public void Init(Material material)
    {
        Effect_Material = material;
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (Effect_Material != null && Effect_Enable)
        {
            Graphics.Blit(src, dest, Effect_Material);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}
