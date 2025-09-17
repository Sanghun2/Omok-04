using UnityEngine;

public class Define
{
    public class Value
    {
        public const float DEFAULT_TIME = 25f;
        public const int BoardRow = 15;
        public const int BoardCol = 15;

        public class Photon
        {
            public const string PLAYER_TYPE = "playerType";
        }
    }

    public class Tag
    {
        public const string MAIN_CANVAS_TAG = "Main Canvas";
        public const string FRONT_CANVAS_TAG = "Front Canvas";
        public const string BACKGROUND_CANVAS_TAG = "Background Canvas";
    }

    public class Path
    {
        // Prefab
        public const string POP_UP_UI_PATH = "UI/Pop Up UI";
        public const string FRONT_CANVAS_PATH = "UI/Front Canvas";
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

        public enum GameResult 
        { 
            NONE, 
            BlackStoneWin, 
            WhiteStoneWin, 
            DRAW 
        }
    }

    public class Type
    {
        public enum PopUpParent {
            Front,
            Main,
        }

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
            Black,
            White
        }
    }
}
