using UnityEngine;

public interface IActressPositionProvider
{
    /// <summary>
    /// 말풍선이 표시될 위치를 가져옵니다.
    /// </summary>
    /// <returns>말풍선 위치를 나타내는 Vector3 값입니다.</returns>
    Vector3 GetBalloonWorldPosition();
}
