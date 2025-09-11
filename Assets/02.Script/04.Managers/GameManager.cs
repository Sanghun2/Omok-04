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

    private void InitCurrentScene(Define.Type.Scene currentScene) {
        switch (currentScene) {
            case Define.Type.Scene.None:
                return;
            case Define.Type.Scene.LogIn:
                break;
            case Define.Type.Scene.MainMenu:
                break;
            case Define.Type.Scene.InGame:
                break;
            default:
                break;
        }
        Debug.LogAssertion($"{currentScene} init");
    }
    private void ReleasePrevScene(Define.Type.Scene prevScene) {
        switch (prevScene) {
            case Define.Type.Scene.None:
                return;
            case Define.Type.Scene.LogIn:
                break;
            case Define.Type.Scene.MainMenu:
                break;
            case Define.Type.Scene.InGame:
                break;
            default:
                break;
        }
        Debug.LogAssertion($"{prevScene} released");
    }
    
    private void ProcessSceneChange(Define.Type.Scene sceneType) {
        ReleasePrevScene(Managers.Scene.PrevScene);
        Managers.Scene.ShowScene(sceneType);
        InitCurrentScene(sceneType);
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
