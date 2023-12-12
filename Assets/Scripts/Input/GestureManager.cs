using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using UnityEngine;


public class GestureManager : Singleton<GestureManager>
{
    // 더블터치와 롱프레스를 감지하기 위한 시간 및 거리 임계값
    const float DOUBLETOUCH_THRESHOLD = 0.2f; // 더블터치 간 시간 간격
    const float TOUCH_DISTANCE_MAX = 10f; // 터치로 인정할 최대 거리
    const float LONGPRESS_THRESHOLD = 0.7f; // 롱프레스로 인정할 최소 시간
    const float DRAG_THRESHOLD = 0.01f; // 드래그로 인정할 최소 이동 거리의 제곱

    int Drag_Id = 0;
    bool IsDragging = false;

    ReadOnlyCollection<ICursorInteractable> TouchDown_Components;
    Coroutine Wait_For_Double_Touch = null;
    Vector2 Last_TouchPosition = Vector2.zero;

    public event Action<TOUCH_GESTURE_TYPE, ICursorInteractable, Vector2, int> OnGestureDetected;

    protected override void Init()
    {
        base.Init();
        InputCanvas.OnInputDown += HandleInputDown;
        InputCanvas.OnInputUp += HandleInputUp;
        InputCanvas.OnDrag += HandleDrag;
        InputCanvas.OnTap += HandleTap;
    }

    protected override void OnDispose()
    {
        base.OnDispose();
        InputCanvas.OnInputDown -= HandleInputDown;
        InputCanvas.OnInputUp -= HandleInputUp;
        InputCanvas.OnDrag -= HandleDrag;
        InputCanvas.OnTap -= HandleTap;

    }

    /// <summary>
    /// 터치 다운 이벤트 핸들러
    /// </summary>
    /// <param name="position">2D 위치</param>
    /// <param name="components">터치 다운 이벤트 위치에 있는 ICursorInteractable</param>
    private void HandleInputDown(Vector2 position, ICollection<ICursorInteractable> components)
    {
        // 터치 다운 시점의 상호작용 가능한 객체들 저장
        TouchDown_Components = new ReadOnlyCollection<ICursorInteractable>((IList<ICursorInteractable>)components);

        // 터치 다운 이벤트 발생
        foreach (var component in components)
        {
            OnGestureDetected?.Invoke(TOUCH_GESTURE_TYPE.DOWN, component, position, 0);
        }
    }

    /// <summary>
    /// 터치 업 이벤트 핸들러
    /// </summary>
    /// <param name="position">2D 위치</param>
    /// <param name="components">터치 업 이벤트 위치에 있는 ICursorInteractable</param>
    private void HandleInputUp(Vector2 position, ICollection<ICursorInteractable> components)
    {
        // 터치 업 이벤트 발생
        foreach (var component in components)
        {
            OnGestureDetected?.Invoke(TOUCH_GESTURE_TYPE.UP, component, position, 0);
        }
        IsDragging = false;
    }

    private void HandleTap(Vector2 position, ICollection<ICursorInteractable> components)
    {
        bool isWithinDistance = Vector2.Distance(Last_TouchPosition, position) < TOUCH_DISTANCE_MAX;

        if (Wait_For_Double_Touch == null)
        {
            Wait_For_Double_Touch = CoroutineManager.Instance.StartCoroutine(WaitForPossibleDoubleTouch(components.ToArray()));
        }
        else
        {
            CoroutineManager.Instance.StopCoroutine(Wait_For_Double_Touch);
            if (isWithinDistance)
            {
                foreach (var component in components)
                {
                    OnGestureDetected?.Invoke(TOUCH_GESTURE_TYPE.DOUBLE_TOUCH, component, Vector2.zero, 0);
                }
            }
            else
            {
                Wait_For_Double_Touch = CoroutineManager.Instance.StartCoroutine(WaitForPossibleDoubleTouch(components.ToArray()));
            }
        }

        Last_TouchPosition = position;
    }

    /// <summary>
    /// 드래그 이벤트 핸들러
    /// </summary>
    /// <param name="phase">현재 액션 페이즈</param>
    /// <param name="dragDelta">드래그 스타트 지점에서 현재까지의 2D 벡터</param>
    /// <param name="position">현재 위치</param>
    private void HandleDrag(InputCanvas.InputActionPhase phase, Vector2 dragDelta, Vector2 position)
    {
        // 드래그 시작 감지 및 드래그 중 상태 업데이트
        if (!IsDragging && dragDelta.sqrMagnitude > DRAG_THRESHOLD)
        {
            Drag_Id ++;
            IsDragging = true;
        }

        // 드래그 중 이벤트 발생
        if (IsDragging)
        {
            int cnt = TouchDown_Components.Count;
            for (int i = 0; i < cnt; ++i)
            {
                OnGestureDetected?.Invoke(TOUCH_GESTURE_TYPE.DRAG, TouchDown_Components[i], dragDelta, Drag_Id);
            }
        }
    }

    IEnumerator WaitForPossibleDoubleTouch(ICursorInteractable[] matched_components)
    {
        yield return new WaitForSeconds(DOUBLETOUCH_THRESHOLD);

        foreach (var component in matched_components)
        {
            OnGestureDetected?.Invoke(TOUCH_GESTURE_TYPE.TOUCH, component, Vector2.zero, 0);
        }

        Wait_For_Double_Touch = null;
    }
}
