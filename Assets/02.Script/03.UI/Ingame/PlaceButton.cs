using UnityEngine;

public class PlaceButton : ButtonBase
{
    [SerializeField] Define.Type.Player defaultPlayerType;
    private Define.Type.Player currentPlayerType;

    public void ResetPlayerType() {
        currentPlayerType = defaultPlayerType;
    }

    public void SetPlayerType(Define.Type.Player playerType) {
        currentPlayerType = playerType;
    }

    protected override void ButtonAction() {
        if (currentPlayerType == Define.Type.Player.Player1) {
            Debug.LogAssertion($"{currentPlayerType} Âø¼ö ½Ãµµ");
            Managers.Board.OnClickBlackStoneLaunchButton();
        }
        else {
            Debug.LogAssertion($"{currentPlayerType} Âø¼ö ½Ãµµ");
            Managers.Board.OnClickWhiteStoneLaunchButton();
        }
    }
}
