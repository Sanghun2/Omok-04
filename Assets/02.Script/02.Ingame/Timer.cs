using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] bool _pause;
    [SerializeField] float currentTime;
    [SerializeField] float totalTime;
    private bool isInit;

    public delegate void TimeHandler(float currentTime, float totalTime);
    public event TimeHandler OnTimeChanged;
    public event TimeHandler OnTimeOver;

    public void SetTime(float currentTime, float totalTime) {
        this.currentTime = currentTime;
        this.totalTime = totalTime;
        OnTimeChanged?.Invoke(this.currentTime, totalTime);
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
            CountTime(Time.deltaTime);
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
            OnTimeOver(0, totalTime);
        }
    }
}
