using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopUpUI : UIBase
{
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI contentText;
    [SerializeField] List<Button> buttonList = new List<Button>();

    public void InitPopUp(PopUpInfo info) {
        if (titleText != null && string.IsNullOrEmpty(info.Title) == false) { titleText.gameObject.SetActive(true); titleText.text = info.Title; }
        else titleText.gameObject.SetActive(false);

        if (contentText != null && string.IsNullOrEmpty(info.Content) == false) { contentText.gameObject.gameObject.SetActive(true); contentText.text = info.Title; }
        else contentText.gameObject.SetActive(false);

        var actions = info.ButtonActions;
        for (int i = 0; i < buttonList.Count; i++) {
            
        }
    }
}

[Serializable]
public class PopUpInfo
{
    public string Title => title;
    public string Content => content;
    public IReadOnlyList<UnityAction> ButtonActions => buttonActions;

    [SerializeField] string title;
    [SerializeField] string content;
    [SerializeField] UnityAction[] buttonActions;

    public PopUpInfo(string title, string content, params UnityAction[] buttonActions) {
        this.title = title;
        this.content = content;
        this.buttonActions = buttonActions;
    }
}
