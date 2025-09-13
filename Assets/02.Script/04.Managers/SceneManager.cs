using System;
using UnityEngine;


[Serializable]
public class SceneInfo
{
    [SerializeField] Define.Type.Scene scene;
    [SerializeField] GameObject[] sceneObjs;

    public void SetActiveObjs(bool active) {
        for (int i = 0; i < sceneObjs.Length; i++) {
            var targetObj = sceneObjs[i];
            targetObj.SetActive(active);
        }
    }
}

public class SceneManager : MonoBehaviour, IInitializable
{
    public Define.Type.Scene CurrentScene => currentScene;
    public Define.Type.Scene PrevScene => prevScene;

    public bool IsInit => isInit;

    private bool isInit;
    private Define.Type.Scene currentScene = Define.Type.Scene.None;
    private Define.Type.Scene prevScene = Define.Type.Scene.None;

    [SerializeField] SceneInfo[] sceneInfos;

    /// <summary>
    /// Scene 전환 기능. scene list의 순서와 Define.Type.Scene의 순서 맞춰서 사용.
    /// </summary>
    /// <param name="sceneType"></param>
    public void ShowScene(Define.Type.Scene sceneType) {
        for (int sceneIndex = 0; sceneIndex < sceneInfos.Length; sceneIndex++) {
            SceneInfo targetScene = sceneInfos[sceneIndex];
            targetScene.SetActiveObjs((int)sceneType == sceneIndex);
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
