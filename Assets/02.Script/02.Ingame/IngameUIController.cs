using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq; // LINQ ���

public class IngameUIController : UIBase
{
    [SerializeField] public GameObject player1;
    [SerializeField] public GameObject player2;
    [SerializeField] public Button pauseButton;
    [SerializeField] public Button startButton;
    [SerializeField] public Button p2OKButton;
    [SerializeField] public Slider timeSlider;

    private TextMeshProUGUI p1Name;
    private TextMeshProUGUI p1Result;
    private TextMeshProUGUI p2Name;
    private TextMeshProUGUI p2Result;
    private GameObject[] p1TurnChecks;
    private GameObject[] p2TurnChecks;

    // ���߰� : Ÿ�̸� ǥ�ÿ� TextMeshPro (���� �ϳ��� ����)
    [SerializeField] private TextMeshProUGUI timerText;

    void Awake()
    {
        p1Name = player1.GetComponentsInChildren<Transform>(true)
                        .First(t => t.CompareTag("Name"))
                        .GetComponent<TextMeshProUGUI>();

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

    public override void InitUI()
    {
        base.InitUI();

        // ���߰� : �̱۸���� �� player2 UI ����
        if (Managers.Game.Mode == Define.Type.Game.Single)
        {
            if (p2Name != null) p2Name.text = "AI";
            if (p2Result != null) p2Result.text = string.Empty;
            if (p2OKButton != null) p2OKButton.gameObject.SetActive(false);
        }

        // �ڱ��� Ÿ�̸� �ʱ�ȭ
        Timer timer = Managers.Time.GetTimer(Define.Type.Player.Player1);
        if (timer != null)
            BindTimer(timer);

        Managers.Turn.OnTurnChanged.AddListener((player) =>
        {
            Timer t = Managers.Time.GetTimer(player);
            if (t != null)
                BindTimer(t);
        });
    }

    // �ڱ��� : Ÿ�̸� �̺�Ʈ ����
    private void BindTimer(Timer timer)
    {
        timer.OnTimeChanged -= UpdateTimerUI;
        timer.OnTimeChanged += UpdateTimerUI;
        UpdateTimerUI(timer.CurrentTime, 30f);
    }

    // �ڱ��� : TextMeshPro UI ����
    private void UpdateTimerUI(float current, float total)
    {
        if (timerText == null) return;

        int seconds = Mathf.CeilToInt(current);
        timerText.text = seconds.ToString();

        if (timeSlider != null && total > 0)
            timeSlider.value = current / total;
    }

    void Start()
    {
        Debug.Log($"[TEST] Managers.Time null? : {Managers.Time == null}");
        var t = Managers.Time?.GetTimer(Define.Type.Player.Player1);
        Debug.Log($"[TEST] Timer ã��? : {t != null}");
        if (t != null)
        {
            t.OnTimeChanged += (c, tot) => Debug.Log($"[TEST] Timer Tick : {c}");
            t.SetTime(30, 30);
            t.StartCount();
        }
    }
}
