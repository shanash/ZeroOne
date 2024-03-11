using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FluffyDuck.Util;
using Cysharp.Threading.Tasks;
using System.Threading;

public class TooltipBase : MonoBehaviour, IPoolableComponent
{
    public const float PRESS_TIME_FOR_SHOW = 0.2f;
    const float GAP_BETWEEN_ICON_AND_TOOLTIP = 20;
    const int DIM_ENABLE_MILLISECONDS = 2000;

    static Texture2D Texture = null;
    static Vector2 Texture_Size = Vector2.zero;

    [SerializeField]
    Image Box = null;

    [SerializeField]
    RectTransform Container = null;

    [SerializeField]
    GameObject Dim = null;

    [SerializeField]
    TMP_Text Title = null;

    [SerializeField]
    TMP_Text Desc = null;

    Material Shader_Mat = null;
    CancellationTokenSource Cancel_Token = new CancellationTokenSource(); // Dim 활성화 작업 취소 토큰

    protected virtual void Initialize(Rect hole, string title, string desc, bool is_screen_modify = true)
    {
        if (Box == null) return;

        EnableDelayed(Dim, DIM_ENABLE_MILLISECONDS, Cancel_Token.Token).Forget();

        float box_width = Box.rectTransform.rect.size.x;
        float box_height = Box.rectTransform.rect.size.y;

        Vector2 texture_size = new Vector2(Screen.width, box_height / box_width * Screen.width);

        if (Texture == null || !Texture_Size.Equals(texture_size))
        {
            Texture_Size = texture_size;
            Texture = CreateSolidTexture(Texture_Size);
        }

        // 해상도에 따른 hole 보정 값
        Vector4 texture_hole = new Vector4(hole.x / Texture_Size.x, hole.y / Texture_Size.y, hole.width / Texture_Size.x, hole.height/ Texture_Size.y);
        if (is_screen_modify && Texture_Size.x != Screen.width)
        {
            float multiple = Texture_Size.x / Screen.width;
            texture_hole *= multiple;
        }

        Shader_Mat = new Material(Shader.Find("FluffyDuck/TransparentHole"));
        Box.sprite = Sprite.Create(Texture, new Rect(0.0f, 0.0f, Texture.width, Texture.height), new Vector2(0.5f, 0.5f));
        Box.material = Shader_Mat;
        Box.material.SetVector("_Rect", texture_hole);

        Title.text = title;
        Desc.text = desc;

        float multi = box_width / Texture_Size.x;

        Container.pivot = new Vector2(0, 0);
        Container.anchoredPosition = new Vector2(hole.center.x * multi, (hole.y + hole.height + GAP_BETWEEN_ICON_AND_TOOLTIP) * multi);

        Canvas.ForceUpdateCanvases();

        AdjustPositionWithinScreen(Box.rectTransform, Container, hole, multi);
    }

    void AdjustPositionWithinScreen(RectTransform full_screen, RectTransform tooltip, Rect hole, float multi)
    {
        Vector3[] screenCorners = new Vector3[4];
        Vector3[] componentCorners = new Vector3[4];

        full_screen.GetWorldCorners(screenCorners);
        tooltip.GetWorldCorners(componentCorners);

        float screenLeft = screenCorners[0].x;
        float screenRight = screenCorners[2].x;
        float screenUp = screenCorners[2].y;

        float componentLeft = componentCorners[0].x;
        float componentRight = componentCorners[2].x;
        float componentUp = componentCorners[2].y;

        // 컴포넌트가 화면 오른쪽으로 벗어나는 경우
        if (componentRight > screenRight)
        {
            float offset = componentRight - screenRight;
            tooltip.position -= new Vector3(offset, 0, 0);
        }
        // 컴포넌트가 화면 왼쪽으로 벗어나는 경우
        else if (componentLeft < screenLeft)
        {
            float offset = screenLeft - componentLeft;
            tooltip.position += new Vector3(offset, 0, 0);
        }

        // 컴포넌트가 화면 위쪽으로 벗어나는 경우
        if (componentUp > screenUp)
        {
            tooltip.pivot = new Vector2(0, 1);
            tooltip.anchoredPosition += new Vector2(0, -(GAP_BETWEEN_ICON_AND_TOOLTIP * 2 + hole.height) * multi);
        }
    }

    /// <summary>
    /// 생성할때 자원을 너무 많이 먹어서 꼭 필요할때만 생성해야합니다아
    /// </summary>
    /// <param name="size"></param>
    /// <returns></returns>
    Texture2D CreateSolidTexture(Vector2 size)
    {
        Texture2D result = new Texture2D((int)size.x, (int)size.y);

        for (int y = 0; y < result.height; y++)
        {
            for (int x = 0; x < result.width; x++)
            {
                result.SetPixel(x, y, Color.white); // 각 픽셀을 하얀색으로 설정
            }
        }
        result.Apply(); // 텍스처 변경사항 적용

        return result;
    }

    async UniTaskVoid EnableDelayed(GameObject go, int delay, CancellationToken cancel_token)
    {
        await UniTask.Delay(delay, cancellationToken: cancel_token);
        if (cancel_token.IsCancellationRequested)
        {
            return;
        }
        go.SetActive(true);
    }

    public void OnClickDim()
    {
        GameObjectPoolManager.Instance.UnusedGameObject(this.gameObject);
    }

    public void Spawned()
    {
        Debug.Log("Spawned");
        var rt = GetComponent<RectTransform>();
        rt.localPosition = Vector3.zero;
    }

    public void Despawned()
    {
        Debug.Log("Despawned");
        Cancel_Token.Cancel();
    }
}
