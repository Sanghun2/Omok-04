using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Networking;

public class LoginManager : MonoBehaviour
{

    [SerializeField] TMP_InputField usernameInput;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] Button loginButton;
    [SerializeField] TextMeshProUGUI statusText;

    void Start()
    {
        loginButton.onClick.AddListener(() => {
            StartCoroutine(Login());
        });
    }

    IEnumerator Login()
    {
        statusText.text = "로그인 시도 중...";

        string username = usernameInput.text;
        string password = passwordInput.text;

        string json = $"{{ \"username\": \"{username}\", \"password\": \"{password}\" }}";

        using (UnityWebRequest request = new UnityWebRequest("https://omokserver-04.onrender.com/login", "POST"))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {

                string jsonResponse = request.downloadHandler.text;
                LoginResponse response = JsonUtility.FromJson<LoginResponse>(jsonResponse);
                UserData loggedInUser = response.user;
                UserInfoManager.Instance.SetCurrentUser(loggedInUser);


                Debug.Log("서버 응답: " + request.downloadHandler.text);
                Debug.Log("유저 아이디: " + loggedInUser._id);
                Debug.Log("유저 닉네임: " + loggedInUser.username);
                Debug.Log("유저 승: " + loggedInUser.wins);
                Debug.Log("유저 패: " + loggedInUser.losses);
                Debug.Log("유저 연승: " + loggedInUser.winStreak);

                statusText.text = "로그인 성공!";
                //로그인 후 메인 씬으로 전환하는 기능 추가 가능
            }
            else
            {
                Debug.LogError("서버 응답: " + request.error);
                statusText.text = "로그인 실패: " + request.downloadHandler.text;
            }
        }
        

    }
}