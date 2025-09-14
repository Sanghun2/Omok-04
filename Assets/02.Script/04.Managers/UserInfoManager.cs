using UnityEngine;

public class UserInfoManager : MonoBehaviour
{
    public static UserInfoManager Instance { get; private set; }
    private UserData currentUser;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetCurrentUser(UserData data)
    {
        currentUser = data;
    }

    public UserData GetCurrentUser()
    {
        return currentUser;
    }
}