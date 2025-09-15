using UnityEngine;

public abstract class Scene : MonoBehaviour
{
    public Define.Type.Scene SceneType => sceneType;

    [SerializeField] Define.Type.Scene sceneType;
    [SerializeField] GameObject sceneUI;


    public virtual void ActiveScene(bool active) {
        gameObject.SetActive(active);
        if (sceneUI != null) {
            sceneUI.SetActive(active);
        }
        else {
            Debug.LogAssertion($"<color=orange>연결된 UI가 없음</color>");
        }
    }

    public virtual void InitScene() {
        Debug.LogAssertion($"{SceneType} Scene Init");
    }
    public virtual void ReleaseScene() {
        Debug.LogAssertion($"{SceneType} Scene Release");
    }
}
