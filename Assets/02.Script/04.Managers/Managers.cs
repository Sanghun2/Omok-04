using System;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    public static UIManager UI => uiManager;
    public static SceneManager Scene
    {
        get
        {
            if (sceneManager == null) {
                sceneManager = Instance.GetComponent<SceneManager>();
            }

            return sceneManager;
        }
    }
    public static Managers Instance
    {
        get
        {
            if (_instance == null) {
                _instance = GameObject.FindAnyObjectByType<Managers>();
            }

            return _instance;
        }
    }
    public static CoroutineManager Coroutine
    {
        get
        {
            if (coroutineManager == null) {
                coroutineManager = Instance.GetComponent<CoroutineManager>();
            }

            return coroutineManager;
        }
    }
    public static TimeManager Time => timeManager;
    public static GameManager Game => gameManager;
    public static NetworkManager Network => networkManager;
    public static TurnManager Turn => turnManager;
    public static UserInfoManager UserInfo
    {
        get
        {
            if (userInfoManager == null) {
                userInfoManager = Instance.GetComponentInChildren<UserInfoManager>();
            }

            return userInfoManager;
        }
    }
    public static GameResultManager GameResult
    {
        get
        {
            if (gameResultManager == null) {
                gameResultManager = Instance.GetComponentInChildren<GameResultManager>();
            }

            return gameResultManager;
        }
    }
    public static BoardController Board
    {
        get
        {
            if (boardController == null) {
                boardController = FindAnyObjectByType<BoardController>(FindObjectsInactive.Include);
            }

            return boardController;
        }
    }
    public static PlayerManager Player => playerManager;

    static PlayerManager playerManager = new PlayerManager();
    static BoardController boardController;
    static GameResultManager gameResultManager;
    static UserInfoManager userInfoManager;
    static TurnManager turnManager = new TurnManager();
    static NetworkManager networkManager = new NetworkManager();
    static GameManager gameManager = new GameManager();
    static TimeManager timeManager = new TimeManager();
    static CoroutineManager coroutineManager;
    static Managers _instance;
    static SceneManager sceneManager;
    static UIManager uiManager = new UIManager();

    private void Awake() {
        List<IInitializable> initializeList = new List<IInitializable>() {
            Scene,
            Time,
            Player,
            Network,
        };

        InitializeAll(initializeList);

        LoadFirstScene();
    }

    private void LoadFirstScene() {
        Managers.Game.GoToLogIn();
    }

    private void InitializeAll(List<IInitializable> initializeList) {
        for (int i = 0; i < initializeList.Count; i++) {
            var initializeTarget = initializeList[i];
            if (!initializeTarget.IsInit) initializeTarget.Initialize();
        }
    }
}
