using UnityEngine;
using UnityEngine.Events;

public class TurnManager
{
    
    public enum GameState
    {
        NotStarted,
        InProgress,
        Ended
    }

    
    public enum Player
    {
        None,
        Black,
        White
    }

    
    private GameState currentState = GameState.NotStarted;
    private Player currentPlayer = Player.None;
    public UnityEvent<Player> OnTurnChanged = new UnityEvent<Player>();

   



    
    public void StartGame() // GameState를 InProgress로 바꾸고 흑돌의 차례로 만드는 함수
    {
        if (currentState == GameState.NotStarted)
        {
            currentState = GameState.InProgress;
            SetTurn(Player.Black);
        }
    }

    
    public void SwitchTurn() // 게임이 진행중일 때 차례를 다음 사람에게 넘기는 함수
    {
        if (currentState != GameState.InProgress)
        {
            return;
        }

        
        Player nextPlayer = (currentPlayer == Player.Black) ? Player.White : Player.Black;
        SetTurn(nextPlayer);
    }

    
    private void SetTurn(Player player)
    {
        currentPlayer = player;

        
        OnTurnChanged?.Invoke(currentPlayer); 

        Debug.Log($"현재 턴: {currentPlayer}");
    }

    
    public Player GetCurrentPlayer()
    {
        return currentPlayer;
    }

    
    public GameState GetGameState()
    {
        return currentState;
    }

    
    public void EndGame(Player winner)
    {
        if (currentState == GameState.InProgress)
        {
            currentState = GameState.Ended;
            Debug.Log($"게임 종료! 승자: {winner}");
            // 게임 종료 UI 표시, 재경기 나가기 버튼 활성화 기능 추가 가능
            // 게임을 다시 시작하기 위해서 GameState를 NotStarted로 변경 필요
        }
    }
}