using UnityEngine;

namespace FluffyDuck.Util
{
    public enum JOYSTICK_EVENT_TYPE
    {
        NONE = 0,
        BEGIN,              //  ���̽�ƽ �̺�Ʈ - ����(��ġ �ٿ� - ����)
        DRAGGING,           //  ���̽�ƽ �̺�Ʈ �巡��
        FINISH,             //  ���̽�ƽ �̺�Ʈ - ��ġ ��(����)
    }
    public class JoystickEventArgs : System.EventArgs
    {
        public JOYSTICK_EVENT_TYPE Event_Type { get; set; } = JOYSTICK_EVENT_TYPE.NONE;
        /// <summary>
        /// ��ֶ����� ���� ����
        /// </summary>
        public Vector2 Direction { get; set; }
        /// <summary>
        /// ȸ�� ����
        /// </summary>
        public Vector3 Rotate { get; set; }
        /// <summary>
        /// �Ŀ�. 0 ~ 1 ����.
        /// </summary>
        public float Force { get; set; }
        /// <summary>
        /// ��� ������ �����Ǿ����� ����
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

