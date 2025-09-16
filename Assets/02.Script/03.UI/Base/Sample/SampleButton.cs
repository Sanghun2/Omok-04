using UnityEngine;

public class SampleButton : ButtonBase
{
    protected override void ButtonAction() {
        LogTestText(); // �� �κп��� ��ư �۵�
    }

    private void LogTestText() {
        Debug.LogAssertion($"This is test log.");
    }
}
