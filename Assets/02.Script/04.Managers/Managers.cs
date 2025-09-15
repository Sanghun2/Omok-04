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
    //public static NetworkManager Network => networkManager;
    public static TurnManager Turn => turnManager;

    static TurnManager turnManager = new TurnManager();
    //static NetworkManager networkManager = new NetworkManager();
    static GameManager gameManager => new GameManager();
    static TimeManager timeManager = new TimeManager();
    static CoroutineManager coroutineManager;
    static Managers _instance;
    static SceneManager sceneManager;
    static UIManager uiManager = new UIManager();

    private void Awake() {
        List<IInitializable> initializeList = new List<IInitializable>() {
            Scene,
            Time,
        };

        InitializeAll(initializeList);

        LoadFirstScene();
    }

    private void LoadFirstScene() {
        Debug.LogAssertion($"첫 씬 로드 필요");
    }

    private void InitializeAll(List<IInitializable> initializeList) {
        for (int i = 0; i < initializeList.Count; i++) {
            var initializeTarget = initializeList[i];
            if (!initializeTarget.IsInit) initializeTarget.Initialize();
        }
    }
}
