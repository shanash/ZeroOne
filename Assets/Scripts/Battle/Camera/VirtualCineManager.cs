using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualCineManager : MonoBehaviour, IEventTrigger
{
    public enum CAMERA_TYPE
    {
        NONE = 0,
        STAGE_CAM,
        LANDSCAPE_CAM,
        FREE_CAM,
        CHARACTER_CAM,
        TARGET_GROUP_CAM,
        ACTIVE_TARGET_GROUP_CAM,
    }
    [Serializable]
    public struct VIRTUAL_CAMERA_DATA
    {
        public CAMERA_TYPE Camera_Type;
        public CinemachineVirtualCamera Camera;
        public CinemachineTargetGroup Target_Group;

        public Vector3 Init_Transform_Position;
        public Vector3 Init_Transform_Rotation;
        public Vector3 Init_Follow_Offset;
        public float Init_FOV;
    }

    const int RELEASE_PRIORITY = 9;
    const int FOCUS_PRIORITY = 100;

    readonly Vector3 ORIGIN_POS = Vector3.zero;

    [SerializeField, Tooltip("Camera Brain")]
    CinemachineBrain Brain_Cam;

    [SerializeField, Tooltip("Virtual Camera List")]
    List<VIRTUAL_CAMERA_DATA> Virtual_Camera_List;
    

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


    public CinemachineBrain GetBrainCam() { return Brain_Cam; }

    VIRTUAL_CAMERA_DATA FindCameraData(CAMERA_TYPE ctype)
    {
        return Virtual_Camera_List.Find(x => x.Camera_Type == ctype);
    }

    VIRTUAL_CAMERA_DATA FindCameraData(CinemachineVirtualCamera cam)
    {
        return Virtual_Camera_List.Find(x => object.ReferenceEquals(x.Camera, cam));
    }

    CinemachineVirtualCamera FindCamera(CAMERA_TYPE ctype)
    {
        if (Virtual_Camera_List.Exists(x => x.Camera_Type == ctype))
        {
            return FindCameraData(ctype).Camera;
        }
        return null;
    }

    CinemachineTargetGroup FindTargetGroup(CAMERA_TYPE ctype)
    {
        if (Virtual_Camera_List.Exists(x => x.Camera_Type == ctype))
        {
            return FindCameraData(ctype).Target_Group;
        }
        return null;
    }




    public CinemachineVirtualCamera GetStageCamera() 
    {
        return FindCamera(CAMERA_TYPE.STAGE_CAM);
    }
    public CinemachineVirtualCamera GetLandscapeCamera() 
    {
        return FindCamera(CAMERA_TYPE.LANDSCAPE_CAM);
    }
    public CinemachineVirtualCamera GetFreeCamera() { return FindCamera(CAMERA_TYPE.FREE_CAM); }
    public CinemachineVirtualCamera GetCharacterCamera() { return FindCamera(CAMERA_TYPE.CHARACTER_CAM); }
    public CinemachineVirtualCamera GetActiveTargetGroupCamera() { return FindCamera(CAMERA_TYPE.ACTIVE_TARGET_GROUP_CAM); }
    public CinemachineTargetGroup GetActiveTargetGroup() { return FindTargetGroup(CAMERA_TYPE.ACTIVE_TARGET_GROUP_CAM); }

    public CinemachineVirtualCamera GetTargetGroupCamera() { return FindCamera(CAMERA_TYPE.TARGET_GROUP_CAM); }
    public CinemachineTargetGroup GetTargetGroup() { return FindTargetGroup(CAMERA_TYPE.TARGET_GROUP_CAM); }

    /// <summary>
    /// Follow Offset / FOV 등 카메라 연출 전 초기화가 필요한 부분 초기화
    /// </summary>
    public void ResetVirtualCameraEtcVars()
    {
        int cnt = Virtual_Camera_List.Count;
        for (int i = 0; i < cnt; i++)
        {
            var cam_data = Virtual_Camera_List[i];

            //  init trans position
            cam_data.Camera.transform.position = cam_data.Init_Transform_Position;

            //  init trans rotation
            cam_data.Camera.transform.rotation = Quaternion.Euler(cam_data.Init_Transform_Rotation);

            //  follow Offset
            CinemachineTransposer transposer = cam_data.Camera.GetCinemachineComponent<CinemachineTransposer>();
            if (transposer != null)
            {
                transposer.m_FollowOffset = cam_data.Init_Follow_Offset;
            }

            //  init fov
            var lens_setting = cam_data.Camera.m_Lens;
            lens_setting.OrthographicSize = cam_data.Init_FOV;
            cam_data.Camera.m_Lens = lens_setting;
        }


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
        //Vector3 vec = ActiveVirtualCamera.transform.TransformDirection(offset); // ActiveVirtualCamera의 local 방향은 다를 수 있으니 그 기준으로 틀어줍니다
        //transform.position = ORIGIN_POS + vec;

        var cam_data = FindCameraData(ActiveVirtualCamera);
        
        CinemachineTransposer transposer = ActiveVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        Vector3 vec = transposer.transform.TransformDirection(offset);
        if (transposer != null)
        {
            transposer.m_FollowOffset = cam_data.Init_Follow_Offset + vec;
        }
    }

    public void TriggerEventListener(string trigger_id, EventTriggerValue evt_val)
    {
        Debug.Log($"trigger name : {trigger_id}, int [{evt_val.IntValue}], double [{evt_val.DoubleValue}], float [{evt_val.FloatValue}], string [{evt_val.StrValue}]");
    }
}
