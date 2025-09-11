using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    public float ScaleFactor
    {
        get
        {
            if (scaleFactor == 0) {
                scaleFactor = MainCanvas.scaleFactor;
            }

            return scaleFactor;
        }
    }
    public Canvas MainCanvas
    {
        get
        {
            if (_mainUICanvas == null) {
                _mainUICanvas = GameObject.FindAnyObjectByType<Canvas>();    
            }

            return _mainUICanvas;
        }
    }
    public Canvas FrontCanvas
    {
        get
        {
            if (_frontUICanvas == null) {
                var canvsObj = GameObject.FindGameObjectWithTag("Front Canvas");
                if (canvsObj == null) {
                    var frontCanvasPrefb = Resources.Load<GameObject>("UI/Front Canvas");
                    canvsObj = GameObject.Instantiate(frontCanvasPrefb);
                }

                _frontUICanvas = canvsObj.GetComponent<Canvas>();   
            }

            return _frontUICanvas;
        }
    }
    public int OpenedUICount => _openedUIStack.Count;

    private Dictionary<Type, UIBase> uiDict = new Dictionary<Type, UIBase>();
    private Stack<UIBase> _openedUIStack = new Stack<UIBase>();
    private Canvas _mainUICanvas;
    private Canvas _frontUICanvas;
    private float scaleFactor;

    public void ClearUIs() {
        CloseAllUIs();
        uiDict.Clear();
        _openedUIStack.Clear();
    }
    public T GetUI<T>() where T : UIBase {
        if (uiDict.TryGetValue(typeof(T), out UIBase result)) {
            return result as T;
        }

        result = GameObject.FindAnyObjectByType<T>(FindObjectsInactive.Include);
        uiDict.Add(typeof(T), result);
        return result as T;
    }

    public UIBase OpenUI<T>() where T : UIBase {
        var targetUI = GetUI<T>();
        targetUI?.OpenUI();
        _openedUIStack.Push(targetUI); 
        return targetUI;
    }
    public void CloseUI<T>() where T : UIBase {
        var tempUIStack = new Stack<UIBase>(_openedUIStack.Count);
        while (_openedUIStack.Count > 0) {
            var ui = _openedUIStack.Pop();
            if (typeof(T) == ui.GetType()) {
                ui.CloseUI();
            }
            else {
                tempUIStack.Push(ui);
            }
        }

        while (tempUIStack.Count > 0) {
            _openedUIStack.Push(tempUIStack.Pop());
        }
    }
    public void CloseTopUI() {
        if (_openedUIStack.Count > 0) {
            _openedUIStack.Pop().CloseUI();
        }
    }

    public void CloseAllUIs() {
        while (_openedUIStack.Count > 0) {
            CloseTopUI();
        }
    }

    public void ToggleUI<T>(T targetUI) where T : UIBase{
        if (targetUI.IsOpened) CloseUI<T>();
        else OpenUI<T>();
    }
}
