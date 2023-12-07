using Spine.Unity;
using Spine.Unity.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRenderTexture : SkeletonRenderTexture
{
    protected MaterialPropertyBlock Property_Block;

    protected string Color_Property = "_Color";
    protected string Black_Tint_Property = "_Black";

    protected override void CreateQuadChild()
    {
        base.CreateQuadChild();

        if (Property_Block == null)
        {
            Property_Block = new MaterialPropertyBlock();
        }
    }

    public void SetHitColor(Color pcolor, Color bcolor, float duration)
    {
        StartCoroutine(StartHitColor(pcolor, bcolor, duration));
    }

    IEnumerator StartHitColor(Color pcolor, Color bcolor, float duration)
    {
        Property_Block.SetColor(Color_Property, pcolor);
        Property_Block.SetColor(Black_Tint_Property, bcolor);
        quadMeshRenderer.SetPropertyBlock(Property_Block);
        yield return new WaitForSeconds(duration);

        Property_Block.SetColor(Color_Property, Color.white);
        Property_Block.SetColor(Black_Tint_Property, Color.black);
        quadMeshRenderer.SetPropertyBlock(Property_Block);


        SkeletonRenderSeparator seperator;
    }


    
}
