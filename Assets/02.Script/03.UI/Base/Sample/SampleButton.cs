using UnityEngine;

public class SampleButton : ButtonBase
{
    protected override void ButtonAction() {
        TestFunc(); // 이 부분에서 버튼 작동
    }

    private void TestFunc() {
        Debug.LogAssertion($"This is test log.");
    }
}
