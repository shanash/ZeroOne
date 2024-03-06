using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleActivateObj : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject onStartObj;
    public GameObject toggleActivateObj;
    private bool toggleCheck = false;

    public void ToggleGameObject()
    {
        if (toggleCheck)
        {
            onStartObj.SetActive(true);
            toggleActivateObj.SetActive(false);
        }
        else
        {
            onStartObj.SetActive(false);
            toggleActivateObj.SetActive(true);
        }
        
    }

}
