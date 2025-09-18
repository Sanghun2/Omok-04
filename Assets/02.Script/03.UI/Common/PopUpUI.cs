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
    [SerializeField] List<CustomButton> buttonList = new List<CustomButton>();

    public void InitPopUp(PopUpInfo info) {
        if (titleText != null && string.IsNullOrEmpty(info.Title) == false) { titleText.gameObject.SetActive(true); titleText.text = info.Title; }
        else titleText.gameObject.SetActive(false);

        if (contentText != null && string.IsNullOrEmpty(info.Content) == false) { contentText.gameObject.gameObject.SetActive(true); contentText.text = info.Title; }
        else contentText.gameObject.SetActive(false);

        var actions = info.ButtonActions;
        for (int i = 0; i < buttonList.Count; i++) {
            CustomButton targetButton = buttonList[i];
            if (i < actions.Count) {
                PopUpButtonInfo buttonInfo = actions[i];
                targetButton.OpenUI();
                targetButton.InitButtonAction(buttonInfo.ButtonTitle, buttonInfo.ButtonAction);
            }
            else {
                targetButton.CloseUI();
            }
        }
    }
}

[Serializable]
public class PopUpInfo
{
    public static readonly PopUpInfo FindMatchPlayer = new PopUpInfo(
            "대전 상대 찾는중",
            string.Empty,
            new PopUpButtonInfo(
                "취소", 
                () => { Managers.UI.ClosePopUp(); Managers.Network.CancelFindMatch(); }
            ));

    public string Title => title;
    public string Content => content;
    public IReadOnlyList<PopUpButtonInfo> ButtonActions => buttonInfos;

    [SerializeField] string title;
    [SerializeField] string content;
    [SerializeField] PopUpButtonInfo[] buttonInfos;

    public PopUpInfo(string title, string content, params PopUpButtonInfo[] buttonInfos) {
        this.title = title;
        this.content = content;
        this.buttonInfos = buttonInfos;
    }
}


[Serializable]
public class PopUpButtonInfo
{
    public string ButtonTitle => buttonTitle;
    public UnityAction ButtonAction => buttonAction;

    [SerializeField] string buttonTitle;
    [SerializeField] UnityAction buttonAction;

    public PopUpButtonInfo(string buttonTitle, UnityAction buttonAction) {
        this.buttonTitle = buttonTitle;
        this.buttonAction = buttonAction;
    }
}