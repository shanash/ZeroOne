using Febucci.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Damage_Text_Node : EffectBase
{
    [SerializeField, Tooltip("Damage Text")]
    TMP_Text Damage_Text;

    [SerializeField, Tooltip("Weak")]
    SpriteRenderer Weak_Icon;

    [SerializeField, Tooltip("Easing Effect List")]
    List<EffectEasingBase> Effect_Easing_List;

    readonly float VELOCITY = 1f;



}
