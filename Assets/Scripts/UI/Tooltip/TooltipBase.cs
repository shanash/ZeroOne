using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FluffyDuck.Util;
using Cysharp.Threading.Tasks;
using System.Threading;
using FluffyDuck.UI;
using ZeroOne.Input;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class TooltipBase : PopupBase, IPoolableComponent
{
    static Texture2D Texture = null;
    static Vector2 Texture_Size = Vector2.zero;

    [SerializeField, Tooltip("팝업박스 이미지")]
    Image Box_Image = null;

    [SerializeField]
    RectTransform Container = null;

    [SerializeField]
    TMP_Text Title = null;

    [SerializeField]
    TMP_Text Desc = null;

    [SerializeField, Tooltip("Rect 보정치")]
    float Modified_Rect = 0;

    [SerializeField, Tooltip("라운딩 반지름 길이, 마이너스면 안쪽으로 들어간다")]
    float Radius = -10.0f;

    [SerializeField, Tooltip("아이콘과 툴팁 사이의 거리")]
    float Gap_Between_Icon_And_Tooltp = 20;

    Material Shader_Mat = null;

    protected virtual void Initialize(Rect hole, string title, string desc, bool is_screen_modify = true)
    {
        if (Box_Image == null) return;

        float box_width = Box_Rect.rect.size.x;
        float box_height = Box_Rect.rect.size.y;

        Vector2 texture_size = new Vector2(Screen.width, box_height / box_width * Screen.width);

        if (Texture == null || !Texture_Size.Equals(texture_size))
        {
            Texture_Size = texture_size;
            Texture = CreateSolidTexture(Texture_Size);
        }

        if (!Modified_Rect.Equals(0.0f))
        {
            hole = new Rect(hole.x - Modified_Rect / 2.0f, hole.y - Modified_Rect / 2.0f, hole.width + Modified_Rect, hole.height + Modified_Rect);
        }

        // 모서리 라운딩 계산
        float sizeup_value = Radius;
        float sizeup_half = sizeup_value / 2.0f;

        Vector4 texture_hole = new Vector4((hole.x) / Texture_Size.x, (hole.y) / Texture_Size.y, (hole.width) / Texture_Size.x, (hole.height) / Texture_Size.y);
        Vector4 texture_ext_hole = new Vector4((hole.x - sizeup_half) / Texture_Size.x, (hole.y - sizeup_half) / Texture_Size.y, (hole.width + sizeup_value) / Texture_Size.x, (hole.height + sizeup_value) / Texture_Size.y);

        if (Radius < 0)
        {
            Vector4 temp = texture_ext_hole;
            texture_ext_hole = texture_hole;
            texture_hole = temp;
        }

        if (is_screen_modify && Texture_Size.x != Screen.width)
        {
            float multiple = Texture_Size.x / Screen.width;
            texture_hole *= multiple;
            texture_ext_hole *= multiple;
        }

        Shader_Mat = new Material(Shader.Find("FluffyDuck/TransparentHole"));
        Box_Image.sprite = Sprite.Create(Texture, new Rect(0.0f, 0.0f, Texture.width, Texture.height), new Vector2(0.5f, 0.5f));
        Box_Image.material = Shader_Mat;
        Box_Image.material.SetVector("_Rect", texture_hole);
        Box_Image.material.SetVector("_RangeRect", texture_ext_hole);

        
        Title.text = title;
        Desc.text = desc;

        float multi = box_width / Texture_Size.x;

        Container.pivot = new Vector2(0, 0);
        Container.anchoredPosition = new Vector2(hole.center.x * multi, (hole.y + hole.height + Gap_Between_Icon_And_Tooltp) * multi);

        Canvas.ForceUpdateCanvases();

        AdjustPositionWithinScreen(Box_Rect, Container, hole, multi);
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
            tooltip.anchoredPosition += new Vector2(0, -(Gap_Between_Icon_And_Tooltp * 2 + hole.height) * multi);
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

    public void OnClickDim()
    {
        Release();
    }

    public void Release()
    {
        GameObjectPoolManager.Instance.UnusedGameObject(this.gameObject);
    }

    public override void Despawned()
    {
        InputCanvas.OnDrag -= HandleDrag;
        InputCanvas.OnInputUp -= HandleInputUp;

        base.Despawned();
    }

    public override void Spawned()
    {
        base.Spawned();
        var rt = GetComponent<RectTransform>();
        rt.localPosition = Vector3.zero;
        InputCanvas.OnDrag += HandleDrag;
        InputCanvas.OnInputUp += HandleInputUp;
    }

    void HandleDrag(InputActionPhase phase, Vector2 delta, Vector2 drag_origin, Vector2 position)
    {
        // 벡터 거리의 제곱..이니 해상도 기준 30
        if (drag_origin.sqrMagnitude > 900)
        {
            TooltipManager.I.CloseAll();
        }
    }

    void HandleInputUp(Vector2 position, ICollection<ICursorInteractable> components)
    {
        TooltipManager.I.CloseAll();
    }
}
