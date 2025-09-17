using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CustomButton : ButtonBase
{
    [SerializeField] TextMeshProUGUI buttonTitleText;
    private UnityAction customAction;

    public void InitButtonAction(string buttonTitle, UnityAction buttonAction) {
        if (this.buttonTitleText != null) buttonTitleText.text = buttonTitle;
        customAction = buttonAction;
    }

    protected override void ButtonAction() {
        customAction?.Invoke();
    }
    protected override void Reset() {
        base.Reset();
        if (buttonTitleText == null) {
            buttonTitleText = GetComponentInChildren<TextMeshProUGUI>();
        }
    }
}
