using FluffyDuck.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    [SerializeField, Tooltip("Touch to Start Ease Alpha")]
    UIEaseCanvasGroupAlpha Touch_To_Start_Ease_Alpha;
    [SerializeField, Tooltip("Touch to Start Ease Slide")]
    UIEaseSlide Touch_To_Start_Ease_Slide;


    bool Is_Enable_Touch;

    // Start is called before the first frame update
    void Start()
    {
        Is_Enable_Touch = false;
        StartCoroutine(ShowTouchToStart(1f));
    }

    IEnumerator ShowTouchToStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        Touch_To_Start_Ease_Alpha.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN, () =>
        {
            Is_Enable_Touch = true;
        });
        Touch_To_Start_Ease_Slide.StartMove(UIEaseBase.MOVE_TYPE.MOVE_IN);
    }

    void OnTouchDown(Vector2 position, ICollection<ICursorInteractable> components)
    {
        if (Is_Enable_Touch)
        {
            SceneManager.LoadScene(GameDefine.SCENE_LOAD);
        }
    }

    private void OnEnable()
    {
        InputCanvas.OnInputDown += OnTouchDown;
    }

    private void OnDisable()
    {
        InputCanvas.OnInputDown -= OnTouchDown;
    }


}
