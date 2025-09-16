using System;
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
        if (currentUser == null)
        {
            GetDummyData();
        }
            return currentUser;
    }


    // [Obsolete("개발용 임시 함수입니다. 실제 게임에서는 사용하지 마세요.")]
    private UserData GetDummyData()
    {
        return new UserData
        {
            _id = "0000",
            username = "DummyUser",
            wins = 10,
            losses = 5,
            winStreak = 3,
            rank = 3,
            rankpoint = 2
        };
    }
}