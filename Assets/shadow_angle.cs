using UnityEngine;

public class shadow_angle : MonoBehaviour
{
    void Start()
    {
        transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
    }
}
