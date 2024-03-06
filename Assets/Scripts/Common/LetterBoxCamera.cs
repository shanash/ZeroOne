using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterBoxCamera : MonoBehaviour
{
    public enum LETTER_BOX_TYPE
    {
        NONE = 0,
        TOP,
        BOTTOM,
        LEFT,
        RIGHT,
    }

    Camera Letter_Box_Cam;

    [SerializeField, Tooltip("Letter Box Type")]
    LETTER_BOX_TYPE box_type = LETTER_BOX_TYPE.NONE;

    private void Start()
    {
        Letter_Box_Cam = GetComponent<Camera>();

        SetResolution();
    }

    void SetResolution()
    {
        int setWidth = GameDefine.RESOLUTION_SCREEN_WIDTH; // 사용자 설정 너비
        int setHeight = GameDefine.RESOLUTION_SCREEN_HEIGHT; // 사용자 설정 높이

        int deviceWidth = Screen.width; // 기기 너비 저장
        int deviceHeight = Screen.height; // 기기 높이 저장

        Screen.SetResolution(setWidth, (int)(((float)deviceHeight / deviceWidth) * setWidth), true); // SetResolution 함수 제대로 사용하기


        float new_width = ((float)setWidth / setHeight) / ((float)deviceWidth / deviceHeight); // 새로운 너비
        float new_height = ((float)deviceWidth / deviceHeight) / ((float)setWidth / setHeight); // 새로운 높이 (실제 View Height)

        float hide_height = (1f - new_height) * 0.5f;
        float hide_width = (1f - new_width) * 0.5f;

        if (box_type == LETTER_BOX_TYPE.TOP)
        {
            Letter_Box_Cam.rect = new Rect((1f - new_width) / 2f, 1f - hide_height, new_width, hide_height);
        }
        else if (box_type == LETTER_BOX_TYPE.BOTTOM)
        {
            Letter_Box_Cam.rect = new Rect((1f - new_width) / 2f, 0f, new_width, hide_height);
        }
        else if (box_type == LETTER_BOX_TYPE.LEFT)
        {
            Letter_Box_Cam.rect = new Rect(0f, 0f, hide_width, 1f);
        }
        else if (box_type == LETTER_BOX_TYPE.RIGHT)
        {
            Letter_Box_Cam.rect = new Rect(1f - hide_width, 0f, hide_width, 1f);
        }
        else
        {
            Debug.Assert(false);
        }

        

    }


}
