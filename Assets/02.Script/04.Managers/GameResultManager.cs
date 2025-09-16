using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GameResultManager : MonoBehaviour
{
    // 게임 종료 시 이 함수를 호출해주세요.
    // isWin이 true이면 승리, false이면 패배입니다.
    public void SendGameResult(bool isWin)
    {
        string userId = Managers.UserInfo.GetCurrentUser()._id;
        string result = isWin ? "win" : "loss";

        StartCoroutine(SendResultCoroutine(userId, result));
    }

    private IEnumerator SendResultCoroutine(string userId, string result)
    {
        Debug.Log($"{userId} 유저의 {result} 결과를 서버에 전송합니다.");
        string json = $"{{ \"userId\": \"{userId}\", \"result\": \"{result}\" }}";
        string url = "https://omokserver-04.onrender.com/api/record";

        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                //서버의 최신 정보로 유저 정보를 갱신
                string responseJson = request.downloadHandler.text;
                UserData updatedUser = JsonUtility.FromJson<UserData>(responseJson);
                Managers.UserInfo.SetCurrentUser(updatedUser);

                string userInfoLog = $"--- 유저 정보 갱신 완료 ---\n" +
                                     $"아이디: {updatedUser.username}\n" +
                                     $"승리: {updatedUser.wins}\n" +
                                     $"패배: {updatedUser.losses}\n" +
                                     $"연승: {updatedUser.winStreak}\n" +
                                     $"랭크: {updatedUser.rank}급\n" +
                                     $"랭크포인트: {updatedUser.rankpoint}점\n" +
                                     $"---------------------------";
                Debug.Log(userInfoLog);

                Debug.Log("전적 업데이트 성공");
            }
            else
            {
                Debug.Log("전적 업데이트 실패: " + request.error);
            }
        }
    }
}