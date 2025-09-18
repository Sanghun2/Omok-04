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

                // UI ���ʱ�ȭ
                var ui = Object.FindObjectOfType<IngameUIController>();
                if (ui != null)
                {
                    ui.ResetTurnUI();   // �� üũ �ʱ�ȭ
                    ui.InitUI();        // Ÿ�̸� & �̺�Ʈ �ٽ� ����
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
