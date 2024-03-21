using Febucci.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Damage_Text_Node : EffectBase
{
    [SerializeField, Tooltip("Damage Text")]
    TMP_Text Damage_Text;

    [SerializeField, Tooltip("Type Writer")]
    TypewriterByCharacter Writer;

    [SerializeField, Tooltip("Weak")]
    SpriteRenderer Weak_Icon;

    readonly float VELOCITY = 1f;

}
