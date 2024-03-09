using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FluffyDuck.Util;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.Debug.Log($"GameObjectUtils.GetScreenRect(GetComponent<RectTransform>(), {GameObjectUtils.GetScreenRect(GetComponent<RectTransform>())}");
        //Debug.Log($"GameObjectUtils.GetScreenRect(GetComponent<RectTransform>(), {GameObjectUtils.GetScreenRect.GetScreenRect(GetComponent<RectTransform>())}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
