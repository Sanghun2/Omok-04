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
    private GameObject[] p1TurnChecks;
    private GameObject[] p2TurnChecks;

    private Coroutine aiTurnCoroutine;

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

        GoBackButton.onClick.AddListener(() =>
        {
            Managers.Scene.ShowScene(Define.Type.Scene.MainMenu);
        });

        // 싱글모드일 때 player2 UI 설정
        if (Managers.Game.CurrentGameType == Define.Type.Game.Single)
        {
            if (p2Name != null) p2Name.text = "AI";
            if (p2Rank != null) p2Result.text = string.Empty;
            if (p2OKButton != null) p2OKButton.gameObject.SetActive(false);
        }

        // 기존 타이머 초기화
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
        
        Managers.Turn.OnTurnChanged.AddListener(HandleTurnChanged);

    }


    private void HandleTurnChanged(Define.Type.Player player)
    {

        // AI 턴일 때
        if (Managers.Game.CurrentGameType == Define.Type.Game.Single &&
            player == Define.Type.Player.Player2)
        {
            // 이전 Coroutine이 있으면 중지
            if (aiTurnCoroutine != null)
            {
                StopCoroutine(aiTurnCoroutine);
                aiTurnCoroutine = null;
            }
            aiTurnCoroutine = StartCoroutine(ShowAITurnAfterDelay(1f));
        }
        else
        {
            // AI가 아닌 플레이어 차례면 바로 끄기
            if (aiTurnCoroutine != null)
            {
                StopCoroutine(aiTurnCoroutine);
                aiTurnCoroutine = null;
            }
            if (AITurn != null)
                AITurn.SetActive(false);
        }
    }

    private IEnumerator ShowAITurnAfterDelay(float delay)
    {
        // delay만큼 기다린 후
        yield return new WaitForSeconds(delay);
        if (AITurn != null)
            AITurn.SetActive(true);
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
