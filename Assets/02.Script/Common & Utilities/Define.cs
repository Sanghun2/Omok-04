using UnityEngine;

public class Define
{
    public class Value {
        public const float DEFAULT_TIME = 25f;
    }

    public class Type {
        public enum Scene {
            None,
            LogIn,
            MainMenu,
            InGame,
        }

        public enum Game {
            Single,
            Local,
            Multi
        }

        public enum Player {
            Player1,
            Player2,
        }
    }
}
