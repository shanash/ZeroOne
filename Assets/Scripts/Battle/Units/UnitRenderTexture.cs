using FluffyDuck.Util;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class UnitRenderTexture : ZeroOne.SkeletonRenderTexture
{
    protected MaterialPropertyBlock Property_Block;

    protected string Color_Property = "_Color";
    protected string Black_Tint_Property = "_Black";

    /// <summary>
    /// 전체 색상 변경
    /// </summary>
    protected string All_Change_Color_Property = "_AllChangeColor";
    protected string Is_Change_Property = "_IsChange";

    protected RendererSortingZ ZOrder;
    protected SortingGroup Sort_Group;

    Coroutine Alpha_Coroutine;
    Coroutine Hit_Coroutine;

    bool Use_Ultimate_Change_Color;


    protected override void CreateQuadChild()
    {
        base.CreateQuadChild();

        if (Property_Block == null)
        {
            Property_Block = new MaterialPropertyBlock();
        }
        if (ZOrder == null)
        {
            if (ZOrder == null)
            {
                ZOrder = quad.AddComponent<RendererSortingZ>();
                ZOrder.SetZorderIndex(ZORDER_INDEX.HERO);
            }
            if (Sort_Group == null)
            {
                Sort_Group = quad.AddComponent<SortingGroup>();
                Sort_Group.sortingLayerName = "Unit";
            }
        }

        // 레이어를 유닛으로 고정합니다
        quad.layer = 12;

        this.enabled = false;
    }

   

    public void SetChangeColor(string color)
    {
        this.enabled = true;
        Color c = CommonUtils.ToRGBFromHex(color);
        Property_Block.SetColor(All_Change_Color_Property, c);
        Property_Block.SetInt(Is_Change_Property, 1);
        quadMeshRenderer.SetPropertyBlock(Property_Block);
        Use_Ultimate_Change_Color = true;
    }
    public void SetRollbackColor()
    {
        this.enabled = true;
        var clear_color = Color.clear;
        clear_color.a = 1f;
        Property_Block.SetColor(All_Change_Color_Property, Color.clear);
        Property_Block.SetFloat(Is_Change_Property, 0);
        quadMeshRenderer.SetPropertyBlock(Property_Block);
        this.enabled = false;
        Use_Ultimate_Change_Color = false;
    }


    public void SetHitColorV2(Color col, float duration)
    {
        if (Use_Ultimate_Change_Color)
        {
            return;
        }
        if (Hit_Coroutine != null)
        {
            StopCoroutine(Hit_Coroutine);
        }
        Hit_Coroutine = StartCoroutine(StartHitWhite(col, duration));
    }

    IEnumerator StartHitWhite(Color white, float duration)
    {
        this.enabled = true;

        Property_Block.SetColor(All_Change_Color_Property, white);
        Property_Block.SetInt(Is_Change_Property, 1);
        quadMeshRenderer.SetPropertyBlock(Property_Block);
        yield return new WaitForSeconds(duration);

        var clear_color = Color.clear;
        clear_color.a = 1f;
        Property_Block.SetColor(All_Change_Color_Property, Color.clear);
        Property_Block.SetFloat(Is_Change_Property, 0);
        quadMeshRenderer.SetPropertyBlock(Property_Block);

        this.enabled = false;

        Hit_Coroutine = null;
        Use_Ultimate_Change_Color = false;
    }


    /// <summary>
    /// </summary>
    /// <param name="alpha"></param>
    /// <param name="duration"></param>
    /// <param name="finish_render_enable">애니메이션 종료 후 랜더러 상태 적용</param>
    public void SetAlphaAnimation(float alpha, float duration, bool finish_render_enable)
    {
        if (Alpha_Coroutine != null)
        {
            StopCoroutine(Alpha_Coroutine);
        }
        Alpha_Coroutine = StartCoroutine(StartAlphaAnimation(alpha, duration, finish_render_enable));
    }
    IEnumerator StartAlphaAnimation(float alpha, float duration, bool finish_render_enable)
    {
        this.enabled = true;
        float delta = 0f;
        while (delta < duration)
        {
            delta += Time.deltaTime;
            this.color.a = Mathf.Lerp(this.color.a, alpha, delta / duration);
            yield return null;
        }
        this.color.a = alpha;
        this.enabled = finish_render_enable;

        Alpha_Coroutine = null;
    }

    public HeroBase_V2 GetHeroBase_V2()
    {
        return GetComponent<HeroBase_V2>();
    }



}
