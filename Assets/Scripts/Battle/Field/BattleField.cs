using UnityEngine;

public class BattleField : MonoBehaviour
{
    [SerializeField, Tooltip("Far BG Container")]
    Transform Far_BG_Container;
    [SerializeField, Tooltip("Unit Container")]
    Transform Unit_Container;
    [SerializeField, Tooltip("Effect Factory & Container")]
    EffectFactory Effect_Container;
    [SerializeField, Tooltip("Near Obj Container")]
    Transform Near_Obj_Container;


    public Transform GetFarBGContainer() { return Far_BG_Container; }
    public Transform GetUnitContainer() { return Unit_Container; }
    public EffectFactory GetEffectFactory() { return Effect_Container; }
    public Transform GetNearBGContainer() { return Near_Obj_Container; }



}
