using UnityEngine;

public class SampleButton : ButtonBase
{
    protected override void ButtonAction() {
        TestFunc(); // �� �κп��� ��ư �۵�
    }

    private void TestFunc() {
        Debug.LogAssertion($"This is test log.");
    }
}
