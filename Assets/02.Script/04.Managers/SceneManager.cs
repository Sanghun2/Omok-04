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
    /// Scene ��ȯ ���. scene list�� ������ Define.Type.Scene�� ���� ���缭 ���.
    /// </summary>
    /// <param name="sceneType"></param>
    public void ShowScene(Define.Type.Scene sceneType) {
        for (int sceneIndex = 0; sceneIndex < scenes.Length; sceneIndex++) {
            GameObject targetScene = scenes[sceneIndex];
            targetScene.SetActive((int)sceneType == sceneIndex);
        }
        currentScene = sceneType;
        Debug.LogAssertion($"<color=yellow>{currentScene}���� �̵�</color>");
    }
}
