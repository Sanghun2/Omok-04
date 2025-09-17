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

    public void StartSinglePlay(Define.Type.GameLevel level) {
        StartGame(Define.Type.Game.Single, level);
    }

    public void StartLocalPlay() {
        StartGame(Define.Type.Game.Local);
    }

    public void StartMultiPlay() {
        StartGame(Define.Type.Game.Multi);
    }

    private void StartGame(Define.Type.Game gameType, Define.Type.GameLevel level = Define.Type.GameLevel.Easy) {

        // setting game options
        Managers.Board.InitBoard();

        GameLogic gameLogic;

        switch (gameType) {
            case Define.Type.Game.Single:
                gameLogic = new GameLogic(Managers.Board.Board, gameType, level);
                break;
            case Define.Type.Game.Local:
                gameLogic = new GameLogic(Managers.Board.Board, gameType);
                break;
            case Define.Type.Game.Multi:
                //gameLogic = new GameLogic(Managers.Board.Board, gameType);
                break;
            default:
                break;
        }

        // transition scene
        ProcessSceneChange(Define.Type.Scene.InGame);
    }

    #endregion

}
