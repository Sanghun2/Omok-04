using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeUI : UIBase
{
    [SerializeField] private Slider timeSlider;
    [SerializeField] private TextMeshProUGUI timerText;

    private int seconds;
    private bool useFullFormat; 

    public override void InitUI()
    {
        var timer = Managers.Time.Timer;

        timer.OnTimeChanged -= UpdateTimerUI;
        timer.OnTimeChanged += UpdateTimerUI;

        timer.OnTimerInitialized -= ShowRnadomTimerUI;
        timer.OnTimerInitialized += ShowRnadomTimerUI;
    }

    private void UpdateTimerUI(float currentTime, float totalTime)
    {
        seconds = Mathf.RoundToInt(currentTime);

        if (useFullFormat)
        {
            timerText.text = string.Format("남은 시간 : {0} 초", seconds);
        }
        else
        {
            timerText.text = seconds.ToString();
        }

        timeSlider.value = currentTime / totalTime;
    }

    private void ShowRnadomTimerUI()
    {
        float randomValue = Random.value;
        useFullFormat = randomValue < 0.5f;
    }
}