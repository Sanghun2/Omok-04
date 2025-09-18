using UnityEngine;
using UnityEngine.UI;

public class RestartButton : MonoBehaviour
{
    [SerializeField] private Button restartButton;
    [SerializeField] private GameObject gameOverUI;

    void Start()
    {
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(() =>
            {
                Managers.Game.RestartLastGame();

                // UI 재초기화
                var ui = Object.FindObjectOfType<IngameUIController>();
                if (ui != null)
                {
                    ui.ResetTurnUI();   // 턴 체크 초기화
                    ui.InitUI();        // 타이머 & 이벤트 다시 연결
                }
                else
                {
                    Debug.Log("ui is null!");
                }

                    gameOverUI.SetActive(false);
            });
        }
    }
}
