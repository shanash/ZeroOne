using Cysharp.Threading.Tasks;
using FluffyDuck.Util;
using System;
using UnityEngine;

/// <summary>
/// 카메라로부터 화면을 찍습니다</br>
/// 카메라로부터 OnRenderImage를 받아야 해서 카메라에 붙입니다
/// </summary>
[RequireComponent(typeof(Camera))]
public class TexturePhotographer : MonoBehaviour//, IDisposable
{
    //bool disposedValue = false;
    //bool Is_Ready_For_Capture = false;
    //Camera Camera = null;
    Action<Texture2D> Callback_For_Capture = null;

    //public RenderTexture RenderTexture { get; private set; }

    /*
    private void OnDestroy()
    {
        Dispose(false);
    }
    public bool CreateRenderTexture(int depth)
    {
        //Rect rt = GameObjectUtils.GetPixelRect(camera);
        this.Camera = GetComponent<Camera>();
        this.RenderTexture = new RenderTexture(Screen.width, Screen.height, depth);
        this.Camera.targetTexture = this.RenderTexture;
        return true;
    }
    */
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (Callback_For_Capture != null)
        {
            Texture2D capturedImage = new Texture2D(src.width, src.height, TextureFormat.RGB24, false);
            RenderTexture.active = src;

            capturedImage.ReadPixels(new Rect(0, 0, src.width, src.height), 0, 0);
            capturedImage.Apply();

            RenderTexture.active = null;

            Callback_For_Capture(capturedImage);
            Callback_For_Capture = null;
        }

        Graphics.Blit(src, dest);
    }

    /// <summary>
    /// 순간을 캡쳐하여 Texture2D로 가져옵니다
    /// </summary>
    /// <returns></returns>
    public void Capture(Action<Texture2D> callback)
    {
        Callback_For_Capture = callback;
    }
    /*
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing) { }// 관리형 리소스를 삭제합니다.

            // 비관리형 리소스
            if (this.RenderTexture != null)
            {
                this.RenderTexture.Release();
                this.RenderTexture = null;
            }

            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    */
}
