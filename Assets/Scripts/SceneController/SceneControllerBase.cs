using System.Collections;
using UnityEngine;

public class SceneControllerBase : MonoBehaviour
{
    public virtual void OnClick(UIButtonBase button)
    {
        AudioManager.Instance.PlayFX("Assets/AssetResources/Audio/FX/click_01");
    }

    #region Monobehaviour Callback
    private void Awake()
    {
        Initialize();
    }
    #endregion

    #region Private Method
    protected virtual void Initialize()
    {
        SCManager.Instance.SetCurrent(this);
    }
    #endregion
}
