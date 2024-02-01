using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventCallback : MonoBehaviour
{
    System.Action<string> Animation_Event_Callback;

    public void SetAnimationEventCallback(System.Action<string> callback)
    {
        Animation_Event_Callback = callback;
    }

    
    public void AnimationEvent(string evt_key)
    {
        Animation_Event_Callback?.Invoke(evt_key);
    }
}
