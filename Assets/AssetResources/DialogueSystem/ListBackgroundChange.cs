using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListBackgroundChange : MonoBehaviour
{
    public List<Sprite> bgList; // 이미지 리스트
    public Image bgGameObj; // 이미지를 적용할 UI Sprite 게임 오브젝트
    private int bgIndex; // 선택한 이미지의 인덱스

    // 이미지를 설정하는 함수
    public void ChangeBackground(string getValue)
    {
        //다이알로그 시퀀스에서는 스트링 형식으로만 값을 줄 수 있기 때문에 인트로 변환해줘야 한다.
        bgIndex = int.Parse(getValue);

        // 이미지 인덱스가 유효한지 확인
        if (bgIndex >= 0 && bgIndex < bgList.Count)
        {
            // 선택한 이미지를 UI Sprite 게임 오브젝트에 적용
            bgGameObj.sprite = bgList[bgIndex];
        }
        else
        {
            Debug.LogError("이미지 인덱스 번호가 존재하지 않습니다.");
        }
    }
}

