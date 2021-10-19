using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

[AddComponentMenu("Emote Player/Emote Preview")]
public class EmotePreview : MonoBehaviour 
{
    private EmotePlayer emotePlayer;
    private EmoteTextureCurve textureCurve;

    private int focusedMenuPageIndex = 0;
    private int curMenuPageIndex = 0;
    private int curMenuItemIndex = 0;
    private int focusedMenuItemIndex = 0;
    private List<int> maxMenuItemCount;
    private int maxMenuPageCount;
    private float prevAxisHorizontal = 0;
    private float prevAxisVertical = 0;
    private bool upPressed = false;
    private bool downPressed = false;
    private bool rightPressed = false;
    private bool leftPressed = false;
    private bool submitPressed = false;
    private bool cancelPressed = false;
    private string menuText;
    private bool modified;

    public RawImage board;
    public Text console;

    enum KeyMask : int{
        RIGHT = 1,
        LEFT = 2,
        SUBMIT = 4,
        CANCEL = 8,
    };

    const string MENU_ACTIVE_KEY = "emprvMenuActive";
    const string MESH_DIVISION_RATIO_KEY = "emprvMeshDivisionRatio";
    const string MAIN_TIMELINE_LABEL_KEY = "emprvMainTimelineLabel";
    const string DIFF_TIMELINE_SLOT1_KEY = "emprvDiffTimelineSlot1";
    const string DIFF_TIMELINE_SLOT2_KEY = "emprvDiffTimelineSlot2";
    const string DIFF_TIMELINE_SLOT3_KEY = "emprvDiffTimelineSlot3";
    const string DIFF_TIMELINE_SLOT4_KEY = "emprvDiffTimelineSlot4";
    const string DIFF_TIMELINE_SLOT5_KEY = "emprvDiffTimelineSlot5";
    const string DIFF_TIMELINE_SLOT6_KEY = "emprvDiffTimelineSlot6";

    const string TEXCURVE_TOP_RATE_KEY = "emprvTexCurveTopRate";
    const string TEXCURVE_TOP_SHIFT_KEY = "emprvTexCurveTopShift";
    const string TEXCURVE_BOTTOM_RATE_KEY = "emprvTexCurveBottomRate";
    const string TEXCURVE_BOTTOM_SHIFT_KEY = "emprvTexCurveBottomShift";
    const string TEXCURVE_MESH_COUNT_KEY = "emprvTexCurveMeshCount";

    void Load() {
        board.enabled = console.enabled = (PlayerPrefs.GetInt(MENU_ACTIVE_KEY, 1) == 1);
        emotePlayer.meshDivisionRatio = PlayerPrefs.GetFloat(MESH_DIVISION_RATIO_KEY, 1.0f);
        emotePlayer.mainTimelineLabel = PlayerPrefs.GetString(MAIN_TIMELINE_LABEL_KEY, "");
        emotePlayer.diffTimelineSlot1 = PlayerPrefs.GetString(DIFF_TIMELINE_SLOT1_KEY, "");
        emotePlayer.diffTimelineSlot2 = PlayerPrefs.GetString(DIFF_TIMELINE_SLOT2_KEY, "");
        emotePlayer.diffTimelineSlot3 = PlayerPrefs.GetString(DIFF_TIMELINE_SLOT3_KEY, "");
        emotePlayer.diffTimelineSlot4 = PlayerPrefs.GetString(DIFF_TIMELINE_SLOT4_KEY, "");
        emotePlayer.diffTimelineSlot5 = PlayerPrefs.GetString(DIFF_TIMELINE_SLOT5_KEY, "");
        emotePlayer.diffTimelineSlot6 = PlayerPrefs.GetString(DIFF_TIMELINE_SLOT6_KEY, "");

        if (textureCurve != null) {
            textureCurve.topRate     = PlayerPrefs.GetFloat(TEXCURVE_TOP_RATE_KEY,     0.0f);
            textureCurve.topShift    = PlayerPrefs.GetFloat(TEXCURVE_TOP_SHIFT_KEY,    0.0f);
            textureCurve.bottomRate  = PlayerPrefs.GetFloat(TEXCURVE_BOTTOM_RATE_KEY,  0.0f);
            textureCurve.bottomShift = PlayerPrefs.GetFloat(TEXCURVE_BOTTOM_SHIFT_KEY, 0.0f);
            textureCurve.meshCount   = PlayerPrefs.GetInt  (TEXCURVE_MESH_COUNT_KEY, 10);
        }
    }

