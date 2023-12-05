using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMainCamera : MonoBehaviour
{
    [SerializeField, Tooltip("Virtual Cine Manager")]
    VirtualCineManager Cine_Manager;

    //private void LateUpdate()
    //{
    //    if (Cine_Manager != null)
    //    {
    //        var target_transform = Cine_Manager.ActiveVirtualCamera.transform;
    //        this.transform.position = target_transform.position;
    //        this.transform.rotation = target_transform.rotation;
    //    }
    //}
}
