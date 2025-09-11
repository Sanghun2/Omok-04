using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameUIController : UIBase
{
    [SerializeField] private GameObject player1;
    [SerializeField] private GameObject player2;
    [SerializeField] private Button pauseButton;
    [SerializeField] private Button startButton;

    private TextMeshProUGUI p1Name;
    private TextMeshProUGUI p1Result;
    private TextMeshProUGUI p2Name;
    private TextMeshProUGUI p2Result;
    private GameObject p1TurnCheck;
    private GameObject p2TurnCheck;

    void Awake()
    {
        p1Name = player1.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        p1Result = player1.transform.Find("Result").GetComponent<TextMeshProUGUI>();
        p1TurnCheck = player1.transform.Find("Turn Check").gameObject;

        p2Name = player2.transform.Find("Name").GetComponent<TextMeshProUGUI>();
        p2Result = player2.transform.Find("Result").GetComponent<TextMeshProUGUI>();
        p2TurnCheck = player2.transform.Find("Turn Check").gameObject;
    }

}