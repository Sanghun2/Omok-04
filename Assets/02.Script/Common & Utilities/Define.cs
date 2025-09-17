using UnityEngine;

public class Define
{
    public class Value
    {
        public const float DEFAULT_TIME = 25f;
    }

    public class State
    {
        public enum GameState
        {
            NotStarted,
            Ready,
            InProgress,
            Ended
        }
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
            White,
            Black,
        }
    }
}
