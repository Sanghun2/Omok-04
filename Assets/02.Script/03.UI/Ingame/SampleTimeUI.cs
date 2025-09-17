using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UIBase ��ӹ޾� �����ϴ� UI ����
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

        Debug.LogAssertion($"<color=orange>timer ��� �ʿ�</color>");
    }

    private void UpdateTimeSlider(float currentTime, float totalTime) {
        if (timeSlider != null) timeSlider.value = currentTime / totalTime;
        if (timeText != null) timeText.text = currentTime.ToString("F0");
    }

    private void Reset() {
        timeSlider = GetComponentInChildren<Slider>();
        timeText = GetComponentInChildren<TextMeshProUGUI>();

        if (!timeText) Debug.LogAssertion($"<color=red>Time Text UI �ʿ�</color>");
    }
}
