using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UIBase 상속받아 구현하는 UI 샘플
/// </summary>
public class SampleTimeUI : UIBase
{
    [SerializeField] Slider timeSlider;
    [SerializeField] TextMeshProUGUI timeText;

    public override void InitUI() {
        Timer targetTimer = Managers.Time.GetTimer();
        AddTimerEvent(targetTimer);
    }
    public void AddTimerEvent(Timer targetTimer) {
        if (targetTimer != null) {
            targetTimer.OnTimeChanged -= UpdateTimeSlider;
            targetTimer.OnTimeChanged += UpdateTimeSlider;
            return;
        }

        Debug.LogAssertion($"<color=orange>timer 등록 필요</color>");
    }

    private void UpdateTimeSlider(float currentTime, float totalTime) {
        if (timeSlider != null) timeSlider.value = currentTime / totalTime;
        if (timeText != null) timeText.text = currentTime.ToString("F0");
    }

    private void Reset() {
        timeSlider = GetComponentInChildren<Slider>();
        timeText = GetComponentInChildren<TextMeshProUGUI>();

        if (!timeText) Debug.LogAssertion($"<color=red>Time Text UI 필요</color>");
    }
}
