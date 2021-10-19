using UnityEngine;
using System.Collections;


[AddComponentMenu("Emote Player/Emote Generate Test")]
public class EmoteGenerateTest : MonoBehaviour {
    private GameObject[] emoteObjectList = new GameObject[10];
    private int curEmoteIndex = 0;
    private int dir = 1;
    private int count = 1;
    public bool auto = false;
    private EmoteDeviceManager mDeviceManager;
    public GameObject prefab;
    public int initialEmoteModelCount = 0;
    public int space = 0;

    void Start() {
#if UNITY_PSP2 && DEVELOPMENT_BUILD
        UnityEngine.PSVita.Diagnostics.enableHUD = true;
#endif
        mDeviceManager = new EmoteDeviceManager();
        for (int i = 0; i < initialEmoteModelCount; i++)
            GenerateEmotePlayer();
    }

    void OnDestroy() {
        mDeviceManager.Unload();
    }

    void OnGUI() {
        Vector2 guiScreenSize = new Vector2(800, 600);
        float scale = System.Math.Max(Screen.width / guiScreenSize.x, Screen.height / guiScreenSize.y);
        GUIUtility.ScaleAroundPivot(new Vector2(scale, scale), Vector2.zero);
        GUILayout.Space(space);

        if (GUILayout.Button("6: Generate"))
            GenerateEmotePlayer();
        if (GUILayout.Button("2: Erase"))
            EraseEmotePlayer();    
        if (GUILayout.Button("8: Reload"))
            ReloadEmotePlayer();    
        if (GUILayout.Button(System.String.Format("4: Toggle Device ({0}))", mDeviceManager.loaded ? "Loaded" : "Unloaded")))
            ToggleDevice();
        if (GUILayout.Button(System.String.Format(">: Toggle Auto ({0})", auto ? "On" : "Off")))
            ToggleAuto();
        if (GUILayout.Button(EmotePlayer.gpuMeshDeformationEnabled
                             ? "Gpu Mesh (*)"
                             : "Gpu Mesh ( )"))
            EmotePlayer.gpuMeshDeformationEnabled = ! EmotePlayer.gpuMeshDeformationEnabled;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
            GenerateEmotePlayer();
        if (Input.GetKeyDown(KeyCode.JoystickButton0))
            EraseEmotePlayer();    
        if (Input.GetKeyDown(KeyCode.JoystickButton3))
            ReloadEmotePlayer();    
        if (Input.GetKeyDown(KeyCode.JoystickButton2))
            ToggleDevice();    
        if (Input.GetKeyDown(KeyCode.JoystickButton9))
            ToggleAuto();    

        ProcessAuto();
    }

    void ProcessAuto() {
        if (! auto)
            return;

        if (count-- > 0)
            return;

        count = 1;

        if (dir == 1 && curEmoteIndex >= 10)
            dir = -1;
        else if (dir == -1 && curEmoteIndex <= 0)
            dir = 1;

        if (dir == 1)
            GenerateEmotePlayer();
        if (dir == -1)
            EraseEmotePlayer();
    }

    void GenerateEmotePlayer() {
        if (curEmoteIndex >= 10)
            return;
        GameObject player = GameObject.Instantiate(prefab) as GameObject;
        EmotePlayer motion = player.GetComponent<EmotePlayer>();
        motion.transform.position = new Vector3((curEmoteIndex % 2) == 0 ? curEmoteIndex / 2 : -curEmoteIndex / 2 - 1, 0, 0) * 0.2f;
        emoteObjectList[curEmoteIndex] = player;
        curEmoteIndex++;
        M2DebugLog.printf("GenerateEmotePlayer(): {0} players.", curEmoteIndex);
    }

    void EraseEmotePlayer() {
        if (curEmoteIndex <= 0)
            return;
        curEmoteIndex--;
        UnityEngine.Object.Destroy(emoteObjectList[curEmoteIndex]);
        emoteObjectList[curEmoteIndex] = null;
        M2DebugLog.printf("EraseEmotePlayer(): {0} players.", curEmoteIndex);
    }

    void ReloadEmotePlayer() {
        for (int i = 0; i < curEmoteIndex; i++) {
            EmotePlayer motion = emoteObjectList[i].GetComponent<EmotePlayer>();
            motion.LoadData("emote/vr_girl");
        }
        M2DebugLog.printf("ReloadEmotePlayer(): {0} players.", curEmoteIndex);
    }

    void ToggleAuto() {
        auto = ! auto;
        M2DebugLog.printf("ToggleAuto(): {0}.", auto);
    }

    void ToggleDevice() {
        if (! mDeviceManager.loaded)
            mDeviceManager.Load();
        else
            mDeviceManager.Unload();
        M2DebugLog.printf("ToggleDevice(): {0}.", mDeviceManager.loaded);
    }
};

