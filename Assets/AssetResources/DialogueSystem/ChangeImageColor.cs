using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeImageColor : MonoBehaviour
{
    public Image imageToChange;
    public Color colorA;
    public Color colorB;
    private bool autoOnInfo = false;
    public float duration = 1.0f;
    private Color startColor;
    private Color targetColor;

    // 다른 스크립트나 이벤트에서 호출할 함수


    public void ColorSetToA()
    {
        if (imageToChange != null)
        {
            imageToChange.color = colorA;
        }
        else
        {
            Debug.LogWarning("이미지가 할당되지 않았습니다. 이미지를 먼저 할당하세요.");
        }
    }

    public void ColorSetToB()
    {
        if (imageToChange != null)
        {
            imageToChange.color = colorB;
        }
        else
        {
            Debug.LogWarning("이미지가 할당되지 않았습니다. 이미지를 먼저 할당하세요.");
        }
    }


    public void ChangeColor()
    {
        if (autoOnInfo)
        {
            if (imageToChange != null)
            {
                startColor = colorB;
                targetColor = colorA;
                autoOnInfo = false;
                StartCoroutine(ChangeColorOverTime());
            }
            else
            {
                Debug.LogWarning("이미지가 할당되지 않았습니다. 이미지를 먼저 할당하세요.");
            }

        }
        else
        {
            if (imageToChange != null)
            {
                startColor = colorA;
                targetColor = colorB;
                autoOnInfo = true;
                StartCoroutine(ChangeColorOverTime());
            }
            else
            {
                Debug.LogWarning("이미지가 할당되지 않았습니다. 이미지를 먼저 할당하세요.");
            }
        }

    }

    IEnumerator ChangeColorOverTime()
    {
        float timer = 0.0f;
        while (timer < duration)
        {
            // 보간하여 색상 변경
            imageToChange.color = Color.Lerp(startColor, targetColor, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        imageToChange.color = targetColor; // 목표 색상으로 설정

    }
}
