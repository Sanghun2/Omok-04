using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Networking;

public class LoginManager : MonoBehaviour
{

    [SerializeField] TMP_InputField usernameInput;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] Button showPasswordButton;
    [SerializeField] Button loginButton;
    [SerializeField] TextMeshProUGUI statusText;

    [SerializeField] GameObject errorPopup;


    void OnEnable()
    {
        statusText.text = "로그인";
    }
    void Start()
    {
        loginButton.onClick.AddListener(() =>
        {
            StartCoroutine(Login());
        });
        showPasswordButton.onClick.AddListener(() =>
        {
            ShowPassword();
        });
        
    }

    IEnumerator Login()
    {
        statusText.text = "로그인 시도 중...";

        string username = usernameInput.text;
        string password = passwordInput.text;

        string json = $"{{ \"username\": \"{username}\", \"password\": \"{password}\" }}";

        using (UnityWebRequest request = new UnityWebRequest("https://omokserver-04.onrender.com/auth/login", "POST"))
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
                Managers.UserInfo.SetCurrentUser(loggedInUser);


                Debug.Log("서버 응답: " + request.downloadHandler.text);
                Debug.Log("유저 아이디: " + loggedInUser._id);
                Debug.Log("유저 닉네임: " + loggedInUser.username);
                Debug.Log("유저 승: " + loggedInUser.wins);
                Debug.Log("유저 패: " + loggedInUser.losses);
                Debug.Log("유저 연승: " + loggedInUser.winStreak);
                Debug.Log("유저 랭크: " + loggedInUser.rank);
                Debug.Log("유저 랭크포인트: " + loggedInUser.rankpoint);

                statusText.text = "로그인 성공!";

                
                Managers.Scene.ShowScene(Define.Type.Scene.MainMenu);
                
            }
            else
            {
                errorPopup.SetActive(true);
                Debug.LogError("서버 응답: " + request.error);
                statusText.text = "로그인 실패";
            }
        }


    }
    
    void ShowPassword()
    {
        if (passwordInput.contentType == TMP_InputField.ContentType.Password)
        {
            passwordInput.contentType = TMP_InputField.ContentType.Standard;
            passwordInput.ForceLabelUpdate();
        }
        else
        {
            passwordInput.contentType = TMP_InputField.ContentType.Password;
            passwordInput.ForceLabelUpdate();
        }
    }
}