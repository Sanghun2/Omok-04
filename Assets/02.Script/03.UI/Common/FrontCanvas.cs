using UnityEngine;

public class FrontCanvas : UIBase
{
    [SerializeField] GameObject touchBlockPanel;

    public void ActiveTouchBlockPanel(bool active) {
        touchBlockPanel.SetActive(active);  
    }
}
