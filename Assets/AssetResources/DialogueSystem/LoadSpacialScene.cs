using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadSecialScene : MonoBehaviour
{
    // 로드할 씬의 이름
    public string sceneName;

    // 씬을 로드하는 함수
    public void LoadSPScene()
    {
        // SceneManager를 사용하여 씬을 로드합니다.
        SceneManager.LoadScene(sceneName);
    }
}
