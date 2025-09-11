using UnityEngine;

public class SceneManager : MonoBehaviour, IInitializable
{
    public Define.Type.Scene CurrentScene => currentScene;
    public Define.Type.Scene PrevScene => prevScene;

    public bool IsInit => isInit;

    private bool isInit;
    private Define.Type.Scene currentScene = Define.Type.Scene.None;
    private Define.Type.Scene prevScene = Define.Type.Scene.None;

    [SerializeField] GameObject[] scenes;

    /// <summary>
    /// Scene 전환 기능. scene list의 순서와 Define.Type.Scene의 순서 맞춰서 사용.
    /// </summary>
    /// <param name="sceneType"></param>
    public void ShowScene(Define.Type.Scene sceneType) {
        for (int sceneIndex = 0; sceneIndex < scenes.Length; sceneIndex++) {
            GameObject targetScene = scenes[sceneIndex];
            targetScene.SetActive((int)sceneType == sceneIndex);
        }

        prevScene = currentScene;
        currentScene = sceneType;
        Debug.LogAssertion($"<color=yellow>{prevScene}->{currentScene}으로 이동</color>");
    }

    public void Initialize() {
        currentScene = Define.Type.Scene.None;
        prevScene = Define.Type.Scene.None;
    }
}
