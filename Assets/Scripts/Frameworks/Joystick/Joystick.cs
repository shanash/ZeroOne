using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace FluffyDuck.Util
{
    /// <summary>
    /// 조이스틱 컴포넌트
    /// 조이스틱 바디의 이동은 불가능하며, 스틱만 이동이 가능.
    /// 추가 옵션으로 조이스틱 컨트롤을 캔슬할 수 있도록 기능 추가
    /// </summary>
    public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IPoolableComponent
    {
        [SerializeField, Tooltip("조이스틱 바디")]
        protected Image Joystick_Rect;
        [SerializeField, Tooltip("조이스틱 배경")]
        protected Image Joystick_BG;
        [SerializeField, Tooltip("조이스틱 스틱 부분")]
        protected Image Knob_Image;
        [SerializeField, Tooltip("조이스틱 캔슬 커버")]
        protected Image Cancel_Cover;
        [SerializeField, Tooltip("조이스틱 사용 여부")]
        protected bool Is_Use_Knob;
        [SerializeField, Tooltip("조이스틱 최대 이동 거리(반지름)")]
        protected float MAX_DISTANCE = 100.0f;
        [SerializeField, Tooltip("취소 영역을 사용할 것인지 여부")]
        protected bool Is_Use_Cancel_Area;
        [SerializeField, Tooltip("버튼 취소 영역")]
        protected RectTransform Cancel_Area;
        [SerializeField, Tooltip("취소 영역의 감지 거리(반지름)")]
        protected float CANCEL_AREA_DETECT_DISTANCE = 50.0f;
        [SerializeField, Tooltip("조이스틱 아이콘 - 스킬 및 공격용 아이콘")]
        protected Image Icon_Image;




        /// <summary>
        /// 조이스틱 이벤트 핸들러
        /// </summary>
        public event EventHandler<JoystickEventArgs> JoystickEventHandler;

        /// <summary>
        /// 조이스틱을 드래깅 중인이 여부 정보
        /// </summary>
        protected bool Is_Dragging;
        /// <summary>
        /// 조이스틱을 눌렀을 때 
        /// </summary>
        protected bool Is_Pressed;
        /// <summary>
        /// 취소 영역 감지 중
        /// </summary>
        bool Is_Detect_Cancel_Area;
        /// <summary>
        /// 사용 가능 여부
        /// </summary>
        protected bool Is_Enable_Joystick;

        Color Origin_Knob_Color;

        float Current_Force;
        Vector2 Current_Direction;
        Vector3 Current_Rotate;
        /// <summary>
        /// 조이스틱 이벤트 핸들러의 값
        /// </summary>
        JoystickEventArgs Event_Args = new JoystickEventArgs();


        void Awake()
        {
            Is_Enable_Joystick = true;
            Origin_Knob_Color = Knob_Image.color;
            Cancel_Area.gameObject.SetActive(false);
            Knob_Image.gameObject.SetActive(false);
        }

        protected void InitJoystick()
        {
            Current_Force = 0.0f;
            Current_Rotate = Vector3.zero;
            Knob_Image.transform.localPosition = Vector2.zero;
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

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!Is_Enable_Joystick)
            {
                return;
            }
            JoystickDragBegin(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!Is_Enable_Joystick)
            {
                return;
            }
            JoystickDragging(eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!Is_Enable_Joystick)
            {
                return;
            }
            JoystickPointDown(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!Is_Enable_Joystick)
            {
                return;
            }
            JoystickPointUp(eventData);
        }

        protected virtual void JoystickPointDown(PointerEventData evt)
        {
            Is_Pressed = true;
            InitJoystick();
            // 취소 영역을 사용할 경우
            if (Is_Use_Cancel_Area)
            {
                // 취소 영역 보이기
                ShowHideCancelArea(true);
            }
            if (Is_Use_Knob)
            {
                ShowHideKnobImage(true);
            }
            ShowHIdeBGImage(true);


            DetectCancelAreaUpdate(false);
            if (JoystickEventHandler != null)
            {
                EventArgsNullCheckAndInit();
                Event_Args.Event_Type = JOYSTICK_EVENT_TYPE.BEGIN;
                JoystickEventHandler.Invoke(this, Event_Args);
            }
        }
        protected virtual JoystickEventArgs JoystickPointUp(PointerEventData evt)
        {
            if (JoystickEventHandler != null)
            {
                EventArgsNullCheckAndInit();
                Event_Args.Event_Type = JOYSTICK_EVENT_TYPE.FINISH;
                Event_Args.Direction = Current_Direction;
                Event_Args.Rotate = Current_Rotate;
                Event_Args.Force = Current_Force / MAX_DISTANCE;
                //  취소 영역이 활성화 되었을 경우에만 취소 정보를 보내준다.
                if (Is_Use_Cancel_Area)
                {
                    Event_Args.Is_Detect_Cancel_Area = Is_Detect_Cancel_Area;
                }
                JoystickEventHandler.Invoke(this, Event_Args);
            }
            // 취소 영역을 사용할 경우
            if (Is_Use_Cancel_Area)
            {
                // 취소 영역 감추기
                ShowHideCancelArea(false);
            }
            // 스틱 사용할 경우
            if (Is_Use_Knob)
            {
                ShowHideKnobImage(false);
            }
            ShowHIdeBGImage(false);
            InitJoystick();
            Is_Dragging = false;
            Is_Pressed = false;
            DetectCancelAreaUpdate(false);

            return Event_Args;
        }
        protected virtual void JoystickDragBegin(PointerEventData evt)
        {
            Is_Dragging = true;


        }
        protected virtual void JoystickDragging(PointerEventData evt)
        {
            if (!Is_Dragging)
            {
                return;
            }
            Vector2 loc = Vector2.zero;
            if (evt.dragging)
            {
                Vector2 p_pos = Vector2.zero;   //  press position
                Vector2 n_pos = Vector2.zero;   // now position

                //  screen point를 rect point로 변환
                RectTransformUtility.ScreenPointToLocalPointInRectangle(this.GetComponent<RectTransform>(), evt.pressPosition, Camera.main, out p_pos);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(this.GetComponent<RectTransform>(), evt.position, Camera.main, out n_pos);
                Vector2 diff = n_pos - p_pos;
                Current_Force = Vector2.Distance(n_pos, p_pos);
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
                Current_Rotate = new Vector3(0.0f, 0.0f, CommonUtils.Math.Angle(n_pos, p_pos));


                //  취소 영역 사용할 시
                if (Is_Use_Cancel_Area)
                {
                    //  cancel area distance
                    float cancel_dist = Vector2.Distance((Vector2)Cancel_Area.localPosition, n_pos);
                    if (!Is_Detect_Cancel_Area) // 감지중이 아닐 때
                    {
                        // 취소 영역에 감지됨
                        if (cancel_dist < CANCEL_AREA_DETECT_DISTANCE)
                        {
                            DetectCancelAreaUpdate(true);
                        }
                    }
                    else // 감지 상태인 경우
                    {
                        // 감지 영역을 벗어나 감지 취소
                        if (cancel_dist > CANCEL_AREA_DETECT_DISTANCE)
                        {
                            DetectCancelAreaUpdate(false);
                        }
                    }
                }

            }
        }
        /// <summary>
        /// EventArgs 널 체크 및 초기화
        /// </summary>
        protected void EventArgsNullCheckAndInit()
        {
            if (Event_Args != null)
            {
                Event_Args = new JoystickEventArgs();
            }
            Event_Args.Reset();
        }
        /// <summary>
        /// 주기적으로 돌면서 드래그 상황 일 경우에만 이벤트 핸들러를 호출
        /// </summary>
        private void Update()
        {
            DraggingUpdate();
        }

        protected virtual void DraggingUpdate()
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
                    //  취소 영역이 활성화 되었을 경우에만 취소 정보를 보내준다.
                    if (Is_Use_Cancel_Area)
                    {
                        Event_Args.Is_Detect_Cancel_Area = Is_Detect_Cancel_Area;
                    }
                    JoystickEventHandler.Invoke(this, Event_Args);
                }
            }
        }


        /// <summary>
        /// 취소 영역 감지 여부에 따라 플레그 저장
        /// 감지 여부에 따라 조이스틱 색 변경
        /// </summary>
        /// <param name="is_detect"></param>
        protected void DetectCancelAreaUpdate(bool is_detect)
        {
            if (Is_Detect_Cancel_Area == is_detect)
            {
                return;
            }
            if (!Is_Use_Cancel_Area)
            {
                return;
            }
            Color knob_color = Knob_Image.color;
            if (Is_Use_Knob)
            {
                if (is_detect)
                {
                    // knob color
                    Color red = Color.red;
                    knob_color.r = red.r;
                    knob_color.g = red.g;
                    knob_color.b = red.b;
                }
                else
                {
                    // knob color
                    knob_color.r = Origin_Knob_Color.r;
                    knob_color.g = Origin_Knob_Color.g;
                    knob_color.b = Origin_Knob_Color.b;
                }
                Knob_Image.color = knob_color;
            }

            // 취소 커버 
            ShowCancelCoverImage(is_detect);

            Is_Detect_Cancel_Area = is_detect;
        }

        /// <summary>
        /// 취소 영역 보이기/감추기
        /// </summary>
        /// <param name="is_show"></param>
        protected void ShowHideCancelArea(bool is_show)
        {
            if (Cancel_Area.gameObject.activeSelf != is_show)
            {
                Cancel_Area.gameObject.SetActive(is_show);
            }
        }
        /// <summary>
        /// 스틱 보이기/감추기
        /// </summary>
        /// <param name="is_show"></param>
        protected void ShowHideKnobImage(bool is_show)
        {
            if (Knob_Image.gameObject.activeSelf != is_show)
            {
                Knob_Image.gameObject.SetActive(is_show);
            }
        }
        /// <summary>
        /// 조이스틱 배경 이미지 보이기/감추기
        /// </summary>
        /// <param name="is_show"></param>
        protected void ShowHIdeBGImage(bool is_show)
        {
            if (Joystick_BG.gameObject.activeSelf != is_show)
            {
                Joystick_BG.gameObject.SetActive(is_show);
            }
        }
        /// <summary>
        /// 취소 감지시 취소 커버 이미지 보이기/감추기
        /// </summary>
        /// <param name="is_show"></param>
        protected void ShowCancelCoverImage(bool is_show)
        {
            if (Cancel_Cover.gameObject.activeSelf != is_show)
            {
                Cancel_Cover.gameObject.SetActive(is_show);
            }
        }



        /// <summary>
        /// 취소 영역의 좌표 이동
        /// 모든 조이스틱의 취소 영역이 동일한 위치에 있어야 하기때문에 취소 영역의 좌표 Parent에 들어갔다가
        /// 다시 조이스틱의 Parent로 돌아온다.
        /// 이때 취소 영역의 Parent에서 좌표값을 (9, 0)으로 초기화 해줘야 한다.
        /// </summary>
        /// <param name="p"></param>
        public void SetCancelAreaParent(RectTransform p)
        {
            Cancel_Area.transform.SetParent(p);
            Cancel_Area.transform.localPosition = Vector2.zero;
            Cancel_Area.transform.SetParent(this.transform);
        }

        public void Spawned()
        {
            ClearJoystickEventHandler();
        }
        public void Despawned()
        {

        }

        /// <summary>
        /// 조이스틱 아이콘
        /// </summary>
        /// <param name="path"></param>
        public void SetJoystickIconImage(string path)
        {
            var spr = Resources.Load<Sprite>(path);
            if (spr != null)
            {
                Icon_Image.overrideSprite = spr;
            }
        }

        public void SetEnableJoystick(bool is_enable)
        {
            Is_Enable_Joystick = is_enable;
        }




    }

}


