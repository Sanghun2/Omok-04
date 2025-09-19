using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeUI : UIBase
{
    [SerializeField] private Slider timeSlider;
    [SerializeField] private TextMeshProUGUI timerText;

    public override void InitUI() {
        var timer = Managers.Time.Timer;
        timer.OnTimeChanged -= UpdateTimerUI;
        timer.OnTimeChanged += UpdateTimerUI;
    }

    // TextMeshPro UI °»½Å
    private void UpdateTimerUI(float currentTime, float totalTime) {
        int seconds = Mathf.RoundToInt(currentTime);
        timerText.text = seconds.ToString();
        timeSlider.value = currentTime / totalTime;
    }
}
