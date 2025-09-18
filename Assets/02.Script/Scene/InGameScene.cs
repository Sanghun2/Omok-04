using UnityEngine;

public class InGameScene : Scene
{
    public override void InitScene() {
        Managers.Time.GetTimer().SetTimeAsDefault().StartCount();

        OnSceneShown();
    }

    protected override void OnSceneShown() {
        Managers.UI.CloseUI<GameOverUI>();
        Debug.LogAssertion("game over ui closed");
    }
}
