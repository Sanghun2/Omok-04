using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UIBase 상속받아 구현하는 UI 샘플
/// </summary>
public class SampleTimeTextUI : UIBase
{
    [SerializeField] Slider timeSlider;
    [SerializeField] TextMeshProUGUI timeText;

    public override void InitUI() {
        Timer targetTimer = Managers.Time.GetTimer(Define.Type.Player.Player1);
        targetTimer.OnTimeChanged -= UpdateTimeSlider;
        targetTimer.OnTimeChanged += UpdateTimeSlider;
    }

    private void UpdateTimeSlider(float currentTime, float totalTime) {
        timeSlider.value = currentTime / totalTime;
        timeText.text = currentTime.ToString("F0");
    }
}
