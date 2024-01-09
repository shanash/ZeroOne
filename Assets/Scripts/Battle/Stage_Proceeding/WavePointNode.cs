using UnityEngine;


public enum WAVE_POINT_TYPE
{
    NONE = 0,
    START_POINT,
    MID_POINT,
    BOSS_POINT,
}
public class WavePointNode : MonoBehaviour
{
    [SerializeField, Tooltip("Wave Point Type")]
    WAVE_POINT_TYPE Wave_Point_Type = WAVE_POINT_TYPE.NONE;


    public WAVE_POINT_TYPE GetWavePointType()
    {
        return Wave_Point_Type;
    }
}
