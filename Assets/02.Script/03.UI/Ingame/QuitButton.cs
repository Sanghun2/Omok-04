using UnityEngine;

public class QuitButton : ButtonBase
{
    protected override void ButtonAction() {
        QuitGame();
    }

    private void QuitGame() {
        Managers.UI.GetUI<GameOverUI>().CloseUI();
        Managers.Game.GoToMainMenu();
    }
}
