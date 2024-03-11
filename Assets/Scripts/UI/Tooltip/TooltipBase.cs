using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FluffyDuck.Util;

public class TooltipBase : MonoBehaviour, IPoolableComponent
{
    public const float PRESS_TIME_FOR_SHOW = 0.2f;
    const float GAP_BETWEEN_ICON_AND_TOOLTIP = 20;
    static Texture2D Texture = null;
    static Vector2 Texture_Size = Vector2.zero;

    [SerializeField]
    Image Box = null;

    [SerializeField]
    RectTransform Container = null;

    [SerializeField]
    TMP_Text Title = null;

    [SerializeField]
    TMP_Text Desc = null;

    Material Shader_Mat = null;

    protected virtual void Initialize(Rect hole, string title, string desc, bool is_screen_modify = true)
    {
        if (Box == null) return;

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

        float pivot_x = 0;
        /*
        if (hole.center.x < Texture_Size.x / 2) pivot_x = 0;// 아이콘이 좌측에 위치
        else pivot_x = 1; // 아이콘이 우측에 위치
        */
        // 아이콘이 위에 위치
        if (hole.center.y > Texture_Size.y / 2)
        {
            Container.pivot = new Vector2(pivot_x, 1);
            Container.anchoredPosition = new Vector2(hole.center.x * multi, (hole.y - GAP_BETWEEN_ICON_AND_TOOLTIP) * multi);
        }
        else // 아이콘이 아래에 위치
        {
            Container.pivot = new Vector2(pivot_x, 0);
            Container.anchoredPosition = new Vector2(hole.center.x * multi, (hole.y + hole.height + GAP_BETWEEN_ICON_AND_TOOLTIP) * multi);
        }

        AdjustPositionWithinScreen(Box.rectTransform, Container);
    }

    void AdjustPositionWithinScreen(RectTransform full_screen, RectTransform tooltip)
    {
        Vector3[] screenCorners = new Vector3[4];
        Vector3[] componentCorners = new Vector3[4];

        full_screen.GetWorldCorners(screenCorners);
        tooltip.GetWorldCorners(componentCorners);

        float screenLeft = screenCorners[0].x;
        float screenRight = screenCorners[2].x;

        float componentLeft = componentCorners[0].x;
        float componentRight = componentCorners[2].x;

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

    public void Spawned()
    {
        var rt = GetComponent<RectTransform>();
        rt.localPosition = Vector3.zero;
    }

    public void Despawned()
    {
    }
}
