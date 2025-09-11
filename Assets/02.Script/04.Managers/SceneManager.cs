using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public Define.Type.Scene CurrentScene => currentScene;
    public Define.Type.Scene PrevScene => prevScene;

    private Define.Type.Scene currentScene;
    private Define.Type.Scene prevScene;

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
        currentScene = sceneType;
        Debug.LogAssertion($"<color=yellow>{currentScene}으로 이동</color>");
    }
}
