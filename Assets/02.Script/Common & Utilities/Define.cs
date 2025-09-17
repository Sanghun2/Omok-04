using UnityEngine;

public class Define
{
    public class Value
    {
        public const float DEFAULT_TIME = 25f;
        public const int BoardRow = 15;
        public const int BoardCol = 15;
    }

    public class State
    {

    }

    public class Type
    {
        public enum Scene
        {
            None = -1,
            LogIn = 0,
            MainMenu,
            InGame,
        }

        public enum Game
        {
            Single,
            Local,
            Multi
        }

        public enum GameState
        {
            NotStarted,
            InProgress,
            Ended
        }

        public enum Player
        {
            Player1,
            Player2,
        }

        public enum GameLevel
        {
            Easy,
            Normal,
            Hard
        }
        
        public enum StoneColor 
        {
            None,
            Black,
            White
        }
    }
}
