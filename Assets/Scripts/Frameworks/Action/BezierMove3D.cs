using FluffyDuck.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FluffyDuck.Util
{
    /// <summary>
    /// 베지어 곡선 이동(2D)
    /// </summary>
    public class BezierMove3D : MonoBehaviour
    {
        /// <summary>
        /// 최소 필요한 3개의 2D 벡터 좌표 / 시작 좌표 => 시작점 근처의 커브 포인트 => 종료지점 근처의 커브 포인트 => 종료 좌표
        /// </summary>
        Vector3[] Bezier_Points = new Vector3[4];
        /// <summary>
        /// 지속 시간
        /// </summary>
        float Duration;
        /// <summary>
        /// 소요 시간
        /// </summary>
        float Delta;
        /// <summary>
        /// 이동 여부
        /// </summary>
        bool Is_Move;
        /// <summary>
        /// 로컬 포지션 여부
        /// </summary>
        bool Is_Local;

        /// <summary>
        /// Pause 여부
        /// </summary>
        bool Is_Pause;

        Transform Target_Transform;

        System.Action<object> Move_End_Callback;

        public void Move(Vector3 start_pt, Transform target, float duration, float start_curve_dist, float end_curve_dist, System.Action<object> cb)
        {
            Target_Transform = target;
            Duration = duration;
            Delta = 0f;
            Move_End_Callback = cb;
            Bezier_Points[0] = start_pt;


            Bezier_Points[1] = CalcBezierPoints(start_pt, start_curve_dist);

            Bezier_Points[2] = CalcBezierPoints(Target_Transform.position, end_curve_dist);

            Bezier_Points[3] = Target_Transform.position;

            this.transform.position = start_pt;

            Is_Move = true;
            Is_Local = false;
        }

        public void Move(Vector3 start_pt, Vector3 end_pt, float duration, float start_curve_dist, float end_curve_dist, System.Action<object> cb)
        {
            Duration = duration;
            Delta = 0f;
            Move_End_Callback = cb;
            Bezier_Points[0] = start_pt;


            Bezier_Points[1] = CalcBezierPoints(start_pt, start_curve_dist);

            Bezier_Points[2] = CalcBezierPoints(end_pt, end_curve_dist);

            Bezier_Points[3] = end_pt;

            this.transform.position = start_pt;

            Is_Move = true;
            Is_Local = false;
        }

        public void MoveLocal(Vector3 start_pt, Vector3 end_pt, float duration, float start_curve_dist, float end_curve_dist, System.Action<object> cb)
        {
            Duration = duration;
            Delta = 0f;
            Move_End_Callback = cb;
            Bezier_Points[0] = start_pt;


            Bezier_Points[1] = CalcBezierPoints(start_pt, start_curve_dist);

            Bezier_Points[2] = CalcBezierPoints(end_pt, end_curve_dist);

            Bezier_Points[3] = end_pt;

            this.transform.localPosition = start_pt;

            Is_Move = true;
            Is_Local = true;
        }

        /// <summary>
        /// 이동 좌표를 미리 지정하고 이동
        /// </summary>
        /// <param name="pos_list"></param>
        /// <param name="duration"></param>
        /// <param name="cb"></param>
        public void Move(List<Vector3> pos_list, float duration, float distance, System.Action<object> cb)
        {
            Duration = duration;
            Delta = 0f;
            Move_End_Callback = cb;

            for (int i = 0; i < pos_list.Count; i++)
            {
                if (i == 0 || i == pos_list.Count - 1)
                {
                    Bezier_Points[i] = pos_list[i];
                }
                else
                {
                    Bezier_Points[i] = CalcBezierPoints(pos_list[i], distance);
                }
            }
            
            this.transform.position = Bezier_Points[0];

            Is_Move = true;
            Is_Local = false;
        }
        /// <summary>
        /// 이동 좌표를 미리 지정하고 이동(로컬 좌표)
        /// </summary>
        /// <param name="pos_list"></param>
        /// <param name="duration"></param>
        /// <param name="cb"></param>
        public void MoveLocal(List<Vector3> pos_list, float duration, float distance, System.Action<object> cb)
        {
            Duration = duration;
            Delta = 0f;
            Move_End_Callback = cb;

            for (int i = 0; i < pos_list.Count; i++)
            {
                if (i == 0 || i == pos_list.Count - 1)
                {
                    Bezier_Points[i] = pos_list[i];
                }
                else
                {
                    Bezier_Points[i] = CalcBezierPoints(pos_list[i], distance);
                }
            }

            this.transform.localPosition = Bezier_Points[0];

            Is_Move = true;
            Is_Local = true;
        }

        /// <summary>
        /// 커브 좌표를 임의로 지정해준다.
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="curve_dist"></param>
        /// <returns></returns>
        public Vector3 CalcBezierPoints(Vector3 pt, float curve_dist)
        {
            Vector3 result = pt;

            result.x = pt.x + curve_dist * Mathf.Cos(Random.Range(0, 360) * Mathf.Deg2Rad);
            result.y = pt.y + curve_dist * Mathf.Sin(Random.Range(0, 360) * Mathf.Deg2Rad);
            result.z = pt.z;

            return result;
        }

        private void Update()
        {
            if (Is_Move && !Is_Pause)
            {
                Delta += Time.deltaTime;
                if (Target_Transform != null)
                {
                    int len = Bezier_Points.Length;
                    if (len > 0)
                    {
                        Bezier_Points[len - 1] = Target_Transform.position;
                    }
                }

                if (Is_Local)
                {
                    this.transform.localPosition = CubicBezierCurve(Bezier_Points);
                }
                else
                {
                    this.transform.position = CubicBezierCurve(Bezier_Points);
                }

                if (Delta > Duration)
                {
                    Is_Move = false;
                    Move_End_Callback?.Invoke(this);
                }
            }
        }

        /// <summary>
        /// 시간에 따른 커브 곡선 계산
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        Vector3 CubicBezierCurve(Vector3[] points)
        {
            float t = Delta / Duration;
            return Mathf.Pow((1f - t), 3f) * points[0]
                + Mathf.Pow((1f - t), 2f) * 3f * t * points[1]
                + Mathf.Pow(t, 2f) * 3 * (1f - t) * points[2]
                + Mathf.Pow(t, 3f) * points[3];
        }

        public void SetPuase(bool pause)
        {
            Is_Pause = pause;
        }
    }

}
