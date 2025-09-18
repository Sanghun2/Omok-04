using UnityEngine;
using UnityEngine.UI;

public class RestartButton : ButtonBase
{
    [SerializeField] private GameObject gameOverUI;

    protected override void ButtonAction() {
        Restart();
    }


    private void Restart() {
        Managers.Game.RestartLastGame();
        Managers.Time.GetTimer().SetTimeAsDefault();

        // UI 재초기화
        var ui = Object.FindObjectOfType<IngameUIController>();
        if (ui != null) {
            ui.ResetTurnUI();   // 턴 체크 초기화
            ui.InitUI();        // 타이머 & 이벤트 다시 연결
        }
        else {
            Debug.Log("ui is null!");
        }

        gameOverUI.SetActive(false);
    }
}
