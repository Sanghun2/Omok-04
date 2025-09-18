using UnityEngine;

public abstract class Scene : MonoBehaviour
{
    public Define.Type.Scene SceneType => sceneType;

    [SerializeField] Define.Type.Scene sceneType;
    [SerializeField] GameObject[] sceneUIs;
    [SerializeField] GameObject[] ingameObjs;


    public virtual void ActiveScene(bool active) {
        gameObject.SetActive(active);
        if (sceneUIs.Length > 0) {
            for (int i = 0; i < sceneUIs.Length; i++) {
                var sceneUI = sceneUIs[i];
                sceneUI.SetActive(active);
            }
        }
        else {
            Debug.LogAssertion($"<color=orange>연결된 UI가 없음</color>");
        }

        if (ingameObjs.Length > 0) {
            for (int i = 0; i < ingameObjs.Length; i++) {
                var targetObj = ingameObjs[i];
                targetObj.SetActive(active);
            }
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
