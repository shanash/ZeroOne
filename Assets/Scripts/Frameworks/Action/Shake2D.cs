using System.Collections;
using UnityEngine;

public class Shake2D : MonoBehaviour
{
    Coroutine Shake_Coroutine = null;

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
        var rt = this.transform as RectTransform;
        Vector2 origin_pos = rt.anchoredPosition;
        float delta = 0f;
        while (delta < duration)
        {
            rt.anchoredPosition = origin_pos + Random.insideUnitCircle * shake_force;
            delta += Time.smoothDeltaTime;
            yield return null;
        }
        rt.anchoredPosition3D = origin_pos;
        cb?.Invoke(this);

        Shake_Coroutine = null;
    }
}
