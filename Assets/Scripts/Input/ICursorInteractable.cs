using UnityEngine;

/// <summary>
/// 커서의 위치에서 신호를 받으려면 이 인터페이스를 구현하면 됩니다.
/// </summary>
public interface ICursorInteractable
{
    // TODO: 이 메소드를 구현할 필요 없으면 이후에 지웁시다 
    virtual void OnInputDown(Vector2 position) { }
    virtual void OnInputUp(Vector2 position) { }
    virtual void OnDrag(Vector2 position) { }
}
