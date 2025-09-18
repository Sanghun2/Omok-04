using UnityEngine;
using UnityEngine.Events;

public class TurnManager
{
    private Define.Type.Player currentPlayer = Define.Type.Player.Player1;
    public UnityEvent<Define.Type.Player> OnTurnChanged = new UnityEvent<Define.Type.Player>();

    
    public void StartGame() // GameState를 InProgress로 바꾸고 흑돌의 차례로 만드는 함수
    {
        if (Managers.Game.CurrentGameState == Define.State.GameState.NotStarted)
        {
            SetTurn(Define.Type.Player.Player1);
        }
    }

    
    public void SwitchTurn() // 게임이 진행중일 때 차례를 다음 사람에게 넘기는 함수
    {
        if (Managers.Game.CurrentGameState != Define.State.GameState.InProgress)
        {
            return;
        }

        
        Define.Type.Player nextPlayer = (currentPlayer == Define.Type.Player.Player1) ? Define.Type.Player.Player2 : Define.Type.Player.Player1;
        SetTurn(nextPlayer);
    }

    private void SetTurn(Define.Type.Player player)
    {
        currentPlayer = player;

        
        OnTurnChanged?.Invoke(currentPlayer); 

        Debug.LogAssertion($"현재 턴: {currentPlayer}");
    }

    
    public Define.Type.Player GetCurrentPlayer()
    {
        return currentPlayer;
    }

    
    public void EndGame(Define.Type.Player winner)
    {
        if (Managers.Game.CurrentGameState == Define.State.GameState.InProgress)
        {
            Debug.Log($"게임 종료! 승자: {winner}");
            // 게임 종료 UI 표시, 재경기 나가기 버튼 활성화 기능 추가 가능
            // 게임을 다시 시작하기 위해서 GameState를 NotStarted로 변경 필요
        }
    }
}