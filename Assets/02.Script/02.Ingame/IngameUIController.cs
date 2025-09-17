using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq; // LINQ 사용

public class IngameUIController : UIBase
{
    [SerializeField] public GameObject player1;
    [SerializeField] public GameObject player2;
    [SerializeField] public Button pauseButton;
    [SerializeField] public Button startButton;
    [SerializeField] public Slider timeSlider;

    private TextMeshProUGUI p1Name;
    private TextMeshProUGUI p1Result;
    private TextMeshProUGUI p2Name;
    private TextMeshProUGUI p2Result;
    private GameObject[] p1TurnChecks;
    private GameObject[] p2TurnChecks;

    void Awake()
    {
        p1Name = player1.GetComponentsInChildren<Transform>(true)
                        .First(t => t.CompareTag("Name"))
                        .GetComponent<TextMeshProUGUI>();

        // turn check은 왼쪽, 오른쪽 두 개.
        p1TurnChecks = player1.GetComponentsInChildren<Transform>(true)
                              .Where(t => t.CompareTag("TurnCheck"))
                              .Select(t => t.gameObject)
                              .ToArray();

        p1Result = player1.GetComponentsInChildren<Transform>(true)
                        .First(t => t.CompareTag("Result"))
                        .GetComponent<TextMeshProUGUI>();

        p2Name = player2.GetComponentsInChildren<Transform>(true)
                        .First(t => t.CompareTag("Name"))
                        .GetComponent<TextMeshProUGUI>();


        p2TurnChecks = player2.GetComponentsInChildren<Transform>(true)
                              .Where(t => t.CompareTag("TurnCheck"))
                              .Select(t => t.gameObject)
                              .ToArray();

        p2Result = player2.GetComponentsInChildren<Transform>(true)
                .First(t => t.CompareTag("Result"))
                .GetComponent<TextMeshProUGUI>();
    }
}
