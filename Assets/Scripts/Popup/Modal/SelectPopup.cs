using FluffyDuck.UI;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class SelectPopup : MessagePopup
{
    [SerializeField, Tooltip("버튼 양산을 위한 원본")]
    UIButtonBase Button_Src = null;

    Action<int> Listener = null;
    protected override bool Initialize(object[] data)
    {
        if (!base.Initialize(data) || data.Length < 4 || data[data.Length-1] is not Action<int>)
        {
            return false;
        }

        Listener = data[data.Length - 1] as Action<int>;

        Button_Src.gameObject.SetActive(true);
        for (int i = 2; i < data.Length - 1; i++)
        {
            if (data[i] is string)
            {
                var button = Instantiate(Button_Src, Button_Src.transform.parent);
                button.gameObject.SetActive(true);
                button.name = $"{i-2}";

                var tmp_text = button.GetComponentInChildren<TMP_Text>();
                tmp_text.text = data[i] as string;

                button.AddClickListener(OnClickButton);
            }
        }

        return true;
    }

    public void OnClickButton(UIButtonBase btn)
    {
        Listener?.Invoke(int.Parse(btn.name));
    }
}
