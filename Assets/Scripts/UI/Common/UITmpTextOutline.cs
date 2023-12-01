using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UITmpTextOutline : MonoBehaviour
{
    [SerializeField, Tooltip("Outline Color")]
    Color Outline_Color = Color.black;

    [SerializeField, Tooltip("Outline Width")]
    [Range(0f, 1f)]
    float Outline_Width;

    private void Start()
    {
        var text = GetComponent<TMP_Text>();
        if (text != null)
        {
            text.outlineColor = Outline_Color;
            text.outlineWidth = Outline_Width;
        }
    }
}