    void Save() {
        PlayerPrefs.SetInt(MENU_ACTIVE_KEY, console.enabled ? 1 : 0);
        PlayerPrefs.SetFloat(MESH_DIVISION_RATIO_KEY, emotePlayer.meshDivisionRatio);
        PlayerPrefs.SetString(MAIN_TIMELINE_LABEL_KEY, emotePlayer.mainTimelineLabel);
        PlayerPrefs.SetString(DIFF_TIMELINE_SLOT1_KEY, emotePlayer.diffTimelineSlot1);
        PlayerPrefs.SetString(DIFF_TIMELINE_SLOT2_KEY, emotePlayer.diffTimelineSlot2);
        PlayerPrefs.SetString(DIFF_TIMELINE_SLOT3_KEY, emotePlayer.diffTimelineSlot3);
        PlayerPrefs.SetString(DIFF_TIMELINE_SLOT4_KEY, emotePlayer.diffTimelineSlot4);
        PlayerPrefs.SetString(DIFF_TIMELINE_SLOT5_KEY, emotePlayer.diffTimelineSlot5);
        PlayerPrefs.SetString(DIFF_TIMELINE_SLOT6_KEY, emotePlayer.diffTimelineSlot6);

        if (textureCurve != null) {
            PlayerPrefs.SetFloat(TEXCURVE_TOP_RATE_KEY,     textureCurve.topRate);
            PlayerPrefs.SetFloat(TEXCURVE_TOP_SHIFT_KEY,    textureCurve.topShift);
            PlayerPrefs.SetFloat(TEXCURVE_BOTTOM_RATE_KEY,  textureCurve.bottomRate);
            PlayerPrefs.SetFloat(TEXCURVE_BOTTOM_SHIFT_KEY, textureCurve.bottomShift);
            PlayerPrefs.SetInt  (TEXCURVE_MESH_COUNT_KEY,   textureCurve.meshCount);
        }
    }

    void Start() {
        emotePlayer = this.GetComponent<EmotePlayer>();
        textureCurve = this.GetComponent<EmoteTextureCurve>();
        Load();
        ReloadData();
    }

    void BeginMenu() {
        modified = false;

        curMenuPageIndex = 0;
        cancelPressed = Input.GetButtonDown("Cancel");
        maxMenuItemCount = new List<int>();

        if (console.enabled) {
            float axisHorizontal = Input.GetAxisRaw("Horizontal");
            float axisVertical = Input.GetAxisRaw("Vertical");
            downPressed = prevAxisVertical >= 0 && axisVertical < 0;
            upPressed = prevAxisVertical <= 0 && axisVertical > 0;
            leftPressed = prevAxisHorizontal >= 0 && axisHorizontal < 0;
            rightPressed = prevAxisHorizontal <= 0 && axisHorizontal > 0;
            submitPressed = Input.GetButtonDown("Submit");
        } else {
            downPressed = false;
            upPressed = false;
            leftPressed = false;
            rightPressed = false;
            submitPressed = false;
        }
    }

    void BeginMenuPage() {
        menuText = "";
        curMenuItemIndex = 0;
    }

    void EndMenuPage() {
        if (curMenuPageIndex == focusedMenuPageIndex) 
            console.text = menuText;
        maxMenuItemCount.Add(curMenuItemIndex);
        curMenuPageIndex++;
    }

    void BeginMenuItem() {
    }

    void EndMenuItem() {
        curMenuItemIndex++;
    }

    void EndMenu() {
        maxMenuPageCount = maxMenuItemCount.Count;
        prevAxisHorizontal = Input.GetAxis("Horizontal");
        prevAxisVertical = Input.GetAxis("Vertical");
        if (upPressed) {
            focusedMenuItemIndex--;
            if (focusedMenuItemIndex < 0) {
                focusedMenuPageIndex = (focusedMenuPageIndex - 1 + maxMenuItemCount.Count) % maxMenuItemCount.Count;
                focusedMenuItemIndex = maxMenuItemCount[focusedMenuPageIndex] - 1;
            }
        }
        if (downPressed) {
            focusedMenuItemIndex++;
            if (focusedMenuItemIndex >= maxMenuItemCount[focusedMenuPageIndex]) {
                focusedMenuPageIndex = (focusedMenuPageIndex + 1) % maxMenuItemCount.Count;
                focusedMenuItemIndex = 0;
            }
        }
        if (cancelPressed) {
            board.enabled = ! board.enabled;
            console.enabled = ! console.enabled;
            modified = true;
        }
        if (modified)
            Save();
   }

    string GetIndexLabel(string label) {
        if (focusedMenuItemIndex == curMenuItemIndex)
            return "* " + label;
        else
            return "  " + label;
    }

    bool MenuButton(string label, KeyMask keyMask) {
        menuText += "[" + label + "]";

        if (focusedMenuPageIndex != curMenuPageIndex
            || focusedMenuItemIndex != curMenuItemIndex)
            return false;

        if (rightPressed && (keyMask & KeyMask.RIGHT) != 0)
            return true;
        if (leftPressed && (keyMask & KeyMask.LEFT) != 0)
            return true;
        if (submitPressed && (keyMask & KeyMask.SUBMIT) != 0)
            return true;
        if (cancelPressed && (keyMask & KeyMask.CANCEL) != 0)
            return true;

        return false;
    }

    void MenuLabel(string label) {
        menuText += GetIndexLabel(label);
    }

