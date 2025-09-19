using UnityEngine;

public class FindMatchButton : ButtonBase
{
    protected override void ButtonAction() {
        Managers.Network.FindMatch(); 
        Managers.UI.OpenPopUp(PopUpInfo.FindMatchPlayer, Define.Type.PopUpParent.Front);
    }
}
