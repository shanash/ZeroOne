using UnityEngine;

public class Angle_auto : MonoBehaviour
{

    [HideInInspector] public GameObject mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        // 시자깋 카메라 앵글 따라가도록
        mainCamera = GameObject.Find("Main Camera");
        transform.rotation = Quaternion.Euler(mainCamera.transform.rotation.eulerAngles.x, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // 카메라 앵글 변경시 따라가도록
        if (transform.rotation.x != mainCamera.transform.rotation.eulerAngles.x)
        {
            transform.rotation = Quaternion.Euler(mainCamera.transform.rotation.eulerAngles.x, 0, 0);
        }
    }
}
