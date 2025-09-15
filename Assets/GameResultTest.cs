using UnityEngine;
using UnityEngine.UI;

public class GameResultTest : MonoBehaviour
{

    [SerializeField] private Button winButton;
    [SerializeField] private Button lossButton;


    [SerializeField] private GameResultManager gameResultManager;

    void Start()
    {
        if (winButton != null)
        {
            winButton.onClick.AddListener(OnWinButtonClick);
        }

        if (lossButton != null)
        {
            lossButton.onClick.AddListener(OnLossButtonClick);
        }
    }


    public void OnWinButtonClick()
    {
        Debug.Log("승");

        gameResultManager.SendGameResult(true);
    }


    public void OnLossButtonClick()
    {
        Debug.Log("패");

        gameResultManager.SendGameResult(false);
    }
}