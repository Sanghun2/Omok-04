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
    [SerializeField] public GameObject AITurn;  // Ai 생각 중.. 문구 표시

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


    // 돌 색깔 기준으로 체크표시
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
                obj.SetActive(!aiTurn);   // AI 턴이면 Player1 체크는 꺼짐
        }

        if (p2TurnChecks != null)
        {
            foreach (var obj in p2TurnChecks)
                obj.SetActive(aiTurn);    // AI 턴이면 Player2 체크는 켜짐
        }
    }


    /// AI 턴 표시, Player 체크 초기화
    public void ResetTurnUI()
    {
        // AI 턴 표시 끄기
        if (AITurn != null)
            AITurn.SetActive(false);

        // Player1 체크 끄기
        if (p1TurnChecks != null)
            foreach (var obj in p1TurnChecks)
                obj.SetActive(true);

        // Player2 체크 끄기
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

        // 싱글모드일 때 player2 UI 설정
        if (Managers.Game.CurrentGameType == Define.Type.Game.Single)
        {
            if (p2Name != null) p2Name.text = "AI";
            if (p2Rank != null) p2Result.text = string.Empty;
            if (p2OKButton != null) p2OKButton.gameObject.SetActive(false);

        }

        // 보드 초기화
        Managers.Board.InitBoard();

        // 타이머 초기화
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


  



    // 타이머 이벤트 연결
    private void BindTimer(Timer timer)
    {
        timer.OnTimeChanged -= UpdateTimerUI;
        timer.OnTimeChanged += UpdateTimerUI;
        timer.SetTimeAsDefault();
    }

    // TextMeshPro UI 갱신
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
        //Debug.Log($"[TEST] Timer 찾음? : {t != null}");
        //if (t != null)
        //{
        //    t.OnTimeChanged += (c, tot) => Debug.Log($"[TEST] Timer Tick : {c}");
        //    t.SetTime(30, 30);
        //    t.StartCount();
        //}
    }
}
