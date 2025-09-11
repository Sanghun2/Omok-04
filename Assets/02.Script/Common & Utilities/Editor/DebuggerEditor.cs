using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Debugger))]
public class DebuggerEditor : Editor
{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        var script = (Debugger)target;
        GUILayout.Space(20);
        GUI.enabled = EditorApplication.isPlaying;
        GUILayout.BeginHorizontal();
        GUI.color = Color.cyan;
        if (GUILayout.Button("Set Timer")) {
            script.Test_SetTimeAsDefault();
        }
        if (GUILayout.Button("Start Timer")) {
            script.Test_StartCount();
        }
        GUI.color = Color.white;
        GUILayout.EndHorizontal();
        GUI.enabled = true;
    }
}
