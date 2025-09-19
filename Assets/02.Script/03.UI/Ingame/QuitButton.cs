using UnityEngine;

public class QuitButton : ButtonBase
{
    protected override void ButtonAction() {
        QuitGame();
    }

    private void QuitGame() {
        Managers.Game.GoToMainMenu();
    }
}
