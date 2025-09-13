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
        

        string json = $"{{ \"username\": \"{username}\", \"password\": \"{password}\" }}";


        using (UnityWebRequest request = new UnityWebRequest("https://omokserver-04.onrender.com/register", "POST"))
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
                Debug.LogError("서버 응답: " + request.error);
                statusText.text = "회원가입 실패: " + request.downloadHandler.text;
            }
        }
    }
}