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

    static TimeManager timeManager = new TimeManager();
    static CoroutineManager coroutineManager;
    static Managers _instance;
    static SceneManager sceneManager;
    static UIManager uiManager = new UIManager();

    private void Awake() {

    }
}
