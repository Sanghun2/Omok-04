

[System.Serializable]
public class UserData
{
    public string _id;
    public string username;
    public int wins;
    public int losses;
    public int winStreak;
}

[System.Serializable]
public class LoginResponse
{
    public string message;
    public UserData user;
}