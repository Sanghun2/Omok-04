using UnityEngine;

public class InGameScene : Scene
{
    [SerializeField] GameObject InGameObjects;

    public override void ActiveScene(bool active)
    {
        base.ActiveScene(active);
        if (InGameObjects != null)
        {
            InGameObjects.SetActive(active);
        }
        else
        {
            Debug.LogAssertion($"<color=orange>연결된 GameObject가 없음</color>");
        }

    }

    public override void InitScene() {
        //Managers.UI.GetUI<IngameUIController>().InitUI();
        Managers.Time.GetTimer().SetTimeAsDefault().StartCount();
        Debug.LogAssertion($"in game scene init");
    }
}
