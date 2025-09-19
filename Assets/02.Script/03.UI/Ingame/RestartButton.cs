using UnityEngine;
using UnityEngine.UI;

public class RestartButton : ButtonBase
{
    protected override void ButtonAction() {
        Restart();
    }


    private void Restart() {
        Managers.Game.RestartLastGame();
        Managers.UI.GetUI<GameOverUI>().CloseUI();
    }
}
