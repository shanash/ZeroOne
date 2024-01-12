using UnityEngine;

public class BattleField : MonoBehaviour
{
    [SerializeField, Tooltip("Far BG Container")]
    Transform Far_BG_Container;
    [SerializeField, Tooltip("Ground BG Container")]
    Transform Ground_BG_Container;
    [SerializeField, Tooltip("Unit_Back Container")]
    Transform Unit_Back_Container;
    [SerializeField, Tooltip("Back Fade BG")]
    SpriteRenderer Back_Fade_BG;
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

    public Transform GetGroundBGContainer() { return Ground_BG_Container; }
    public SpriteRenderer GetUnitBackFaceBG() { return Back_Fade_BG; }



}
