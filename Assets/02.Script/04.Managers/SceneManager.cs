using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public Define.Type.Scene CurrentScene => currentScene;

    private Define.Type.Scene currentScene;

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
    }
}
