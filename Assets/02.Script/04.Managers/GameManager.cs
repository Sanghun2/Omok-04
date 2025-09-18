using System;
using UnityEngine;

public class GameManager
{
    public GameManager()
    {
        Debug.Log("GameManager 생성");
    }

    public Define.Type.Game CurrentGameType => currentGameType;

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
        Managers.Coroutine.WaitFrame(1, () => InitCurrentScene(Managers.Scene.CurrentScene));
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
    public void SetGameStatePlay()
    {
        currentGameState = Define.State.GameState.InProgress;
        Managers.Time.GetTimer().StartCount();
    }

    #endregion

    #region End Game
    public void EndGame()
    {
        Debug.Log($"게임 종료. 모드: {currentGameType}, 난이도: {lastGameLevel}");
        gameLogic = null;
    }
    #endregion

    #region Restart Game
    public void RestartLastGame()
    {
        Debug.Log($"게임 재시작. 모드: {currentGameType}, 난이도: {lastGameLevel}");
        StartGame(currentGameType, lastGameLevel);
    }
    #endregion
}
