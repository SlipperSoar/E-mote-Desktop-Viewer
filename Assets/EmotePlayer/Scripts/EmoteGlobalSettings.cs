using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EmoteGlobalSettings : ScriptableObject
{
    public bool toggleAppearanceSettings = true;
    public int maskMode = 0;
    public bool maskRegionClipping = false;
    public int alphaCutoff = 1;
    public bool gpuMeshDeformationEnabled = false;

    public bool toggleRenderTextureSettings = true;
    public bool generatePremultipliedAlphaTexture = false;
    public bool protectTranslucentTextureColor = false;
    public bool outlineTranslucentTexture = false;
    public float outlineWidth = 3;
    public Color outlineColor = Color.red;
    
    public bool toggleBehaviorSettings = true;
    public float meshDivisionRatio = 1.0f;
    public float translateLimitVelocity = 0.0f;
    public float hairScale = 1.0f;
    public float bustScale = 1.0f;
    public float partsScale = 1.0f;
    
    public bool toggleWarningSettings = true;
    public bool supressBuiltinTextureImageWarning = false;

    public bool togglePlatformSpecificSettings = false;
    
    public bool toggleWindowsSettings = false;
    public int windowsMainMemSize = 1;

    public bool toggleOSXSettings = false;
    public int osxMainMemSize = 1;

    public bool toggleIosSettings = false;
    public int iosMainMemSize = 1;
    
    public bool toggleAndroidSettings = false;
    public int androidMainMemSize = 1;

    public bool toggleWebglSettings = false;
    public int webglMainMemSize = 1;

    public bool toggleSwitchSettings = false;
    public int switchMainMemSize = 20;

    public bool togglePlayStation4Settings = false;
    public int playStation4MainMemSize = 20;

    public bool toggleInformationSettings = false;
};

