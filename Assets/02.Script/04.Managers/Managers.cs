using UnityEngine;

public class Managers : MonoBehaviour
{
    public static UIManager UI => uiManager;

    static UIManager uiManager = new UIManager();

    private void Awake() {
        
    }
}
