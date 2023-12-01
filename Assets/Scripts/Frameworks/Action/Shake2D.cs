using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake2D : MonoBehaviour
{
    Coroutine Shake_Coroutine;

    public void Shake(float duration, float shake_force, System.Action<object> cb)
    {
        if (Shake_Coroutine != null)
        {
            return;
        }
        Shake_Coroutine = StartCoroutine(ShakeCoroutine(duration, shake_force, cb));
    }

    IEnumerator ShakeCoroutine(float duration, float shake_force, System.Action<object> cb)
    {
        Vector2 origin_pos = this.transform.position;
        float delta = 0f;
        while (delta < duration)
        {
            this.transform.position = origin_pos + Random.insideUnitCircle * shake_force;
            delta += Time.smoothDeltaTime;
            yield return null;
        }
        this.transform.position = origin_pos;
        cb?.Invoke(this);

        Shake_Coroutine = null;
        yield break;
        
    }

}
