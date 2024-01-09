using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FluffyDuck.Util
{
    /// <summary>
    /// 조이스틱 패널
    /// 일정 지역의 아무곳이나 터치할 경우 조이스틱의 위치가 지정되며 해당 위치에서 조이스틱을 컨트롤 할 수 있는 컴포넌트
    /// </summary>
    public class JoystickPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler
    {
        [SerializeField, Tooltip("조이스틱 패널 바디")]
        RectTransform Joystick_Rect;
        [SerializeField, Tooltip("조이스틱 스틱 부분")]
        RectTransform Knob_Image;
        //  최대 이동 거리. 중앙으로부터 100 px
        [SerializeField]
        float MAX_DISTANCE = 100.0f;

        public event EventHandler<JoystickEventArgs> JoystickEventHandler;

        bool Is_Dragging;

        float Current_Force;
        Vector2 Current_Direction;
        Vector3 Current_Rotate;

        JoystickEventArgs Event_Args = new JoystickEventArgs();


        public void OnPointerDown(PointerEventData eventData)
        {
            InitJoystick();
            Vector2 pos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(this.GetComponent<RectTransform>(), eventData.position, Camera.main, out pos);
            if (!Joystick_Rect.gameObject.activeSelf)
            {
                Joystick_Rect.gameObject.SetActive(true);
            }
            Joystick_Rect.transform.localPosition = pos;
            if (JoystickEventHandler != null)
            {
                EventArgsNullCheckAndInit();
                Event_Args.Event_Type = JOYSTICK_EVENT_TYPE.BEGIN;
                JoystickEventHandler.Invoke(this, Event_Args);
            }
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            Is_Dragging = true;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            Is_Dragging = false;
            InitJoystick();

            if (Joystick_Rect.gameObject.activeSelf)
            {
                Joystick_Rect.gameObject.SetActive(false);
            }
            if (JoystickEventHandler != null)
            {
                EventArgsNullCheckAndInit();
                Event_Args.Event_Type = JOYSTICK_EVENT_TYPE.FINISH;
                JoystickEventHandler.Invoke(this, Event_Args);
            }
        }

        void InitJoystick()
        {
            Current_Force = 0.0f;
            Current_Rotate = Vector3.zero;
            Knob_Image.transform.localPosition = Vector2.zero;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!Is_Dragging)
            {
                return;
            }
            Vector2 loc = Vector2.zero;
            if (eventData.dragging)
            {
                Vector2 p_pos = Vector2.zero;   //  press position
                Vector2 n_pos = Vector2.zero;   //  now position

                //  screen point를 rect point로 변환
                RectTransformUtility.ScreenPointToLocalPointInRectangle(this.GetComponent<RectTransform>(), eventData.pressPosition, Camera.main, out p_pos);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(this.GetComponent<RectTransform>(), eventData.position, Camera.main, out n_pos);
                Vector2 diff = n_pos - p_pos;
                Current_Force = Vector2.Distance(n_pos, p_pos);

                Current_Force = Mathf.Clamp(Current_Force, 0, MAX_DISTANCE);
                Vector2 dir = diff.normalized;
                if (Current_Force < MAX_DISTANCE)
                {
                    Knob_Image.transform.localPosition = loc + (dir * Current_Force);
                }
                else
                {
                    Current_Force = MAX_DISTANCE;
                    Knob_Image.transform.localPosition = loc + (dir * Current_Force);
                }
                //  바라보는 방향
                Current_Direction = diff.normalized;

                //  현재 조이스틱의 방향
                float angle = FluffyDuck.Util.CommonUtils.Math.Angle(n_pos, p_pos);
                //Debug.LogFormat("Angle [{0}]", angle);
                Current_Rotate = new Vector3(0.0f, 0.0f, angle);
            }

        }

        void Update()
        {
            if (Is_Dragging)
            {
                if (JoystickEventHandler != null)
                {
                    EventArgsNullCheckAndInit();
                    Event_Args.Event_Type = JOYSTICK_EVENT_TYPE.DRAGGING;
                    Event_Args.Direction = Current_Direction;
                    Event_Args.Rotate = Current_Rotate;
                    Event_Args.Force = Current_Force / MAX_DISTANCE;
                    JoystickEventHandler.Invoke(this, Event_Args);
                }
            }
        }
        void EventArgsNullCheckAndInit()
        {
            if (Event_Args != null)
            {
                Event_Args = new JoystickEventArgs();
            }
            Event_Args.Reset();
        }
        /// <summary>
        /// 조이스틱 이벤트 핸들러 등록
        /// </summary>
        /// <param name="listener"></param>
        public void AddJoystickEventHandler(EventHandler<JoystickEventArgs> listener)
        {
            JoystickEventHandler += listener;
        }
        /// <summary>
        ///  조이스틱 이벤트 핸들러 제거
        /// </summary>
        /// <param name="listener"></param>
        public void RemoveJoystickEventHandler(EventHandler<JoystickEventArgs> listener)
        {
            JoystickEventHandler -= listener;
        }
        /// <summary>
        /// 등록되어 있는 모든 이벤트 핸들러 제거
        /// </summary>
        public void ClearJoystickEventHandler()
        {
            if (JoystickEventHandler != null)
            {
                var invoke_list = JoystickEventHandler.GetInvocationList();
                for (int i = 0; i < invoke_list.Length; i++)
                {
                    var invoke = invoke_list[i];
                    RemoveJoystickEventHandler((EventHandler<JoystickEventArgs>)invoke);
                }
            }
        }



    }

}

