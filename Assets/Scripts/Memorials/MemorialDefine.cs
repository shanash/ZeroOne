
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public enum MEMORIAL_TYPE
{
    NONE = 0,
    MAIN_LOBBY,         //  메인 로비에서 사용 - 기능이 다름
    MEMORIAL,           //  메모리얼 페이지에서 사용
}

public class MemorialDefine
{
    public static bool TryParseEvent(string eventName, out SPINE_EVENT result)
    {
        return Enum.TryParse(eventName, true, out result);
    }

    /// <summary>
    /// 스파인리소스에서 이벤트 이름으로 보내주는 값들의 정의
    /// 스파인 제작 아트분들과 소통합시다
    /// </summary>
    public enum SPINE_EVENT
    {
        NONE = 0,
        VOICE,
        MOUTH_SHAPE,
    }
}
