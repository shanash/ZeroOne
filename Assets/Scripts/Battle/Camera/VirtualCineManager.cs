using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCineManager : MonoBehaviour, IEventTrigger
{
    const int RELEASE_PRIORITY = 9;
    const int FOCUS_PRIORITY = 100;

    readonly Vector3 ORIGIN_POS = Vector3.zero;

    [SerializeField, Tooltip("Camera Brain")]
    CinemachineBrain Brain_Cam;
    
    [SerializeField, Tooltip("Stage Camera")]
    CinemachineVirtualCamera Stage_Cam;
    [SerializeField, Tooltip("Character Camera")]
    CinemachineVirtualCamera Character_Cam;

    [SerializeField, Tooltip("Target Group")]
    CinemachineTargetGroup Target_Group;
    [SerializeField, Tooltip("Target Group Camera")]
    CinemachineVirtualCamera Target_Group_Cam;

   
    public CinemachineVirtualCamera ActiveVirtualCamera { get { return Brain_Cam.ActiveVirtualCamera as CinemachineVirtualCamera; } }

    void Start()
    {
        ResetPos();
    }

    /// <summary>
    /// VirtualCineManager 위치 리셋
    /// </summary>
    public void ResetPos()
    {
        transform.position = ORIGIN_POS;
    }

    public void FocusCharacter(GameObject unit)
    {
        Brain_Cam.m_DefaultBlend.m_Time = 0.5f;
        Character_Cam.Follow = unit.transform;
        Character_Cam.m_Priority = FOCUS_PRIORITY;
    }

    public void ReleaseCharacterCam()
    {
        Character_Cam.Follow = null;
        Character_Cam.m_Priority = RELEASE_PRIORITY;
    }

    public void SetAngleView(float angle)
    {
        Brain_Cam.m_DefaultBlend.m_Time = 1f;
        Vector3 ang = Stage_Cam.transform.rotation.eulerAngles;
        ang.x = angle;

        var pos = Stage_Cam.transform.position;
        if (angle == 60f)
        {
            pos.y = 70f;
            pos.z = -41f;
        }
        else if(angle == 45f)
        {
            pos.y = 59f;
            pos.z = -60f;
        }
        else if(angle == 30f)
        {
            pos.y = 43f;
            pos.z = -75f;
        }
        else if (angle == 20f)
        {
            pos.y = 32f;
            pos.z = -42f;
        }

        StartCoroutine(StartAngleUpdate(ang, pos));
    }

    IEnumerator StartAngleUpdate(Vector3 angle, Vector3 pos)
    {
        float duration = 1f;
        float delta = 0f;
        while (delta < duration)
        {
            delta += Time.deltaTime;
            Stage_Cam.transform.rotation = Quaternion.Euler(Vector3.Lerp(Stage_Cam.transform.rotation.eulerAngles, angle, delta));
            Stage_Cam.transform.position = Vector3.Lerp(Stage_Cam.transform.position, pos, delta);
            yield return null;
            if (delta > duration)
            {
                break;
            }
        }
    }

    public void AddTarget(Transform target)
    {
        Target_Group.AddMember(target, 1, 10);
    }

    public void ClearTarget()
    {
        int len = Target_Group.m_Targets.Length;
        for (int i = len - 1; i >= 0; i--)
        {
            var t = Target_Group.m_Targets[i].target;
            Target_Group.RemoveMember(t);
        }
    }

    public void SetGroupView()
    {
        Brain_Cam.m_DefaultBlend.m_Time = 1f;
        Target_Group_Cam.Priority = FOCUS_PRIORITY;
    }
    
    public void ReleaseGroupView()
    {
        Target_Group_Cam.Priority = RELEASE_PRIORITY;
        ClearTarget();
    }

    /// <summary>
    /// VirtualCineManager를 흔듭니다.
    /// </summary>
    /// <param name="offset"></param>
    public void ShakeFromOriginalPositionBy(Vector3 offset)
    {
        /// 원래는 ActiveVirtualCamera를 흔들까 싶었는데 원래 위치 저장하고 다시 설정하고 하는게 애매하기도하고
        /// 흔들던 와중에 ActiveVirtualCamera가 변경되면 또 설정해줘야 하는것도 많고 해서
        /// VirtualCineManager가 기왕에 ActiveVirtualCamera들을 다 가지고 있으니
        /// VirtualCineManager의 원래 위치를 선언하고 그 기준으로 흔드는게 편하겠다 싶었던

        if (ActiveVirtualCamera == null)
            return;
        Vector3 vec = ActiveVirtualCamera.transform.TransformDirection(offset); // ActiveVirtualCamera의 local 방향은 다를 수 있으니 그 기준으로 틀어줍니다
        transform.position = ORIGIN_POS + vec;
    }

    public void TriggerEventListener(string trigger_id, EventTriggerValue evt_val)
    {
        Debug.Log($"trigger name : {trigger_id}, int [{evt_val.IntValue}], double [{evt_val.DoubleValue}], float [{evt_val.FloatValue}], string [{evt_val.StrValue}]");
    }
}
