using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using System.Collections;

public class IngameUIController : UIBase
{
    [SerializeField] public GameObject player1;
    [SerializeField] public GameObject player2;
    [SerializeField] public GameObject AITurn;  // Ai ���� ��.. ���� ǥ��

    [SerializeField] public Button pauseButton;
    [SerializeField] public Button startButton;
    [SerializeField] public Button p2OKButton;
    [SerializeField] Button GoBackButton;

    [SerializeField] public Slider timeSlider;
    [SerializeField] private TextMeshProUGUI p2Rank;
    [SerializeField] private TextMeshProUGUI timerText;

    private TextMeshProUGUI p1Name;
    private TextMeshProUGUI p1Result;
    private TextMeshProUGUI p2Name;
    private TextMeshProUGUI p2Result;
    public GameObject[] p1TurnChecks;
    public GameObject[] p2TurnChecks;



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


    // �� ���� �������� üũǥ��
    //public void UpdateTurnUI(Define.Type.StoneColor currentStone)
    //{
    //    if (currentStone == Define.Type.StoneColor.Black)
    //    {
    //        SetTurnChecksActive(p1TurnChecks, true);
    //        SetTurnChecksActive(p2TurnChecks, false);
    //    }
    //    else if (currentStone == Define.Type.StoneColor.White)
    //    {
    //        SetTurnChecksActive(p1TurnChecks, false);
    //        SetTurnChecksActive(p2TurnChecks, true);
    //    }
    //}

    //private void SetTurnChecksActive(GameObject[] turnChecks, bool isActive)
    //{
    //    if (turnChecks == null) return;
    //    foreach (var obj in turnChecks)
    //    {
    //        obj.SetActive(isActive);
    //    }
    //}


    public void SetTurnChecks(bool aiTurn)
    {
        if (AITurn != null) AITurn.gameObject.SetActive(aiTurn);
        if (p1TurnChecks != null)
        {
            foreach (var obj in p1TurnChecks)
                obj.SetActive(!aiTurn);   // AI ���̸� Player1 üũ�� ����
        }

        if (p2TurnChecks != null)
        {
            foreach (var obj in p2TurnChecks)
                obj.SetActive(aiTurn);    // AI ���̸� Player2 üũ�� ����
        }
    }


    /// AI �� ǥ��, Player üũ �ʱ�ȭ
    public void ResetTurnUI()
    {
        // AI �� ǥ�� ����
        if (AITurn != null)
            AITurn.SetActive(false);

        // Player1 üũ ����
        if (p1TurnChecks != null)
            foreach (var obj in p1TurnChecks)
                obj.SetActive(true);

        // Player2 üũ ����
        if (p2TurnChecks != null)
            foreach (var obj in p2TurnChecks)
                obj.SetActive(false);
    }




    public override void InitUI()
    {
        base.InitUI();

        GoBackButton.onClick.AddListener(() =>
        {
            ResetTurnUI();
            Managers.Scene.ShowScene(Define.Type.Scene.MainMenu);
        });

        // �̱۸���� �� player2 UI ����
        if (Managers.Game.CurrentGameType == Define.Type.Game.Single)
        {
            if (p2Name != null) p2Name.text = "AI";
            if (p2Rank != null) p2Result.text = string.Empty;
            if (p2OKButton != null) p2OKButton.gameObject.SetActive(false);

        }

        // ���� �ʱ�ȭ
        Managers.Board.InitBoard();

        // Ÿ�̸� �ʱ�ȭ
        Timer timer = Managers.Time.GetTimer();
        if (timer != null)
        { BindTimer(timer);
            Debug.Log("Timer out");
            // timer.StartCount();
        }

        Managers.Turn.OnTurnChanged.AddListener((player) =>
        {
            Managers.Time.GetTimer().SetTimeAsDefault();
        });

        //Managers.Turn.OnTurnChanged.AddListener((player) =>
        //{
        //    var stone = player == Define.Type.Player.Player1 ?
        //                Define.Type.StoneColor.Black : Define.Type.StoneColor.White;
        //    UpdateTurnUI(stone);
        //});

    }


  



    // Ÿ�̸� �̺�Ʈ ����
    private void BindTimer(Timer timer)
    {
        timer.OnTimeChanged -= UpdateTimerUI;
        timer.OnTimeChanged += UpdateTimerUI;
        timer.SetTimeAsDefault();
    }

    // TextMeshPro UI ����
    private void UpdateTimerUI(float current, float total)
    {
        if (timerText == null) return;

        int seconds = Mathf.CeilToInt(current);
        timerText.text = seconds.ToString();

        if (timeSlider != null && total > 0)
            timeSlider.value = current / total;
    }

    protected override void Start()
    {
        base.Start();
        // Debug.Log($"[TEST] Managers.Time null? : {Managers.Time == null}");
        //var t = Managers.Time?.GetTimer();
        //Debug.Log($"[TEST] Timer ã��? : {t != null}");
        //if (t != null)
        //{
        //    t.OnTimeChanged += (c, tot) => Debug.Log($"[TEST] Timer Tick : {c}");
        //    t.SetTime(30, 30);
        //    t.StartCount();
        //}
    }
}
