using UnityEngine;

public class GameOverUI : UIBase
{
    [SerializeField] RestartButton restartButton;
    [SerializeField] QuitButton quitButton;

    protected override void OnOpen() {
        if (Managers.Game.CurrentGameType == Define.Type.Game.Multi) {
            restartButton.CloseUI();
        }
        else {
            restartButton.OpenUI();
        }
    }
}
