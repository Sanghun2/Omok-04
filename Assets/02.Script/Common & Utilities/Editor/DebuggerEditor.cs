using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Debugger))]
public class DebuggerEditor : Editor
{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        var _script = (Debugger)target;
        GUILayout.Space(20);

        GUI.color = Color.orange;
        if (GUILayout.Button("Change Scene")) {
            _script.Test_ShowScene();
        }
        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Login")){
                _script.Test_GoToLogIn();
            }
            if (GUILayout.Button("Main menu")) {
                _script.Test_GoToMenu();
            }
            if (GUILayout.Button("In Game")) {
                _script.Test_GoToInGame();
            }
        }
        GUILayout.EndHorizontal();
        GUI.color = Color.white;

        GUI.enabled = EditorApplication.isPlaying;
        GUILayout.Space(20);
        GUI.color = Color.cyan;
        if (GUILayout.Button("Toggle Timer UI")) {
            _script.Test_ToggleTimerObj();
        }
        GUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("Set Timer")) {
                _script.Test_SetTimeAsDefault();
            }
            if (GUILayout.Button("Start Timer")) {
                _script.Test_StartCount();
            }
            GUI.color = Color.white;
        }
        GUILayout.EndHorizontal();
        GUI.enabled = true;
    }
}
