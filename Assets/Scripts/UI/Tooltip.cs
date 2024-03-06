using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FluffyDuck.Util;

public class Tooltip : MonoBehaviour, IPoolableComponent
{
    public const float PRESS_TIME_FOR_SHOW = 0.2f;
    const float GAP_BETWEEN_ICON_AND_TOOLTIP = 20;

    [SerializeField]
    Image Box = null;

    [SerializeField]
    RectTransform Container = null;

    [SerializeField]
    TMP_Text Title = null;

    [SerializeField]
    TMP_Text Desc = null;

    [SerializeField]
    NpcCardBase Npc_Card = null;

    Material Shader_Mat = null;
    Texture2D Texture = null;

    public void Initialize(Rect hole, Npc_Data npc_data)
    {
        if (Box == null) return;

        float box_width = Box.rectTransform.rect.size.x;
        float box_height = Box.rectTransform.rect.size.y;

        Texture = CreateSolidTexture((int)box_width, (int)box_height);

        Shader_Mat = new Material(Shader.Find("Standard"));
        Shader_Mat.shader = Resources.Load<Shader>("Shaders/TransparentHole");

        Box.sprite = Sprite.Create(Texture, new Rect(0.0f, 0.0f, Texture.width, Texture.height), new Vector2(0.5f, 0.5f));
        Box.material = Shader_Mat;

        Box.material.SetVector("_Rect", new Vector4(hole.x/box_width, hole.y/box_height, hole.width/box_width, hole.height/box_height));
        Box.material.SetColor("_Color", Box.color);

        Title.text = GameDefine.GetLocalizeString(npc_data.name_id);
        Desc.text = GameDefine.GetLocalizeString(npc_data.desc_id);

        Npc_Card.SetNpcData(npc_data);

        // 아이콘이 위에 위치
        if (hole.center.y > box_height / 2)
        {
            Container.pivot = new Vector2(0, 1);
            Container.anchoredPosition = new Vector2(hole.center.x, hole.y - GAP_BETWEEN_ICON_AND_TOOLTIP);
        }
        else // 아이콘이 아래에 위치
        {
            Container.pivot = Vector2.zero;
            Container.anchoredPosition = new Vector2(hole.center.x, hole.y + hole.height + GAP_BETWEEN_ICON_AND_TOOLTIP);
        }
    }

    Texture2D CreateSolidTexture(int width, int height)
    {
        Texture2D result = new Texture2D(width, height);

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
        rt.anchoredPosition = Vector2.zero;
    }

    public void Despawned()
    {
    }
}
