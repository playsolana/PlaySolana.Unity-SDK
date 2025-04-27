using UnityEditor;
using UnityEngine;

public class Psg1Debugger : EditorWindow {
    [MenuItem("Window/PlaySolana/PSG1 Debugger")]
    public static void ShowWindow() {
        GetWindow<Psg1Debugger>("PSG1 Emulator");
    }

    private void OnGUI() {
        GUILayout.Label("PSG1 Emulator", EditorStyles.boldLabel);
        // Add your GUI code here
        if (GUILayout.Button("Connect")) {
            // Connect to the PSG1 device
        }

        if (GUILayout.Button("Disconnect")) {
            // Disconnect from the PSG1 device
        }

    }
}