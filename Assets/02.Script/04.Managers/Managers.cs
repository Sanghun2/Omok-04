using UnityEngine;

public class Managers : MonoBehaviour
{
    public static UIManager UI => uiManager;
    public static SceneManager Scene => sceneManager;

    static SceneManager sceneManager = new SceneManager();
    static UIManager uiManager = new UIManager();

    private void Awake() {

    }
}
