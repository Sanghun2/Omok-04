using System;
using UnityEngine;


public class SceneManager : MonoBehaviour, IInitializable
{
    public Define.Type.Scene CurrentSceneType => _currentScene == null ? Define.Type.Scene.None : _currentScene.SceneType;
    public Scene CurrentScene => _currentScene;
    public Scene PrevScene => _prevScene;

    public bool IsInit => isInit;

    private bool isInit;
    private Scene _currentScene;
    private Scene _prevScene;

    [SerializeField] Scene[] scenes;

    /// <summary>
    /// Scene 전환 기능. scene list의 순서와 Define.Type.Scene의 순서 맞춰서 사용.
    /// </summary>
    /// <param name="activeSceneType"></param>
    public void ShowScene(Define.Type.Scene activeSceneType) {
        if (CurrentSceneType == activeSceneType) return;

        _prevScene = _currentScene;
        for (int sceneIndex = 0; sceneIndex < scenes.Length; sceneIndex++) {
            Scene targetScene = scenes[sceneIndex];
            if (targetScene.SceneType == activeSceneType) {
                targetScene.ActiveScene(true);
                _currentScene = targetScene;
            }
            else {
                targetScene.ActiveScene(false);
            }
        }

#if UNITY_EDITOR
        var prevSceneType = Define.Type.Scene.None;
        if (_prevScene != null) prevSceneType = PrevScene.SceneType;
        Debug.LogAssertion($"<color=yellow>{prevSceneType}->{_currentScene.SceneType}으로 이동</color>");
#endif
    }

    public void Initialize() {
        _currentScene = null;
        _prevScene = null;
    }
}
