using UnityEngine;

public class ChildUnitBillBoard : MonoBehaviour
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
        int cnt = this.transform.childCount;

        for (int i = 0; i < cnt; i++)
        {
            Transform tran = this.transform.GetChild(i);
            float angle = Main_Camera.transform.rotation.eulerAngles.x;
            tran.rotation = Quaternion.Euler(angle, 0, 0);
        }
    }
}
