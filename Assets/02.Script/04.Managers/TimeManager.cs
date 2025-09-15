using System.Security.Cryptography;
using Mono.Cecil.Cil;
using UnityEngine;

public class TimeManager : IInitializable
{
    [SerializeField] Timer player1_Timer;
    [SerializeField] Timer player2_Timer;

    public bool IsInit => isInit;
    private bool isInit;



    #region Interface

    /// <summary>
    /// 게임 시작 시 player 생성 후 각자의 timer를 manager에 연결
    /// </summary>
    /// <param name="player"></param>
    /// <param name="timer"></param>
    public void RegisterTimer(Define.Type.Player player, Timer timer) {
        switch (player) {
            case Define.Type.Player.Player1:
                player1_Timer = timer;
                Debug.LogAssertion($"player1 timer registered");
                break;
            case Define.Type.Player.Player2:
                player2_Timer = timer;
                Debug.LogAssertion($"player2 timer registered");
                break;
            default:
                break;
        }

        timer.SetTimeAsDefault();
    }
    public Timer GetTimer(Define.Type.Player player) {
        switch (player) {
            case Define.Type.Player.Player1:
                return player1_Timer;
            case Define.Type.Player.Player2:
                return player2_Timer;
            default:
                return null;
        }
    }

    #endregion

    #region Capsule

    public void Initialize() {
        Managers.Turn.OnTurnChanged.AddListener((targetPlayer) => {
            try {
                GetTimer(targetPlayer).SetTimeAsDefault();
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
    public void SetTimeAsDefault() {
        SetTime(Define.Value.DEFAULT_TIME, Define.Value.DEFAULT_TIME);
    }
}
