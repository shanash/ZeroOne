using UnityEngine;

namespace ZeroOne.Input
{
    /// <summary>
    /// 커서의 위치에서 신호를 받으려면 이 인터페이스를 구현하면 됩니다.
    /// </summary>
    public interface ICursorInteractable
    {
        abstract string GameObjectName { get; }

        virtual void OnInputDown(Vector2 position) { }
        virtual void OnInputUp(Vector2 position) { }
        virtual void OnDrag(Vector2 position) { }
    }
}
