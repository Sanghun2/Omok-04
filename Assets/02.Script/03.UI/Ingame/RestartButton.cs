using UnityEngine;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private GameObject gameOverUI;

    void Start()
    {
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(() =>
            {
                Managers.Game.RestartLastGame();
                gameOverUI.SetActive(false);
            });
        }
    }
}
