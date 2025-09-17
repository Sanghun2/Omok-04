using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;


public class LogoutManager : MonoBehaviour
{
    [SerializeField] private Button logoutButton;

    void Start()
    {
        if (logoutButton != null)
        {
            logoutButton.onClick.AddListener(LogoutAndReturnToLogin);
        }
        else
        {
            Debug.LogError("Logout Button이 연결되지 않았습니다!");
        }
    }
    public void LogoutAndReturnToLogin()
    {
        StartCoroutine(LogoutCoroutine());
    }
    private IEnumerator LogoutCoroutine()
    {
        // string url = "http://localhost:3000/auth/logout"; 
        string url = "https://omokserver-04.onrender.com/auth/logout";

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.downloadHandler = new DownloadHandlerBuffer();
            
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("서버로부터 로그아웃 성공 응답을 받았습니다.");
            }
            else
            {
                Debug.LogError("로그아웃 요청 실패: " + request.error);
            }
        }

        if (Managers.UserInfo != null)
        {
            Managers.UserInfo.SetCurrentUser(null);
        }
        Managers.Scene.ShowScene(Define.Type.Scene.LogIn);
    }
}