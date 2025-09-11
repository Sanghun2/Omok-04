using System;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    [SerializeField] Timer testTimer;
    [SerializeField] float testTime = 3f;

    public void Test_SetTimeAsDefault() {
        testTimer.SetTime(testTime, testTime);
    }

    public void Test_StartCount() {
        testTimer.StartCount();
    }

    private void Awake() {
        if (testTimer != null) {
            testTimer.OnTimeChanged += Test_CheckTime;
            testTimer.OnTimeOver += Test_CheckTimeOver;
        }
    }

    private void Test_CheckTimeOver(float currentTime, float totalTime) {
        Debug.LogAssertion($"time over");
    }

    private void Test_CheckTime(float currentTime, float totalTime) {
        Debug.LogAssertion($"{currentTime} / {totalTime}");
    }
}
