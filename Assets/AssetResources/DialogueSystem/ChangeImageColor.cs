using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeImageColor : MonoBehaviour
{
    public Image imageToChange;
    public Color OnColor;
    public Color OffColor;
    public bool autoOnInfo;

    // 다른 스크립트나 이벤트에서 호출할 함수
    public void ChangeColor()
    {
        if (autoOnInfo)
        {
            if (imageToChange != null)
            {
                imageToChange.color = OffColor;
                autoOnInfo = false;
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
                imageToChange.color = OnColor;
                autoOnInfo = true;
            }
            else
            {
                Debug.LogWarning("이미지가 할당되지 않았습니다. 이미지를 먼저 할당하세요.");
            }
        }

    }
}
