using System.Collections;
using UnityEngine;

public class CoroutineManager : MonoBehaviourSingleton<CoroutineManager>
{
    protected override bool _Is_DontDestroyOnLoad { get { return true; } }

    public Coroutine StartManagedCoroutine(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }
}
