using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Networking;

public class RegistrationManager : MonoBehaviour
{

    [SerializeField] TMP_InputField usernameInput;
    [SerializeField] TMP_InputField passwordInput;
    [SerializeField] TMP_InputField confirmPasswordInput;
    [SerializeField] Button registerButton;
    [SerializeField] TextMeshProUGUI statusText;

    [SerializeField] GameObject errorPopup;


    void Start()
    {
        // 버튼이 클릭되었을 때 Register 함수를 실행하도록 연결
        registerButton.onClick.AddListener(() => {
            StartCoroutine(Register());
        });
    }

    IEnumerator Register()
    {
        statusText.text = "회원가입 시도 중...";

        // 서버로 보낼 JSON 데이터 만들기
        string username = usernameInput.text;
        string password = passwordInput.text;
        string confirmPassword = confirmPasswordInput.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            statusText.text = "아이디와 비밀번호를 모두 입력해주세요.";
            yield break;
        }

        if (password != confirmPassword)
        {
            statusText.text = "비밀번호가 일치하지 않습니다.";
            yield break;
        }

        string json = $"{{ \"username\": \"{username}\", \"password\": \"{password}\" }}";

        


        using (UnityWebRequest request = new UnityWebRequest("https://omokserver-04.onrender.com/auth/register", "POST"))
        {
            // 데이터와 헤더 설정
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // 서버에 요청 보내고 응답 기다리기
            yield return request.SendWebRequest();

            // 응답 결과 확인
            if (request.result == UnityWebRequest.Result.Success)
            {
                // 성공
                Debug.Log("서버 응답: " + request.downloadHandler.text);
                statusText.text = "회원가입 성공!";
            }
            else
            {
                // 실패
                errorPopup.SetActive(true);
                Debug.LogError("서버 응답: " + request.error);
                statusText.text = "회원가입 실패";
            }
        }
    }
}