using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListBackgroundChange : MonoBehaviour
{
    public List<Sprite> bgList; // 이미지 리스트
    public int bgIndex = 0; // 선택한 이미지의 인덱스
    public Image bgGameObj; // 이미지를 적용할 UI Sprite 게임 오브젝트


    // 이미지를 설정하는 함수
    public void NextBackground()
    {
        // 이미지 인덱스가 유효한지 확인
        if (bgIndex >= 0 && bgIndex < bgList.Count)
        {
            // 선택한 이미지를 UI Sprite 게임 오브젝트에 적용
            bgGameObj.sprite = bgList[bgIndex];
            bgIndex = bgIndex + 1;
        }
        else
        {
            Debug.LogError("이미지 인덱스가 유효하지 않습니다.");
        }
    }
}

