using UnityEngine;

public class UnitBillBoard : MonoBehaviour
{
    Camera Main_Camera;

    private void Start()
    {
        CheckMainCamera();
    }

    void CheckMainCamera()
    {
        if (Main_Camera == null)
        {
            Main_Camera = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Main_Camera.transform.rotation.eulerAngles.x;

        this.transform.rotation = Quaternion.Euler(angle, 0, 0);
    }
}
