using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using UnityEngine;


public class GestureManager : Singleton<GestureManager>
{
    // 더블터치와 롱프레스를 감지하기 위한 시간 및 거리 임계값
    const float DOUBLETOUCH_THRESHOLD = 0.2f; // 더블터치 간 시간 간격
    const float TOUCH_DISTANCE_MAX = 10f; // 터치로 인정할 최대 거리
    const float LONGPRESS_THRESHOLD = 0.7f; // 롱프레스로 인정할 최소 시간
    const float DRAG_THRESHOLD = 0.01f; // 드래그로 인정할 최소 이동 거리의 제곱

    int Drag_Id = 0;

    float Last_TouchTime = 0f;
    float TouchDown_Time = 0f;
    bool IsDragging = false;
    bool waitingForDoubleTouch = false;

    ReadOnlyCollection<ICursorInteractable> TouchDown_Components;

    Vector2 Last_TouchPosition = Vector2.zero;

    public event Action<TOUCH_GESTURE_TYPE, ICursorInteractable, Vector2, int> OnGestureDetected;

    protected override void Init()
    {
        base.Init();
        InputCanvas.OnInputDown += HandleInputDown;
        InputCanvas.OnInputUp += HandleInputUp;
        InputCanvas.OnDrag += HandleDrag;
    }

    protected override void OnDispose()
    {
        base.OnDispose();
        InputCanvas.OnInputDown -= HandleInputDown;
        InputCanvas.OnInputUp -= HandleInputUp;
        InputCanvas.OnDrag -= HandleDrag;
    }

    /// <summary>
    /// 터치 다운 이벤트 핸들러
    /// </summary>
    /// <param name="position">2D 위치</param>
    /// <param name="components">터치 다운 이벤트 위치에 있는 ICursorInteractable</param>
    private void HandleInputDown(Vector2 position, ICollection<ICursorInteractable> components)
    {
        // 터치 다운 시간과 위치 기록
        TouchDown_Time = Time.time;

        bool isWithinDistance = Vector2.Distance(Last_TouchPosition, position) < TOUCH_DISTANCE_MAX;
        
        // 멀리 터치하면 마지막 터치타임을 초기화하여 더블터치가 안되도록 합니다.
        if (!isWithinDistance)
        {
            Last_TouchTime = 0;
        }

        Last_TouchPosition = position;
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
        // 터치 지속 시간 및 마지막 터치 이후 시간 계산
        float touchDuration = Time.time - TouchDown_Time;
        float timeSinceLastTouch = Time.time - Last_TouchTime;
        bool isWithinDistance = Vector2.Distance(Last_TouchPosition, position) < TOUCH_DISTANCE_MAX;

        // 터치 다운 시점과 터치 업 시점에서 겹치는 상호작용 가능한 객체들 추출
        List<ICursorInteractable> matchedComponents = new List<ICursorInteractable>();
        foreach (var component in components)
        {
            if (TouchDown_Components.Contains(component))
            {
                matchedComponents.Add(component);
            }
        }

        int cnt = matchedComponents.Count;

        // 더블 터치 또는 롱 프레스 감지 및 해당 이벤트 발생
        if (waitingForDoubleTouch && isWithinDistance)
        {
            waitingForDoubleTouch = false;

            for (int i = 0; i < cnt; ++i)
            {
                OnGestureDetected?.Invoke(TOUCH_GESTURE_TYPE.DOUBLE_TOUCH, matchedComponents[i], position, 0);
            }
        }
        else
        {
            if (isWithinDistance && touchDuration < LONGPRESS_THRESHOLD)
            {
                if (!waitingForDoubleTouch)
                {
                    waitingForDoubleTouch = true;
                    CoroutineManager.Instance.StartCoroutine(WaitForPossibleDoubleTouch(matchedComponents, position));
                }

                if (cnt != 0)
                {
                    Last_TouchTime = Time.time;
                }
            }
        }

        // 터치 업 이벤트 발생
        foreach (var component in components)
        {
            OnGestureDetected?.Invoke(TOUCH_GESTURE_TYPE.UP, component, position, 0);
        }
        
        Last_TouchPosition = position;
        IsDragging = false;
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

    IEnumerator WaitForPossibleDoubleTouch(List<ICursorInteractable> matched_components, Vector2 position)
    {
        yield return new WaitForSeconds(DOUBLETOUCH_THRESHOLD);

        if (waitingForDoubleTouch)  // 두 번째 터치가 발생하지 않은 경우
        {
            waitingForDoubleTouch = false;
            foreach (var component in matched_components)
            {
                OnGestureDetected?.Invoke(TOUCH_GESTURE_TYPE.TOUCH, component, position, 0);
            }
        }
    }
}
