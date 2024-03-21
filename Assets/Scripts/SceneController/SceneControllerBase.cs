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
    /// <summary>
    /// SceneController에서 초기화 할것이 있다면 override해서 사용합니다.</br>
    /// SetCurrent를 하지 않으면
    /// </summary>
    protected virtual void Initialize()
    {
        Debug.Log("Call SetCurrent at Base");
        SCManager.Instance.SetCurrent(this);
    }
    #endregion
}
