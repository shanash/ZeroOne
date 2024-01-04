using System;
using System.Collections.Concurrent;
using UnityEngine;

/// <summary>
/// 다른 스레드에서 액션을 받아서 메인스레드에서 처리하기 위한 클래스
/// </summary>
public class MainThreadDispatcher : MonoBehaviourSingleton<MainThreadDispatcher>
{
    ConcurrentQueue<Action> Actions_Queue;

    protected override bool _Is_DontDestroyOnLoad {  get { return true; } }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void CallInstance()
    {
        _ = MainThreadDispatcher.Instance;
    }

    protected override void OnAwake()
    {
        base.OnAwake();

        Actions_Queue = new ConcurrentQueue<Action>();
    }

    public void Enqueue(Action action)
    {
        Actions_Queue.Enqueue(action);
    }

    void Update()
    {
        while (Actions_Queue.TryDequeue(out var action))
        {
            action?.Invoke();
        }
    }
}
