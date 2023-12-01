using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchEffectNode : MonoBehaviour, IPoolableComponent
{
    System.Action<TouchEffectNode> End_Callback;

    bool Is_Action;
    bool Is_Dragging;
    float Delta;

    Vector3 Origin_Scale = Vector3.one; // 스폰시에 기존 스케일로 되돌리기 위해 참고하는 값

    void Awake()
    {
        Origin_Scale = transform.localScale;
    }

    void Update()
    {
        if (Is_Action)
        {
            if (!Is_Dragging)
            {
                Delta -= Time.deltaTime;
                if (Delta < 0f)
                {
                    Is_Action = false;
                    End_Callback?.Invoke(this);
                }
            }
        }
    }

    /// <summary>
    /// 파티클 시작
    /// </summary>
    /// <param name="cb"></param>
    public void StartParticle(System.Action<TouchEffectNode> cb)
    {
        Delta = .8f;
        Is_Action = true;
        Is_Dragging = false;
        End_Callback = cb;
    }

    /// <summary>
    /// 파티클 종료
    /// </summary>
    public void StopParticle()
    {
        Is_Action = false;
        Is_Dragging = false;
        Delta = -1f;
    }
    /// <summary>
    /// 드래그 상태 설정
    /// </summary>
    /// <param name="is_drag"></param>
    public void SetDragging(bool is_drag)
    {
        Is_Dragging = is_drag;
    }
    /// <summary>
    /// 액션 상태 반환
    /// </summary>
    /// <returns></returns>
    public bool IsAction()
    {
        return Is_Action;
    }

    public bool IsDragging()
    {
        return Is_Dragging;
    }

    public void Spawned()
    {
        transform.localScale = Origin_Scale;
    }
    
    public void Despawned()
    {
        Is_Action = false;
        Is_Dragging = false;
    }


}
