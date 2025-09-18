using UnityEngine;
using UnityEngine.UI;

public class RestartButton : ButtonBase
{
    [SerializeField] private Button restartButton;
    [SerializeField] private GameObject gameOverUI;
    protected override void ButtonAction() {
        Restart();
    }


    private void Restart() {
        Managers.Game.RestartLastGame();
        Managers.Time.GetTimer().SetTimeAsDefault();

        // UI 재초기화
        var inGameUI = Managers.InGameUI;
        inGameUI.ResetTurnUI();
        inGameUI.InitUI();

        if (gameOverUI != null) {
            gameOverUI.SetActive(false);
        }
        else {
            Debug.LogError($"game over ui null");
        }
    }
}
