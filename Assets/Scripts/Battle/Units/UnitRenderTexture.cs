using FluffyDuck.Util;
using Spine.Unity.Examples;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class UnitRenderTexture : SkeletonRenderTexture
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
            }
        }

        this.enabled = false;
    }

    public void SetHitColor(Color pcolor, Color bcolor, float duration)
    {
        StartCoroutine(StartHitColor(pcolor, bcolor, duration));
    }

    IEnumerator StartHitColor(Color pcolor, Color bcolor, float duration)
    {
        this.enabled = true;
        Property_Block.SetColor(Color_Property, pcolor);
        Property_Block.SetColor(Black_Tint_Property, bcolor);
        quadMeshRenderer.SetPropertyBlock(Property_Block);
        yield return new WaitForSeconds(duration);

        Property_Block.SetColor(Color_Property, Color.white);
        Property_Block.SetColor(Black_Tint_Property, Color.black);
        quadMeshRenderer.SetPropertyBlock(Property_Block);
        this.enabled = false;
    }

    public void SetHitColorV2(Color col, float duration)
    {
        StartCoroutine(StartHitWhite(col, duration));
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
    }

    public HeroBase_V2 GetHeroBase_V2()
    {
        return GetComponent<HeroBase_V2>();
    }



}
