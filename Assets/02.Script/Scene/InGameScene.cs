using UnityEngine;

public class InGameScene : Scene
{
    public override void InitScene() {
        Managers.Time.GetTimer().SetTimeAsDefault().StartCount();
        Managers.InGameUI.InitUIs(Managers.Game.CurrentGameType, Managers.Turn.GetCurrentPlayer());
        Managers.UI.GetUI<GameOverUI>().CloseUI();
    }

    public override void ReleaseScene() {
    }
}
