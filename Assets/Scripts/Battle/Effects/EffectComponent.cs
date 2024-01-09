using UnityEngine;

public class EffectComponent : MonoBehaviour
{
    [SerializeField, Tooltip("투사체 타입")]
    PROJECTILE_TYPE Projectile_Type = PROJECTILE_TYPE.NONE;

    [SerializeField, Tooltip("Spawn Position Bone")]
    string Spawn_Position_Bone;

    [SerializeField, Tooltip("Target Position Bone")]
    string Target_Position_Bone;


}
