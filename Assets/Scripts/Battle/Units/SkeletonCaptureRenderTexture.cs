using Cinemachine;
using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCaptureRenderTexture : MonoBehaviour, IPoolableComponent
{
    SpriteRenderer Sprite_Renderer;
    RendererSortingZ ZOrder;


    
    private void Awake()
    {
        Sprite_Renderer = GetComponent<SpriteRenderer>();
        ZOrder = GetComponent<RendererSortingZ>();
    }

    public void CaptureObject(GameObject obj)
    {
        if (obj == null)
        {
            return;
        }
        var texture = RenderObjectToTexture(obj);
        Sprite_Renderer.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
    }

    Texture2D RenderObjectToTexture(GameObject obj)
    {
        if (obj == null)
        {
            return null;
        }
        //  오브젝트를 렌더링 하기 위한 임시 카메라 생성
        Camera tempCamera = new GameObject("TempCamera").AddComponent<Camera>();
        tempCamera.transform.position = obj.transform.position;
        tempCamera.transform.rotation = obj.transform.rotation;

        // 카메라 설정
        tempCamera.clearFlags = CameraClearFlags.Color;
        tempCamera.backgroundColor = Color.clear;
        tempCamera.orthographic = false; // 원하는 카메라 설정에 따라 변경

        obj.layer = LayerMask.NameToLayer("Unit");
        int render_layer_mask = 1 << LayerMask.NameToLayer("Unit");

        //  카메라 렌더링 및 Texture 생성
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        tempCamera.targetTexture = renderTexture;
        tempCamera.cullingMask = render_layer_mask;
        tempCamera.Render();


        // Texture2D로 변환하여 반환
        RenderTexture.active = renderTexture;
        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height);
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();

        // 리소스 정리
        RenderTexture.active = null;
        Destroy(renderTexture);
        Destroy(tempCamera.gameObject);

        return texture;
    }



    public void Spawned()
    {
    }
    public void Despawned()
    {
    }

}
