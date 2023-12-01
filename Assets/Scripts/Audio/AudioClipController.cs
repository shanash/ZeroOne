using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// AudioClip리소스의 생성과 Release를 편하게 하기 위한 클래스
/// </summary>
public class AudioClipController : IDisposable
{
    public string Key { get; private set; }
    public AudioClip Clip { get; private set; }

    AsyncOperationHandle<AudioClip> _Handle;
    bool _Disposed = false;

    public async static Task<AudioClipController> Create(string key)
    {
        AudioClipController result = new AudioClipController();
        if (!await result.Init(key))
        {
            result = null;
        }

        return result;
    }

    AudioClipController() { }

    async Task<bool> Init(string key)
    {
        var handle = Addressables.LoadAssetAsync<AudioClip>(key);
        AudioClip result = await handle.Task;
        if (handle.Status == AsyncOperationStatus.Failed)
        {
            return false;
        }

        _Handle = handle;
        Clip = result;
        Key = key;

        return true;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_Disposed)
        {
            if (disposing)
            {
                Addressables.Release(_Handle);
                Clip = null;
            }
            _Disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
