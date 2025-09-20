using Photon.Pun;
using UnityEngine;

public class QuitButton : ButtonBase
{
    protected override void ButtonAction() {
        QuitGame();
    }

    private void QuitGame()
    {
        Managers.UI.CloseUI<GameOverUI>();
        Managers.Game.GoToMainMenu();

        if (Managers.Game.CurrentGameType == Define.Type.Game.Multi)
        {
            Managers.Network.LeaveRoom();
        }

        SoundManager.Instance.SetBGMSound("Intro");
    }
}
