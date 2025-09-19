using UnityEngine;
using UnityEngine.UI;

public abstract class ButtonBase : UIBase
{

    [SerializeField] protected Button targetButton;

    #region Interface

    protected abstract void ButtonAction();

    #endregion

    #region Capsule

    protected override void Start() {
        base.Start();
        if (targetButton != null) {
            targetButton.onClick.RemoveAllListeners();
            targetButton.onClick.AddListener(ButtonAction);
        }
    }

    protected virtual void Reset() {
        if (targetButton == null) targetButton = GetComponentInChildren<Button>();
    }

    #endregion
}
