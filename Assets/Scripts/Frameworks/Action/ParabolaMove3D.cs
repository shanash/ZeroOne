using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

namespace FluffyDuck.Util
{
    /// <summary>
    /// 포물선 이동(3D)<br/>
    /// 시작점, 최고 높은 지점, 종료점 3개의 좌표 필요
    /// </summary>
    public class ParabolaMove3D : MonoBehaviour
    {
        Vector3 Start_Position = Vector3.zero;
        Vector3 End_Position = Vector3.zero;
        float Height;

        float Duration;

        float Velocity;

        float Delta;

        bool Is_Move;
        bool Is_Pause;

        float Distance;

        System.Action<object> Move_End_Callback;

        public void Move(Vector3 start_pt, Vector3 end_pt, float parabola_height, float velocity, System.Action<object> cb)
        {
            this.Velocity = velocity;
            
            Start_Position = start_pt;
            End_Position = end_pt;
            Height = parabola_height;

            Delta = 0f;
            Move_End_Callback = cb;

            InitParabolaVars_V2();

            this.transform.position = start_pt;
            Is_Move = true;
            Is_Pause = false;

        }

        #region HowTo V2
        void InitParabolaVars_V2()
        {
            Distance = Vector3.Distance(Start_Position, End_Position);
            Duration = Distance / Velocity;
        }
        Vector3 CalcParabolaPos_V2()
        {
            var pos = this.transform.position;

            float t = Mathf.Clamp01(Delta / Duration);

            float x = Mathf.Lerp(Start_Position.x, End_Position.x, t);
            float y = Mathf.Lerp(Start_Position.y, End_Position.y, t) + CalcVerticalOffset(t);
            float z = Mathf.Lerp(Start_Position.z, End_Position.z, t);

            pos.x = x;
            pos.y = y;
            pos.z = z;

            return pos;
        }

        float CalcVerticalOffset(float t)
        {
            float y_offset = Height * 4f * t * (1f - t);
            return y_offset;
        }

        #endregion



        private void Update()
        {
            if (Is_Move && !Is_Pause)
            {
                Delta += Time.deltaTime;

                Vector3 pos = CalcParabolaPos_V2();
                this.transform.position = pos;

                if (Delta >= Duration)
                {
                    Is_Move = false;
                    this.transform.position = End_Position;
                    Move_End_Callback?.Invoke(this);
                }
            }
        }

        public void SetPuase(bool pause)
        {
            Is_Pause = pause;
        }


    }

}