    void MenuCaption(string label) {
        menuText += label;
        if (maxMenuPageCount >= 2) {
            menuText += String.Format("({0}/{1})", curMenuPageIndex + 1, maxMenuPageCount);
        }
        MenuNewline();
        MenuNewline();
    }

    void MenuNewline() {
        menuText += "\n";
    }

    void Update() {
        BeginMenu();

        BeginMenuPage();

        MenuCaption("Parameters");

        BeginMenuItem();
        MenuLabel("Reload E-mote Data");
        MenuNewline();
        if (MenuButton("Submit", KeyMask.SUBMIT))
            ReloadData();
        MenuNewline();
        EndMenuItem();

        emotePlayer.meshDivisionRatio = Slider("Mesh division ratio: {0:0.00}", emotePlayer.meshDivisionRatio,
                                               1.0f, 0.0f, 1.0f, 0.1f);

        string[] labels = emotePlayer.mainTimelineLabels;
        emotePlayer.mainTimelineIndex = SelectTimeline(labels, emotePlayer.mainTimelineIndex, "Main");

        labels = emotePlayer.diffTimelineLabels;
        emotePlayer.diffTimelineSlot1Index = SelectTimeline(labels, emotePlayer.diffTimelineSlot1Index, "Diff#1");
        emotePlayer.diffTimelineSlot2Index = SelectTimeline(labels, emotePlayer.diffTimelineSlot2Index, "Diff#2");
        emotePlayer.diffTimelineSlot3Index = SelectTimeline(labels, emotePlayer.diffTimelineSlot3Index, "Diff#3");
        emotePlayer.diffTimelineSlot4Index = SelectTimeline(labels, emotePlayer.diffTimelineSlot4Index, "Diff#4");
        emotePlayer.diffTimelineSlot5Index = SelectTimeline(labels, emotePlayer.diffTimelineSlot5Index, "Diff#5");
        emotePlayer.diffTimelineSlot6Index = SelectTimeline(labels, emotePlayer.diffTimelineSlot6Index, "Diff#6");

        EndMenuPage();

        if (textureCurve != null) {
            var prevModified = modified;
            
            BeginMenuPage();

            MenuCaption("Texture Curve");

            textureCurve.topRate = Slider("Top Rate: {0:0.00}", 
                                          textureCurve.topRate, 0.0f, 0.0f, 1.0f, 0.1f);
            textureCurve.topShift = Slider("Top Shift: {0:0.00}", 
                                          textureCurve.topShift, 0.0f, -1.0f, 1.0f, 0.1f);
            textureCurve.bottomRate = Slider("Bottom Rate: {0:0.00}", 
                                          textureCurve.bottomRate, 0.0f, 0.0f, 1.0f, 0.1f);
            textureCurve.bottomShift = Slider("Bottom Shift: {0:0.00}", 
                                              textureCurve.bottomShift, 0.0f, -1.0f, 1.0f, 0.1f);
            textureCurve.meshCount = (int)Slider("Mesh Count: {0}", 
                                                 (float)textureCurve.meshCount, 10.0f, 1.0f, 30.0f, 1.0f);
            
            EndMenuPage();

            if (! prevModified && modified)
                textureCurve.UpdateMesh();
        }

        EndMenu();
    }

    float Slider(string label, float curValue, float initialValue, float minValue, float maxValue, float valueStride) {
        BeginMenuItem();
        MenuLabel(String.Format(label, curValue));
        MenuNewline();
        var prevValue = curValue;
        if (MenuButton("<", KeyMask.RIGHT))
            curValue = Mathf.Min(maxValue, curValue + valueStride);
        if (MenuButton(">", KeyMask.LEFT))
            curValue = Mathf.Max(minValue, curValue - valueStride);
        if (MenuButton("Reset", KeyMask.SUBMIT))
            curValue = initialValue;
        if (prevValue != curValue)
            modified = true;
        MenuNewline();
        EndMenuItem();
        return curValue;
    }

    int SelectTimeline(string[] labels, int index, string label) {
        if (labels.Length == 0)
            return 0;
        int prevIndex = index;
        BeginMenuItem();
        MenuLabel(String.Format("{0}: {1}", label, labels[index]));
        MenuNewline();
        if (MenuButton("<", KeyMask.LEFT))
            index--;
        if (MenuButton(">", KeyMask.RIGHT))
            index++;
        if (MenuButton("Reset", KeyMask.SUBMIT))
            index = 0;
        MenuNewline();
        index = (index + labels.Length) % labels.Length;
        if (index != prevIndex)
            modified = true;
        EndMenuItem();

        return index;
    }

    void ReloadData() {
        StartCoroutine(_ReloadData());
    }

    IEnumerator _ReloadData() {
        string path = Application.dataPath + "/preview.bytes";
        var www = new WWW("file://" + path);
        yield return www;
        if (string.IsNullOrEmpty(www.error)) {
            var emoteAsset = new EmoteAsset();
            emoteAsset.rawFileImage = www.bytes;
            emotePlayer.LoadData(emoteAsset);
            Load();
        }
    }
}