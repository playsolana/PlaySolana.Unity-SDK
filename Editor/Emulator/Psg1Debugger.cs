using PlaySolanaSdk;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace PlaySolanaSdk {
    public class Psg1Debugger : EditorWindow {
        public readonly string UXML_PATH = "Packages/com.playsolana.psdk/Editor/Emulator/UI/Main.uxml";
        public readonly string USS_PATH = "Packages/com.playsolana.psdk/Editor/Emulator/UI/Styles.uss";

        private bool isConnected = false;
        private Button connectButton;
        private Button disconnectButton;
        private Label warningConnect;


        private void CreatePSG1Device() {
            InputSystem.AddDevice<PSG1>().MakeCurrent();
            isConnected = true;
        } 

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
            var window = GetWindow<Psg1Debugger>();
            window.titleContent = new GUIContent("PSG1 Emulator");

            // To avoid resize, set a fixed min and max size
            window.minSize = new Vector2(400, 800);
            window.maxSize = new Vector2(400, 800);
        }

        private void OnEnable() {
            Debug.Log("Linked UXML_PATH: " + UXML_PATH);
            VisualTreeAsset original = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UXML_PATH);
            TemplateContainer treeAsset = original.CloneTree();
            rootVisualElement.Add(treeAsset);


            Debug.Log("Linked USS_PATH: " + USS_PATH);
            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(USS_PATH);
            rootVisualElement.styleSheets.Add(styleSheet);

            connectButton = rootVisualElement.Q<Button>("connectButton");
            disconnectButton = rootVisualElement.Q<Button>("disconnectButton");
            warningConnect = rootVisualElement.Q<Label>("helperBox");

            connectButton.RegisterCallback<ClickEvent>(evt => {
                Debug.Log("Button clicked: " + connectButton.name);
                ConnectPSG1();
                UpdateButtonVisibility();
            });

            disconnectButton.RegisterCallback<ClickEvent>(evt => {
                Debug.Log("Button clicked: " + disconnectButton.name);
                DisconnectPSG1();
                UpdateButtonVisibility();
            });                    

            UpdateButtonVisibility();
        }

        private void ConnectPSG1() {
                          
            // If PSG1 device not found, add a new one
            // If more gamepads were found, check if some of them is a PSG1 device
            if(Gamepad.all.Count > 0) {
                foreach (var device in PSG1.all) {
                    if (device.name == "PSG1") {
                        PSG1.current = (PSG1) device;
                        isConnected = true;
                        Debug.Log("Connected to PSG1 device: " + device.name);
                    }
                }

                if (isConnected == false) CreatePSG1Device();
                
            } else CreatePSG1Device();

            CreateMirrorGameObject();   
        }

        private void DisconnectPSG1() {
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

                isConnected = false;
                Debug.Log("Disconnected PSG1 device");
            } else {
                Debug.Log("No PSG1 device found to disconnect.");
            }
        }

        private void UpdateButtonVisibility() {
            warningConnect.style.display = isConnected ? DisplayStyle.None : DisplayStyle.Flex;
            connectButton.style.display = isConnected ? DisplayStyle.None : DisplayStyle.Flex;
            disconnectButton.style.display = isConnected ? DisplayStyle.Flex : DisplayStyle.None;
        }

    }
}