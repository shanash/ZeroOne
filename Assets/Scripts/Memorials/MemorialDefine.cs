
using System;

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
