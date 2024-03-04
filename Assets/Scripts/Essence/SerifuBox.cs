using System.Collections;
using TMPro;
using UnityEngine;

public class SerifuBox : MonoBehaviour
{
    const float CANVAS_ALPHA_DURATION_PERCENT = 0.2f; // 20퍼센트 시간이 지나면 창이 완전히 보임

    [SerializeField]
    CanvasGroup CanvasGroup;

    [SerializeField]
    TextMeshProUGUI Serifu;

    [SerializeField]
    float wait_seconds = 2.0f;

    public void OnReceiveSpineMessage(string message, float duration)
    {
        Serifu.text = message;
        StartCoroutine(AnimateTextGradient(duration));
    }

    IEnumerator AnimateTextGradient(float duration)
    {
        Serifu.ForceMeshUpdate();
        var textInfo = Serifu.textInfo;
        int characterCount = textInfo.characterCount;

        float alphaDuration = duration * CANVAS_ALPHA_DURATION_PERCENT; // CanvasGroup.alpha가 1이 되는 데까지 걸리는 시간
        float time = 0;

        CanvasGroup.alpha = 0; // CanvasGroup의 투명도를 초기화합니다.

        while (time < duration)
        {
            if (time <= alphaDuration)
            {
                // 전체 duration의 20% 동안 CanvasGroup.alpha를 0에서 1까지 부드럽게 증가시킵니다.
                CanvasGroup.alpha = Mathf.Clamp01(time / alphaDuration);
            }
            else
            {
                // 20% 시간이 지난 후에는 CanvasGroup.alpha를 1로 유지합니다.
                CanvasGroup.alpha = 1;
            }

            for (int characterIndex = 0; characterIndex < characterCount; characterIndex++)
            {
                var charInfo = textInfo.characterInfo[characterIndex];
                if (!charInfo.isVisible) continue;

                int materialIndex = textInfo.characterInfo[characterIndex].materialReferenceIndex;
                Color32[] newVertexColors = textInfo.meshInfo[materialIndex].colors32;
                int vertexIndex = textInfo.characterInfo[characterIndex].vertexIndex;

                float overlapFactor = 0.5f;
                float timePerCharacter = (duration * overlapFactor) / characterCount;
                float characterStartTime = timePerCharacter * characterIndex;
                float characterElapsed = Mathf.Clamp01((time - characterStartTime) / (duration - characterStartTime));
                byte alphaByte = (byte)(255 * characterElapsed);

                for (int i = 0; i < 4; i++)
                {
                    newVertexColors[vertexIndex + i].a = alphaByte;
                }

                Serifu.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            }

            time += Time.deltaTime;
            yield return null;
        }

        // 애니메이션이 끝난 후, CanvasGroup.alpha가 확실히 1이 되도록 설정합니다.
        CanvasGroup.alpha = 1;

        // 애니메이션 종료 후 1초 대기
        yield return new WaitForSeconds(1);

        // 1초 동안 CanvasGroup.alpha를 0으로 부드럽게 사라지게 함
        float fadeOutDuration = 1f;
        float fadeOutStartTime = Time.time;
        while (Time.time - fadeOutStartTime < fadeOutDuration)
        {
            Serifu.alpha = 1 - ((Time.time - fadeOutStartTime) / fadeOutDuration);
            yield return null;
        }

        Serifu.alpha = 0;

        // 1초 동안 CanvasGroup.alpha를 0으로 부드럽게 사라지게 함
        fadeOutDuration = 1f;
        fadeOutStartTime = Time.time;
        while (Time.time - fadeOutStartTime < fadeOutDuration)
        {
            CanvasGroup.alpha = 1 - ((Time.time - fadeOutStartTime) / fadeOutDuration);
            yield return null;
        }

        CanvasGroup.alpha = 0; // 확실히 alpha를 0으로 설정
    }
}
