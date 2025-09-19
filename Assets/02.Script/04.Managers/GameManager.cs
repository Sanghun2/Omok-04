using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : IInitializable
{
    public Define.Type.Game CurrentGameType => currentGameType;
    public Define.State.GameState CurrentGameState => currentGameState;

    public bool IsInit => isInit;

    private bool isInit;
    private Define.State.GameState currentGameState;
    private Define.Type.Game currentGameType;
    private Define.Type.GameLevel lastGameLevel;
    private GameLogic gameLogic;

    #region Flow Control

    public void GoToMainMenu()
    {
        ProcessSceneChange(Define.Type.Scene.MainMenu);
        currentGameState = Define.State.GameState.NotStarted;
    }

    public void GoToLogIn()
    {
        ProcessSceneChange(Define.Type.Scene.LogIn);
        currentGameState = Define.State.GameState.NotStarted;
    }

    private void InitCurrentScene(Scene currentScene)
    {
        currentScene.InitScene();
    }
    private void ReleasePrevScene(Scene prevScene)
    {
        prevScene?.ReleaseScene();
    }

    private void ProcessSceneChange(Define.Type.Scene targetSceneType)
    {
        //if (Managers.Scene.CurrentSceneType == targetSceneType) { Debug.LogAssertion($"같은 씬이므로 이동하지 않음"); return; }

        var prevScene = Managers.Scene.CurrentScene;
        ReleasePrevScene(prevScene);
        Managers.Scene.ShowScene(targetSceneType);
        InitCurrentScene(Managers.Scene.CurrentScene);
        //Managers.Coroutine.WaitFrame(1, () => {
        //});
    }

    #endregion

    #region Game Start


    public void StartSinglePlay(Define.Type.GameLevel level)
    {
        StartGame(Define.Type.Game.Single, level);

    }

    public void EnterLocalPlay()
    {
        StartGame(Define.Type.Game.Local);
    }

    public void EnterMultiPlay()
    {
        StartGame(Define.Type.Game.Multi);
    }

    private void StartGame(Define.Type.Game gameType, Define.Type.GameLevel level = Define.Type.GameLevel.Easy)
    {
        Debug.Log($"게임 시작. 모드: {gameType}, 난이도: {level}");
        //게임 다시하기 기능때문에 추가함
        this.currentGameType = gameType;
        this.lastGameLevel = level;


        // setting game options
        Managers.Board.InitBoard();
        Managers.Turn.StartGame();
        

        var timer = Managers.Time.GetTimer();
        if (timer != null) {
            timer.SetTimeAsDefault();   // 25초 설정
        }

        currentGameState = Define.State.GameState.Ready;

        switch (gameType)
        {
            case Define.Type.Game.Single:
                gameLogic = new GameLogic(Managers.Board.Board, gameType, level);
                SetGameStatePlay();
                break;
            case Define.Type.Game.Local:
                gameLogic = new GameLogic(Managers.Board.Board, gameType);
                SetGameStatePlay();
                break;
            case Define.Type.Game.Multi:
                gameLogic = new GameLogic(Managers.Board.Board, gameType);
                SetGameStatePlay();
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
    public void SetGameStatePlay()
    {
        currentGameState = Define.State.GameState.InProgress;
        Managers.Time.GetTimer().StartCount();
    }
    #endregion

    #region End Game

    public void EndGame() {
        if (Managers.Turn.GetCurrentPlayer() == Define.Type.Player.Player1)
            Managers.Game.EndGame(Define.State.GameResult.WhiteStoneWin);
        else if (Managers.Turn.GetCurrentPlayer() == Define.Type.Player.Player2)
            Managers.Game.EndGame(Define.State.GameResult.BlackStoneWin);
    }

    public void EndGame(Define.State.GameResult gameResult)
    {
        currentGameState = Define.State.GameState.NotStarted;
        Managers.Time.GetTimer().Pause();

        gameLogic.SetState(null);
        gameLogic.firstPlayerState = null;
        gameLogic.secondPlayerState = null;

        Define.Type.Player player;

        switch (gameResult)
        {
            case Define.State.GameResult.BlackStoneWin:
                player = Define.Type.Player.Player1;
                Managers.Turn.EndGame(player);
                break;
            case Define.State.GameResult.WhiteStoneWin:
                player = Define.Type.Player.Player2;
                Managers.Turn.EndGame(player);
                break;
            default:
                break;
        }

        if (currentGameType == Define.Type.Game.Multi)
        {
            if (gameResult == Define.State.GameResult.BlackStoneWin)
                Managers.GameResult.SendGameResult(true);
            else if (gameResult == Define.State.GameResult.WhiteStoneWin)
                Managers.GameResult.SendGameResult(false);
            //else // Draw일 때
        }

        Debug.Log($"### DEV_JSH Game Over Result : {gameResult.ToString()} ###");
        Managers.InGameUI.ShowGameResult(gameResult);
        Managers.Board.DeactiveLaunchButton();
        Managers.GameResult.EndGame();
        gameLogic = null;
    }
    #endregion

    #region Restart Game
    public void RestartLastGame()
    {
        Debug.Log($"게임 재시작. 모드: {currentGameType}, 난이도: {lastGameLevel}");
        StartGame(currentGameType, lastGameLevel);
    }

    public void Initialize() {
        Managers.Time.Timer.OnTimeOver += EndGame;
    }

    #endregion
}
