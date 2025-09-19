using System;
using UnityEngine;

public partial class Timer : MonoBehaviour
{
    public float CurrentTime => currentTime;
    public float TotalTime => totalTime;
    public float CustomTimeScale
    {
        get
        {
            return customTimeScale;
        }
        set
        {
            customTimeScale = value;
        }
    }

    [SerializeField] bool _pause;
    [SerializeField] float customTimeScale = 1;
    [SerializeField] float currentTime;
    [SerializeField] float totalTime;
    private bool isInit;

    public delegate void TimeChangeHandler(float currentTime, float totalTime);
    public event TimeChangeHandler OnTimeChanged;

    public delegate void TimeEventHandler();
    public event TimeEventHandler OnTimeOver;

    public event TimeEventHandler OnTimerInitialized; // ���� ������ �߰���

    public void SetTimeAsDefaultAndStart() {

    }
    private void SetTimeAndStart(float currentTime, float totalTime) {
        SetTime(currentTime, totalTime);
        StartCount();
    }
    public void SetTime(float currentTime, float totalTime) {
        this.currentTime = currentTime;
        this.totalTime = totalTime;
        OnTimeChanged?.Invoke(this.currentTime, totalTime);

        OnTimerInitialized?.Invoke(); // Ÿ�̸� �ʱ�ȭ �� �̺�Ʈ ȣ�� (���� ������ �߰���)

        Pause();
        isInit = true;
    }
    public void StartCount() {
        Unpause();
    }
    public void Pause() {
        _pause = true;
    }
    public void Unpause() {
        _pause = false;
    }


    private void Update() {
        if (!_pause && isInit) {
            CountTime(Time.unscaledDeltaTime * customTimeScale);
        }
    }

    private void CountTime(float deltaTime) {
        currentTime -= deltaTime;
        if (currentTime <= 0) {
            currentTime = 0;
        }

        OnTimeChanged?.Invoke(currentTime, totalTime);

        if (Mathf.Approximately(currentTime, 0)) {
            Pause();
            OnTimeOver?.Invoke();
            Debug.LogAssertion($"���ӿ���. ���� �ð�: {currentTime}");
        }
    }
}
