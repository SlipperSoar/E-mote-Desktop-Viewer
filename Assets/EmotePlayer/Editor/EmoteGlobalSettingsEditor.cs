using UnityEngine;
using UnityEditor;

// #define EMOTE_GPU_MESH_DEFORMATION_SUPPORT

[CustomEditor(typeof(EmoteGlobalSettings))]
public class EmoteGlobalSettingsEditor : Editor
{
    [PreferenceItem("E-mote")]
    public static void OnPreferencesGUI() { 
        EditorGUILayout.HelpBox(
@"E-mote Preferences is moved to ""Project Settings"".

select menu item,
  ""Edit/Project Settings/E-mote Global Settings""

or select EmotePlayer context menu item ,
  ""E-mote Global Settings""

or
",
            MessageType.Warning);

        if (GUILayout.Button(@"open ""E-mote Global Settings"" now."))
            OpenEmoteProjectSettings();
    }

    [MenuItem("CONTEXT/EmotePlayer/Emote Global Settings")]
    [MenuItem("Edit/Project Settings/Emote Global Settings")]
    static void OpenEmoteProjectSettings() {
        EmotePlayer.touchGlobalSettings();
        AssetDatabase.OpenAsset(EmotePlayer.sGlobalSettings);
    }

    private string _infoString;
    
    private string infoString {
        get {
            if (_infoString == null)
                _infoString = System.String.Format("SDK Version: {0}\nBuild DateTime: {1}",
                                                   EmotePlayer.sdkVersion,
                                                   EmotePlayer.buildDateTime);
            return _infoString;
        }
    }
    
