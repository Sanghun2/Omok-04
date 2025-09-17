using System.Security.Cryptography;
using Mono.Cecil.Cil;
using UnityEngine;

public class TimeManager : IInitializable
{
    private Timer timer;

    public bool IsInit => isInit;
    private bool isInit;



    #region Interface

    /// <summary>
    /// 게임 시작 시 player 생성 후 각자의 timer를 manager에 연결
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
            Debug.LogAssertion($"timer 초기화 됨");
        }
        return timer;
    }

    #endregion

    #region Capsule

    public void Initialize() {
        Managers.Turn.OnTurnChanged.AddListener((targetPlayer) => {
            try {
                GetTimer().SetTimeAsDefault();
                Debug.LogAssertion($"<color=green>timer init complete</color>");
                isInit = true;
            }
            catch {
                Debug.LogError($"timer null");
            }
        });
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
