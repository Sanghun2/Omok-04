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

        // UI ���ʱ�ȭ
        var ui = Object.FindObjectOfType<IngameUIController>();
        if (ui != null) {
            ui.ResetTurnUI();   // �� üũ �ʱ�ȭ
            ui.InitUI();        // Ÿ�̸� & �̺�Ʈ �ٽ� ����
        }
        else {
            Debug.Log("ui is null!");
        }

        gameOverUI.SetActive(false);
    }
}
