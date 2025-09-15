using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserProfile : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI usernameText;
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private TextMeshProUGUI recordText;
    [SerializeField] private TextMeshProUGUI winStreakText;
    [SerializeField] private Image[] rankPointBar;

    void OnEnable()
    {
        UpdateProfileUI();
    }

    public void UpdateProfileUI()
    {

        UserData currentUser = Managers.UserInfo.GetCurrentUser();

        if (currentUser == null)
        {
            Debug.Log("유저 정보가 없습니다");
            return;
        }

        int totalGames = currentUser.wins + currentUser.losses;
        float winRate = totalGames > 0 ? (float)currentUser.wins / totalGames * 100f : 0f;


        usernameText.text = currentUser.username;
        rankText.text = $"급수: {currentUser.rank}급";
        recordText.text = $"승: {currentUser.wins} / 패: {currentUser.losses} / 승률: {winRate:F0}%";
        winStreakText.text = $"{currentUser.winStreak}연승";

        for (int i = 0; i < rankPointBar.Length; i++)
        {
            rankPointBar[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < currentUser.rankpoint + 3; i++)
        {
            rankPointBar[i].gameObject.SetActive(true);
        }
        
    }
}