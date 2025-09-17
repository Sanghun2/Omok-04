using System;
using UnityEngine;

public class GameManager
{
    public Define.Type.Game CurrentGameType => currentGameType;

    private Define.State.GameState currentGameState;
    private Define.Type.Game currentGameType;

    #region Flow Control

    public void GoToMainMenu() {
        ProcessSceneChange(Define.Type.Scene.MainMenu);
        currentGameState = Define.State.GameState.NotStarted;
    }

    public void GoToLogIn() {
        ProcessSceneChange(Define.Type.Scene.LogIn);
        currentGameState = Define.State.GameState.NotStarted;
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

    public void EnterLocalPlay() {
        StartGame(Define.Type.Game.Local);
    }

    public void EnterMultiPlay() {
        StartGame(Define.Type.Game.Multi);
    }

    private void StartGame(Define.Type.Game gameType, Define.Type.GameLevel level = Define.Type.GameLevel.Easy) {

        // setting game options
        Managers.Board.InitBoard();
        currentGameState = Define.State.GameState.Ready;
        currentGameType = gameType;

        GameLogic gameLogic;

        switch (gameType) {
            case Define.Type.Game.Single:
                gameLogic = new GameLogic(Managers.Board.Board, gameType, level);
                SetStatePlay();
                break;
            case Define.Type.Game.Local:
                gameLogic = new GameLogic(Managers.Board.Board, gameType);
                SetStatePlay();
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

    /// <summary>
    /// 모든 플레이어가 준비되었을 때 실행하는 코드. 멀티플레이용
    /// </summary>
    public void SetStatePlay() {
        currentGameState = Define.State.GameState.InProgress;
    }

    #endregion

}
