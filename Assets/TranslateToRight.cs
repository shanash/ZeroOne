using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateToRight : MonoBehaviour
{
    public float speed = 3f;

// Update is called once per frame
    void Update()
    {
        // 오브젝트를 우측으로 이동
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
    
}
