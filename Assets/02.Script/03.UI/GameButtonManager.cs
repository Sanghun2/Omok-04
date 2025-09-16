using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;


public class GameButtonManager : UIBase, IPointerDownHandler, IPointerUpHandler
{
    public BTNType currentType;
    public Transform buttonScale;

    public GameUIManger gameUIManger;
    Vector3 defaultScale;

    void Start()
    {
        defaultScale = buttonScale.localScale;

        AutoSetButtonType();
    }
    public void OnBtnClick()
    {
        switch (currentType)
        {
            case BTNType.STARTSINGLE:
                Debug.Log("STARTSINGLE");
                break;

            case BTNType.STARTMULTI:
                gameUIManger.OnmMultiPopup();
                Debug.Log("STARTMULTI");
                break;

            case BTNType.SETTING:
                gameUIManger.OnSettingPopup();
                Debug.Log("SETTING");
                break;

            case BTNType.EXIT:
                gameUIManger.OnExit();
                Debug.Log("나가기");
                break;
        }
    }

void AutoSetButtonType()
    {
        string objectName = gameObject.name.ToLower();

        if (objectName.Contains("single"))
            currentType = BTNType.STARTSINGLE;
        else if (objectName.Contains("multi"))
            currentType = BTNType.STARTMULTI;
        else if (objectName.Contains("setting"))
            currentType = BTNType.SETTING;
        else if (objectName.Contains("exit"))
            currentType = BTNType.EXIT;

        Debug.Log($"{gameObject.name}의 버튼 타입: {currentType}");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonScale.localScale = defaultScale * 1.2f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonScale.localScale = defaultScale;
    }
}
