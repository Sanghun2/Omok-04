using UnityEngine;

public class TimeManager
{
    [SerializeField] Timer player1_Timer;
    [SerializeField] Timer player2_Timer;

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
}