	public override void OnInspectorGUI() {
        EditorGUILayout.HelpBox("E-mote Global Settings", MessageType.Info, true);
        bool toggle;
        toggle = EmotePlayer.toggleAppearanceSettings = EditorGUILayout.Foldout(EmotePlayer.toggleAppearanceSettings, "Appearance");
        if (toggle) {
            EditorGUI.indentLevel++;
            GUIContent[] maskModeLabels = { new GUIContent("STENCIL"), new GUIContent("ALPHA") };
            EmotePlayer.maskMode = EditorGUILayout.Popup(new GUIContent("Mask Mode",
                                                                        "Select whether to use stencil buffer or alpha rendertexture for masking."),
                                                         EmotePlayer.maskMode, maskModeLabels);
            EmotePlayer.maskRegionClipping = EditorGUILayout.Toggle(new GUIContent("Mask Region Clipping", 
                                                                                   "When checked, optimize mask area to minimize rendering area."),
                                                                    EmotePlayer.maskRegionClipping);
            EmotePlayer.alphaCutoff = EditorGUILayout.IntSlider(new GUIContent("Alpha Cutoff",
                                                                               "Pixel which alpha value is less than (n/256.0) will cut off while rendering."),
                                                                EmotePlayer.alphaCutoff, 1, 16);

            
            #if EMOTE_GPU_MESH_DEFORMATION_SUPPORT
            EmotePlayer.gpuMeshDeformationEnabled = EditorGUILayout.Toggle(new GUIContent("Gpu Mesh Deformation Enabled", 
                                                                                   "When checked, mesh deformation is performed using GPU instead of CPU."),
                                                                    EmotePlayer.gpuMeshDeformationEnabled);
                                                                    */
            #endif // EMOTE_GPU_MESH_DEFORMATION_SUPPORT
            EditorGUI.indentLevel--;
        }
        toggle = EmotePlayer.toggleRenderTextureSettings = EditorGUILayout.Foldout(EmotePlayer.toggleRenderTextureSettings, "Render Texture");
        if (toggle) {
            EditorGUI.indentLevel++;
            EmotePlayer.generatePremultipliedAlphaTexture = EditorGUILayout.Toggle(new GUIContent("Generate Pre-multiplied Alpha", 
                                                                                                  "When checked, generate pre-multiplied alpha texture while rendering on rendertexture."),
                                                                                   EmotePlayer.generatePremultipliedAlphaTexture);
            EmotePlayer.protectTranslucentTextureColor = EditorGUILayout.BeginToggleGroup(new GUIContent("Protect Translucent Texture Color", 
                                                                                               "When checked, protect translucent texture color while rendering to renedertexture."),
                                                                                EmotePlayer.protectTranslucentTextureColor);
            EditorGUI.indentLevel++;
            EmotePlayer.outlineTranslucentTexture =  EditorGUILayout.BeginToggleGroup(new GUIContent("Outline Translucent Texture", 
                                                                                                     "When checked, outline translucent texture while rendering to renedertexture."),
                                                                                      EmotePlayer.outlineTranslucentTexture);
            EmotePlayer.outlineWidth = EditorGUILayout.Slider("Outline Width", EmotePlayer.outlineWidth, 1, 10);
            EmotePlayer.outlineColor = EditorGUILayout.ColorField("Outline Color", EmotePlayer.outlineColor);
            EditorGUILayout.EndToggleGroup();
            EditorGUI.indentLevel--;
            EditorGUILayout.EndToggleGroup();
            EditorGUI.indentLevel--;
        }
        
        toggle = EmotePlayer.toggleBehaviorSettings = EditorGUILayout.Foldout(EmotePlayer.toggleBehaviorSettings, "Behavior");
        if (toggle) {
            EditorGUI.indentLevel++;
            EmotePlayer.globalMeshDivisionRatio = EditorGUILayout.Slider("Global Mesh Division Ratio", EmotePlayer.globalMeshDivisionRatio, 0, 3);
            EmotePlayer.globalTranslateLimitVelocity = EditorGUILayout.Slider("Global Translate Limit Velocity", EmotePlayer.globalTranslateLimitVelocity, 0, 3);
            EmotePlayer.globalHairScale = EditorGUILayout.Slider("Global Hair Scale", EmotePlayer.globalHairScale, 0, 3);
            EmotePlayer.globalBustScale = EditorGUILayout.Slider("Global Bust Scale", EmotePlayer.globalBustScale, 0, 3);
            EmotePlayer.globalPartsScale = EditorGUILayout.Slider("Global Parts Scale", EmotePlayer.globalPartsScale, 0, 3);
            EditorGUI.indentLevel--;
        }

        toggle = EmotePlayer.toggleWarningSettings = EditorGUILayout.Foldout(EmotePlayer.toggleWarningSettings, "Warning");
        if (toggle) {
            EditorGUI.indentLevel++;
            EmotePlayer.supressBuiltinTextureImageWarning = EditorGUILayout.Toggle(new GUIContent("Supress builtin texture warning",
                                                                                                  "When checked, supress builtin texture warning."),
                                                                                                  EmotePlayer.supressBuiltinTextureImageWarning);
            EditorGUI.indentLevel--;
        }

        toggle = EmotePlayer.togglePlatformSpecificSettings = EditorGUILayout.Foldout(EmotePlayer.togglePlatformSpecificSettings, "Platform Specific Settings");
        if (toggle) {
            EditorGUI.indentLevel++;
        
            toggle = EmotePlayer.toggleWindowsSettings = EditorGUILayout.Foldout(EmotePlayer.toggleWindowsSettings, "Windows Settings");
            if (toggle) {
                EditorGUI.indentLevel++;
                EmotePlayer.windowsMainMemSize = EditorGUILayout.IntField("Main Mem (MB)", EmotePlayer.windowsMainMemSize);
                EditorGUI.indentLevel--;
            }
            
            toggle = EmotePlayer.toggleOSXSettings = EditorGUILayout.Foldout(EmotePlayer.toggleOSXSettings, "OSX Settings");
            if (toggle) {
                EditorGUI.indentLevel++;
                EmotePlayer.osxMainMemSize = EditorGUILayout.IntField("Main Mem (MB)", EmotePlayer.osxMainMemSize);
                EditorGUI.indentLevel--;
            }
            
            toggle = EmotePlayer.toggleIosSettings = EditorGUILayout.Foldout(EmotePlayer.toggleIosSettings, "iOS Settings");
            if (toggle) {
                EditorGUI.indentLevel++;
                EmotePlayer.iosMainMemSize = EditorGUILayout.IntField("Main Mem (MB)", EmotePlayer.iosMainMemSize);
                EditorGUI.indentLevel--;
            }
            
            toggle = EmotePlayer.toggleAndroidSettings = EditorGUILayout.Foldout(EmotePlayer.toggleAndroidSettings, "Android Settings");
            if (toggle) {
                EditorGUI.indentLevel++;
                EmotePlayer.androidMainMemSize = EditorGUILayout.IntField("Main Mem (MB)", EmotePlayer.androidMainMemSize);
                EditorGUI.indentLevel--;
            }

            toggle = EmotePlayer.toggleWebglSettings = EditorGUILayout.Foldout(EmotePlayer.toggleWebglSettings, "WebGL Settings");
            if (toggle) {
                EditorGUI.indentLevel++;
                EmotePlayer.webglMainMemSize = EditorGUILayout.IntField("Main Mem (MB)", EmotePlayer.webglMainMemSize);
                EditorGUI.indentLevel--;
            }
            
            toggle = EmotePlayer.toggleSwitchSettings = EditorGUILayout.Foldout(EmotePlayer.toggleSwitchSettings, "Switch Settings");
            if (toggle) {
                EditorGUI.indentLevel++;
                EmotePlayer.switchMainMemSize = EditorGUILayout.IntField("Main Mem (MB)", EmotePlayer.switchMainMemSize);
                EditorGUI.indentLevel--;
            }
            
            toggle = EmotePlayer.togglePlayStation4Settings = EditorGUILayout.Foldout(EmotePlayer.togglePlayStation4Settings, "PlayStation4 Settings");
            if (toggle) {
                EditorGUI.indentLevel++;
                EmotePlayer.playStation4MainMemSize = EditorGUILayout.IntField("Main Mem (MB)", EmotePlayer.playStation4MainMemSize);
                EditorGUI.indentLevel--;
            }

            EditorGUI.indentLevel--;
        }

        toggle = EmotePlayer.toggleInformationSettings = EditorGUILayout.Foldout(EmotePlayer.toggleInformationSettings, "Information");
        if (toggle) {
            EditorGUI.indentLevel++;

            EditorGUILayout.HelpBox(infoString, MessageType.Info);
            
            EditorGUI.indentLevel--;
        }
    }
}
