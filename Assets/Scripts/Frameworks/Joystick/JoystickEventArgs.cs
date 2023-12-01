using UnityEngine;

namespace FluffyDuck.Util
{
    public enum JOYSTICK_EVENT_TYPE
    {
        NONE = 0,
        BEGIN,              //  조이스틱 이벤트 - 시작(터치 다운 - 시작)
        DRAGGING,           //  조이스틱 이벤트 드래깅
        FINISH,             //  조이스틱 이벤트 - 터치 업(종료)
    }
    public class JoystickEventArgs : System.EventArgs
    {
        public JOYSTICK_EVENT_TYPE Event_Type { get; set; } = JOYSTICK_EVENT_TYPE.NONE;
        /// <summary>
        /// 노멀라이즈 벡터 방향
        /// </summary>
        public Vector2 Direction { get; set; }
        /// <summary>
        /// 회전 방향
        /// </summary>
        public Vector3 Rotate { get; set; }
        /// <summary>
        /// 파워. 0 ~ 1 사이.
        /// </summary>
        public float Force { get; set; }
        /// <summary>
        /// 취소 영역에 감지되었는지 여부
        /// </summary>
        public bool Is_Detect_Cancel_Area { get; set; }

        public void Reset()
        {
            Event_Type = JOYSTICK_EVENT_TYPE.NONE;
            Direction = Vector2.zero;
            Rotate = Vector3.zero;
            Force = 0.0f;
            Is_Detect_Cancel_Area = false;
        }
    }

}

