using UnityEngine;

public abstract class UIBase : MonoBehaviour
{
    public virtual bool IsOpened => gameObject.activeSelf; 
    protected bool isInit;

    public virtual void InitUI() { }

    public virtual void OpenUI() {
        gameObject.SetActive(true);
        OnOpen();
    }
    public virtual void CloseUI() {
        gameObject.SetActive(false);
        OnClose();
    }

    protected virtual void OnOpen() { }
    protected virtual void OnClose() { }

    protected virtual void Start() {
        if (!isInit) {
            InitUI();
            isInit = true;
        }
    }
}
