using UnityEngine;

public class PlaceButton : ButtonBase
{
    [SerializeField] Define.Type.Player playerType;

    protected override void ButtonAction() {
        if (playerType == Define.Type.Player.Player1) {
            Debug.LogAssertion($"{playerType} Âø¼ö");
            Managers.Board.OnClickBlackStoneLaunchButton();
        }
        else {
            Debug.LogAssertion($"{playerType} Âø¼ö");
            Managers.Board.OnClickWhiteStoneLaunchButton();
        }
    }
}
