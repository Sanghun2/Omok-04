using System;
using UnityEngine;

public class GameManager
{
    #region Flow Control

    public void GoToMainMenu() {
        ProcessSceneChange(Define.Type.Scene.MainMenu);
    }

    public void GoToLogIn() {
        ProcessSceneChange(Define.Type.Scene.LogIn);
    }

    private void InitCurrentScene(Scene currentScene) {
        currentScene.InitScene();
        Debug.LogAssertion($"{currentScene} init");
    }
    private void ReleasePrevScene(Scene prevScene) {
        prevScene?.ReleaseScene();

#if UNITY_EDITOR
        var sceneType = prevScene == null ? Define.Type.Scene.None : prevScene.SceneType;
        Debug.LogAssertion($"{sceneType} released");
#endif
    }
    
    private void ProcessSceneChange(Define.Type.Scene targetSceneType) {
        var prevScene = Managers.Scene.CurrentScene;
        ReleasePrevScene(prevScene);
        Managers.Scene.ShowScene(targetSceneType);
        InitCurrentScene(Managers.Scene.CurrentScene);
    }

    #endregion

    #region Game Start

    public void StartSinglePlay() {
        StartGame(Define.Type.Game.Single);
    }

    public void StartLocalPlay() {
        StartGame(Define.Type.Game.Local);
    }

    public void StartMultiPlay() {
        StartGame(Define.Type.Game.Multi);
    }

    private void StartGame(Define.Type.Game gameType) {

        // setting game options
        switch (gameType) {
            case Define.Type.Game.Single:
                break;
            case Define.Type.Game.Local:
                break;
            case Define.Type.Game.Multi:
                break;
            default:
                break;
        }

        // transition scene
        ProcessSceneChange(Define.Type.Scene.InGame);
    }

    #endregion

}
