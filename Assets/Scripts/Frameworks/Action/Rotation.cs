using UnityEngine;

namespace FluffyDuck.Util
{
    public class Rotation : MonoBehaviour
    {
        [SerializeField]
        float rotation_velocity = -180.0f;
        Vector3 rotationEuler;

        bool Is_Rotation_Stop;

        private void Awake()
        {
            Is_Rotation_Stop = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (Is_Rotation_Stop)
            {
                return;
            }
            rotationEuler += Vector3.forward * rotation_velocity * Time.deltaTime;
            gameObject.transform.rotation = Quaternion.Euler(rotationEuler);
        }

        public void SetStopRotation(bool is_stop)
        {
            Is_Rotation_Stop = is_stop;
        }
    }

}
