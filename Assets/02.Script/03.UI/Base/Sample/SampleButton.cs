using UnityEngine;

public class SampleButton : ButtonBase
{
    protected override void ButtonAction() {
        LogTestText(); // 이 부분에서 버튼 작동
    }

    private void LogTestText() {
        Debug.LogAssertion($"This is test log.");
    }
}
