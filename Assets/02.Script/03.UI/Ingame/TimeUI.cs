using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeUI : UIBase
{
    [SerializeField] private Slider timeSlider;
    [SerializeField] private TextMeshProUGUI timerText;

    public override void InitUI()
    {
        var timer = Managers.Time.Timer;

        timer.OnTimeChanged -= UpdateTimerUI;
        timer.OnTimeChanged += UpdateTimerUI;

    }

    private void UpdateTimerUI(float currentTime, float totalTime)
    {
        int seconds = Mathf.RoundToInt(currentTime);
        timerText.text = string.Format("남은 시간 : {0} 초", seconds);
        timeSlider.value = currentTime / totalTime;
    }

}