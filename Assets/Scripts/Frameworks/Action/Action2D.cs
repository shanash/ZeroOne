using System.Collections;
using UnityEngine;

namespace FluffyDuck.Util
{
    public static class Action2D
    {
        /*
            * 지정된 시간동안 지정된 위치로 이동한다.
            * 
            * @param target 애니메이션을 적용할 타겟 GameObject
            * @param to 이동할 목표 위치
            * @param duration 이동 시간
            * @param callback 애니메이션 종류 후 타겟 반환 콜백
            */
        public static IEnumerator MoveTo(Transform target, Vector3 to, float duration, System.Action<Transform> callback)
        {
            Vector2 startPos = target.transform.position;

            float elapsed = 0.0f;
            while (elapsed < duration)
            {
                elapsed += Time.smoothDeltaTime;
                target.transform.position = Vector2.Lerp(startPos, to, elapsed / duration);

                yield return null;
            }

            target.transform.position = to;

            callback?.Invoke(target);

            yield break;
        }

        public static IEnumerator MoveToRectTransform(RectTransform target, Vector3 to, float duration, System.Action<RectTransform> cb)
        {
            Vector2 startPos = target.transform.localPosition;

            float elapsed = 0.0f;
            while (elapsed < duration)
            {
                elapsed += Time.smoothDeltaTime;
                target.transform.localPosition = Vector2.Lerp(startPos, to, elapsed / duration);

                yield return null;
            }

            target.transform.localPosition = to;

            cb?.Invoke(target);

            yield break;
        }


        /*
            * param toScale 커지느(줄어지는) 크기, 예를 들어, 0.5인 경우 현재 크기에서 절반으로 줄어든다.
            * param speed 초당 커지는 속도. 예를 들어, 2인 경우 초당 2배 만큼 커지거나 줄어든다. 
            */
        public static IEnumerator Scale(Transform target, float toScale, float speed, System.Action<Transform> callback)
        {
            //1. 방향 결정 : 커지는 방향이면 +, 줄어드는 방향이면 -
            bool bInc = target.localScale.x < toScale;
            float fDir = bInc ? 1 : -1;

            float factor;
            while (true)
            {
                factor = Time.deltaTime * speed * fDir;
                target.localScale = new Vector3(target.localScale.x + factor, target.localScale.y + factor, target.localScale.z);

                if ((!bInc && target.localScale.x <= toScale) || (bInc && target.localScale.x >= toScale))
                    break;

                yield return null;
            }
            callback?.Invoke(target);
            yield break;
        }

    }

}
