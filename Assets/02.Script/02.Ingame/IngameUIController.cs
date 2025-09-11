using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngameUIController : UIBase
{
    [SerializeField] private GameObject player1Obj;
    [SerializeField] private GameObject player2Obj;
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
        p1Name = player1Obj.transform.Find("Player1").GetComponent<TextMeshProUGUI>();
        p1Result = player1Obj.transform.Find("Result").GetComponent<TextMeshProUGUI>();
        p1TurnCheck = player1Obj.transform.Find("Turn Check").GetComponent<GameObject>();

        p2Name = player2Obj.transform.Find("Player2").GetComponent<TextMeshProUGUI>();
        p2Result = player2Obj.transform.Find("Result").GetComponent<TextMeshProUGUI>();
        p2TurnCheck = player2Obj.transform.Find("Turn Check").GetComponent<GameObject>();
    }

}