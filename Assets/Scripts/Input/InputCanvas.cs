using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ZeroOne.Input
{
    /// <summary>
    /// 유저 입력을 총괄하는 캔버스
    /// </summary>
    public class InputCanvas : MonoBehaviourSingleton<InputCanvas>
    {
        #region Input Event
        public delegate void InputAction(Vector2 position, ICollection<ICursorInteractable> components);
        public delegate void InputPhaseAction(InputActionPhase phase, Vector2 position, ICollection<ICursorInteractable> components);
        public delegate void InputDragAction(InputActionPhase phase, Vector2 delta, Vector2 drag_origin, Vector2 position);
        public delegate void InputTap(ICursorInteractable[] components);

        public static event InputAction OnInputDown;
        public static event InputAction OnInputUp;
        public static event InputDragAction OnDrag;
        public static event InputAction OnTap;
        public static event InputPhaseAction OnLongTap;
        #endregion

        [SerializeField, Tooltip("Cursor")]
        Cursor _Cursor = null;
        [SerializeField]
        PlayerInput _PlayerInput = null;

        // 방향버튼 눌렀을때 실행된 코루틴을 여기 저장
        private Coroutine _CoMoveCursorByKeyPress = null;
        // 방향버튼으로 커서를 움직일때 매프레임마다 큐를 체크하여 중단 메시지가 들어왔는지 확인
        private ConcurrentQueue<InputData> _queue = new ConcurrentQueue<InputData>();
        // 버튼다운 -> 버튼업을 거쳐서 클릭으로 확정할 대상
        // 같은 오브젝트 안에서 버튼다운-> 버튼업이 되어 정상적으로 오브젝트를 클릭한 것
        private ReadOnlyCollection<ICursorInteractable> _Focus_Components = null;

        private GraphicRaycaster graphicRaycaster = null;
        private EventSystem eventSystem = null;
        UnityEngine.InputSystem.InputAction Tap_Action = null;
        UnityEngine.InputSystem.InputAction Hold_Action = null;

        public bool Enable
        {
            get => _Enable;
            set
            {
                _Enable = value;
            }
        }
        bool _Enable = true;

        // 버튼이 눌린 상황인지를 확인하여 드래그 체크하는데 쓴다
        bool _Is_Pressed { get; set; } = false;
        bool _Is_Dragged { get; set; } = false;

        Vector2 _Drag_Start_Position { get; set; } = Vector2.zero;
        // 뎁스가 겹쳐있는 여러 오브젝트가 클릭가능하게 할 것인가
        bool _Is_Multiple_Input { get; set; } = false;
        protected override bool _Is_DontDestroyOnLoad { get { return true; } }

        public Camera RenderCamera { get; set; } = null;
        public RawImage RenderImage { get; set; } = null;

        int Input_Down_Hold_Reference
        {
            get => _Input_Down_Hold_Reference;
            set
            {
                //Debug.Log($"Input_Down_Hold_Reference : {value}");
                Debug.Assert(value >= 0, "레퍼런스 값은 마이너스가 되면 안됩니다.");
                _Input_Down_Hold_Reference = value;
            }
        }
        int _Input_Down_Hold_Reference = 0;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void SetUp()
        {
            _Addressables_Key = "Assets/AssetResources/Prefabs/UI/InputCanvas";
        }
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void CallInstance()
        {
            _ = InputCanvas.Instance;
        }

        protected override void OnAwake()
        {
            Hold_Action = _PlayerInput.actions.FindAction("Hold");
            Tap_Action = _PlayerInput.actions.FindAction("Tap");

            // GraphicRaycaster 및 EventSystem 참조를 가져옵니다.
            graphicRaycaster = GetComponent<GraphicRaycaster>();
            eventSystem = GetComponent<EventSystem>();
        }

        #region Input System Methods
        public void OnTapListen(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            //Debug.Log($"OnListenTap : {context.phase}");
            Input_Down_Hold_Reference += (context.phase == InputActionPhase.Started ? 1 : -1);

            if (context.phase == InputActionPhase.Performed)
            {
                // OnInputUp 처리
                if (Enable)
                {
                    var components = GetRayCastHittedCursorInteractable();
                    OnTap?.Invoke(_Cursor.Position, components);
                }
            }
        }

        /// <summary>
        /// 일반적인 커서 움직임으로 호출
        /// 마우스가 움직인다던지 터치패드 위치라던지
        /// </summary>
        /// <param name="context"></param>
        public void OnMoveListen(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            //Debug.LogWarning($"OnMove : {context.phase} : {context.ReadValue<Vector2>()}");

            //Vector2 delta;
            InputDevice device = InputSystem.GetDevice(context.control.device.name);
            if (device is Pointer/* pointer_device*/)
            {
                //delta = pointer_device.delta.ReadValue();
            }
            else
            {
                Debug.Assert(false, $"지원되는 디바이스가 아닙니다. : {device.GetType()}");
                return;
            }

            Vector2 pos = context.ReadValue<Vector2>();
            if (pos != Vector2.zero)
            {
                _Cursor.Position = pos;
            }

            CheckDragAndExecute(/*delta*/);
        }

        /// <summary>
        /// 키를 눌러서 커서를 이동시에 사용하는 메소드
        /// 유니티 Input System 버그가 있어서 별도로 구현
        /// </summary>
        /// <param name="context"></param>
        public void OnMoveByKeyPressListen(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            _queue.Enqueue(new InputData(context.control.device.name, context.phase, context.ReadValue<Vector2>()));
            if (_CoMoveCursorByKeyPress == null)
            {
                while (_queue.TryPeek(out InputData front_data))
                {
                    if (front_data.Phase == InputActionPhase.Started)
                    {
                        break;
                    }
                    else
                    {
                        _queue.TryDequeue(out front_data);
                    }
                }
                _CoMoveCursorByKeyPress = StartCoroutine(CoMoveCursorByKeyPress(_queue));
            }

            CheckDragAndExecute();
        }

        /// <summary>
        /// 터치 및 결정 버튼 눌렀을때 호출됩니다
        /// </summary>
        /// <param name="context"></param>
        public void OnPressListen(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                Input_Down_Hold_Reference += context.ReadValueAsButton() ? 1 : -1;
            }

            InputDevice device = InputSystem.GetDevice(context.control.device.name);
            switch (device)
            {
                case Mouse mouse:
                    //Debug.Log($"{context.phase} : mouse.position.value : {mouse.position.value}");
                    break;
                case Touchscreen touchscreen:
                    for (int i = 0; i < 2; i++)
                    {
                        Debug.Log($"{context.phase} : touchscreen.touches[{i}] : {touchscreen.touches[i].position.value}");
                    }
                    break;
            }

            ICursorInteractable[] components;
            int cnt;
            if (context.phase == InputActionPhase.Performed)
            {
                if (context.ReadValueAsButton())
                {
                    _Cursor.Show();

                    // OnInputDown 처리
                    if (Enable)
                    {
                        components = GetRayCastHittedCursorInteractable();
                        cnt = components.Length;
                        for (int i = 0; i < cnt; ++i)
                        {
                            ICursorInteractable iCursor = components[i];
                            iCursor?.OnInputDown(_Cursor.Position);
                        }
                        _Focus_Components = new ReadOnlyCollection<ICursorInteractable>(components);

                        // OnInputDown 처리
                        OnInputDown?.Invoke(_Cursor.Position, _Focus_Components);
                    }
                    _Is_Pressed = context.ReadValueAsButton();
                }
                else
                {
                    _Is_Pressed = context.ReadValueAsButton();
                    // OnInputUp
                    if (Enable)
                    {
                        // OnDrag 처리
                        if (_Is_Dragged)
                        {
                            OnDrag?.Invoke(InputActionPhase.Canceled, _Cursor.Delta, _Cursor.Position - _Drag_Start_Position, _Cursor.Position);
                        }

                        components = GetRayCastHittedCursorInteractable();
                        OnInputUp?.Invoke(_Cursor.Position, new ReadOnlyCollection<ICursorInteractable>(components));

                        // OnInputUp 처리
                        cnt = components.Length;
                        for (int i = 0; i < cnt; ++i)
                        {
                            ICursorInteractable iCursor = components[i];
                            iCursor?.OnInputUp(_Cursor.Position);
                        }
                    }

                    _Drag_Start_Position = Vector2.zero;
                    Hold_Action.Enable();
                    Tap_Action.Enable();
                    _Is_Dragged = false;

                    if (Input_Down_Hold_Reference <= 0)
                    {
                        _Focus_Components = null;
                    }

                    _Cursor.Hide();
                }
            }
        }

        public void OnHoldListen(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (Enable)
            {
                var components = GetRayCastHittedCursorInteractable();
                OnLongTap?.Invoke(context.phase, _Cursor.Position, components);
            }
        }
        #endregion

        #region Methods
        List<ICursorInteractable> GetUIInteractablesAtPosition(Vector2 position)
        {
            PointerEventData pointerData = new PointerEventData(eventSystem) { position = position };
            List<RaycastResult> results = new List<RaycastResult>();
            graphicRaycaster.Raycast(pointerData, results);

            List<ICursorInteractable> interactables = new List<ICursorInteractable>();
            foreach (RaycastResult result in results)
            {
                ICursorInteractable interactable = result.gameObject.GetComponent<ICursorInteractable>();
                if (interactable != null)
                {
                    interactables.Add(interactable);
                }
            }
            return interactables;
        }

        /// <summary>
        /// 커서의 위치에 ICursor 컴포넌트들이 있는지 확인해서 가져옵니다
        /// </summary>
        /// <returns>ICursorInteractable Array</returns>
        ICursorInteractable[] GetRayCastHittedCursorInteractable()
        {
            // UI 상에서 ICursorInteractable 객체들을 찾습니다.
            List<ICursorInteractable> uiInteractables = GetUIInteractablesAtPosition(_Cursor.Position);
            if (uiInteractables.Count > 0)
            {
                // UI 요소가 터치되었다면, 해당 요소들을 반환합니다.
                if (!_Is_Multiple_Input)
                {
                    return new ICursorInteractable[] { uiInteractables[0] };
                }

                return uiInteractables.ToArray();
            }

            RaycastHit2D[] hits = null;

            Vector3 mpos = _Cursor.Position;
            mpos.z = -Camera.main.transform.position.z;
            mpos = Camera.main.ScreenToWorldPoint(mpos);
            hits = Physics2D.RaycastAll(mpos, Vector2.zero);

            int hit_cnt = hits.Length;
            List<ICursorInteractable> components = new List<ICursorInteractable>();
            for (int i = 0; i < hit_cnt; i++)
            {
                // 여러개 감지해야 할 경우도 있어서 List에 담았다가 Array로 돌려줍니다
                var hit = hits[i];
                var b = hit.collider.gameObject.GetComponent<ICursorInteractable>();
                if (b != null)
                {
                    if (!_Is_Multiple_Input)
                    {
                        return new ICursorInteractable[] { b };
                    }

                    components.Add(b);
                }
            }

            return components.ToArray();
        }

        /// <summary>
        /// 키로 움직일때 실행하는 코루틴
        /// 매 프레임마다 누르던 키를 땠는지 다른키를 눌렀는지 확인합니다
        /// </summary>
        /// <param name="queue">입력데이터 큐</param>
        /// <returns></returns>
        IEnumerator CoMoveCursorByKeyPress(ConcurrentQueue<InputData> queue)
        {
            InputData data = null;
            if (!queue.TryPeek(out data))
            {
                _CoMoveCursorByKeyPress = null;
                yield break;
            }

            bool keep_running = true;
            while (queue.Count > 0 && keep_running)
            {
                if (!queue.TryDequeue(out data))
                {
                    continue;
                }

                switch (data.Phase)
                {
                    case UnityEngine.InputSystem.InputActionPhase.Started:
                        yield return ProcessPhase(queue, data, () =>
                        {
                            return new WaitUntil(() => queue.Count > 0);
                        });
                        break;
                    case UnityEngine.InputSystem.InputActionPhase.Performed:
                        yield return ProcessPhase(queue, data, () =>
                        {
                            if (!data.Move.Equals(Vector2.zero))
                            {
                                _Cursor.MoveByFrame(data.Move);
                            }
                            return null;
                        });
                        break;
                    case UnityEngine.InputSystem.InputActionPhase.Canceled:
                        if (!data.Move.Equals(Vector2.zero))
                        {
                            _Cursor.MoveByFrame(data.Move);
                        }
                        keep_running = false;
                        break;
                }
            }

            _CoMoveCursorByKeyPress = null;
        }

        /// <summary>
        /// 키 입력으로 움직일때 다른 데이터를 걸러주고 다음 데이터를 기다립니다.
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="last_data"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        IEnumerator ProcessPhase(ConcurrentQueue<InputData> queue, InputData last_data, Func<IEnumerator> action)
        {
            bool move_Next_Phase = false;
            while (!move_Next_Phase)
            {
                yield return action.Invoke();

                while (queue.TryPeek(out InputData d))
                {
                    // 같은 입력장치로만 들어온 데이터만 
                    if (d.Device_Name.Equals(last_data.Device_Name))
                    {
                        // 진행시키고
                        move_Next_Phase = true;
                        break;
                    }
                    else // 아니면
                    {
                        // 빼버립니다
                        queue.TryDequeue(out _);
                    }
                }
            }
        }

        /// <summary>
        /// 드래그 상태 처리
        /// </summary>
        void CheckDragAndExecute()
        {
            if (_Is_Pressed)
            {
                InputActionPhase phase = _Is_Dragged ? InputActionPhase.Performed : InputActionPhase.Started;
                if (phase == InputActionPhase.Started)
                {
                    _Is_Dragged = true;
                    _Drag_Start_Position = _Cursor.Position;
                    Hold_Action.Disable();
                    Tap_Action.Disable();
                }
                else
                {
                    var move_origin = _Cursor.Position - _Drag_Start_Position;
                    if (!move_origin.Equals(Vector2.zero))
                    {
                        var components = GetRayCastHittedCursorInteractable();
                        int cnt = components.Length;
                        for (int i = 0; i < cnt; i++)
                        {
                            var iCursor = components[i];
                            iCursor?.OnDrag(_Cursor.Position);
                            if (i == 0 && _Focus_Components != null && iCursor != _Focus_Components && Input_Down_Hold_Reference <= 0)
                            {
                                _Focus_Components = null;
                            }
                        }
                    }
                }
                if (Enable)
                {
                    OnDrag?.Invoke(phase, _Cursor.Delta, _Cursor.Position - _Drag_Start_Position, _Cursor.Position);
                }
            }
        }
        #endregion

        /// <summary>
        /// 간단한 입력 데이터
        /// </summary>
        public record InputData
        {
            public string Device_Name { get; }
            public UnityEngine.InputSystem.InputActionPhase Phase { get; }
            public Vector2 Move { get; }

            public InputData(string device_name, UnityEngine.InputSystem.InputActionPhase phase, Vector2 move)
                => (Device_Name, Phase, Move) = (device_name, phase, move);

            public override string ToString()
            {
                return $"{Device_Name} : {Phase} : {Move}";
            }
        }
    }
}
