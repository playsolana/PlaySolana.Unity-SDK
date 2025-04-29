using PlaySolanaSdk;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlaySolanaSdk {
    public class Psg1Debugger : EditorWindow {

        private void CreatePSG1Device() => InputSystem.AddDevice<PSG1>().MakeCurrent();

        private void CreateMirrorGameObject() {
            if (GameObject.Find("PSG1InputMirror") == null) {
                var go = new GameObject("PSG1InputMirror");
                go.AddComponent<PSG1InputMirror>();
                if (Application.isPlaying) DontDestroyOnLoad(go);
            }
        }

        private void DestroyMirrorGameObject() {
            var go = GameObject.Find("PSG1InputMirror");
            if (go != null) {
                DestroyImmediate(go);
            }
        }
        

        [MenuItem("Window/PlaySolana/PSG1 Debugger")]
        public static void ShowWindow() {
            GetWindow<Psg1Debugger>("PSG1 Emulator");
        }

        private void OnGUI() {
            GUILayout.Label("PSG1 Emulator", EditorStyles.boldLabel);

            if (GUILayout.Button("Connect")) {
                bool hasPsg = false;
                          
                // If PSG1 device not found, add a new one
                // If more gamepads were found, check if some of them is a PSG1 device
                if(Gamepad.all.Count > 0) {
                    foreach (var device in PSG1.all) {
                        if (device.name == "PSG1") {
                            PSG1.current = (PSG1) device;
                            hasPsg = true;
                            Debug.Log("Connected to PSG1 device: " + device.name);
                        }
                    }

                    if (hasPsg == false) CreatePSG1Device();
                    
                } else CreatePSG1Device();

                CreateMirrorGameObject();   
            }

            if (GUILayout.Button("Disconnect")) {
                // Disconnect from the PSG1 device
                var psg1 = PSG1.current;

                if (psg1 != null) {
                    InputSystem.RemoveDevice(psg1.device);
                    DestroyMirrorGameObject();

                    foreach (var device in PSG1.all) {
                        if (device.name == "PSG1") {
                            PSG1.current = null;
                            break;
                        }
                    }

                    Debug.Log("Disconnected PSG1 device");
                } else {
                    Debug.Log("No PSG1 device found to disconnect.");
                }
            }

        }
    }
}