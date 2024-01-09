using UnityEngine;
using UnityEngine.UI;

public class Blink : MonoBehaviour
{
    [SerializeField, Tooltip("Image")]
    Image Image_Rect;
    [SerializeField, Tooltip("Blink Duration")]
    float Blink_Duration = 1f;
    float Delta;
    bool Is_Blink_Stop;

    enum BLINK_STATE
    {
        FADE_OUT,
        FADE_IN
    }
    BLINK_STATE State = BLINK_STATE.FADE_OUT;
    // Start is called before the first frame update
    void Start()
    {
        Is_Blink_Stop = false;
        Delta = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Is_Blink_Stop)
        {
            return;
        }
        var color = Image_Rect.color;

        Delta += Time.deltaTime;
        if (State == BLINK_STATE.FADE_OUT)
        {
            color.a = 1f - (Delta / Blink_Duration);
            if (Delta > Blink_Duration)
            {
                Delta = 0f;
                State = BLINK_STATE.FADE_IN;
            }
        }
        else
        {
            color.a = Delta / Blink_Duration;
            if (Delta > Blink_Duration)
            {
                Delta = 0f;
                State = BLINK_STATE.FADE_OUT;
            }
        }
        Image_Rect.color = color;
    }

    public void SetStopBlink(bool is_stop)
    {
        Is_Blink_Stop = is_stop;
    }
}
