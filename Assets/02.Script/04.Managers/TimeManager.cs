using UnityEngine;

public class TimeManager : IInitializable
{
    public Timer Timer
    {
        get
        {
            if (timer == null) {
                GetTimer();
            }

            return timer;
        }
    }

    private Timer timer;

    public bool IsInit => isInit;
    private bool isInit;



    #region Interface

    /// <summary>
    /// ���� ���� �� player ���� �� ������ timer�� manager�� ����
    /// </summary>
    /// <param name="player"></param>
    /// <param name="timer"></param>
    public void RegisterTimer(Timer timer) {
        this.timer = timer;
        timer.SetTimeAsDefault();
    }
    public Timer GetTimer() {
        if (timer == null) {
            timer = GameObject.FindAnyObjectByType<Timer>(FindObjectsInactive.Include);
        }
        return timer;
    }

    #endregion

    #region Capsule

    public void Initialize() {
        Managers.Turn.OnTurnChanged.AddListener((targetPlayer) => {
            GetTimer().SetTimeAsDefault().StartCount();
        });
        isInit = true;
        Debug.LogAssertion($"<color=green>timer init complete</color>");
    }

    #endregion
}

public partial class Timer : MonoBehaviour
{
    public Timer SetTimeAsDefault() {
        SetTime(Define.Value.DEFAULT_TIME, Define.Value.DEFAULT_TIME);
        return this;
    }
}
