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
    }
    private void ReleasePrevScene(Scene prevScene) {
        prevScene?.ReleaseScene();
    }
    
    private void ProcessSceneChange(Define.Type.Scene targetSceneType) {
        if (Managers.Scene.CurrentSceneType == targetSceneType) { Debug.LogAssertion($"같은 씬이므로 이동하지 않음"); return; }

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

        Managers.Board.InitBoard();
        Managers.Board.AssignLaunchRole();
        GameLogic gameLogic = new GameLogic(Managers.Board.Board,gameType);

        // transition scene
        ProcessSceneChange(Define.Type.Scene.InGame);
    }

    #endregion

}
