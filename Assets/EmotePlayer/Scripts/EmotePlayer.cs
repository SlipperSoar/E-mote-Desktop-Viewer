// ◇UTF-8
// EmotePlayer

#if (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
#define EMOTE_PLATFORM_WIN
#endif

#if (UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX)
#define EMOTE_PLATFORM_OSX
#endif

#if (UNITY_IPHONE && ! UNITY_EDITOR)
#define EMOTE_PLATFORM_IPHONE
#endif

#if (UNITY_ANDROID && ! UNITY_EDITOR)
#define EMOTE_PLATFORM_ANDROID
#endif

#if (UNITY_WEBGL && ! UNITY_EDITOR)
#define EMOTE_PLATFORM_WEBGL
#endif

#if (UNITY_PS4 && ! UNITY_EDITOR)
#define EMOTE_PLATFORM_PS4
#endif

#if (UNITY_SWITCH && ! UNITY_EDITOR)
#define EMOTE_PLATFORM_SWITCH
#endif

#if (EMOTE_PLATFORM_WIN || EMOTE_PLATFORM_OSX)
#define EMOTE_PLATFORM_PC
#endif

#if (EMOTE_PLATFORM_IPHONE || EMOTE_PLATFORM_ANDROID || EMOTE_PLATFORM_WEBGL || EMOTE_PLATFORM_PS4 || EMOTE_PLATFORM_SWITCH)
#define EMOTE_PLATFORM_NONPC
#endif

#if (EMOTE_PLATFORM_IPHONE || EMOTE_PLATFORM_ANDROID || EMOTE_PLATFORM_WEBGL || EMOTE_PLATFORM_PS4 || EMOTE_PLATFORM_SWITCH || EMOTE_PLATFORM_WIN || EMOTE_PLATFORM_OSX)
#define EMOTE_SUPPORT_OWNHEAP
#endif

#if ! EMOTE_PLATFORM_PC && ! EMOTE_PLATFORM_NONPC
#define EMOTE_NATIVE_PLUGIN_DISABLED
#endif

#if UNITY_5_6_OR_NEWER
#define EMOTE_SUPPORT_BBM
#endif // UNITY_5_6_OR_NEWER

#if UNITY_5_6_OR_NEWER
#define EMOTE_SUPPORT_SINGLECAMERA_STEREOVISION
#endif // UNITY_5_6_OR_NEWER

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.IO;

[AddComponentMenu("Emote Player/Emote Player")]
[ExecuteInEditMode]
public class EmotePlayer : MonoBehaviour {
    // EmotePlayer DLLインポート定義.
    #if EMOTE_PLATFORM_WIN
    const string importName = "EmotePlayerPlain";
    const CharSet importCharSet = CharSet.Unicode;
    #elif (EMOTE_PLATFORM_OSX || EMOTE_PLATFORM_ANDROID || EMOTE_PLATFORM_PS4)
    const string importName = "EmotePlayerPlain";
    const CharSet importCharSet = CharSet.Ansi;
    #elif (EMOTE_PLATFORM_IPHONE || EMOTE_PLATFORM_WEBGL || EMOTE_PLATFORM_SWITCH)
    const string importName = "__Internal";
    const CharSet importCharSet = CharSet.Ansi;
    #endif

    #if ! EMOTE_NATIVE_PLUGIN_DISABLED

    enum BuiltinTextureImageFormat : int {
        RGBA32 = 0,
        RGBA4444
    };

    internal struct ImageInfo {
        public int hasBuiltinImage;
        public int width;
        public int height;
        public int format;
        public int ast;
    };
    internal struct PSBRef {
        public System.IntPtr ptr;
        public int size;
        public PSBRef(System.IntPtr ptr, int size) {
            this.ptr = ptr;
            this.size = size;
        }
    };
    internal class EmotePlayerRef {
        private System.IntPtr mPlayerID = (System.IntPtr)0;
        public System.IntPtr playerID { get { return mPlayerID; } }
        public EmotePlayerRef(PSBRef[] psbArray, int size) {
            EmotePlayer.requireDevice();
            mPlayerID = Native_EmotePlayer_Initialize(psbArray, size);
        }
        ~EmotePlayerRef() {
            Destroy();
        }
        public void Destroy() {
            if (mPlayerID != (System.IntPtr)0) {
                Native_EmotePlayer_Finish(mPlayerID);
                mPlayerID = (System.IntPtr)0;
                EmotePlayer.releaseDevice();
            }
        }
    };
    [DllImport (importName, CharSet=importCharSet)]
    private static extern System.IntPtr Native_Emote_GetSDKVersion();
    [DllImport (importName, CharSet=importCharSet)]
    private static extern System.IntPtr Native_Emote_GetBuildDateTime();
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_Emote_CheckValidObject(System.IntPtr image, int imageSize);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmoteDevice_Require();
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmoteDevice_Release();
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmoteDevice_RefCount();
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmoteDevice_Initialize();
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmoteDevice_Finish();
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmoteDevice_SetMaskRegionClipping(bool value);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmoteDevice_SetProtectTranslucentTextureColor(bool value);
    [DllImport (importName, CharSet=importCharSet)]
	private static extern int Native_EmoteDevice_SetForceBufferedBlend(bool value);
    [DllImport (importName, CharSet=importCharSet)]
	private static extern int Native_EmoteDevice_SetGpuMeshDeformationEnabled(bool value);
	[DllImport (importName, CharSet=importCharSet)]
    private static extern System.IntPtr Native_EmotePlayer_Initialize(PSBRef[] psbArray, int size);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_Finish(System.IntPtr playerId);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_SetScreenSize(System.IntPtr playerId, int width, int height);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_SetPixelScale(System.IntPtr playerId, float x, float y);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_SetCommandBufferOutputPrimitive(System.IntPtr playerId, int primitive);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern System.IntPtr Native_EmotePlayer_GetCommandBuffer(System.IntPtr playerId);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_GetCommandBufferLength(System.IntPtr playerId);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_GetCommandBufferMaxImageCount(System.IntPtr playerId);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_GetCommandBufferBuiltinImageCount(System.IntPtr playerId);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_GetCommandBufferBuiltinImageInfo(System.IntPtr playerId, int index, ref ImageInfo imageInfo);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_ExtractCommandBufferBuiltinImage(System.IntPtr playerId, int index, System.IntPtr imagePtr, int format);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_DestroyCommandBufferBuiltinImage(System.IntPtr playerId, int index);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_Update(System.IntPtr playerId, float multiplier);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_Draw(System.IntPtr playerId);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_Skip(System.IntPtr playerId);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_Pass(System.IntPtr playerId);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_Step(System.IntPtr playerId);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_SetColor(System.IntPtr playerId, float r, float g, float b, float a, float frameCount, float easing);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_SetCoord(System.IntPtr playerId, float x, float y, float frameCount, float easing);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_SetScale(System.IntPtr playerId, float scale, float frameCount, float easing);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_SetRot(System.IntPtr playerId, float rot, float frameCount, float easing);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_SetAsOriginalScale(System.IntPtr playerId, bool state);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_SetHairScale(System.IntPtr playerId, float scale);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_SetBustScale(System.IntPtr playerId, float scale);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_SetPartsScale(System.IntPtr playerId, float scale);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_SetMeshDivisionRatio(System.IntPtr playerId, float scale);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_SetGrayscale(System.IntPtr playerId, float rate, float frameCount, float easing);

    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_CountMainTimelines(System.IntPtr playerId);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern System.IntPtr Native_EmotePlayer_GetMainTimelineLabelAt(System.IntPtr playerId, int index);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_CountDiffTimelines(System.IntPtr playerId);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern System.IntPtr Native_EmotePlayer_GetDiffTimelineLabelAt(System.IntPtr playerId, int index);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern bool Native_EmotePlayer_IsLoopTimeline(System.IntPtr playerId, string label);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern float Native_EmotePlayer_GetTimelineTotalFrameCount(System.IntPtr playerId, string label);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_PlayTimeline(System.IntPtr playerId, string label, int flags);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_StopTimeline(System.IntPtr playerId, string label);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern bool Native_EmotePlayer_IsTimelinePlaying(System.IntPtr playerId, string label);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_SetTimelineBlendRatio(System.IntPtr playerId, string label, float value, float frameCount, float easing, bool stopWhenBlendDone);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern float Native_EmotePlayer_GetTimelineBlendRatio(System.IntPtr playerId, string label);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_FadeInTimeline(System.IntPtr playerId, string label, float frameCount, float easing);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_FadeOutTimeline(System.IntPtr playerId, string label, float frameCount, float easing);

    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_CountVariables(System.IntPtr playerId);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern System.IntPtr Native_EmotePlayer_GetVariableLabelAt(System.IntPtr playerId, int variableIndex);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_CountVariableFrameAt(System.IntPtr playerId, int variableIndex);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern System.IntPtr Native_EmotePlayer_GetVariableFrameLabelAt(System.IntPtr playerId, int variableIndex, int frameIndex);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern float Native_EmotePlayer_GetVariableFrameValueAt(System.IntPtr playerId, int variableIndex, int frameIndex);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_SetVariable(System.IntPtr playerId, string label, float value, float frameCount, float easing);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern float Native_EmotePlayer_GetVariable(System.IntPtr playerId, string label);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_SetVariableDiff(System.IntPtr playerId, string module, string label, float value, float frameCount, float easing);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern float Native_EmotePlayer_GetVariableDiff(System.IntPtr playerId, string module, string label);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_SetOuterForce(System.IntPtr playerId, string label, float ofx, float ofy, float frameCount, float easing);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_SetOuterRot(System.IntPtr playerId, float rot, float frameCount, float easing);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_StartWind(System.IntPtr playerId, float start, float goal, float speed, float powMin, float powMax);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_StopWind(System.IntPtr playerId);

    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_CountPlayingTimelines(System.IntPtr playerId);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern System.IntPtr Native_EmotePlayer_GetPlayingTimelineLabelAt(System.IntPtr playerId, int index);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_GetPlayingTimelineFlagsAt(System.IntPtr playerId, int index);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern bool Native_EmotePlayer_IsAnimating(System.IntPtr playerId);

    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_SetStereovisionEnabled(System.IntPtr playerId, bool value);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_SetStereovisionVolume(System.IntPtr playerId, float volume);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_SetStereovisionParallaxRatio(System.IntPtr playerId, float ratio);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_SetStereovisionRenderScreen(System.IntPtr playerId, int index);

    [DllImport (importName, CharSet=importCharSet)]
    private static extern bool Native_EmotePlayer_IsCharaProfileAvailable(System.IntPtr playerId);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern float Native_EmotePlayer_GetCharaHeight(System.IntPtr playerId);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_CountCharaProfiles(System.IntPtr playerId);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern System.IntPtr Native_EmotePlayer_GetCharaProfileLabelAt(System.IntPtr playerId, int profileIndex);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern float Native_EmotePlayer_GetCharaProfile(System.IntPtr playerId, string label);

    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_StartRecordAPILog(System.IntPtr playerId);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_StopRecordAPILog(System.IntPtr playerId);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern bool Native_EmotePlayer_IsRecordingAPILog(System.IntPtr playerId);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_StartReplayAPILog(System.IntPtr playerId);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_StopReplayAPILog(System.IntPtr playerId);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern bool Native_EmotePlayer_IsReplayingAPILog(System.IntPtr playerId);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_ClearAPILog(System.IntPtr playerId);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern System.IntPtr Native_EmotePlayer_GetAPILog(System.IntPtr playerId);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_SetAPILog(System.IntPtr playerId, string log);

    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmotePlayer_GetSuitableClearColor(System.IntPtr playerId);
    
    #endif

    #if EMOTE_SUPPORT_OWNHEAP
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmoteDevice_SetInitializeParams(int mainMemSize);
    [DllImport (importName, CharSet=importCharSet)]
    private static extern float Native_EmoteDevice_GetMainMemUsage();
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmoteDevice_GetMainMemTotalSize();
    [DllImport (importName, CharSet=importCharSet)]
    private static extern int Native_EmoteDevice_GetMainMemAllocatedSize();

    public static float getMainMemUsage() {
        return Native_EmoteDevice_GetMainMemUsage();
    }

    public static int getMainMemTotalSize() {
        return Native_EmoteDevice_GetMainMemTotalSize();
    }

    public static int getMainMemAllocatedSize() {
        return Native_EmoteDevice_GetMainMemAllocatedSize();
    } 
   
    #else // EMOTE_SUPPORT_OWNHEAP

    public static float getMainMemUsage() {
        return 0;
    }

    public static int getMainMemTotalSize() {
        return 0;
    }

    public static int getMainMemAllocatedSize() {
        return 0;
    } 
    
    #endif // EMOTE_SUPPORT_OWNHEAP

    private static string _sdkVersion = null;
    
    public static string sdkVersion {
        get {
            if (_sdkVersion == null)
                _sdkVersion = Marshal.PtrToStringAuto(Native_Emote_GetSDKVersion());
            return _sdkVersion;
        }
    }

    private static string _buildDateTime = null;

    public static string buildDateTime {
        get {
            if (_buildDateTime == null)
                _buildDateTime = Marshal.PtrToStringAuto(Native_Emote_GetBuildDateTime());
            return _buildDateTime;
        }
    }
    
    public static int deviceRefCount {
        get { return Native_EmoteDevice_RefCount(); }
    }
    static internal void requireDevice() {
        int refCount = Native_EmoteDevice_Require();
        // M2DebugLog.printf("require device {0}->{1}", refCount, refCount+1);
        if (refCount != 0)
            return;
        #if EMOTE_PLATFORM_WIN
        Native_EmoteDevice_SetInitializeParams(windowsMainMemSize * 1024 * 1024);
        #endif // EMOTE_PLATFORM_WIN
        #if EMOTE_PLATFORM_OSX
        Native_EmoteDevice_SetInitializeParams(osxMainMemSize * 1024 * 1024);
        #endif // EMOTE_PLATFORM_OSX
        #if EMOTE_PLATFORM_IPHONE
        Native_EmoteDevice_SetInitializeParams(iosMainMemSize * 1024 * 1024);
        #endif // EMOTE_PLATFORM_IPHONE
        #if EMOTE_PLATFORM_ANDROID
        Native_EmoteDevice_SetInitializeParams(androidMainMemSize * 1024 * 1024);
        #endif // EMOTE_PLATFORM_ANDROID
        #if EMOTE_PLATFORM_WEBGL
        Native_EmoteDevice_SetInitializeParams(webglMainMemSize * 1024 * 1024);
        #endif // EMOTE_PLATFORM_WEBGL
        #if EMOTE_PLATFORM_SWITCH
        Native_EmoteDevice_SetInitializeParams(switchMainMemSize * 1024 * 1024);
        #endif // EMOTE_PLATFORM_ANDROID
        #if EMOTE_PLATFORM_PS4
        Native_EmoteDevice_SetInitializeParams(playStation4MainMemSize * 1024 * 1024);
        #endif // EMOTE_PLATFORM_PS4
        Native_EmoteDevice_SetMaskRegionClipping(maskRegionClipping);
        Native_EmoteDevice_SetProtectTranslucentTextureColor(protectTranslucentTextureColor);
        Native_EmoteDevice_SetForceBufferedBlend(generatePremultipliedAlphaTexture);
        Native_EmoteDevice_SetGpuMeshDeformationEnabled(gpuMeshDeformationEnabled);

        if (Native_EmoteDevice_Initialize() == 0)
            M2DebugLog.printf("EmoteDeviceInitialize() Failed!");
        /*
          else
          M2DebugLog.printf("EmoteDeviceInitialize() Succeed!");
        */

        InitShaders();
        RenderTextureMapper.InitObjectPool();
        var mesh = defaultRenderTextureMesh;
        var material = defaultRenderTextureMaterial;
    }
  
    static internal void releaseDevice() {
        int refCount = Native_EmoteDevice_Release();
        // M2DebugLog.printf("release device {0}->{1}", refCount+1, refCount);
        if (refCount != 0)
            return;

        RenderTextureMapper.DestroyObjectPool();
        DestroyShaders();

        // M2DebugLog.printf("EmoteDeviceFinish()");
        if (Native_EmoteDevice_Finish() == 0) 
            M2DebugLog.printf("EmoteDeviceFinish() Failed!");
        /*
          else
          M2DebugLog.printf("EmoteDeviceFinish() Succeed!");
        */
    }   

    static public EmoteGlobalSettings sGlobalSettings;

    static public void touchGlobalSettings() {
        #if UNITY_EDITOR
        if (sGlobalSettings == null) {
            sGlobalSettings = (EmoteGlobalSettings)AssetDatabase.LoadAssetAtPath("Assets/Resources/emote/globalSettings.asset", typeof(EmoteGlobalSettings)); 
            if (sGlobalSettings == null) {
                sGlobalSettings = ScriptableObject.CreateInstance<EmoteGlobalSettings>();
                AssetDatabase.CreateFolder("Assets", "Resources");
                AssetDatabase.CreateFolder("Assets/Resources", "emote");
                AssetDatabase.CreateAsset(sGlobalSettings, "Assets/Resources/emote/globalSettings.asset");
                EditorUtility.SetDirty(sGlobalSettings);
                AssetDatabase.Refresh();
            }
            if (EditorApplication.isPlaying) 
                sGlobalSettings = (EmoteGlobalSettings)Object.Instantiate(sGlobalSettings);
        }
        Native_EmoteDevice_SetMaskRegionClipping(sGlobalSettings.maskRegionClipping);
        Native_EmoteDevice_SetProtectTranslucentTextureColor(sGlobalSettings.protectTranslucentTextureColor);
      Native_EmoteDevice_SetGpuMeshDeformationEnabled(sGlobalSettings.gpuMeshDeformationEnabled);
        #else
        if (sGlobalSettings == null)
            sGlobalSettings = (EmoteGlobalSettings)Resources.Load("emote/globalSettings"); 
        if (sGlobalSettings == null)
            sGlobalSettings = ScriptableObject.CreateInstance<EmoteGlobalSettings>();
        #endif
    }

    static void writeGlobalSettings() {
        #if UNITY_EDITOR
        if (! EditorApplication.isPlaying) {
            EditorUtility.SetDirty(sGlobalSettings);
            AssetDatabase.Refresh();
        }
        #endif
    } 

    static public bool toggleAppearanceSettings {
        get { 
            touchGlobalSettings(); 
            return sGlobalSettings.toggleAppearanceSettings; 
        }
        set {
            touchGlobalSettings(); 
            if (sGlobalSettings.toggleAppearanceSettings == value)
                return; 
            sGlobalSettings.toggleAppearanceSettings = value; 
            writeGlobalSettings(); 
        }
    }

    static public int maskMode {
        get {
            touchGlobalSettings();
            return sGlobalSettings.maskMode;
        }
        set {
            touchGlobalSettings(); 
            if (sGlobalSettings.maskMode == value)
                return;
            sGlobalSettings.maskMode = value;
            writeGlobalSettings(); 
        }
    }

    static public bool maskRegionClipping {
        get {
            touchGlobalSettings();
            return sGlobalSettings.maskRegionClipping; 
        }
        set {
            touchGlobalSettings(); 
            if (sGlobalSettings.maskRegionClipping == value)
                return;
            sGlobalSettings.maskRegionClipping = value;
            writeGlobalSettings(); 
            Native_EmoteDevice_SetMaskRegionClipping(value);
        }
    }

    static public int alphaCutoff {
        get { 
            touchGlobalSettings(); 
            return sGlobalSettings.alphaCutoff;
        }
        set { 
            touchGlobalSettings(); 
            if (sGlobalSettings.alphaCutoff == value)
                return;
            sGlobalSettings.alphaCutoff = value; 
            writeGlobalSettings(); 
        }
    }
    
    static public bool gpuMeshDeformationEnabled {
        get {
            touchGlobalSettings();
            return sGlobalSettings.gpuMeshDeformationEnabled;
        }
        set {
            touchGlobalSettings(); 
            if (sGlobalSettings.gpuMeshDeformationEnabled == value)
                return;
          sGlobalSettings.gpuMeshDeformationEnabled = value;
          writeGlobalSettings(); 
          Native_EmoteDevice_SetGpuMeshDeformationEnabled(value);
        }
    }

    static public bool toggleRenderTextureSettings {
        get { 
            touchGlobalSettings(); 
            return sGlobalSettings.toggleRenderTextureSettings;
        }
        set {
            touchGlobalSettings(); 
            if (sGlobalSettings.toggleRenderTextureSettings == value)
                return; 
            sGlobalSettings.toggleRenderTextureSettings = value; 
            writeGlobalSettings(); 
        }
    }

    static public bool generatePremultipliedAlphaTexture {
        get { 
            touchGlobalSettings(); 
            return sGlobalSettings.generatePremultipliedAlphaTexture;
        }
        set { 
            touchGlobalSettings(); 
            if (sGlobalSettings.generatePremultipliedAlphaTexture == value)
                return;
            sGlobalSettings.generatePremultipliedAlphaTexture = value; 
            writeGlobalSettings(); 
            Native_EmoteDevice_SetForceBufferedBlend(value);
        }
    }
        
    static public bool protectTranslucentTextureColor {
        get { 
            touchGlobalSettings(); 
            return sGlobalSettings.protectTranslucentTextureColor; 
        }
        set { 
            touchGlobalSettings(); 
            if (sGlobalSettings.protectTranslucentTextureColor == value)
                return;
            sGlobalSettings.protectTranslucentTextureColor = value; 
            writeGlobalSettings(); 
            Native_EmoteDevice_SetProtectTranslucentTextureColor(value); 
        }
    }

    static public bool outlineTranslucentTexture {
        get { 
            touchGlobalSettings(); 
            return sGlobalSettings.outlineTranslucentTexture; 
        }
        set { 
            touchGlobalSettings(); 
            if (sGlobalSettings.outlineTranslucentTexture == value)
                return;
            sGlobalSettings.outlineTranslucentTexture = value; 
            writeGlobalSettings(); 
        }
    }

    static public float outlineWidth {
        get { 
            touchGlobalSettings(); 
            return sGlobalSettings.outlineWidth;
        }
        set { 
            touchGlobalSettings(); 
            if (sGlobalSettings.outlineWidth == value)
                return;
            sGlobalSettings.outlineWidth = value; 
            writeGlobalSettings(); 
        }
    }

    static public Color outlineColor {
        get { 
            touchGlobalSettings(); 
            return sGlobalSettings.outlineColor;
        }
        set { 
            touchGlobalSettings(); 
            if (sGlobalSettings.outlineColor == value)
                return;
            sGlobalSettings.outlineColor = value; 
            writeGlobalSettings(); 
        }
    }
    
    static public bool toggleBehaviorSettings {
        get { 
            touchGlobalSettings(); 
            return sGlobalSettings.toggleBehaviorSettings; 
        }
        set {
            touchGlobalSettings(); 
            if (sGlobalSettings.toggleBehaviorSettings == value)
                return; 
            sGlobalSettings.toggleBehaviorSettings = value; 
            writeGlobalSettings(); 
        }
    }

    static public float globalMeshDivisionRatio {
        get { 
            touchGlobalSettings(); 
            return sGlobalSettings.meshDivisionRatio;
        }
        set { 
            touchGlobalSettings(); 
            if (sGlobalSettings.meshDivisionRatio == value)
                return;
            sGlobalSettings.meshDivisionRatio = value; 
            writeGlobalSettings(); 
        }
    }

    static public float globalTranslateLimitVelocity {
        get { 
            touchGlobalSettings(); 
            return sGlobalSettings.translateLimitVelocity;
        }
        set { 
            touchGlobalSettings(); 
            if (sGlobalSettings.translateLimitVelocity == value)
                return;
            sGlobalSettings.translateLimitVelocity = value; 
            writeGlobalSettings(); 
        }
    }

    static public float globalHairScale {
        get { 
            touchGlobalSettings(); 
            return sGlobalSettings.hairScale; 
        }
        set { 
            touchGlobalSettings(); 
            if (sGlobalSettings.hairScale == value)
                return;
            sGlobalSettings.hairScale = value; 
            writeGlobalSettings(); 
            invalidateAllPlayersPhysics();
        }
    }

    static public float globalBustScale {
        get { 
            touchGlobalSettings(); 
            return sGlobalSettings.bustScale;
        }
        set { 
            touchGlobalSettings(); 
            if (sGlobalSettings.bustScale == value)
                return;
            sGlobalSettings.bustScale = value; 
            writeGlobalSettings(); 
            invalidateAllPlayersPhysics();
        }
    }

    static public float globalPartsScale {
        get { 
            touchGlobalSettings(); 
            return sGlobalSettings.partsScale; 
        }
        set { 
            touchGlobalSettings(); 
            if (sGlobalSettings.partsScale == value)
                return;
            sGlobalSettings.partsScale = value; 
            writeGlobalSettings(); 
            invalidateAllPlayersPhysics();
        }
    }

    static public bool toggleWarningSettings {
        get { 
            touchGlobalSettings(); 
            return sGlobalSettings.toggleWarningSettings; 
        }
        set {
            touchGlobalSettings(); 
            if (sGlobalSettings.toggleWarningSettings == value)
                return; 
            sGlobalSettings.toggleWarningSettings = value; 
            writeGlobalSettings(); 
        }
    }

    static public bool togglePlatformSpecificSettings {
        get { 
            touchGlobalSettings(); 
            return sGlobalSettings.togglePlatformSpecificSettings;
        }
        set {
            touchGlobalSettings(); 
            if (sGlobalSettings.togglePlatformSpecificSettings == value)
                return;
            sGlobalSettings.togglePlatformSpecificSettings = value;
            writeGlobalSettings(); 
        }
    }
    
    static public bool supressBuiltinTextureImageWarning {
        get { 
            touchGlobalSettings(); 
            return sGlobalSettings.supressBuiltinTextureImageWarning; 
        }
        set {
            touchGlobalSettings(); 
            if (sGlobalSettings.supressBuiltinTextureImageWarning == value)
                return; 
            sGlobalSettings.supressBuiltinTextureImageWarning = value; 
            writeGlobalSettings(); 
        }
    }

    static public bool toggleWindowsSettings {
        get { 
            touchGlobalSettings(); 
            return sGlobalSettings.toggleWindowsSettings;
        }
        set {
            touchGlobalSettings(); 
            if (sGlobalSettings.toggleWindowsSettings == value)
                return;
            sGlobalSettings.toggleWindowsSettings = value;
            writeGlobalSettings(); 
        }
    }

    static public int windowsMainMemSize {
        get { 
            touchGlobalSettings(); 
            return sGlobalSettings.windowsMainMemSize;
        }
        set { 
            touchGlobalSettings(); 
            if (sGlobalSettings.windowsMainMemSize == value)
                return;
            sGlobalSettings.windowsMainMemSize = value;
            writeGlobalSettings(); 
        }
    }

    static public bool toggleOSXSettings {
        get { 
            touchGlobalSettings(); 
            return sGlobalSettings.toggleOSXSettings;
        }
        set {
            touchGlobalSettings(); 
            if (sGlobalSettings.toggleOSXSettings == value)
                return;
            sGlobalSettings.toggleOSXSettings = value;
            writeGlobalSettings(); 
        }
    }

    static public int osxMainMemSize {
        get { 
            touchGlobalSettings(); 
            return sGlobalSettings.osxMainMemSize;
        }
        set { 
            touchGlobalSettings(); 
            if (sGlobalSettings.osxMainMemSize == value)
                return;
            sGlobalSettings.osxMainMemSize = value;
            writeGlobalSettings(); 
        }
    }
    
    static public bool toggleIosSettings {
        get { 
            touchGlobalSettings(); 
            return sGlobalSettings.toggleIosSettings;
        }
        set {
            touchGlobalSettings(); 
            if (sGlobalSettings.toggleIosSettings == value)
                return;
            sGlobalSettings.toggleIosSettings = value;
            writeGlobalSettings(); 
        }
    }

    static public int iosMainMemSize {
        get { 
            touchGlobalSettings(); 
            return sGlobalSettings.iosMainMemSize;
        }
        set { 
            touchGlobalSettings(); 
            if (sGlobalSettings.iosMainMemSize == value)
                return;
            sGlobalSettings.iosMainMemSize = value;
            writeGlobalSettings(); 
        }
    }

    static public bool toggleAndroidSettings {
        get { 
            touchGlobalSettings(); 
            return sGlobalSettings.toggleAndroidSettings;
        }
        set { 
            touchGlobalSettings(); 
            if (sGlobalSettings.toggleAndroidSettings == value)
                return;
            sGlobalSettings.toggleAndroidSettings = value; 
            writeGlobalSettings(); 
        }
    }

    static public int androidMainMemSize {
        get { 
            touchGlobalSettings(); 
            return sGlobalSettings.androidMainMemSize; 
        }
        set { 
            touchGlobalSettings(); 
            if (sGlobalSettings.androidMainMemSize == value)
                return;
            sGlobalSettings.androidMainMemSize = value; 
            writeGlobalSettings(); 
        }
    }

    static public bool toggleWebglSettings {
        get { 
            touchGlobalSettings(); 
            return sGlobalSettings.toggleWebglSettings;
        }
        set { 
            touchGlobalSettings(); 
            if (sGlobalSettings.toggleWebglSettings == value)
                return;
            sGlobalSettings.toggleWebglSettings = value; 
            writeGlobalSettings(); 
        }
    }

    static public int webglMainMemSize {
        get { 
            touchGlobalSettings(); 
            return sGlobalSettings.webglMainMemSize; 
        }
        set { 
            touchGlobalSettings(); 
            if (sGlobalSettings.webglMainMemSize == value)
                return;
            sGlobalSettings.webglMainMemSize = value; 
            writeGlobalSettings(); 
        }
    }

    static public bool toggleSwitchSettings {
        get { 
            touchGlobalSettings(); 
            return sGlobalSettings.toggleSwitchSettings;
        }
        set {
            touchGlobalSettings(); 
            if (sGlobalSettings.toggleSwitchSettings == value)
                return;
            sGlobalSettings.toggleSwitchSettings = value;
            writeGlobalSettings(); 
        }
    }

    static public int switchMainMemSize {
        get { 
            touchGlobalSettings(); 
            return sGlobalSettings.switchMainMemSize;
        }
        set { 
            touchGlobalSettings(); 
            if (sGlobalSettings.switchMainMemSize == value)
                return;
            sGlobalSettings.switchMainMemSize = value;
            writeGlobalSettings(); 
        }
    }


    static public bool togglePlayStation4Settings {
        get { 
            touchGlobalSettings(); 
            return sGlobalSettings.togglePlayStation4Settings;
        }
        set {
            touchGlobalSettings(); 
            if (sGlobalSettings.togglePlayStation4Settings == value)
                return;
            sGlobalSettings.togglePlayStation4Settings = value;
            writeGlobalSettings(); 
        }
    }

    static public int playStation4MainMemSize {
        get { 
            touchGlobalSettings(); 
            return sGlobalSettings.playStation4MainMemSize;
        }
        set { 
            touchGlobalSettings(); 
            if (sGlobalSettings.playStation4MainMemSize == value)
                return;
            sGlobalSettings.playStation4MainMemSize = value;
            writeGlobalSettings(); 
        }
    }

    static public bool toggleInformationSettings {
        get { 
            touchGlobalSettings(); 
            return sGlobalSettings.toggleInformationSettings; 
        }
        set {
            touchGlobalSettings(); 
            if (sGlobalSettings.toggleInformationSettings == value)
                return; 
            sGlobalSettings.toggleInformationSettings = value; 
            writeGlobalSettings(); 
        }
    }
    
    public static void DestroyObjectProperly(Object obj) {
        #if UNITY_EDITOR
        if (! EditorApplication.isPlaying)
            Object.DestroyImmediate(obj);
        else
#endif // UNITY_EDITOR
        Object.Destroy(obj);
    }

    // PSBファイル.
    public bool toggleAppearance = true;

    [SerializeField]
    private M2PSBFile mPSBFile = null;

    [SerializeField]
    private TextAsset mPreloadTextAsset = null;
    [SerializeField]
    private List<Texture2D> mPreloadTextureList = null;


    public string DataFilePath {
        set { 
          LoadData(value);
        }
    }

#if UNITY_EDITOR
    public M2PSBFile DataFile {
        set { 
            if (value == null)
                dataTextAsset = null;
            else
                dataTextAsset = value.PSBObject;
        }
    }

    public TextAsset dataTextAsset {
        get {
            return mPreloadTextAsset;
        }
        set {
            if (value == mPreloadTextAsset)
                return;
            if (value == null) {
                mPreloadTextAsset = null;
            } else {
                byte[] image = value.bytes;
                int size = image.Length;
                GCHandle gch = GCHandle.Alloc(image, GCHandleType.Pinned);
                bool validObject = (Native_Emote_CheckValidObject(gch.AddrOfPinnedObject(), size) != 0);
                gch.Free();
                if (! validObject)
                    return;
                EmoteAsset asset = LoadAssetFromAssetDatabase(AssetDatabase.GetAssetPath(value));
                mPreloadTextAsset = value;
                mPreloadTextureList = asset.textures;
            }
            DestroyCore();
            StartCore();
        }
    }
#endif

    EmoteAsset extractPreloadAssets() {
        if (mPSBFile != null) {
            if (mPSBFile.PSBObject == null)
                return null;
#if UNITY_EDITOR
            EmoteAsset asset = LoadAssetFromAssetDatabase(AssetDatabase.GetAssetPath(mPSBFile.PSBObject));
#else // UNITY_EDITOR
            EmoteAsset asset = LoadAsset(mPSBFile.path);
#endif
            mPreloadTextAsset = mPSBFile.PSBObject;
            mPreloadTextureList = asset.textures;
            mPSBFile = null;
        }
        if (mPreloadTextAsset == null)
            return null;
        EmoteAsset result = new EmoteAsset();
        result.files.Add(mPreloadTextAsset);
        result.textures.AddRange(mPreloadTextureList);
        return result;
    }

    // スナップショットフラグ
    [SerializeField]
    private bool mStepUpdate = false;
    public bool stepUpdate {
        get { return mStepUpdate; }
        set { if (mStepUpdate == value)
                return;
              mStepUpdate = value;
              mIsModified = true;
        }
    }
    // 更新タイミング
    public enum UpdateTiming {
        UPDATE,
        LATE_UPDATE
    };
    public UpdateTiming updateTiming = UpdateTiming.LATE_UPDATE;   
    
    // 更新フラグ
    private bool mIsModified = true;

    [SerializeField]
    private float mMeshDivisionRatio = 1.0f;
    public float meshDivisionRatio {
        set { 
            if (mMeshDivisionRatio == value)
                return;
            mMeshDivisionRatio = value; 
            if (mInitialized) 
                Native_EmotePlayer_SetMeshDivisionRatio(mPlayerID, mMeshDivisionRatio * globalMeshDivisionRatio); 
        }
        get { return mMeshDivisionRatio; }
    }

    public Color mainColor = new Color(1, 1, 1, 1);
    [SerializeField]
    private Color mVertexColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);  // RGBA.
    public Color vertexColor {
        set {
            if (mVertexColor == value)
                return;
            mVertexColor = value;
            mIsModified = true;
            if (mInitialized) 
                Native_EmotePlayer_SetColor(mPlayerID, mVertexColor.r, mVertexColor.g, mVertexColor.b, mVertexColor.a, 0, 0); 
        }
        get { return mVertexColor; }
    }
    private bool mCulled = false;
    public bool culled { 
        set { mCulled = value; }
        get { return mCulled; }
    }

    public bool toggleTransform = true;
    [SerializeField]
    private Vector2 mCoord = new Vector2(0, 0);
    public Vector2 coord {
        set { 
            if (mCoord == value)
                return;
            mCoord = value; 
            mIsModified = true;
            updateParams(ParamUpdateFlags.TRANSFORM);
        }
        get { return mCoord; }
    }
    [SerializeField]
    private float mScale = 1.0f;
    public float scale {
        set { 
            if (mScale == value)
                return;
            mScale = value; 
            mIsModified = true;
            updateParams(ParamUpdateFlags.TRANSFORM);
        }
        get { return mScale; }
    }
    [SerializeField]
    private float mRot = 0.0f;
    public float rot {
        set { 
            value *= Mathf.Deg2Rad; 
            if (mRot == value)
                return;
            mRot = value;
            mIsModified = true;
            updateParams(ParamUpdateFlags.TRANSFORM);
        }
        get { return mRot * Mathf.Rad2Deg; }
    }
    [SerializeField]
    private float mGrayscale = 0.0f;
    public float grayscale {
        get { return mGrayscale; }
        set { if (mGrayscale == value)
                return;
            mGrayscale = value;
            mIsModified = true;
            if (mInitialized) 
                Native_EmotePlayer_SetGrayscale(mPlayerID, mGrayscale, 0, 0);
        }
    }

    private Matrix4x4 mE2WMatrix;
    private Matrix4x4 mE2UMatrix;
    private Matrix4x4 mU2EMatrix;
    private Vector2 mPixelScale;
    private Vector3 mPrevPosition;
    private Vector3 mPrevOuterVec = Vector2.zero;
    private float mPrevOuterRot = 0;
    
    public bool convolveObjectTransformToPhysics = true;

    [SerializeField]
    private float mFixedScale = 0;
    public float fixedScale {
        set {
            if (mFixedScale == value)
                return;
            mFixedScale = value;
            mIsModified = true;
            if (mInitialized) {
                Native_EmotePlayer_SetAsOriginalScale(mPlayerID, useFixedScale);
                loadCharaProfile();
            }
            updateParams(ParamUpdateFlags.TRANSFORM);
        }
        get { return mFixedScale; }
    }

    public bool useFixedScale {
        get {
            return mFixedScale > 0;
        }
    }

    public bool toggleStereovision = false;
    [SerializeField]
    private bool mStereovisionEnabled = false;
    public bool stereovisionEnabled {
        set { 
            if (mStereovisionEnabled == value)
                return;
            mStereovisionEnabled = value; 
            if (mInitialized) {
                Native_EmotePlayer_SetStereovisionEnabled(mPlayerID, mStereovisionEnabled); 
                updateParams(ParamUpdateFlags.RENDERTEXTURE);
            }
        }
        get { return mStereovisionEnabled; }
    }

    public int stereoVisionLeftEyeLayer = 0;
    public int stereoVisionRightEyeLayer = 0;
    [SerializeField]
    private float mStereovisionVolume = 1.0f;
    public float stereovisionVolume {
        set { 
            if (mStereovisionVolume == value)
                return;
            mStereovisionVolume = value; 
            if (mInitialized)
                Native_EmotePlayer_SetStereovisionVolume(mPlayerID, mStereovisionVolume); 
        }
        get { return mStereovisionVolume; }
    }
    [SerializeField]
    private float mStereovisionParallaxRatio = 0.0f;
    public float stereovisionParallaxRatio {
        set { 
            if (mStereovisionParallaxRatio == value)
                return;
            mStereovisionParallaxRatio = value;
            if (mInitialized)
                Native_EmotePlayer_SetStereovisionParallaxRatio(mPlayerID, mStereovisionParallaxRatio); 
        }
        get { return mStereovisionParallaxRatio; }
    }

    private bool singleCameraStereovision {
        get {
#if EMOTE_SUPPORT_SINGLECAMERA_STEREOVISION
            return stereovisionEnabled
                && (stereoVisionLeftEyeLayer == stereoVisionRightEyeLayer);
#else //  EMOTE_SUPPORT_SINGLECAMERA_STEREOVISION
            return false;
#endif // EMOTE_SUPPORT_SINGLECAMERA_STEREOVISION
        }
    }

    public bool toggleBehavior = true;

    // タイムスケール
    [SerializeField]
    private bool mApplyTimeScale = true;
    public bool applyTimeScale {
        get { return mApplyTimeScale; }
        set { if (mApplyTimeScale == value)
                return;
            mApplyTimeScale = value;
            mIsModified = true;
        }
    }
    private float deltaTime {
        get {
            if (mApplyTimeScale)
                return Time.deltaTime;
            else
                return Time.unscaledDeltaTime;
        }
    }

    public float speed = 1.0f;
    [SerializeField]
    private float mHairScale = 1.0f;
    public float hairScale {
        set { 
            if (mHairScale == value)
                return;
            mHairScale = value; 
            if (mInitialized) 
                Native_EmotePlayer_SetHairScale(mPlayerID, mHairScale * globalHairScale); 
        }
        get { return mHairScale; }
    }
    [SerializeField]
    private float mBustScale = 1.0f;
    public float bustScale {
        set { 
            if (mBustScale == value)
                return;
            mBustScale = value; 
            if (mInitialized) 
                Native_EmotePlayer_SetBustScale(mPlayerID, mBustScale * globalBustScale); 
        }
        get { return mBustScale; }
    }
    [SerializeField]
    private float mPartsScale = 1.0f;
    public float partsScale {
        set { 
            if (mPartsScale == value)
                return;
            mPartsScale = value; 
            if (mInitialized) 
                Native_EmotePlayer_SetPartsScale(mPlayerID, mPartsScale * globalPartsScale); 
        }
        get { return mPartsScale; }
    }

    static void invalidateAllPlayersPhysics() {
        foreach (EmotePlayer player in activePlayers)
            player.invalidatePhysics();
    }
    void invalidatePhysics() {
        if (! mInitialized) 
            return;
        Native_EmotePlayer_SetHairScale(mPlayerID, mHairScale * globalHairScale); 
        Native_EmotePlayer_SetBustScale(mPlayerID, mBustScale * globalBustScale); 
        Native_EmotePlayer_SetPartsScale(mPlayerID, mPartsScale * globalPartsScale); 
    }
 
    [SerializeField]
    private float mWindSpeed = 0.0f;
    [SerializeField]
    private float mWindPowMin = 0.0f;
    [SerializeField]
    private float mWindPowMax = 2.0f;
    
    public float windSpeed {
        set {
            if (mWindSpeed == value)
                return;
            mWindSpeed = value; 
            updateParams(ParamUpdateFlags.WIND);
        }
        get { return mWindSpeed; }
    }
    public float windPowMin {
        set { 
            if (mWindPowMin == value)
                return;
            mWindPowMin = value; 
            updateParams(ParamUpdateFlags.WIND);
        }
        get { return mWindPowMin; }
    }
    public float windPowMax {
        set { 
            if (mWindPowMax == value)
                return;
            mWindPowMax = value;
            updateParams(ParamUpdateFlags.WIND);
        }
        get { return mWindPowMax; }
    }

    public enum TimelinePlayFlags : int {
      TIMELINE_PLAY_PARALLEL   = 1 << 0,
      TIMELINE_PLAY_DIFFERENCE = 1 << 1
    };
    public bool toggleTimeline = true;
    private List<string> mMainTimelineLabels = new List<string>();
    private bool mMainTimelineLabelsLoaded = false;
    private void clearMainTimelineLabels() {
        mMainTimelineLabels.Clear();
        mMainTimelineLabelsLoaded = false;
    }
    private void touchMainTimelineLabels() {
        if (mMainTimelineLabelsLoaded
            || ! mInitialized)
            return;
        int timelineCount = Native_EmotePlayer_CountMainTimelines(mPlayerID);
        if (timelineCount > 0) {
            mMainTimelineLabels.Add("<Empty>");
            for (int i = 0; i < timelineCount; i++) {
                string s = Marshal.PtrToStringAuto(Native_EmotePlayer_GetMainTimelineLabelAt(mPlayerID, i));
                mMainTimelineLabels.Add(s);
            }
        }
        mMainTimelineLabelsLoaded = true;
    }
    [SerializeField]
    private string mMainTimelineLabel = "";
    public string[] mainTimelineLabels {
        get { touchMainTimelineLabels(); return (string[])mMainTimelineLabels.ToArray(); }
    }
    public bool HasMainTimelineLabel(string label) {
        touchMainTimelineLabels();
        return mMainTimelineLabels.Contains(label);
    }

    public string mainTimelineLabel {
        set { 
            if (mMainTimelineLabel == value)
                return;
            if (! mInitialized) {
                mMainTimelineLabel = value;
                return;
            }
            if (mMainTimelineLabel != "")
                Native_EmotePlayer_StopTimeline(mPlayerID, mMainTimelineLabel);
            mMainTimelineLabel = value;
            mIsModified = true;
            if (mMainTimelineLabel != "")
                Native_EmotePlayer_PlayTimeline(mPlayerID, mMainTimelineLabel, (int)TimelinePlayFlags.TIMELINE_PLAY_PARALLEL);
        }
        get { return mMainTimelineLabel; }
    }
    public int mainTimelineIndex {
        set { 
            touchMainTimelineLabels();
            if (value >= 1 && value < mMainTimelineLabels.Count) {
                mainTimelineLabel = (string)mMainTimelineLabels[value];
            } else {
                mainTimelineLabel = "";
            }
        }
        get {
            touchMainTimelineLabels();
            if (mMainTimelineLabel == "")
                return 0;
            else {
                int index = mMainTimelineLabels.IndexOf(mMainTimelineLabel, 1);
                if (index > 0)
                    return index;
                else
                    return 0;
            }
        }
    }
    private List<string> mDiffTimelineLabels = new List<string>();
    private bool mDiffTimelineLabelsLoaded = false;
    private void clearDiffTimelineLabels() {
        mDiffTimelineLabels.Clear();
        mDiffTimelineLabelsLoaded = false;
    }
    private void touchDiffTimelineLabels() {
        if (mDiffTimelineLabelsLoaded
            || ! mInitialized)
            return;
        int timelineCount = Native_EmotePlayer_CountDiffTimelines(mPlayerID);
        if (timelineCount > 0) {
            mDiffTimelineLabels.Add("<Empty>");
            for (int i = 0; i < timelineCount; i++) {
                string s = Marshal.PtrToStringAuto(Native_EmotePlayer_GetDiffTimelineLabelAt(mPlayerID, i));
                mDiffTimelineLabels.Add(s);
            }
        }
        mDiffTimelineLabelsLoaded = true;
    }
    public string[] diffTimelineLabels {
        get { touchDiffTimelineLabels(); return (string[])mDiffTimelineLabels.ToArray(); }
    }
    public bool HasDiffTimelineLabel(string label) {
        touchDiffTimelineLabels();
        return mDiffTimelineLabels.Contains(label);
    }

    void setDiffTimelineLabel(ref string label, string value, float fadeoutSec) {
        if (label == value)
            return;
        if (! mInitialized) {
            label = value; 
            return;
        }
        if (label != "")
            Native_EmotePlayer_FadeOutTimeline(mPlayerID, label, fadeoutSec * 60, 0);
        label = value;
        mIsModified = true;
        if (label != "")
            Native_EmotePlayer_PlayTimeline(mPlayerID, label, (int)(TimelinePlayFlags.TIMELINE_PLAY_PARALLEL | TimelinePlayFlags.TIMELINE_PLAY_DIFFERENCE));
    }
    string getDiffTimelineLabelForIndex(int index) {
        touchDiffTimelineLabels(); 
        if (index >= 1 && index < mDiffTimelineLabels.Count) 
            return  (string)mDiffTimelineLabels[index];
        else 
            return "";
    }
    int getDiffTimelineIndexForLabel(string label) {
        touchDiffTimelineLabels(); 
        if (label == "")
            return 0;
        else {
            int index = mDiffTimelineLabels.IndexOf(label, 1);
            if (index > 0)
                return index;
            else
                return 0;
        }
    }        

    [SerializeField]
    private string mDiffTimelineLabel = "";
    public string diffTimelineLabel {
        set { setDiffTimelineLabel(ref mDiffTimelineLabel, value, 0); }
        get { return mDiffTimelineLabel; }
    }
    public int diffTimelineIndex {
        set { diffTimelineLabel = getDiffTimelineLabelForIndex(value); }
        get { return getDiffTimelineIndexForLabel(diffTimelineLabel); }
    }

    [SerializeField]
    private float mDiffTimelineBlendRatio = 1;
    public float diffTimelineBlendRatio {
        set {
            if (mDiffTimelineBlendRatio == value)
                return;
            mDiffTimelineBlendRatio = value;
            if (! mInitialized)
                return;
            mIsModified = true;
            Native_EmotePlayer_SetTimelineBlendRatio(mPlayerID, mDiffTimelineLabel, value, 0, 0, false);
        }
        get {
            if (! mInitialized)
                return 0;
            return mDiffTimelineBlendRatio;
        }
    }

    [SerializeField]
    private string mDiffTimelineSlot1 = "";
    [SerializeField]
    private string mDiffTimelineSlot2 = "";
    [SerializeField]
    private string mDiffTimelineSlot3 = "";
    [SerializeField]
    private string mDiffTimelineSlot4 = "";
    [SerializeField]
    private string mDiffTimelineSlot5 = "";
    [SerializeField]
    private string mDiffTimelineSlot6 = "";
    public float diffTimelineFadeOutTime = 0.3f;
    public string diffTimelineSlot1 {
        set { setDiffTimelineLabel(ref mDiffTimelineSlot1, value, diffTimelineFadeOutTime); }
        get { return mDiffTimelineSlot1; }
    }
    public int diffTimelineSlot1Index {
        set { diffTimelineSlot1 = getDiffTimelineLabelForIndex(value); }
        get { return getDiffTimelineIndexForLabel(diffTimelineSlot1); }
    }
    public string diffTimelineSlot2 {
        set { setDiffTimelineLabel(ref mDiffTimelineSlot2, value, diffTimelineFadeOutTime); }
        get { return mDiffTimelineSlot2; }
    }
    public int diffTimelineSlot2Index {
        set { diffTimelineSlot2 = getDiffTimelineLabelForIndex(value); }
        get { return getDiffTimelineIndexForLabel(diffTimelineSlot2); }
    }
    public string diffTimelineSlot3 {
        set { setDiffTimelineLabel(ref mDiffTimelineSlot3, value, diffTimelineFadeOutTime); }
        get { return mDiffTimelineSlot3; }
    }
    public int diffTimelineSlot3Index {
        set { diffTimelineSlot3 = getDiffTimelineLabelForIndex(value); }
        get { return getDiffTimelineIndexForLabel(diffTimelineSlot3); }
    }
    public string diffTimelineSlot4 {
        set { setDiffTimelineLabel(ref mDiffTimelineSlot4, value, diffTimelineFadeOutTime); }
        get { return mDiffTimelineSlot4; }
    }
    public int diffTimelineSlot4Index {
        set { diffTimelineSlot4 = getDiffTimelineLabelForIndex(value); }
        get { return getDiffTimelineIndexForLabel(diffTimelineSlot4); }
    }
    public string diffTimelineSlot5 {
        set { setDiffTimelineLabel(ref mDiffTimelineSlot5, value, diffTimelineFadeOutTime); }
        get { return mDiffTimelineSlot5; }
    }
    public int diffTimelineSlot5Index {
        set { diffTimelineSlot5 = getDiffTimelineLabelForIndex(value); }
        get { return getDiffTimelineIndexForLabel(diffTimelineSlot5); }
    }
    public string diffTimelineSlot6 {
        set { setDiffTimelineLabel(ref mDiffTimelineSlot6, value, diffTimelineFadeOutTime); }
        get { return mDiffTimelineSlot6; }
    }
    public int diffTimelineSlot6Index {
        set { diffTimelineSlot6 = getDiffTimelineLabelForIndex(value); }
        get { return getDiffTimelineIndexForLabel(diffTimelineSlot6); }
    }

    public class VariableFrame {
        public string label;
        public float value;
    };
    public class Variable {
        public List<VariableFrame> frameList = new List<VariableFrame>();
        public string label;
        public float minValue = float.MaxValue, maxValue = float.MinValue;
    };
    public bool toggleVariable = true;
    public float variableTransitionTime = 0.25f;
    public float variableTransitionEasing = 0;
    private List<Variable> mVariableList = new List<Variable>();
    private bool mVariableListLoaded = false;
    private void clearVariableList() {
        mVariableList.Clear();
        mVariableListLoaded = false;
    }
    private void touchVariableList() {
        if (mVariableListLoaded
            || ! mInitialized)
            return;
        int variableCount = Native_EmotePlayer_CountVariables(mPlayerID);
        for (int i = 0; i < variableCount; i++) {
            Variable variable = new Variable();
            string s = Marshal.PtrToStringAuto(Native_EmotePlayer_GetVariableLabelAt(mPlayerID, i));
            variable.label = s;
            int frameCount = Native_EmotePlayer_CountVariableFrameAt(mPlayerID, i);
            if (frameCount == 0) 
                continue;
            for (int j = 0; j < frameCount; j++) {
                VariableFrame frame = new VariableFrame();
                s = Marshal.PtrToStringAuto(Native_EmotePlayer_GetVariableFrameLabelAt(mPlayerID, i, j));
                frame.label = s;
                frame.value = Native_EmotePlayer_GetVariableFrameValueAt(mPlayerID, i, j);
                variable.minValue = System.Math.Min(variable.minValue, frame.value);
                variable.maxValue = System.Math.Max(variable.maxValue, frame.value);
                variable.frameList.Add(frame);
            }
            mVariableList.Add(variable);
        }
        mVariableListLoaded = true;
    }
    public List<Variable> variableList {
        get {
            touchVariableList();
            return mVariableList;
        }
    }

    public void SetVariable(string label, float value, float sec = 0, float easing = 0) {
        if (! mInitialized) {
            return;
        }
        mIsModified = true;
        Native_EmotePlayer_SetVariable(mPlayerID, label, value, sec * 60, easing);
    }
    public float GetVariable(string label) {
        if (! mInitialized)
            return 0;
        return Native_EmotePlayer_GetVariable(mPlayerID, label);
    }

    public void SetVariableDiff(string module, string label, float value, float sec = 0, float easing = 0) {
        if (! mInitialized) {
            return;
        }
        mIsModified = true;
        Native_EmotePlayer_SetVariableDiff(mPlayerID, module, label, value, sec * 60, easing);
    }
    public float GetVariableDiff(string module, string label) {
        if (! mInitialized)
            return 0;
        return Native_EmotePlayer_GetVariableDiff(mPlayerID, module, label);
    }

    public void SetOuterForce(string label, float ofx, float ofy, float sec = 0, float easing = 0) {
        if (! mInitialized)
            return;
        Native_EmotePlayer_SetOuterForce(mPlayerID, label, ofx, ofy, sec * 60, easing);
    }

    public void SetOuterRot(float rot, float sec = 0, float easing = 0) {
        if (! mInitialized)
            return;
        Native_EmotePlayer_SetOuterRot(mPlayerID, rot * Mathf.Deg2Rad, sec * 60, easing);
    }

    public bool IsLoopTimeline(string label) {
        if (! mInitialized)
          return false;
        return Native_EmotePlayer_IsLoopTimeline(mPlayerID, label);
    }
    public float GetTimelineTotalSeconds(string label) {
        if (! mInitialized)
          return 0;
        return Native_EmotePlayer_GetTimelineTotalFrameCount(mPlayerID, label) / 60;
    }

    public void PlayTimeline(string label, int flags = 0) {
        if (! mInitialized)
            return;
        mIsModified = true;
        Native_EmotePlayer_PlayTimeline(mPlayerID, label, flags);
    }

    public void StopTimeline(string label = "") {
        if (! mInitialized)
            return;
        mIsModified = true;
        Native_EmotePlayer_StopTimeline(mPlayerID, label);
    }

    public bool IsTimelinePlaying(string label = "") {
        if (! mInitialized)
            return false;
        return Native_EmotePlayer_IsTimelinePlaying(mPlayerID, label);
    }
    public void SetTimelineBlendRatio(string label, float value, float sec = 0, float easing = 0, bool stopWhenBlendDone = false) {
        if (! mInitialized)
            return;
        mIsModified = true;
        Native_EmotePlayer_SetTimelineBlendRatio(mPlayerID, label, value, sec * 60, easing, stopWhenBlendDone);
    }
    public float GetTimelineBlendRatio(string label) {
        if (! mInitialized)
            return 0;
        return Native_EmotePlayer_GetTimelineBlendRatio(mPlayerID, label);
    }
    public void FadeInTimeline(string label, float sec, float easing = 0) {
        if (! mInitialized)
            return;
        mIsModified = true;
        Native_EmotePlayer_FadeInTimeline(mPlayerID, label, sec * 60, easing);
    }
    public void FadeOutTimeline(string label, float sec, float easing = 0) {
        if (! mInitialized)
            return;
        mIsModified = true;
        Native_EmotePlayer_FadeOutTimeline(mPlayerID, label, sec * 60, easing);
    }

    public struct TimelineInfo {
      public string label;
      public int flags;
    };
    public TimelineInfo[] GetPlayingTimelineInfoList() {
        List<TimelineInfo> result = new List<TimelineInfo>();
        if (! mInitialized) {
            return result.ToArray();
        }
        int count = Native_EmotePlayer_CountPlayingTimelines(mPlayerID);
        for (int i = 0; i < count; i++) {
            TimelineInfo info = new TimelineInfo();
            info.label = Marshal.PtrToStringAuto(Native_EmotePlayer_GetPlayingTimelineLabelAt(mPlayerID, i));
            info.flags = Native_EmotePlayer_GetPlayingTimelineFlagsAt(mPlayerID, i);
            result.Add(info);
        }
        return result.ToArray();
    }

    public bool animating {
        get { 
            if (! mInitialized)
                return false;
            return Native_EmotePlayer_IsAnimating(mPlayerID);
        }
    }           

    public void Skip() {
        if (! mInitialized) {
            return;
        }
        Native_EmotePlayer_Skip(mPlayerID);
        mPrevPosition = transform.position;
    }

    public void Pass(){ 
        if (! mInitialized)
            return;
        Native_EmotePlayer_Pass(mPlayerID);
    }

    public void Step() {
        if (! mInitialized) {
            return;
        }
        Native_EmotePlayer_Step(mPlayerID);
        mPrevPosition = transform.position;
    }   

    public void StartRecordAPILog() {
        if (! mInitialized)
            return;
        Native_EmotePlayer_StartRecordAPILog(mPlayerID);
    }

    public void StopRecordAPILog() {
        if (! mInitialized)
            return;
        Native_EmotePlayer_StopRecordAPILog(mPlayerID);
    }

    public bool recordingAPILog {
        get {
            if (! mInitialized)
                return false;
            return Native_EmotePlayer_IsRecordingAPILog(mPlayerID);
        }
    }

    public void StartReplayAPILog() {
        if (! mInitialized)
            return;
        Native_EmotePlayer_StartReplayAPILog(mPlayerID);
    }

    public void StopReplayAPILog() {
        if (! mInitialized)
            return;
        Native_EmotePlayer_StopReplayAPILog(mPlayerID);
    }

    public bool replayingAPILog {
        get {
            if (! mInitialized)
                return false;
            return Native_EmotePlayer_IsReplayingAPILog(mPlayerID);
        }
    }

    public void ClearAPILog() {
        if (! mInitialized)
            return;
        Native_EmotePlayer_ClearAPILog(mPlayerID);
    }

    public string apiLog {
        get {
            if (! mInitialized)
                return "";
            string s = Marshal.PtrToStringAuto(Native_EmotePlayer_GetAPILog(mPlayerID));
            return s;
        }
        set {
            if (! mInitialized)
                return;
            Native_EmotePlayer_SetAPILog(mPlayerID, value);
        }
    }
        
    private bool mInitialized = false;
    public bool IsInitialized{
        get{ return mInitialized; }
    }
    private EmotePlayerRef mPlayerRef;
    private System.IntPtr mPlayerID = (System.IntPtr)0;

    struct SpriteMaterialKey : System.IEquatable<SpriteMaterialKey> {
        public int shaderIndex;
        public int modulate;
        public int stencil;
        public int refAlpha;
        public int meshDeformation;
        public bool ast;
#if EMOTE_SUPPORT_BBM
        public int bbmBlendIndex;
        public bool pma;
#endif //  EMOTE_SUPPORT_BBM
        
        public override int GetHashCode() {        
            int hashCode = shaderIndex.GetHashCode();
            hashCode = hashCode * 31 ^ modulate.GetHashCode();
            hashCode = hashCode * 31 ^ stencil.GetHashCode();
            hashCode = hashCode * 31 ^ refAlpha.GetHashCode();
            hashCode = hashCode * 31 ^ meshDeformation.GetHashCode();
            hashCode = hashCode * 31 ^ ast.GetHashCode();
            #if EMOTE_SUPPORT_BBM
            hashCode = hashCode * 31 ^ bbmBlendIndex.GetHashCode();
            hashCode = hashCode * 31 ^ pma.GetHashCode();
            #endif //  EMOTE_SUPPORT_BBM
            return hashCode;
        }

        bool System.IEquatable<SpriteMaterialKey>.Equals(SpriteMaterialKey other) {
            return shaderIndex == other.shaderIndex
                && modulate == other.modulate
                && stencil == other.stencil
                && refAlpha == other.refAlpha
                && meshDeformation == other.meshDeformation
                && ast == other.ast
                #if EMOTE_SUPPORT_BBM
                && bbmBlendIndex == other.bbmBlendIndex
                && pma == other.pma
                #endif // EMOTE_SUPPORT_BBM
                ;
        }          
    };
    struct StencilClearMaterialKey : System.IEquatable<StencilClearMaterialKey> {
        public bool projection;
        public int stencil;
        public int meshDeformation;
        
        public override int GetHashCode() {        
            int hashCode = projection.GetHashCode();
            hashCode = hashCode * 31 ^ stencil.GetHashCode();
            hashCode = hashCode * 31 ^ meshDeformation.GetHashCode();
            return hashCode;
        }
        bool System.IEquatable<StencilClearMaterialKey>.Equals(StencilClearMaterialKey other) {
            return projection == other.projection
                && stencil == other.stencil
                && meshDeformation == other.meshDeformation;
        }
        
    };
    struct AlphaClearMaterialKey : System.IEquatable<AlphaClearMaterialKey> {
        public bool projection;
        public float clearValue;
        public int meshDeformation;

        public override int GetHashCode() {        
            int hashCode = projection.GetHashCode();
            hashCode = hashCode * 31 ^ clearValue.GetHashCode();
            hashCode = hashCode * 31 ^ meshDeformation.GetHashCode();
            return hashCode;
        }
        bool System.IEquatable<AlphaClearMaterialKey>.Equals(AlphaClearMaterialKey other) {
            return projection == other.projection
                && clearValue == other.clearValue
                && meshDeformation == other.meshDeformation;
        }
    };

    private static Shader[] sShader;
    private static Dictionary<StencilClearMaterialKey, Material> sStencilClearMaterialDict;
    private static Dictionary<AlphaClearMaterialKey, Material> sAlphaClearMaterialDict;
    private static Dictionary<SpriteMaterialKey, Material> sSpriteMaterialDict;

    private byte[][] mCmdBufList = { null, null };
    private int[] mCmdBufActiveLengthList = { 0, 0 };
    private byte[] mCmdBuf;
    private int mCmdBufActiveLength = 0;

    class UnityEngineObjectCache<Type>
        where Type : UnityEngine.Object, new()
    {
        private List<Type> mCache = new List<Type>();
        private int mCurIndex = 0;
        
        public void Reset() {
            mCurIndex = 0;
        }
        public void Clear() {
            foreach (Type value in mCache) {
                EmotePlayer.DestroyObjectProperly(value);
            }
            mCache.Clear();
            mCurIndex = 0;
        }
        public Type Require() {
            if (mCache.Count > mCurIndex) {
                return mCache[mCurIndex++];
            } else {
                Type value = new Type();
                mCache.Add(value);
                mCurIndex++;
                return value;
            }
        }
    };

    class ObjectCache<Type>
        where Type : new()
    {
        private List<Type> mCache = new List<Type>();
        private int mCurIndex = 0;
        
        public void Reset() {
            mCurIndex = 0;
        }
        public void Clear() {
            mCache.Clear();
            mCurIndex = 0;
        }
        public Type Require() {
            if (mCache.Count > mCurIndex) {
                return mCache[mCurIndex++];
            } else {
                Type value = new Type();
                mCache.Add(value);
                mCurIndex++;
                return value;
            }
        }
    };

    class ArrayCache<Type> {
        class CacheTable {
            public List<Type[]> list = new List<Type[]>();
            public int curIndex;
        };

        private Dictionary<int, CacheTable> mCache = new Dictionary<int, CacheTable>();

        public void Reset() {
            foreach (var pair in mCache) {
                pair.Value.curIndex = 0;
            } 
        }

        public void Clear() {
            mCache.Clear();
        }

        public Type[] Require(int length) {
            CacheTable table;
            if (mCache.ContainsKey(length)) {
                table = mCache[length];
            } else {
                table = new CacheTable();
                mCache[length] = table;
            }
            if (table.list.Count > table.curIndex) {
                return table.list[table.curIndex++];
            } else {
                Type[] array = new Type[length];
                table.list.Add(array);
                table.curIndex++;
                return array;
            }
        }
    };                  

    private UnityEngineObjectCache<Mesh> mMeshCache = new UnityEngineObjectCache<Mesh>();
    private ObjectCache<CommandBuffer> mCommandBufferCache = new ObjectCache<CommandBuffer>();
    private ArrayCache<Vector2> mVector2ArrayCache = new ArrayCache<Vector2>();
    private ArrayCache<Vector3> mVector3ArrayCache = new ArrayCache<Vector3>();
    private ArrayCache<Color32> mColor32ArrayCache = new ArrayCache<Color32>();
    private ArrayCache<int> mIntArrayCache = new ArrayCache<int>();

    private float[] mFloat4Buffer = new float[4];
    private float[] mFloat16Buffer = new float[16];
    
    private bool mIsCharaProfileAvailable = false;
    private float mCharaHeight = 0;
    private Dictionary<string,float> mCharaProfileTable;
    private Rect mE_CharaBounds = new Rect(0, 0, 0, 0);
    private Rect mU_CharaBounds = new Rect(0, 0, 0, 0);
    private Rect mU_CharaMarginBounds = new Rect(0, 0, 0, 0);
    private Rect mU_RenderTextureBounds = new Rect(0, 0, 0, 0);

    private CommandBuffer[] mRenderCommandBufferList;

    void loadCharaProfile() {
        // キャラプロファイル取得
        mIsCharaProfileAvailable = Native_EmotePlayer_IsCharaProfileAvailable(mPlayerID);
        if (mIsCharaProfileAvailable) {
            mCharaHeight = Native_EmotePlayer_GetCharaHeight(mPlayerID);
            mCharaProfileTable = new Dictionary<string,float>();
            int charaProflleCount = Native_EmotePlayer_CountCharaProfiles(mPlayerID);
            for (int i = 0; i < charaProflleCount; i++) {
                string s = Marshal.PtrToStringAuto(Native_EmotePlayer_GetCharaProfileLabelAt(mPlayerID, i));
                float value = Native_EmotePlayer_GetCharaProfile(mPlayerID, s);
                mCharaProfileTable[s] = value;
            }
            float left   = mCharaProfileTable["boundsLeft"];
            float top    = mCharaProfileTable["boundsTop"];
            float right  = mCharaProfileTable["boundsRight"];
            float bottom = mCharaProfileTable["boundsBottom"];
            mCharaProfileTable.Remove("boundsLeft");
            mCharaProfileTable.Remove("boundsTop");
            mCharaProfileTable.Remove("boundsRight");
            mCharaProfileTable.Remove("boundsBottom");
            mE_CharaBounds = new Rect(left, top, right - left, bottom - top);
        }
    }
    void clearCharaProfile() {
        mIsCharaProfileAvailable = false;
        mCharaHeight = 0;
        mCharaProfileTable = null;
        mE_CharaBounds.Set(0, 0, 0, 0);
    }

    public Rect CharaBounds {
        get { return mE_CharaBounds; }
    }
    public bool isCharaProfileAvailable {
        get { return mIsCharaProfileAvailable; }
    }
    public float charaHeight {
        get { return mCharaHeight / 100; }
    }
    public bool HasCharaProfile(string key) {
        return mCharaProfileTable.ContainsKey(key);
    }
    public float GetCharaProfile(string key) {
        if (mCharaProfileTable.ContainsKey(key))
            return (float)mCharaProfileTable[key];
        else
            return 0.0f;
    }
    public enum TransformAlignment {
        LEGACY,
        ORIGIN,
        TOP,
        BOTTOM,
        CENTER,
        BUST,
        MOUTH,
        EYE,
    };
    [SerializeField]
    private TransformAlignment mTransformAlignment = TransformAlignment.ORIGIN;
    public TransformAlignment transformAlignment {
        set { 
            if (mTransformAlignment == value)
                return;
            mTransformAlignment = value;
            updateParams(ParamUpdateFlags.WIND | ParamUpdateFlags.RENDERTEXTURE);
        }
        get { return mTransformAlignment; }
    }
    public bool isLegacyTransform { 
        get { return (mTransformAlignment == TransformAlignment.LEGACY
                      || (! mIsCharaProfileAvailable
                          && ! useFixedScale)); }
    }

    [SerializeField]
    private bool _mapToRenderTexture = false;
    public bool mapToRenderTexture {
        get { return _mapToRenderTexture; }
        set {
            if (_mapToRenderTexture == value)
                return;
            _mapToRenderTexture = value;
            updateParams(ParamUpdateFlags.RENDERTEXTURE);
        }
    }

    public bool pmaEnabled {
        get { return _mapToRenderTexture && EmotePlayer.generatePremultipliedAlphaTexture; }
    }
    
    [SerializeField]
    private bool _renderTextureResolutionCorrection = true;
    public bool renderTextureResolutionCorrection {
        get { return _renderTextureResolutionCorrection; }
        set {
            if (_renderTextureResolutionCorrection == value)
                return;
            _renderTextureResolutionCorrection = value;
            updateParams(ParamUpdateFlags.RENDERTEXTURE);
        }
    }

    [SerializeField]
    private Color _renderTextureClearColor = new Color(0, 0, 0, 0);
    public Color renderTextureClearColor {
        get { return _renderTextureClearColor; }
        set {
            if (_renderTextureClearColor == value)
                return;
            _renderTextureClearColor = value;
            if (! pmaEnabled)
                foreach (var mapper in mRenderTextureMapper)
                    if (mapper.inited)
                        mapper.camera.backgroundColor = value;
        }
    }

    public void calcSuitableRenderTextureClearColor() {
        var color = suitableClearColor;
        if (! Equals(color, new Color32(0, 0, 0, 0))) {
            renderTextureClearColor = color;
            return;
        }
        long r = 0, g = 0, b = 0;
        int pixelCount = 0;
        foreach (var texInfo in mTexture) {
            var texture = texInfo.texture;
            var temp = RenderTexture.GetTemporary(texture.width, texture.height);
            Graphics.Blit(texture, temp);
            var copy = new Texture2D(texture.width, texture.height);
            copy.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
            var pixels = copy.GetPixels32();
            RenderTexture.ReleaseTemporary(temp);
            foreach (var pixel in pixels) {
                if (pixel.a == 0
                    || pixel.a == 255)
                    continue;
                r += pixel.r;
                g += pixel.g;
                b += pixel.b;
                pixelCount++;
            }
        }
        renderTextureClearColor = new Color32((byte)(r / pixelCount),
                                              (byte)(g / pixelCount),
                                              (byte)(b / pixelCount),
                                              0);
    }

    public Color32 suitableClearColor {
        get {
            if (! mInitialized)
                return new Color32(0, 0, 0, 0);
            int rgba = Native_EmotePlayer_GetSuitableClearColor(mPlayerID);
            return new Color32((byte)((rgba >> 24) & 0xff),
                               (byte)((rgba >> 16) & 0xff),
                               (byte)((rgba >> 8) & 0xff),
                               0);
        }
    }
    
    const string TexWidthNumbers = "1,2,4,8,16,32,64,128,256,512,1024,2048,4096";
    static public readonly string[] TEX_WIDTH_POPUP_NAMES = TexWidthNumbers.Split(',');
    static public readonly int[] TEX_WIDTH_POPUP_NUMS = new int[] {1,2,4,8,16,32,64,128,256,512,1024,2048,4096};

    public bool toggleRenderTexture = false;

    [SerializeField]
    [FormerlySerializedAs("mTexWidth")]
    private int _texWidth = 512;
    [SerializeField]
    [FormerlySerializedAs("mTexHeight")]
    private int _texHeight = 512;
    public int texWidth {
        get { return (int)Mathf.Min(_texWidth, SystemInfo.maxTextureSize); }
        set { 
            if (_texWidth == value)
                return;
            _texWidth = value;
            updateParams(ParamUpdateFlags.RENDERTEXTURE);
        }
    }
    public int texHeight {
        get { return (int)Mathf.Min(_texHeight, SystemInfo.maxTextureSize); }
        set { 
            if (_texHeight == value)
                return;
            _texHeight = value;
            updateParams(ParamUpdateFlags.RENDERTEXTURE);
        }
    }

    public static int INNER_RENDER_LAYER = 31;
    public static int INNER_RENDER_CULLING_MASK = 1 << INNER_RENDER_LAYER;

    public class RenderTextureMapper {
        private static List<GameObject> sQuadObjectPool;
        private static List<GameObject> sCameraObjectPool;

        public static void InitObjectPool() {
            sQuadObjectPool = new List<GameObject>();
            sCameraObjectPool = new List<GameObject>();
        }

        public static void DestroyObjectPool() {
            foreach (var obj in sQuadObjectPool) {
                // M2DebugLog.printf("destroy quad object: {0}", obj);
                EmotePlayer.DestroyObjectProperly(obj);
            }
            
            foreach (var obj in sCameraObjectPool) {
                // M2DebugLog.printf("destroy camera object: {0}", obj);
                EmotePlayer.DestroyObjectProperly(obj);
            }
            sQuadObjectPool = null;
            sCameraObjectPool = null;
        }
        
        private static GameObject requireQuadObject() {
            while (sQuadObjectPool.Count > 0) {
                GameObject obj = sQuadObjectPool[0];
                sQuadObjectPool.RemoveAt(0);
                if (obj == null) {
                    // M2DebugLog.printf("current pool object is already destory: : {0}", obj);
                    continue;
                }
                obj.SetActive(true);
                // M2DebugLog.printf("pop quad object from pool: {0}", obj);
                return obj;
            }

            GameObject gameObject = new GameObject("Emote Quad");
            gameObject.hideFlags = HideFlags.HideAndDontSave;
            gameObject.AddComponent<MeshFilter>();
            gameObject.AddComponent<MeshRenderer>();
            // M2DebugLog.printf("create quad object: {0}", gameObject);
            return gameObject;
        }

        private static void releaseQuadObject(GameObject obj) {
            // M2DebugLog.printf("push quad object to pool: {0}", obj);
            obj.SetActive(false);
            sQuadObjectPool.Add(obj);
        }

        private static GameObject requireCameraObject() {
            while (sCameraObjectPool.Count > 0) {
                GameObject obj = sCameraObjectPool[0];
                sCameraObjectPool.RemoveAt(0);
                if (obj == null) {
                    // M2DebugLog.printf("current pool object is already destory: : {0}", obj);
                    continue;
                }
                obj.SetActive(true);
                // M2DebugLog.printf("pop camera object from pool: {0}", obj);
                return obj;
            } 

            GameObject gameObject = new GameObject("Emote Camera");
            gameObject.hideFlags = HideFlags.HideAndDontSave;
            Camera camera = gameObject.AddComponent<Camera>();
            camera.depth = float.MinValue;
            camera.nearClipPlane = 0.1f;
            camera.backgroundColor = new Color(0, 0, 0, 0);
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.cullingMask = EmotePlayer.INNER_RENDER_CULLING_MASK;
            camera.orthographic = false;
            // M2DebugLog.printf("create camera object: {0}", gameObject);
            return gameObject;
        }

        private static void releaseCameraObject(GameObject obj) {
            // M2DebugLog.printf("push camera object to pool: {0}", obj);
            obj.SetActive(false);
            obj.transform.SetParent(null);
            Camera camera = obj.GetComponent<Camera>();
            camera.targetTexture = null;
            sCameraObjectPool.Add(obj);
        }

        public bool inited = false;
        public GameObject quadObject;
        public GameObject cameraObject;
        public RenderTexture renderTexture;
        public Transform  quadTransform;
        public Camera camera;
        public MaterialPropertyBlock materialProp;
        public MeshRenderer meshRenderer;

        GameObject createQuadObject() {
            GameObject gameObject = requireQuadObject();
            meshRenderer = gameObject.GetComponent<MeshRenderer>();
            return gameObject;
        }

        GameObject createCameraObject(RenderTexture renderTexture) {
            GameObject gameObject = requireCameraObject();
            Camera camera = gameObject.GetComponent<Camera>();
            camera.targetTexture = renderTexture;
            return gameObject;
        }

        public void Init(GameObject parentObject, int texWidth, int texHeight, int depthBits, Color clearColor) {
            if (inited)
                return;
            inited = true;
            renderTexture = RenderTexture.GetTemporary(texWidth, texHeight, depthBits, RenderTextureFormat.ARGB32);
            cameraObject = createCameraObject(renderTexture);
            camera = cameraObject.GetComponent<Camera>();
            camera.backgroundColor = clearColor;
            quadObject = createQuadObject();
            quadObject.transform.SetParent(parentObject.transform, false);
            quadTransform = quadObject.transform;
            materialProp = new MaterialPropertyBlock();
        }

        public void UpdateMesh(Mesh mesh) {
            if (! inited)
                return;
            MeshFilter meshFilter = quadObject.GetComponent<MeshFilter>();
            meshFilter.mesh = mesh;
        }

        public void UpdateLegacyMaterial(Renderer renderer) {
            if (! inited)
                return;
            materialProp.SetTexture(EmotePlayer.ID_MainTex, renderTexture);
            renderer.SetPropertyBlock(materialProp);
            quadObject.SetActive(false);
        }

        public void UpdateSortingLayer(string sortingLayerName, int sortingOrder) {
            if (! inited)
                return;
            meshRenderer.sortingLayerName = sortingLayerName;
            meshRenderer.sortingOrder = sortingOrder;
        }
        
        public void UpdateMaterial(Material material) {
            if (! inited)
                return;
            meshRenderer.material = material;
            materialProp.SetTexture(EmotePlayer.ID_MainTex, renderTexture);
            meshRenderer.SetPropertyBlock(materialProp);
            quadObject.SetActive(true);
        }

        public void UpdateStereovisionMaterial(Material material, RenderTexture rightEyeRenderTexture) {
            if (! inited)
                return;
            meshRenderer.material = material;
            materialProp.SetTexture(EmotePlayer.ID_MainTex, renderTexture);
            materialProp.SetTexture(EmotePlayer.ID_MainTex2, rightEyeRenderTexture);
            meshRenderer.SetPropertyBlock(materialProp);
            quadObject.SetActive(true);
        }

        public void SetActive(bool state) {
            if (! inited)
                return;
            quadObject.SetActive(state);
            cameraObject.SetActive(state);
        }

        public void Destroy() {
            if (! inited)
                return;
            releaseQuadObject(quadObject);
            releaseCameraObject(cameraObject);
            RenderTexture.ReleaseTemporary(renderTexture);
            inited = false;
            quadObject = null;
            cameraObject = null;
            renderTexture = null;
            quadTransform = null;
            camera = null;
        }
    };
    private RenderTextureMapper[] mRenderTextureMapper;

    [SerializeField]
    private float _renderTextureMargin = 0.10f;
    public float renderTextureMargin {
        set { 
            if (_renderTextureMargin == value)
                return;
            _renderTextureMargin = value; 
            updateParams(ParamUpdateFlags.WIND | ParamUpdateFlags.TRANSFORM);
        }
        get { return _renderTextureMargin; }
    }

    [SerializeField]
    private Mesh _renderTextureMesh = null;
    public Mesh renderTextureMesh {
        set {
            if (_renderTextureMesh == value)
                return;
            _renderTextureMesh = value;
            updateParams(ParamUpdateFlags.MESH);
        }
        get {
            return _renderTextureMesh;
        }
    }

    static private Mesh _defaultRenderTextureMesh = null;
    static public Mesh defaultRenderTextureMesh {
        get {
            if (_defaultRenderTextureMesh == null) {
                Mesh m = new Mesh();
                m.name = "Quad";
                Vector3[] vertices = new Vector3[]
                    {
                        new Vector3(-0.5f, -0.5f, 0),
                        new Vector3(0.5f, -0.5f, 0),
                        new Vector3(0.5f, 0.5f, 0),
                        new Vector3(-0.5f, 0.5f, 0),
                    };
                Vector2[] uv = new Vector2[]
                    {
                        new Vector2(0.0f, 0.0f),
                        new Vector2(1.0f, 0.0f),
                        new Vector2(1.0f, 1.0f),
                        new Vector2(0.0f, 1.0f)
                    };
                int[] triangles = new int[]
                    {
                        0, 1, 2,
                        0, 2, 3
                    };
                m.vertices = vertices;
                m.triangles = triangles;
                m.uv = uv;
                m.RecalculateNormals();
                m.RecalculateBounds();
                _defaultRenderTextureMesh = m;
            }
            return _defaultRenderTextureMesh;
        }
    }

    [SerializeField]
    private string _renderTextureSortingLayerName = "Default";
    public string renderTextureSortingLayerName {
        set {
            if (_renderTextureSortingLayerName == value)
                return;
            _renderTextureSortingLayerName = value;
            updateParams(ParamUpdateFlags.SORTINGLAYER);
        }
        get {
            return _renderTextureSortingLayerName;
        }
    }

    [SerializeField]
    private int _renderTextureSortingOrder = 0;
    public int renderTextureSortingOrder {
        set {
            if (_renderTextureSortingOrder == value)
                return;
            _renderTextureSortingOrder = value;
            updateParams(ParamUpdateFlags.SORTINGLAYER);
        }
        get {
            return _renderTextureSortingOrder;
        }
    }
    
    [SerializeField]
    private Material _renderTextureMaterial = null;
    public Material renderTextureMaterial {
        set {
            if (_renderTextureMaterial == value)
                return;
            _renderTextureMaterial = value;
            updateParams(ParamUpdateFlags.MATERIAL);
        }
        get {
            return _renderTextureMaterial;
        }
    }

    static private Material _defaultRenderTextureMaterial = null;
    static public Material defaultRenderTextureMaterial {
        get {
            if (_defaultRenderTextureMaterial == null) {
                _defaultRenderTextureMaterial = new Material(Shader.Find("Sprites/Default"));
            }
            return _defaultRenderTextureMaterial;
        }
    }

    static private Material _defaultRenderTexturePMAMaterial = null;
    static public Material defaultRenderTexturePMAMaterial {
        get {
            if (_defaultRenderTexturePMAMaterial == null) {
                _defaultRenderTexturePMAMaterial = new Material(Shader.Find("Emote/PMARenderer"));
            }
            return _defaultRenderTexturePMAMaterial;
        }
    }
    
    static private Material _defaultRenderTextureStereovisionMaterial = null;
    static public Material defaultRenderTextureStereivosionMaterial {
        get {
            if (_defaultRenderTextureStereovisionMaterial == null) {
                _defaultRenderTextureStereovisionMaterial = new Material(Shader.Find("Emote/StereovisionRenderer"));
                _defaultRenderTextureStereovisionMaterial.EnableKeyword("PMA_OFF");
            }
            return _defaultRenderTextureStereovisionMaterial;
        }
    }

    static private Material _defaultRenderTextureStereovisionPMAMaterial = null;
    static public Material defaultRenderTextureStereivosionPMAMaterial {
        get {
            if (_defaultRenderTextureStereovisionPMAMaterial == null) {
                _defaultRenderTextureStereovisionPMAMaterial = new Material(Shader.Find("Emote/StereovisionRenderer"));
                _defaultRenderTextureStereovisionPMAMaterial.EnableKeyword("PMA_ON");
            }
            return _defaultRenderTextureStereovisionPMAMaterial;        }
    }
    
    public float renderTextureOriginOffet {
        get {
            if (mU_RenderTextureBounds.width == 0)
               return 0;
            else
               return - mU_RenderTextureBounds.xMin / mU_RenderTextureBounds.width - 0.5f;
        }
    }

    [SerializeField]
    private bool _clipRednerTexutre = false;
    public bool clipRenderTexture {
        set { 
            if (_clipRednerTexutre == value)
                return;
            _clipRednerTexutre = value; 
            updateParams(ParamUpdateFlags.WIND | ParamUpdateFlags.TRANSFORM);
        }
        get { return _clipRednerTexutre; }
    }
    [SerializeField]
    private Rect _renderTextureClipRect = new Rect(-0.5f, -0.5f, 1.0f, 1.0f);
    public Rect renderTextureClipRect {
        set { 
            if (_renderTextureClipRect == value)
                return;
            _renderTextureClipRect = value; 
            updateParams(ParamUpdateFlags.WIND | ParamUpdateFlags.TRANSFORM);
        }
        get { return _renderTextureClipRect; }
    }

    [System.FlagsAttribute]
    enum ParamUpdateFlags {
        WIND          = 1 << 0,
        TRANSFORM     = 1 << 1,
        RENDERTEXTURE = 1 << 2,
        MESH          = 1 << 3,
        MATERIAL      = 1 << 4,
        SORTINGLAYER  = 1 << 5
    };

    void updateParams(ParamUpdateFlags flags) {
        if (! mInitialized)
            return;
        mIsModified = true;
        if ((flags & ParamUpdateFlags.RENDERTEXTURE) == ParamUpdateFlags.RENDERTEXTURE)
            flags |= ParamUpdateFlags.TRANSFORM | ParamUpdateFlags.MESH | ParamUpdateFlags.MATERIAL;
        if ((flags & ParamUpdateFlags.RENDERTEXTURE) == ParamUpdateFlags.RENDERTEXTURE)
            updateRenderTexture();
        if ((flags & ParamUpdateFlags.TRANSFORM) == ParamUpdateFlags.TRANSFORM)
            updateTransform();
        if ((flags & ParamUpdateFlags.MESH) == ParamUpdateFlags.MESH)
            updateMesh();
        if ((flags & ParamUpdateFlags.MATERIAL) == ParamUpdateFlags.MATERIAL)
            updateMaterial();
        if ((flags & ParamUpdateFlags.SORTINGLAYER) == ParamUpdateFlags.SORTINGLAYER)
            updateSortingLayer();
        if ((flags & ParamUpdateFlags.WIND) == ParamUpdateFlags.WIND)
            updateWind();
    }

    void updateWind() {
        if (mWindSpeed == 0)
            Native_EmotePlayer_StopWind(mPlayerID);
        else {
            if (! mIsCharaProfileAvailable)
                Native_EmotePlayer_StartWind(mPlayerID, -500, 500, mWindSpeed, mWindPowMin, mWindPowMax);
            else
                Native_EmotePlayer_StartWind(mPlayerID, mU_CharaBounds.xMin, mU_CharaBounds.xMax, mWindSpeed, mWindPowMin, mWindPowMax);
        }
    }

    void updateTransform() {
        if (isLegacyTransform) {
            Native_EmotePlayer_SetCoord(mPlayerID, mCoord.x, mCoord.y, 0, 0);        
            Native_EmotePlayer_SetScale(mPlayerID, mScale, 0, 0);
            Native_EmotePlayer_SetRot(mPlayerID, mRot, 0, 0);
            mE2UMatrix = Matrix4x4.TRS(Vector3.zero,
                                       Quaternion.Euler(-90, 0, -180),
                                       new Vector3(10.0f/1024, 10.0f/1024, 10.0f/1024));
            mE_CharaBounds = new Rect(-500 / scale, -500 / scale, 1000 / scale, 1000 / scale);
            if (mapToRenderTexture) {
                Vector3 cameraPosition = new Vector3(0, 10 * texHeight / 1024, 0);
                float cameraFieldOfView = Mathf.Atan2(1, 2) * Mathf.Rad2Deg * 2;
                float cameraAspect = 1.0f * texWidth / texHeight;
                foreach (RenderTextureMapper mapper in mRenderTextureMapper) {
                    if (mapper.inited) {
                        mapper.camera.transform.localPosition = cameraPosition;
                        mapper.camera.transform.LookAt(Vector3.zero, Vector3.back);
                        mapper.camera.fieldOfView = cameraFieldOfView;
                        mapper.camera.aspect = cameraAspect;
                    }
                }                
                mPixelScale = new Vector2(Mathf.Abs(0.01f / (10.0f / texWidth)),
                                          Mathf.Abs(0.01f / (10.0f / texHeight)));
                Native_EmotePlayer_SetPixelScale(mPlayerID, Mathf.Abs(mPixelScale.x), Mathf.Abs(mPixelScale.y));
            }
            return;
        }
        float e2u_scale;
        Vector2 e_origin = new Vector2(0, 0);
        if (mIsCharaProfileAvailable) {
            float height = charaHeight;
            float e_top = GetCharaProfile("top");
            float e_bottom = GetCharaProfile("bottom");
            e2u_scale = useFixedScale ? fixedScale : (height / (e_bottom - e_top));
            switch (mTransformAlignment) {
            case TransformAlignment.TOP:
                e_origin.y = e_top;
                break;
            case TransformAlignment.BOTTOM:
                e_origin.y = e_bottom;
                break;
            case TransformAlignment.CENTER:
                e_origin.y = (e_top + e_bottom) / 2;
                break;
            case TransformAlignment.BUST:
                e_origin.y = GetCharaProfile("bust");
                break;
            case TransformAlignment.MOUTH:
                e_origin.y = GetCharaProfile("mouth");
                break;
            case TransformAlignment.EYE:
                e_origin.y = GetCharaProfile("eye");
                break;
            }
        } else {
            float scale = mScale > 0 ? mScale : 1.0f;
            e2u_scale = fixedScale * scale;
            mE_CharaBounds = new Rect(-500 / scale, -500 / scale, 1000 / scale, 1000 / scale);
        }           
        
        mE2UMatrix = Matrix4x4.TRS(e_origin * e2u_scale,
                                                   Quaternion.identity,
                                                   new Vector3(e2u_scale, -e2u_scale, e2u_scale));
        mU2EMatrix = mE2UMatrix.inverse;

        Native_EmotePlayer_SetCoord(mPlayerID, 0, 0, 0, 0);        
        Native_EmotePlayer_SetScale(mPlayerID, 1, 0, 0);
        Native_EmotePlayer_SetRot(mPlayerID, 0, 0, 0);

        Vector2 bMin = mE2UMatrix.MultiplyPoint3x4(new Vector2(mE_CharaBounds.xMin, mE_CharaBounds.yMax));
        Vector2 bMax = mE2UMatrix.MultiplyPoint3x4(new Vector2(mE_CharaBounds.xMax, mE_CharaBounds.yMin));
        mU_CharaBounds = Rect.MinMaxRect(bMin.x, bMin.y, bMax.x, bMax.y);

        if (mapToRenderTexture) {
            mU_CharaMarginBounds = mU_CharaBounds;
            if (mIsCharaProfileAvailable) {
                mU_CharaMarginBounds.xMin -= renderTextureMargin;
                mU_CharaMarginBounds.xMax += renderTextureMargin;
                mU_CharaMarginBounds.yMin -= renderTextureMargin;
                mU_CharaMarginBounds.yMax += renderTextureMargin;
            }
            mU_RenderTextureBounds = mU_CharaMarginBounds;
            if (clipRenderTexture) {
                mU_RenderTextureBounds = Rect.MinMaxRect(Mathf.Max(mU_RenderTextureBounds.xMin, renderTextureClipRect.xMin),
                                                         Mathf.Max(mU_RenderTextureBounds.yMin, renderTextureClipRect.yMin),
                                                         Mathf.Min(mU_RenderTextureBounds.xMax, renderTextureClipRect.xMax),
                                                         Mathf.Min(mU_RenderTextureBounds.yMax, renderTextureClipRect.yMax));
                if (mU_RenderTextureBounds.width <= 0
                    || mU_RenderTextureBounds.height <= 0) {
                    mU_RenderTextureBounds = renderTextureClipRect;
                    if (mU_RenderTextureBounds.width <= 0) 
                        mU_RenderTextureBounds.width = 0.1f;
                    if (mU_RenderTextureBounds.height <= 0) 
                        mU_RenderTextureBounds.height = 0.1f;
                }
            }

            Vector2 u_RenderTextureCenter = mU_RenderTextureBounds.center;
            Vector3 quadPosition = u_RenderTextureCenter;
            Vector3 cameraPosition = new Vector3(quadPosition.x, quadPosition.y, -mU_RenderTextureBounds.height);
            float cameraFieldOfView = Mathf.Atan2(1, 2) * Mathf.Rad2Deg * 2;
            float cameraAspect = (mU_RenderTextureBounds.width) / (mU_RenderTextureBounds.height);
            Vector3 quadScale = new Vector3(mU_RenderTextureBounds.width, mU_RenderTextureBounds.height, 1);
            foreach (RenderTextureMapper mapper in mRenderTextureMapper) {
                if (mapper.inited) {
                    mapper.camera.transform.localPosition = cameraPosition;
                    mapper.camera.fieldOfView = cameraFieldOfView;
                    mapper.camera.aspect = cameraAspect;
                    mapper.quadTransform.localPosition = quadPosition;
                    mapper.quadTransform.localScale = quadScale;
                }
            }

            if (renderTextureResolutionCorrection) {
                var e2u_pixel = mE2UMatrix.MultiplyPoint3x4(new Vector2(1, 1)) - mE2UMatrix.MultiplyPoint3x4(new Vector2(0, 0));
                mPixelScale = new Vector2(Mathf.Abs(e2u_pixel.x / (mU_RenderTextureBounds.width / texWidth)),
                                          Mathf.Abs(e2u_pixel.y / (mU_RenderTextureBounds.height / texHeight)));
                Native_EmotePlayer_SetPixelScale(mPlayerID, Mathf.Abs(mPixelScale.x), Mathf.Abs(mPixelScale.y));
            } else {
                Native_EmotePlayer_SetPixelScale(mPlayerID, 1.0f, 1.0f);
            }
        }
    }

    public Vector3 GetCharaMarker(string key) {
        if (! mIsCharaProfileAvailable
            || ! HasCharaProfile(key))
            return transform.position;

        return transform.TransformPoint(mE2UMatrix.MultiplyPoint3x4(new Vector3(0, GetCharaProfile(key), 0)));
    }

    void updateRenderTexture() {
        if (mInitialized)
            Native_EmotePlayer_SetScreenSize(mPlayerID, texWidth, texHeight);
        foreach (RenderTextureMapper mapper in mRenderTextureMapper) 
            mapper.Destroy();
        if (mapToRenderTexture) {
            int depthBits = effectiveMaskMode == (int)MaskMode.Stencil ? 24 : 0;
            Color clearColor;
            if (pmaEnabled)
                clearColor = new Color(0, 0, 0, 0);
            else
                clearColor =  _renderTextureClearColor;
            if (! stereovisionEnabled) {
                mRenderTextureMapper[0].Init(gameObject, texWidth, texHeight, depthBits, clearColor);
            } else {
                mRenderTextureMapper[0].Init(gameObject, texWidth, texHeight, depthBits, clearColor);
                mRenderTextureMapper[1].Init(gameObject, texWidth, texHeight, depthBits, clearColor);
            }
        }
    }

    void updateMesh() {
        if (mapToRenderTexture) {
            if (isLegacyTransform) {
                var renderer = this.GetComponent<Renderer>();
                if (renderer != null
                    && renderer.enabled)
                    return;
            }
            Mesh mesh = renderTextureMesh;
            if (mesh == null)
                mesh = defaultRenderTextureMesh;
            if (! stereovisionEnabled) {
                mRenderTextureMapper[0].UpdateMesh(mesh);
            } else {
                mRenderTextureMapper[0].UpdateMesh(mesh);
                mRenderTextureMapper[1].UpdateMesh(mesh);
            }
        }
    }

    void updateMaterial() {
        if (mapToRenderTexture) {
            if (isLegacyTransform) {
                var renderer = this.GetComponent<Renderer>();
                if (renderer != null
                    && renderer.enabled) {
                    mRenderTextureMapper[0].UpdateLegacyMaterial(renderer);
                    return;
                }
            }
            Material material = renderTextureMaterial;
            if (material == null) {
                if (singleCameraStereovision) {
                    if (pmaEnabled)
                        material = defaultRenderTextureStereivosionPMAMaterial;
                    else
                        material = defaultRenderTextureStereivosionMaterial;
                } else {
                    if (pmaEnabled)
                        material = defaultRenderTexturePMAMaterial;
                    else
                        material = defaultRenderTextureMaterial;
                }
            }
            if (! stereovisionEnabled) {
                mRenderTextureMapper[0].UpdateMaterial(material);
            } else if (! singleCameraStereovision) {
                mRenderTextureMapper[0].UpdateMaterial(material);
                mRenderTextureMapper[1].UpdateMaterial(material);
            } else {
                mRenderTextureMapper[0].UpdateStereovisionMaterial(material, mRenderTextureMapper[1].renderTexture);
                mRenderTextureMapper[1].meshRenderer.enabled = false;
            }
        }
    }

    void updateSortingLayer() {
        if (mapToRenderTexture) {
            foreach (var mapper in mRenderTextureMapper)
                mapper.UpdateSortingLayer(renderTextureSortingLayerName, renderTextureSortingOrder);
        }
    }
    
    private ArrayList ArrayToList<T>(T[] array) {
        ArrayList list = new ArrayList();
        list.AddRange(array);
        return list;
    }                          

    static private string[] extractPathObject(object pathObject) {
        string[] paths = null;
        if (pathObject is string)
            paths = new string[] { (string) pathObject };
        else if (pathObject is string[])
            paths = (string[]) pathObject;
        return paths;
    }

    static public EmoteAsset LoadAsset(object pathObject, AssetBundle bundle = null) {
        string[] paths = extractPathObject(pathObject);
        if (paths == null)
            return null;
        EmoteAssetRequest request;
        if (bundle == null)
            request = new EmoteAssetFromResourcesRequest(paths, false);
        else
            request = new EmoteAssetFromAssetBundleRequest(bundle, paths, false);
        return request.asset;
    }

    static public EmoteAssetRequest LoadAssetAsync(object pathObject, AssetBundle bundle = null) {
        string[] paths = extractPathObject(pathObject);
        if (paths == null)
            return null;
        EmoteAssetRequest request;
        if (bundle == null)
            request = new EmoteAssetFromResourcesRequest(paths, true);
        else
            request = new EmoteAssetFromAssetBundleRequest(bundle, paths, true);
        return request;
    }

#if UNITY_EDITOR
    static public EmoteAsset LoadAssetFromAssetDatabase(string path) {
        string[] paths = { path };
        EmoteAssetRequest request;
        request = new EmoteAssetFromAssetDatabaseRequest(paths, false);
        return request.asset;
    }
#endif // UNITY_EDITOR

    public void LoadData(object data, AssetBundle bundle = null) {
        EmoteAsset asset;
        if (data is string
            || data is string[]) {
            asset = LoadAsset(data, bundle);
        } else if (data is EmoteAsset) {
            asset = (EmoteAsset) data;
        } else if (data is TextAsset) {
            asset = new EmoteAsset();
            asset.files.Add((TextAsset)data);
            if (! supressBuiltinTextureImageWarning) {
                Debug.LogWarning(@"
The TextAsset object was passed as an argument to the LoadData() method.
Texture separation type data can not be loaded by specifying TextAsset.
To supress this warning, please check the appropriate setting in E-mote Global Settings.");
            }
        } else {
            M2DebugLog.printf("LoadData(): illegul type of input data.");
            return;
        }
        DestroyCore();
        StartCore(asset);
    }

    public void UnloadData() {
      DestroyCore();
    }
    
    enum MaskMode : int {
        Stencil,
        Alpha
    };

    enum MaskType : int {
        None,
        Apply,
        Inner,
        Outer
    };

    enum ShaderIndex : int {
        Alpha,
        PMA,
        Add,
        PSSubtract,
        Multiply,
        Screen,
        Embed,
        Outline,
#if EMOTE_SUPPORT_BBM
        BBM,
#endif // EMOTE_SUPPORT_BBM
        AlphaApplyMask,
        PMAApplyMask,
        AddAplyMask,
        PSSubtractApplyMask,
        MultiplyApplyMask,
        ScreenApplyMask,
        EmbedApplyMask,
        OutlineApplyMask,
#if EMOTE_SUPPORT_BBM
        BBMApplyMask,
#endif // EMOTE_SUPPORT_BBM
        StencilInnerMask,
        StencilOuterMask,
        StencilClearMask,
        AlphaInnerMask,
        AlphaOuterMask,
        AlphaClearMask,
        MAX
    };

    static readonly string[] SHADER_PATH = {
        "Emote/Sprite",
        "Emote/SpritePMA",
        "Emote/SpriteAdd",
        "Emote/SpritePSSubtract",
        "Emote/SpriteMultiply",
        "Emote/SpriteScreen",
        "Emote/SpriteEmbed",
        "Emote/SpriteOutline",
#if EMOTE_SUPPORT_BBM
        "Emote/SpriteBBM",
#endif // EMOTE_SUPPORT_BBM
        "Emote/SpriteApplyMask",
        "Emote/SpritePMAApplyMask",
        "Emote/SpriteAddApplyMask",
        "Emote/SpritePSSubtractApplyMask",
        "Emote/SpriteMultiplyApplyMask",
        "Emote/SpriteScreenApplyMask",
        "Emote/SpriteEmbedApplyMask",
        "Emote/SpriteOutlineApplyMask",
#if EMOTE_SUPPORT_BBM
        "Emote/SpriteBBMApplyMask",
#endif // EMOTE_SUPPORT_BBM
        "Emote/SpriteStencilInnerMask",
        "Emote/SpriteStencilOuterMask",
        "Emote/SpriteStencilClearMask",
        "Emote/SpriteAlphaInnerMask",
        "Emote/SpriteAlphaOuterMask",
        "Emote/SpriteAlphaClearMask"
    };

#if EMOTE_SUPPORT_BBM
    enum BBMBlendIndex : int {
        Invalid = -1,
        PsAdditive = 0,
        PsSubtractive,
        PsOverlay,
        PsHardlight,
        PsSoftlight,
        PsColordodge,
        PsColorburn,
        PsLighten,
        PsDarken,
        PsDifference,
        PsExclusion,
        FltGrayscale,
        FltMosaic,
        FltBlur1,
        FltBlur2,
        FltBlur3,
        FltBlur6,
        FltBlur9,

        RawAdd,
        RawSub,
        RawMultiply,
        RawScreen
    };

    static readonly string[] BBM_BLEND_KEYWORD = {
        "BBM_BLEND_PSADDITIVE",
        "BBM_BLEND_PSSUBTRACTIVE",
        "BBM_BLEND_PSOVERLAY",
        "BBM_BLEND_PSHARDLIGHT",
        "BBM_BLEND_PSSOFTLIGHT",
        "BBM_BLEND_PSCOLORDODGE",
        "BBM_BLEND_PSCOLORBURN",
        "BBM_BLEND_PSLIGHTEN",
        "BBM_BLEND_PSDARKEN",
        "BBM_BLEND_PSDIFFERENCE",
        "BBM_BLEND_PSEXCLUSION",
        "BBM_BLEND_FLTGRAYSCALE",
        "BBM_BLEND_FLTMOSAIC",
        "BBM_BLEND_FLTBLUR1",
        "BBM_BLEND_FLTBLUR2",
        "BBM_BLEND_FLTBLUR3",
        "BBM_BLEND_FLTBLUR6",
        "BBM_BLEND_FLTBLUR9",

        "BBM_BLEND_RAWADD",
        "BBM_BLEND_RAWSUB",
        "BBM_BLEND_RAWMULTIPLY",
        "BBM_BLEND_RAWSCREEN",
    };
#endif // EMOTE_SUPPORT_BBM

    void InitDrawResources() {
        if (mRenderTextureMapper == null) {
            mRenderTextureMapper = new RenderTextureMapper[2];
            mRenderTextureMapper[0] = new RenderTextureMapper();
            mRenderTextureMapper[1] = new RenderTextureMapper();
        }
        if (mRenderCommandBufferList == null)
            mRenderCommandBufferList = new CommandBuffer[3];
    }

    void DestroyDrawResources() {
        mMeshCache.Clear();
        mCommandBufferCache.Clear();
        
        if (mRenderCommandBufferList != null) {
            foreach (var cb in mRenderCommandBufferList) 
                if (cb != null) {
                    cb.Clear();
                }
            mRenderCommandBufferList = null;
        }

        if (mRenderTextureMapper != null)
            foreach (RenderTextureMapper mapper in mRenderTextureMapper)
                mapper.Destroy();

        mVector2ArrayCache.Clear();
        mVector3ArrayCache.Clear();
        mColor32ArrayCache.Clear();
        mIntArrayCache.Clear();
    }

    void SetDrawResourceActive(bool state) {
        foreach (RenderTextureMapper mapper in mRenderTextureMapper)
            mapper.SetActive(state);
    }

    static void InitShaders() {
        sShader = new Shader[(int)ShaderIndex.MAX];
        for (int i = 0; i < (int)ShaderIndex.MAX; i++) {
            sShader[i] = Shader.Find(SHADER_PATH[i]);
        }
        sStencilClearMaterialDict = new Dictionary<StencilClearMaterialKey, Material>();
        sAlphaClearMaterialDict = new Dictionary<AlphaClearMaterialKey, Material>();
        sSpriteMaterialDict = new Dictionary<SpriteMaterialKey, Material>();
    }

    static void DestroyShaders() {
        sShader = null;

        foreach (var material in sSpriteMaterialDict.Values) 
            DestroyObjectProperly(material);
        sSpriteMaterialDict = null;

        foreach (var material in sStencilClearMaterialDict.Values) 
            DestroyObjectProperly(material);
        sStencilClearMaterialDict = null;

        foreach (var material in sAlphaClearMaterialDict.Values) 
            DestroyObjectProperly(material);
        sAlphaClearMaterialDict = null;
    }

    void ResetRenderEnvironments() {
         for (int i = 0; i < 3; i++) {
             if (mRenderCommandBufferList[i] != null) {
                 mRenderCommandBufferList[i].Clear();
                 mRenderCommandBufferList[i] = null;
             }
         }

         mMeshCache.Reset();
         mCommandBufferCache.Reset();
         
         mVector2ArrayCache.Reset();
         mVector3ArrayCache.Reset();
         mColor32ArrayCache.Reset();
         mIntArrayCache.Reset();
    }

    void SetColorFilter(CommandBuffer cb) {
        if (mColorFilter != 5
            || (mFilterColor & 0xff) == 0) 
            cb.DisableShaderKeyword("GRAYSCALE_ON");
        else {
            cb.EnableShaderKeyword("GRAYSCALE_ON");
            cb.SetGlobalFloat(sID_Grayscale, (float)(mFilterColor & 0xff) / 255);
        }
        cb.SetGlobalFloat(sID_TestAlpha, mMaskAlphaTestRef);
    }

    static Material RequireStencilClearMaterial(bool projection, int stencil, int meshDeformation) {
        StencilClearMaterialKey key;
        key.projection = projection;
        key.stencil = stencil;
        key.meshDeformation = meshDeformation;
        if (sStencilClearMaterialDict.ContainsKey(key)) {
            return sStencilClearMaterialDict[key];
        } else {
            Material material = new Material(sShader[(int)ShaderIndex.StencilClearMask]);
            sStencilClearMaterialDict.Add(key, material);
            material.SetInt(sID_Stencil, stencil);
            if (projection)
                material.EnableKeyword("PROJECTION_ON");
            switch (meshDeformation) {
                default: break;
                case 0: material.EnableKeyword("MESH_DEFORMATION_OFF"); break;
                case 1: material.EnableKeyword("MESH_DEFORMATION_ON");  break;
            }
            return material;
        }
    }

    static Material RequireAlphaClearMaterial(bool projection, float clearValue, int meshDeformation) {
        AlphaClearMaterialKey key;
        key.projection = projection;
        key.clearValue = clearValue;
        key.meshDeformation = meshDeformation;
        if (sAlphaClearMaterialDict.ContainsKey(key)) {
            return sAlphaClearMaterialDict[key];
        } else {
            Material material = new Material(sShader[(int)ShaderIndex.AlphaClearMask]);
            sAlphaClearMaterialDict.Add(key, material);
            material.SetColor(sID_ClearColor, new Color(clearValue, clearValue, clearValue, clearValue));
            if (projection)
                material.EnableKeyword("PROJECTION_ON");
            switch (meshDeformation) {
                default: break;
                case 0: material.EnableKeyword("MESH_DEFORMATION_OFF"); break;
                case 1: material.EnableKeyword("MESH_DEFORMATION_ON");  break;
            }
            return material;
        }
    }

    static Material RequireSpriteMaterial(ShaderIndex shaderIndex, int modulate, int stencil, int refAlpha, int meshDeformation, bool ast
#if EMOTE_SUPPORT_BBM
                                          , BBMBlendIndex bbmBlendIndex = BBMBlendIndex.Invalid
                                          , bool pma = false
#endif // EMOTE_SUPPORT_BBM
                                          ) {
        SpriteMaterialKey key;
        key.shaderIndex = (int)shaderIndex;
        key.modulate = modulate;
        key.stencil = stencil;
        key.refAlpha = refAlpha;
        key.meshDeformation = meshDeformation;
        key.ast = ast;
#if EMOTE_SUPPORT_BBM
        key.bbmBlendIndex = (int)bbmBlendIndex;
        key.pma = pma;
#endif // EMOTE_SUPPORT_BBM
        if (sSpriteMaterialDict.ContainsKey(key)) {
            return sSpriteMaterialDict[key];
        } else {
            Material material = new Material(sShader[key.shaderIndex]);
            sSpriteMaterialDict.Add(key, material);
            material.SetInt(sID_Stencil, stencil);
            switch (modulate) {
            default: break;
            case 1: material.EnableKeyword("VERTCOLOR_SINGLE"); break;
            case 2: material.EnableKeyword("VERTCOLOR_DOUBLE"); break;
            }
            switch (refAlpha)  {
            default: break;
            case 1: material.EnableKeyword("REFALPHAMASK_ON"); break;
            case 2: material.EnableKeyword("REFALPHAMASK_ON2"); break;
            }
            switch (meshDeformation) {
                default: break;
                case 0: material.EnableKeyword("MESH_DEFORMATION_OFF"); break;
                case 1: material.EnableKeyword("MESH_DEFORMATION_ON");  break;
            }
            if (ast) 
                material.EnableKeyword("TEXTURE_AST_ON");
            else
                material.EnableKeyword("TEXTURE_AST_OFF");
#if EMOTE_SUPPORT_BBM
            if (bbmBlendIndex != BBMBlendIndex.Invalid) {
                material.EnableKeyword(BBM_BLEND_KEYWORD[(int)bbmBlendIndex]);
            }
            if (pma)
                material.EnableKeyword("PMA_ON");
            else
                material.EnableKeyword("PMA_OFF");
#endif // EMOTE_SUPPORT_BBM
            return material;
        }
    }

    internal class TemporaryTextureInfo {
        public int lifespan;
        public Texture texture;
    };

    private List<TemporaryTextureInfo> mTemoraryTextureInfoList = new List<TemporaryTextureInfo>();
    private List<TemporaryTextureInfo> mTemoraryTextureInfoPool = new List<TemporaryTextureInfo>();
    
    private void ReleaseTemporaryTextures(bool force = false) {
        for (var i = 0; i < mTemoraryTextureInfoList.Count; i++) {
            var texInfo = mTemoraryTextureInfoList[i];
            if (force
                || texInfo.lifespan-- <= 0) {
#if UNITY_EDITOR
                if (! EditorApplication.isPlaying)
                    Object.DestroyImmediate(texInfo.texture);
                else
#endif // UNITY_EDITOR
                    Object.Destroy(texInfo.texture);

                texInfo.texture = null;
                mTemoraryTextureInfoPool.Add(texInfo);
                mTemoraryTextureInfoList.RemoveAt(i--);
            }
        }
    }
    
    private void MarkAsTemporaryTexture(Texture tex) {
        TemporaryTextureInfo texInfo;
        if (mTemoraryTextureInfoPool.Count == 0) {
            texInfo = new TemporaryTextureInfo();
        } else {
            texInfo = mTemoraryTextureInfoPool[0];
            mTemoraryTextureInfoPool.RemoveAt(0);
        }
        texInfo.lifespan = 2;
        texInfo.texture = tex;
        mTemoraryTextureInfoList.Add(texInfo);
    }

    void ExtractBuiltinTextureImage() {
        int builtinImageCount = Native_EmotePlayer_GetCommandBufferBuiltinImageCount(mPlayerID);
        int maxImageCount = Native_EmotePlayer_GetCommandBufferMaxImageCount(mPlayerID);
        if (builtinImageCount == 0) {
            ImageInfo imageInfo = new ImageInfo();
            for (int index =  0; index < maxImageCount; index++) {
                Native_EmotePlayer_GetCommandBufferBuiltinImageInfo(mPlayerID, index, ref imageInfo);
                var texInfo = mTexture[index];
                texInfo.isAst = (imageInfo.ast == 1);
                mTexture[index] = texInfo;
            }
            return;
        }
        if (! supressBuiltinTextureImageWarning)
            Debug.LogWarning("Builtin texture image format detected in E-mote data.\nPlease use the \"texture separation\" setting if possible.\nTo supress this warning, please check the appropriate setting in E-mote Global Settings.");
        byte[] pixelBuffer = null;
        int pixelBufferSize = 0;
        for (int index =  0; index < maxImageCount; index++) {
            ImageInfo imageInfo = new ImageInfo();
            Native_EmotePlayer_GetCommandBufferBuiltinImageInfo(mPlayerID, index, ref imageInfo);
            if (imageInfo.hasBuiltinImage == 0) 
                continue;
            int format = imageInfo.format;
            TextureFormat textureFormat;
            int pitchbytes;
            switch ((BuiltinTextureImageFormat)imageInfo.format) {
            default:
            case BuiltinTextureImageFormat.RGBA32:
                format = (int)BuiltinTextureImageFormat.RGBA32;
                textureFormat = TextureFormat.RGBA32;
                pitchbytes = 4;
                break;
#if (EMOTE_PLATFORM_IPHONE || EMOTE_PLATFORM_ANDROID || EMOTE_PLATFORM_WEBGL)
            case BuiltinTextureImageFormat.RGBA4444:
                textureFormat = TextureFormat.RGBA4444;
                pitchbytes = 2;
                break;
#endif
            }
            if (! SystemInfo.SupportsTextureFormat(textureFormat)) {
                format = (int)BuiltinTextureImageFormat.RGBA32;
                textureFormat = TextureFormat.RGBA32;
                pitchbytes = 4;
            }
            int size = imageInfo.width * imageInfo.height * pitchbytes;
            if (size > pixelBufferSize) {
                pixelBufferSize = size;
                pixelBuffer = new byte[pixelBufferSize];
            }
            GCHandle pixelHandle = GCHandle.Alloc(pixelBuffer, GCHandleType.Pinned);
            Native_EmotePlayer_ExtractCommandBufferBuiltinImage(mPlayerID, index, pixelHandle.AddrOfPinnedObject(), format);
            pixelHandle.Free();
            Native_EmotePlayer_DestroyCommandBufferBuiltinImage(mPlayerID, index);
            Texture2D tex = new Texture2D(imageInfo.width, imageInfo.height, textureFormat, false);
            tex.wrapMode = TextureWrapMode.Clamp;
            tex.LoadRawTextureData(pixelBuffer);
            tex.Apply(false, true);
            mTexture.Insert(index, new TextureInfo(tex, false, imageInfo.ast == 1));
        }
    }
        
    bool StartCore(EmoteAsset asset = null) {
        // M2DebugLog.printf("StartCore {0}", asset);
        
        if (mInitialized)
            return true;

#if EMOTE_NATIVE_PLUGIN_DISABLED
        return true;
#else // EMOTE_NATIVE_PLUGIN_DISABLED

        if (asset == null) {
            asset = extractPreloadAssets();
            if (asset == null)
                return false;
        }
        
        List<byte[]> psbList = asset.files.Select(x => x.bytes).ToList();
        if (asset.rawFileImage != null)
            psbList.Add(asset.rawFileImage);
        
      if (psbList.Count == 0)
        return false;
        
        if (!mInitialized) {
            InitDrawResources();
            mInitialized = true;
            mCmdBuf = new byte[1024*8*4];
            mIsModified = true;
            mTexture = new List<TextureInfo>();
            foreach (var texture in asset.textures) 
                mTexture.Add(new TextureInfo(texture));

            // PSBファイルをEmotePlayerに登録.
            PSBRef[] psbRefArray = new PSBRef[psbList.Count];
            GCHandle[] gchArray = new GCHandle[psbList.Count];
            for (int i = 0; i < psbList.Count; i++) {
              byte[] image = psbList[i];
              int size = image.Length;
              GCHandle gch = GCHandle.Alloc(image, GCHandleType.Pinned);
              psbRefArray[i] = new PSBRef(gch.AddrOfPinnedObject(), size);
              gchArray[i] = gch;
            }
            mPlayerRef = new EmotePlayerRef(psbRefArray, psbRefArray.Length);
            foreach (GCHandle g in gchArray)
              g.Free();
            mPlayerID = mPlayerRef.playerID;
            // テクスチャ
            ExtractBuiltinTextureImage();
            // EmotePlayer初期化.
            Native_EmotePlayer_SetAsOriginalScale(mPlayerID, useFixedScale);
            loadCharaProfile();
            updateParams(ParamUpdateFlags.WIND | ParamUpdateFlags.TRANSFORM | ParamUpdateFlags.RENDERTEXTURE | ParamUpdateFlags.SORTINGLAYER);
            Native_EmotePlayer_SetStereovisionEnabled(mPlayerID, mStereovisionEnabled);
            Native_EmotePlayer_SetStereovisionVolume(mPlayerID, mStereovisionVolume);
            Native_EmotePlayer_SetStereovisionParallaxRatio(mPlayerID, mStereovisionParallaxRatio);
            Native_EmotePlayer_SetColor(mPlayerID, mVertexColor.r, mVertexColor.g, mVertexColor.b, mVertexColor.a, 0, 0);
            Native_EmotePlayer_SetGrayscale(mPlayerID, mGrayscale, 0, 0);
            Native_EmotePlayer_SetHairScale(mPlayerID, mHairScale * globalHairScale);
            Native_EmotePlayer_SetBustScale(mPlayerID, mBustScale * globalBustScale);
            Native_EmotePlayer_SetPartsScale(mPlayerID, mPartsScale * globalPartsScale);
            Native_EmotePlayer_SetMeshDivisionRatio(mPlayerID, mMeshDivisionRatio * globalMeshDivisionRatio);
            if (mMainTimelineLabel != "") {
                Native_EmotePlayer_PlayTimeline(mPlayerID, mMainTimelineLabel, (int)TimelinePlayFlags.TIMELINE_PLAY_PARALLEL);
            }
            if (mDiffTimelineLabel != "") {
                Native_EmotePlayer_PlayTimeline(mPlayerID, mDiffTimelineLabel, (int)(TimelinePlayFlags.TIMELINE_PLAY_PARALLEL | TimelinePlayFlags.TIMELINE_PLAY_DIFFERENCE));
                Native_EmotePlayer_SetTimelineBlendRatio(mPlayerID, mDiffTimelineLabel, mDiffTimelineBlendRatio, 0, 0, false);
            }
            if (mDiffTimelineSlot1 != "") 
                Native_EmotePlayer_PlayTimeline(mPlayerID, mDiffTimelineSlot1, (int)(TimelinePlayFlags.TIMELINE_PLAY_PARALLEL | TimelinePlayFlags.TIMELINE_PLAY_DIFFERENCE));
            if (mDiffTimelineSlot2 != "") 
                Native_EmotePlayer_PlayTimeline(mPlayerID, mDiffTimelineSlot2, (int)(TimelinePlayFlags.TIMELINE_PLAY_PARALLEL | TimelinePlayFlags.TIMELINE_PLAY_DIFFERENCE));
            if (mDiffTimelineSlot3 != "") 
                Native_EmotePlayer_PlayTimeline(mPlayerID, mDiffTimelineSlot3, (int)(TimelinePlayFlags.TIMELINE_PLAY_PARALLEL | TimelinePlayFlags.TIMELINE_PLAY_DIFFERENCE));
            if (mDiffTimelineSlot4 != "") 
                Native_EmotePlayer_PlayTimeline(mPlayerID, mDiffTimelineSlot4, (int)(TimelinePlayFlags.TIMELINE_PLAY_PARALLEL | TimelinePlayFlags.TIMELINE_PLAY_DIFFERENCE));
            if (mDiffTimelineSlot5 != "") 
                Native_EmotePlayer_PlayTimeline(mPlayerID, mDiffTimelineSlot5, (int)(TimelinePlayFlags.TIMELINE_PLAY_PARALLEL | TimelinePlayFlags.TIMELINE_PLAY_DIFFERENCE));
            if (mDiffTimelineSlot6 != "") 
                Native_EmotePlayer_PlayTimeline(mPlayerID, mDiffTimelineSlot6, (int)(TimelinePlayFlags.TIMELINE_PLAY_PARALLEL | TimelinePlayFlags.TIMELINE_PLAY_DIFFERENCE));

            mPrevPosition = transform.position;


            // M2DebugLog.printf("StartCore({0}): Initialized", mPlayerID);
        }

        return true;
#endif // EMOTE_NATIVE_PLUGIN_DISABLED
    }

    bool DestroyCore(bool onDisable = false) {
        if (! mInitialized) {
            return true;
        }
        DestroyDrawResources();
        releaseTexture();
        mInitialized = false;
        if (mPlayerRef != null) {
            mPlayerRef.Destroy();
            mPlayerRef = null;
        }
        mPlayerID = (System.IntPtr)0;
        clearMainTimelineLabels();
        clearDiffTimelineLabels();
        clearVariableList();
        clearCharaProfile();
        mCulled = false;
        
        return true;
    }

    internal struct TextureInfo {
        public Texture2D texture;
        public bool isAsset;
        public bool isAst;
        
        public TextureInfo(Texture2D texture, bool isAsset = true, bool isAst = false) {
            this.texture = texture;
            this.isAsset = isAsset;
            this.isAst   = isAst;
        }
    };
    private List<TextureInfo> mTexture;

    // テクスチャ開放.
    void releaseTexture() {
        if (mTexture != null) {
            foreach (var textureInfo in mTexture) {
                if (! textureInfo.isAsset)
                    EmotePlayer.DestroyObjectProperly(textureInfo.texture);
            }
            mTexture.Clear();
            mTexture = null;
        }
        ReleaseTemporaryTextures(true);
    }

    [System.NonSerialized]
    public static List<EmotePlayer> activePlayers = new List<EmotePlayer>();
    [System.NonSerialized]
    public float cameraDepth;

    void OnEnable() {
        // M2DebugLog.printf("OnEnable({0})", mPlayerID);
        InitDrawResources();
        SetDrawResourceActive(true);
        activePlayers.Add(this);
        updateParams(ParamUpdateFlags.RENDERTEXTURE);
        StartCore();
    }

    void OnDisable() {
        // M2DebugLog.printf("OnDisable({0})", mPlayerID);
        activePlayers.Remove(this);
        DestroyDrawResources();
        SetDrawResourceActive(false);
#if UNITY_EDITOR
       // force DestroyCore when edit mode (countermeasure for resource lost in build)
       if (! EditorApplication.isPlaying)
          DestroyCore();
#endif // UNITY_EDITOR
    }

    void OnDestroy() {
        // M2DebugLog.printf("OnDestroy({0})", mPlayerID);
        DestroyCore();
        DestroyDrawResources();
    }

    private static bool sID_Initialized = false;
    private static int sID_Stencil;
    private static int sID_Color;
    private static int sID_ClearColor;
    private static int sID_Grayscale;
    private static int sID_TestAlpha;
    private static int sID_MainTex;
    private static int sID_MainTex2;
    private static int sID_MeshDeformerIndexArray;
    private static int sID_MeshDeformerIndexArraySize;
    private static int sID_MeshDeformerParamsTex;
    private static int sID_MeshDeformerParamsSampleArg;
    private static int sID_MeshDeformerOffset;
    private static int[] sID_AlphaMaskTex = new int[2];
    private static int sID_OutlineParam;
    private static int sID_OutlineColor;
    private static int sID_AlphaCutoff;
    #if EMOTE_SUPPORT_BBM
    private static int sID_BlurParam;
    private static int sID_MosaicParam;
    private static int sID_BBMBufferTex;
    private static int sID_BBMBufferOffset;
    #endif // EMOTE_SUPPORT_BBM

    static int ID_MainTex { get { return sID_MainTex; } }
    static int ID_MainTex2 { get { return sID_MainTex2; } }

    void Awake() {
        InitID();
    }

    void InitID() {
        if (sID_Initialized)
            return;
        sID_Initialized = true;
        sID_Stencil = Shader.PropertyToID("_Stencil");
        sID_ClearColor = Shader.PropertyToID("_ClearColor");
        sID_Color = Shader.PropertyToID("_Color");
        sID_Grayscale = Shader.PropertyToID("_Grayscale");
        sID_TestAlpha = Shader.PropertyToID("_TestAlpha");
        sID_MainTex = Shader.PropertyToID("_MainTex");
        sID_MainTex2 = Shader.PropertyToID("_MainTex2");
        sID_MeshDeformerIndexArray = Shader.PropertyToID("_MeshDeformerIndexArray");
        sID_MeshDeformerIndexArraySize = Shader.PropertyToID("_MeshDeformerIndexArraySize");
        sID_MeshDeformerParamsTex = Shader.PropertyToID("_MeshDeformerParamsTex");
        sID_MeshDeformerParamsSampleArg = Shader.PropertyToID("_MeshDeformerParamsSampleArg");
        sID_MeshDeformerOffset = Shader.PropertyToID("_MeshDeformerOffset");
        sID_OutlineParam = Shader.PropertyToID("_OutlineParam");
        sID_OutlineColor = Shader.PropertyToID("_OutlineColor");
        sID_AlphaCutoff = Shader.PropertyToID("_AlphaCutoff");
            
        sID_AlphaMaskTex[0] = Shader.PropertyToID("_AlphaMaskTex");
        sID_AlphaMaskTex[1] = Shader.PropertyToID("_AlphaMaskTex2");
        
        #if EMOTE_SUPPORT_BBM

        sID_BBMBufferTex = Shader.PropertyToID("_BBMBufferTex");
        sID_BBMBufferOffset = Shader.PropertyToID("_BBMBufferOffset");
        sID_BlurParam = Shader.PropertyToID("_BlurParam");
        sID_MosaicParam = Shader.PropertyToID("_MosaicParam");

        #endif // EMOTE_SUPPORT_BBM
    }
    
    void Start() {
        // M2DebugLog.printf("Start({0}): STANDALONE: start: mPSBFile={1}", mPlayerID, mPSBFile);
        StartCore();
        if (mPSBFile == null) {
            // M2DebugLog.printf("Start({0}): STANDALONE: mPSBFile is null", mPlayerID);
        }
    }

    void Update()
    {
        if (updateTiming == UpdateTiming.UPDATE)
            UpdateCore();
    }

    void LateUpdate() 
    {
        if (updateTiming == UpdateTiming.LATE_UPDATE)
            UpdateCore();
    }

    void UpdateCore() {
        // M2DebugLog.printf("Update: {0}", mPlayerID);
        
        if (! mInitialized) {
            return;
        }

        if (mCulled)
            return;

#if EMOTE_PLATFORM_PS4
        // adhoc patch for PSVR-HMD on/off unexpected camera matrix modification.
        updateTransform();
#endif // EMOTE_PLATFORM_PS4

        if (! isLegacyTransform
            && convolveObjectTransformToPhysics
            && ! stepUpdate) {
            Vector3 vec = mU2EMatrix.MultiplyPoint3x4(Vector3.zero) - mU2EMatrix.MultiplyPoint3x4(transform.InverseTransformPoint(mPrevPosition)); 
            if (globalTranslateLimitVelocity != 0.0f) {
                float e2u_scale = Mathf.Abs(mE2UMatrix.lossyScale.x);
                float limit = globalTranslateLimitVelocity / e2u_scale * deltaTime;
                vec = vec.normalized * ((1 - Mathf.Exp(-1.6f * (vec.magnitude / limit))) * limit);
            }
            vec *= (1.0f / 60) / deltaTime;
            if (vec != mPrevOuterVec) {
                SetOuterForce("bust", vec.x, vec.y);
                SetOuterForce("parts", vec.x, vec.y);
                SetOuterForce("hair", vec.x, vec.y);
                mPrevOuterVec = vec;
            }
            float rot = -transform.eulerAngles.z;
            if (rot != mPrevOuterRot) {
                SetOuterRot(rot);
                mPrevOuterRot = rot;
            }
        }
        mPrevPosition = transform.position;
        if (mapToRenderTexture) {
            foreach (RenderTextureMapper mapper in mRenderTextureMapper) 
                if (mapper.inited) {
                    mapper.materialProp.SetColor(sID_Color, mainColor);
                    var renderer = this.GetComponent<Renderer>();
                    if (isLegacyTransform
                        && renderer != null
                        && renderer.enabled) 
                        renderer.SetPropertyBlock(mapper.materialProp);
                    else
                        mapper.meshRenderer.SetPropertyBlock(mapper.materialProp);
                }
            if (! stereovisionEnabled) {
                if (mRenderTextureMapper[0].inited)
                    mRenderTextureMapper[0].quadObject.layer = gameObject.layer;
            } else {
                if (mRenderTextureMapper[0].inited)
                    mRenderTextureMapper[0].quadObject.layer = stereoVisionLeftEyeLayer;
                if (mRenderTextureMapper[1].inited)
                    mRenderTextureMapper[1].quadObject.layer = stereoVisionRightEyeLayer;
            }
        }
        
        if (stepUpdate
#if UNITY_EDITOR
            || ! EditorApplication.isPlaying
#endif
            ) {
            if (mIsModified) {
                Native_EmotePlayer_Step(mPlayerID);
                Native_EmotePlayer_Update(mPlayerID, 0);
                FetchCommandBuffer();
            }
        } else {
            Native_EmotePlayer_Update(mPlayerID, deltaTime * speed);
            FetchCommandBuffer();
        }
        mIsModified = false;
    }

    void FetchCommandBuffer() {
         ResetRenderEnvironments();
         ReleaseTemporaryTextures();
         
         if (! mStereovisionEnabled) {
             Native_EmotePlayer_SetStereovisionRenderScreen(mPlayerID, 0);
             Native_EmotePlayer_Draw(mPlayerID);
             // コマンドバッファのコピー.
             int len = Native_EmotePlayer_GetCommandBufferLength(mPlayerID)*4;
             // M2DebugLog.printf("DrawCore: Command Buffer len={0}", len);
             System.IntPtr ptr = Native_EmotePlayer_GetCommandBuffer(mPlayerID);
             // M2DebugLog.printf("DrawCore: Command Buffer ptr={0}", ptr);
             if (ptr != System.IntPtr.Zero) {
                mCmdBufActiveLength = len;
                if (len > mCmdBuf.Length) {
                    mCmdBuf = null;
                    mCmdBuf = new byte[len*2];
                    // M2DebugLog.printf("DrawCore: Stretch Command Buffer to {0}", mCmdBuf.Length);
                }
                Marshal.Copy(ptr, mCmdBuf, 0, len);
            }
        } else {
            for (int i = 0; i < 2; i++) {
                Native_EmotePlayer_SetStereovisionRenderScreen(mPlayerID, i);
                Native_EmotePlayer_Draw(mPlayerID);
                // コマンドバッファのコピー.
                int len = Native_EmotePlayer_GetCommandBufferLength(mPlayerID)*4;
                // M2DebugLog.printf("DrawCore: Command Buffer len={0}", len);
                System.IntPtr ptr = Native_EmotePlayer_GetCommandBuffer(mPlayerID);
                if (ptr == System.IntPtr.Zero)
                    continue;
                // M2DebugLog.printf("DrawCore: Command Buffer ptr={0}", ptr);
                mCmdBufActiveLengthList[i] = len;
                if (mCmdBufList[i] == null
                    || len > mCmdBufList[i].Length) {
                    mCmdBufList[i] = null;
                    mCmdBufList[i] = new byte[len*2];
                    // M2DebugLog.printf("DrawCore: Stretch Command Buffer to {0}", mCmdBuf.Length);
                }
                Marshal.Copy(ptr, mCmdBufList[i], 0, len);
            }
        }
    }

    [System.NonSerialized]
    public bool skipNextDrawCall = false;

    private void OnRenderObject() {
        // M2DebugLog.printf("OnRenderObject({0})", mPlayerID);
        if (skipNextDrawCall) 
            skipNextDrawCall = false;
        else
            DrawCore();
    }

    const int M_TEXTURE_ATTRIBUTE_TRANSLUCENT = 1 << 0;
    const int M_TEXTURE_ATTRIBUTE_OPAQUE = 1 << 1;
    const int M_TEXTURE_ATTRIBUTE_NO_DIFFUSE = 1 << 2;

    const int M_BLEND_MODE_ALPHABLEND = 0;
    const int M_BLEND_MODE_ADDITIVE = 1;
    const int M_BLEND_MODE_SUBTRACT = 2;
    const int M_BLEND_MODE_PSMULTIPLICATIVE = 3; ///< 乗算
    const int M_BLEND_MODE_PSSCREEN         = 4; ///< スクリーン
    const int M_BLEND_MODE_SUBTRACTIVE = 5;
    const int M_BLEND_MODE_PIXELATE = 6;
    const int M_BLEND_MODE_EMBED    = 7;
    const int M_BLEND_MODE_PMA      = 8;

    // ここから deprecated な旧名称の名前
    const int M_BLEND_MODE_PSSUBTRACT       = M_BLEND_MODE_SUBTRACTIVE;
    const int M_BLEND_MODE_ADD              = M_BLEND_MODE_ADDITIVE;
    const int M_BLEND_MODE_MULTIPLY         = M_BLEND_MODE_PSMULTIPLICATIVE;
    const int M_BLEND_MODE_SCREEN           = M_BLEND_MODE_PSSCREEN;
    // ここまで deprecated な旧名称の名前

    const int M_BLEND_MODE_PSADDITIVE       = 0x100;
    const int M_BLEND_MODE_PSSUBTRACTIVE    = 0x101;
    const int M_BLEND_MODE_PSOVERLAY        = 0x102;
    const int M_BLEND_MODE_PSHARDLIGHT      = 0x103;
    const int M_BLEND_MODE_PSSOFTLIGHT      = 0x104;
    const int M_BLEND_MODE_PSCOLORDODGE     = 0x105;
    const int M_BLEND_MODE_PSCOLORBURN      = 0x106;
    const int M_BLEND_MODE_PSLIGHTEN        = 0x107;
    const int M_BLEND_MODE_PSDARKEN         = 0x108;
    const int M_BLEND_MODE_PSDIFFERENCE     = 0x109;
    const int M_BLEND_MODE_PSEXCLUSION      = 0x10a;
    const int M_BLEND_MODE_FLTGRAYSCALE     = 0x10b;
    const int M_BLEND_MODE_FLTMOSAIC        = 0x10c;
    const int M_BLEND_MODE_FLTBLUR          = 0x10d;

    const int M_BLEND_MODE_BBM_RAWADD       = 0x1100;
    const int M_BLEND_MODE_BBM_RAWSUB       = 0x1101;
    const int M_BLEND_MODE_BBM_RAWMULTIPLY  = 0x1102;
    const int M_BLEND_MODE_BBM_RAWSCREEN    = 0x1103;
    
    const int M_BLEND_COLOR_SINGLE = 0 << 4;
    const int M_BLEND_COLOR_DOUBLE = 1 << 4;

    const int M_BLEND_COLOR_MASK        = 0x00f0;
    const int M_BLEND_ACTION_MASK       = 0x0f00;
    const int M_BLEND_INDEX_L_MASK      = 0x000f;
    const int M_BLEND_INDEX_H_MASK      = 0xf000;
    const int M_BLEND_MODE_MASK         = 0xff0f;
    
    const int M_BLEND_INDEX_L_SHIFT   = 0;
    const int M_BLEND_COLOR_SHIFT     = 4;
    const int M_BLEND_ACTION_SHIFT    = 8;
    const int M_BLEND_INDEX_H_SHIFT   = 12;

    const int M_BLEND_ACTION_NORMAL   = 0x0000;
    const int M_BLEND_ACTION_BUFFERED = 0x0100;

    const int M_BLUR_SHADER_COUNT = 5;

    static int 
    extract_blend_index(int mode)
    {
        int index_l = (mode & M_BLEND_INDEX_L_MASK) >> M_BLEND_INDEX_L_SHIFT;
        int index_h = (mode & M_BLEND_INDEX_H_MASK) >> M_BLEND_INDEX_H_SHIFT;
        return index_h << 4 | index_l;
    }

    static int 
    extract_blend_color(int  mode)
    {
        return mode & M_BLEND_COLOR_MASK;
    }

    static int 
    extract_blend_action(int mode)
    {
        return mode & M_BLEND_ACTION_MASK;
    }

    private int mSpriteId;
    private int mBlendMode;
    private MaskType mMaskType = MaskType.None;
    private int mMaskRefCount;
    private float mMaskAlphaTestRef = 64/255.0f;
    private Rect mClipRect;
    private bool mIsClipValid = false;
    private int mColorFilter;
    private int mFilterColor;
    private int mAlphaMaskLayerCount;
    private bool[] mAlphaMaskTextureAlloced = new bool[2];
    private int mCurrentAlphaMaskTextureIndex = 0;
    private int mRefAlphaIndex;
    private bool mIsFirstAlphaMaskBuild;
#if EMOTE_SUPPORT_BBM
    private float mBlendParam;
    private bool mIsBBM;
    private bool mIsBBMBufferValid;
    private Rect mBBMBufferRect;
    private bool mIsBBMBufferAlloced;
#endif //  EMOTE_SUPPORT_BBM
    private int mMeshDeformationMode;
    
    void CreateClearMaskMesh(ref Mesh mesh, ref Matrix4x4 mat, ref bool proj) {
        Vector3[] verts = mVector3ArrayCache.Require(4);
        if (mIsClipValid) {
            // M2DebugLog.printf("ClipRect({0}):", mClipRect);
            verts[0] = new Vector3(mClipRect.xMin, mClipRect.yMin, 0);
            verts[1] = new Vector3(mClipRect.xMax, mClipRect.yMin, 0);
            verts[2] = new Vector3(mClipRect.xMin, mClipRect.yMax, 0);
            verts[3] = new Vector3(mClipRect.xMax, mClipRect.yMax, 0);
            proj = true;
            mat = mE2WMatrix;

        } else if (mE_CharaBounds != new Rect(0, 0, 0, 0)
                   && ! isLegacyTransform) {
            // M2DebugLog.printf("CharaBounds({0}):", mE_CharaBounds);
            verts[0] = new Vector3(mE_CharaBounds.xMin, mE_CharaBounds.yMin, 0);
            verts[1] = new Vector3(mE_CharaBounds.xMax, mE_CharaBounds.yMin, 0);
            verts[2] = new Vector3(mE_CharaBounds.xMin, mE_CharaBounds.yMax, 0);
            verts[3] = new Vector3(mE_CharaBounds.xMax, mE_CharaBounds.yMax, 0);
            proj = true;
            mat = mE2WMatrix * Matrix4x4.TRS(new Vector3(mCoord.x, mCoord.y, 0), 
                                             Quaternion.Euler(0, 0, rot),
                                             new Vector3(mScale, mScale, mScale));
        } else {
            // M2DebugLog.printf("AllClear");
            verts[0] = new Vector3(-1, -1, 0);
            verts[1] = new Vector3(-1, 1, 0);
            verts[2] = new Vector3(1, -1, 0);
            verts[3] = new Vector3(1, 1, 0);
            proj = false;
            mat = Matrix4x4.identity;
        }        

        int[] indices = mIntArrayCache.Require(4);
        indices[0] = 0;
        indices[1] = 1;
        indices[2] = 3;
        indices[3] = 2;

        mesh = mMeshCache.Require();
        mesh.MarkDynamic();
        mesh.Clear();
        mesh.subMeshCount = 1;
        mesh.vertices = verts;
        mesh.SetIndices(indices, MeshTopology.Quads, 0);
   }

    void ClearStencilMask(CommandBuffer result, int clearValue) {
        Mesh mesh = null;
        Matrix4x4 mat = Matrix4x4.identity;
        bool proj = false;
        CreateClearMaskMesh(ref mesh, ref mat, ref proj);
        var material = RequireStencilClearMaterial(proj, clearValue, mMeshDeformationMode);
        result.DrawMesh(mesh, mat, material);
    }

    void ClearAlphaMask(CommandBuffer result, float clearValue) {
        Mesh mesh = null;
        Matrix4x4 mat = Matrix4x4.identity;
        bool proj = false;
        CreateClearMaskMesh(ref mesh, ref mat, ref proj);
        var material = RequireAlphaClearMaterial(proj, clearValue, mMeshDeformationMode);
        result.DrawMesh(mesh, mat, material);
    }

#if EMOTE_SUPPORT_BBM
    void PrepareBufferedBlend(CommandBuffer result, float l, float b, float r, float t) {
        // M2DebugLog.printf("PrepareBufferedBlend({0},{1},{2},{3})", l, b, r, t);
        if (! mapToRenderTexture) {
            mIsBBMBufferValid = false;
            return;
        }
        
        switch (mBlendMode & M_BLEND_MODE_MASK) {
        case M_BLEND_MODE_FLTMOSAIC: {
            float cw, ch;
            if (renderTextureResolutionCorrection) {
                float p;
                if (mBlendParam < 0) {
                    p = Mathf.Max((-mBlendParam / mPixelScale.x), (-mBlendParam / mPixelScale.y));
                } else {
                    p = Mathf.Max((texWidth / Mathf.Max(1.0f, mBlendParam)) / mPixelScale.x,
                                  (texHeight / Mathf.Max(1.0f, mBlendParam)) / mPixelScale.y);
                }
                cw = Mathf.Ceil(p * mPixelScale.x);
                ch = Mathf.Ceil(p * mPixelScale.y);
            } else {
                cw = ch = mBlendParam < 0 ? -mBlendParam : Mathf.Ceil(Mathf.Max(texWidth, texHeight) / Mathf.Max(1.0f, mBlendParam));
            }
            // セルサイズを基準にバウンズをアライメント
            l = Mathf.Floor(l / cw) * cw;
            b = Mathf.Floor(b / ch) * ch;
            r = Mathf.Ceil(r / cw) * cw;
            t = Mathf.Ceil(t / ch) * ch;
            break;
        }
        case M_BLEND_MODE_FLTBLUR: {
            // ブラーは5段階。0〜4は横、5〜9は縦ブラーをかける。
            // 処理はバッファのピクセル基準。
            float[] expand = { 1, 2, 2, 4, 6 };
            float cw = expand[(int)mBlendParam % M_BLUR_SHADER_COUNT];
            if ((int)mBlendParam < M_BLUR_SHADER_COUNT) {
                l -= cw;
                r += cw;
            } else {
                b -= cw;
                t += cw;
            }
            break;
        }
        }

        l = Mathf.Max(0, l);
        b = Mathf.Max(0, b);
        r = Mathf.Min(texWidth, r);
        t = Mathf.Min(texHeight, t);

        mIsBBMBufferValid = (l < r && b < t);
        if (! mIsBBMBufferValid)
            return;

        TouchBlendBuffer(result, l, b, r, t);
        CaptureBlendBuffer(result);
    }

    void TouchBlendBuffer(CommandBuffer result, float l, float b, float r, float t) {
        var newRect = Rect.MinMaxRect(l, b, r, t);
        if (! mIsBBMBufferAlloced
            || (mBBMBufferRect.width != newRect.width
                || mBBMBufferRect.height != newRect.height)) {
            if (! mIsBBMBufferAlloced) {
                mIsBBMBufferAlloced = true;
            } else {
                result.ReleaseTemporaryRT(sID_BBMBufferTex);
            } 
#if UNITY_2017_1_OR_NEWER
            RenderTextureDescriptor bufDesc = new RenderTextureDescriptor((int)newRect.width,
                                                                          (int)newRect.height,
                                                                          RenderTextureFormat.ARGB32,
                                                                          0 );
            result.GetTemporaryRT(sID_BBMBufferTex, bufDesc);
#else //  UNITY_2017_1_OR_NEWER
            result.GetTemporaryRT(sID_BBMBufferTex, 
                                  (int)newRect.width,
                                  (int)newRect.height,
                                  0);
#endif //  UNITY_2017_1_OR_NEWER
        }
        mBBMBufferRect = newRect;

        switch (mBlendMode & M_BLEND_MODE_MASK) {
        case M_BLEND_MODE_FLTMOSAIC: {
            float cw, ch;
            if (renderTextureResolutionCorrection) {
                float p;
                if (mBlendParam < 0) {
                    p = Mathf.Max((-mBlendParam / mPixelScale.x), (-mBlendParam / mPixelScale.y));
                } else {
                    p = Mathf.Max((texWidth / Mathf.Max(1.0f, mBlendParam)) / mPixelScale.x,
                                  (texHeight / Mathf.Max(1.0f, mBlendParam)) / mPixelScale.y);
                }
                cw = Mathf.Ceil(p * mPixelScale.x);
                ch = Mathf.Ceil(p * mPixelScale.y);
            } else {
                cw = ch = mBlendParam < 0 ? -mBlendParam : Mathf.Ceil(Mathf.Max(texWidth, texHeight) / Mathf.Max(1.0f, mBlendParam));
            }
            result.SetGlobalVector(sID_MosaicParam,
                                   new Vector4(mBBMBufferRect.width / cw, 
                                               mBBMBufferRect.height / ch, 
                                               (cw / 2 + 0.5f) / mBBMBufferRect.width, 
                                               (ch / 2 + 0.5f) / mBBMBufferRect.height));
            break;
        }
        case M_BLEND_MODE_FLTBLUR: {
            var vec = Vector4.zero;
            if ((int)mBlendParam < M_BLUR_SHADER_COUNT)
                vec.x = 1.0f / mBBMBufferRect.width;
            else
                vec.y = 1.0f / mBBMBufferRect.height;
            result.SetGlobalVector(sID_BlurParam, vec);
            break;
        }
        }
    }

    void CaptureBlendBuffer(CommandBuffer result) {
#if UNITY_2017_1_OR_NEWER
        result.Blit(BuiltinRenderTextureType.CurrentActive, 
                    sID_BBMBufferTex,
                    new Vector2(mBBMBufferRect.width / texWidth, mBBMBufferRect.height / texHeight),
                    new Vector2(mBBMBufferRect.xMin / texWidth, mBBMBufferRect.yMin / texHeight));
#else // UNITY_2017_1_OR_NEWER
        result.Blit(BuiltinRenderTextureType.CurrentActive, sID_BBMBufferTex);
#endif // UNITY_2017_1_OR_NEWER
        result.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);
        result.SetGlobalVector(sID_BBMBufferOffset, 
                               new Vector4(-mBBMBufferRect.xMin / texWidth,
                                           -mBBMBufferRect.yMin / texHeight,
                                           texWidth / mBBMBufferRect.width,
                                           texHeight / mBBMBufferRect.height));
    }
#endif //  EMOTE_SUPPORT_BBM

    private enum DrawTarget : int {
        NONE,
        SINGLE,
        LEFTEYE,
        RIGHTEYE,
    };

    //****************************************
    // adhoc dummy function
    //****************************************
    static void Dummy() {
    }

    public void DrawCore()
    {
        //****************************************
        // Don't delete this dummy function call
        // this is adhoc countermeasure for 
        // Unity5.5/WebGL code optimization bug.
        //****************************************
        Dummy();

        if (! mInitialized)
            return;

        if (culled)
            return;

        Camera camera = Camera.current;
        DrawTarget drawTarget = DrawTarget.NONE;

        if (mapToRenderTexture) {
            if (camera.cullingMask == INNER_RENDER_CULLING_MASK) {
                if (! mStereovisionEnabled) {
                    if (camera == mRenderTextureMapper[0].camera)
                        drawTarget = DrawTarget.SINGLE;
                } else {
                    if (camera == mRenderTextureMapper[0].camera)
                        drawTarget = DrawTarget.LEFTEYE;
                    else if (camera == mRenderTextureMapper[1].camera)
                        drawTarget = DrawTarget.RIGHTEYE;
                }
            }
        } else {
#if UNITY_EDITOR
            if (! Camera.allCameras.Contains(camera)
                && ! (UnityEditor.SceneView.currentDrawingSceneView != null
                      && UnityEditor.SceneView.currentDrawingSceneView.camera == camera)) {
                return;
            }
#endif // UNITY_EDITOR

            if (! mStereovisionEnabled) {
                if ((camera.cullingMask & (1 << gameObject.layer)) != 0) 
                    drawTarget = DrawTarget.SINGLE;
            } else {
                if ((camera.cullingMask & (1 << stereoVisionLeftEyeLayer)) != 0
#if EMOTE_SUPPORT_SINGLECAMERA_STEREOVISION
                    && (camera.stereoActiveEye == Camera.MonoOrStereoscopicEye.Mono || camera.stereoActiveEye == Camera.MonoOrStereoscopicEye.Left)
#endif //  EMOTE_SUPPORT_SINGLECAMERA_STEREOVISION
                   ) 
                    drawTarget = DrawTarget.LEFTEYE;
                else if ((camera.cullingMask & (1 << stereoVisionRightEyeLayer)) != 0
#if EMOTE_SUPPORT_SINGLECAMERA_STEREOVISION
                         && (camera.stereoActiveEye == Camera.MonoOrStereoscopicEye.Mono || camera.stereoActiveEye == Camera.MonoOrStereoscopicEye.Right)
#endif //  EMOTE_SUPPORT_SINGLECAMERA_STEREOVISION
                        ) 
                    drawTarget = DrawTarget.RIGHTEYE;
            }
        }

        byte[] cmdBuf;
        int cmdBufLength;
        switch (drawTarget) {
        default:
        case DrawTarget.NONE: return;
        case DrawTarget.SINGLE: cmdBuf = mCmdBuf; cmdBufLength = mCmdBufActiveLength; break;
        case DrawTarget.LEFTEYE: cmdBuf = mCmdBufList[0]; cmdBufLength = mCmdBufActiveLengthList[0]; break;
        case DrawTarget.RIGHTEYE: cmdBuf = mCmdBufList[1]; cmdBufLength = mCmdBufActiveLengthList[1]; break;
        }

        int renderCommandBufferIndex = (int)drawTarget - 1;
        if (mRenderCommandBufferList[renderCommandBufferIndex] == null)
            mRenderCommandBufferList[renderCommandBufferIndex] = CreateRenderCommandBuffer(cmdBuf, cmdBufLength);

        Graphics.ExecuteCommandBuffer(mRenderCommandBufferList[renderCommandBufferIndex]);
    }

    private int effectiveMaskMode {
        get {
#if ! UNITY_5_6_OR_NEWER
          return (int)MaskMode.Stencil;
#else
          return EmotePlayer.maskMode;
#endif        
        }
    }

    private void BeginCreateStencilMask(CommandBuffer cb, int initialValue) {
        ClearStencilMask(cb, initialValue);
    }
    
    private void PrepareInnerStencilMask(CommandBuffer cb, int refCount) {
        mMaskType = MaskType.Inner;
        mMaskRefCount = refCount;
    }
    
    private void PrepareOuterStencilMask(CommandBuffer cb, int refCount) {
        mMaskType = MaskType.Outer;
        mMaskRefCount = refCount;
    }
    
    private void EndCreateStencilMask(CommandBuffer cb, int refCount) {
        mMaskType = MaskType.Apply;
        mMaskRefCount = refCount;
    }
    
    private void ResetStencilMask(CommandBuffer cb) {
        mMaskType = MaskType.None;
    }
    

    private void AllocAlphaMaskTexture(CommandBuffer cb, int texIndex) {
        if (! mAlphaMaskTextureAlloced[texIndex]) {
            mAlphaMaskTextureAlloced[texIndex] = true;
#if UNITY_2017_1_OR_NEWER
            RenderTextureDescriptor bufDesc = new RenderTextureDescriptor(-1,
                                                                          -1,
                                                                          RenderTextureFormat.R8,
                                                                          0 );
            cb.GetTemporaryRT(sID_AlphaMaskTex[texIndex], bufDesc);
#else //  UNITY_2017_1_OR_NEWER
            cb.GetTemporaryRT(sID_AlphaMaskTex[texIndex], 
                              -1,
                              -1,
                              0,
                              FilterMode.Bilinear,
                              RenderTextureFormat.R8);
#endif //  UNITY_2017_1_OR_NEWER
        }
        cb.SetRenderTarget(sID_AlphaMaskTex[texIndex]);
    }            
    
    private void BeginCreateAlphaMask(CommandBuffer cb) {
        mCurrentAlphaMaskTextureIndex = 0;
        mIsFirstAlphaMaskBuild = true;
        mRefAlphaIndex = 0;
    }
    
    private void PrepareInnerAlphaMask(CommandBuffer cb) {
        if (mIsFirstAlphaMaskBuild) {
            mIsFirstAlphaMaskBuild = false;
            AllocAlphaMaskTexture(cb, 0);
            ClearAlphaMask(cb, 0);
            mRefAlphaIndex = 0;
        } else {
            mRefAlphaIndex = mCurrentAlphaMaskTextureIndex + 1;
            mCurrentAlphaMaskTextureIndex = (mCurrentAlphaMaskTextureIndex + 1) % 2;
            AllocAlphaMaskTexture(cb, mCurrentAlphaMaskTextureIndex);
            ClearAlphaMask(cb, 0);
        }
        mMaskType = MaskType.Inner;
    }
    
    private void PrepareOuterAlphaMask(CommandBuffer cb) {
        if (mIsFirstAlphaMaskBuild) {
            mIsFirstAlphaMaskBuild = false;
            AllocAlphaMaskTexture(cb, 0);
            ClearAlphaMask(cb, 1.0f);
        } 
        mMaskType = MaskType.Outer;
        mRefAlphaIndex = 0;
    }
    
    private void EndCreateAlphaMask(CommandBuffer cb) {
        mMaskType = MaskType.None;
        mRefAlphaIndex = mCurrentAlphaMaskTextureIndex + 1;
        cb.SetRenderTarget(BuiltinRenderTextureType.CameraTarget);
    }
    
    private void ResetAlphaMask(CommandBuffer cb) {
        mMaskType = MaskType.None;
        mRefAlphaIndex = 0;
    }
    
    private void BeginCreateMask(CommandBuffer cb, int initialValue) {
        switch (effectiveMaskMode) {
        case (int)MaskMode.Stencil: BeginCreateStencilMask(cb, initialValue); break;
        case (int)MaskMode.Alpha:   BeginCreateAlphaMask(cb); break;
        }
    }
    
    private void PrepareInnerMask(CommandBuffer cb, int refCount) {
        switch (effectiveMaskMode) {
        case (int)MaskMode.Stencil: PrepareInnerStencilMask(cb, refCount); break;
        case (int)MaskMode.Alpha:   PrepareInnerAlphaMask(cb); break;
        }
    }
    
    private void PrepareOuterMask(CommandBuffer cb, int refCount) {
        switch (effectiveMaskMode) {
        case (int)MaskMode.Stencil: PrepareOuterStencilMask(cb, refCount); break;
        case (int)MaskMode.Alpha:   PrepareOuterAlphaMask(cb); break;
        }
    }
    
    private void EndCreateMask(CommandBuffer cb, int refCount) {
        switch (effectiveMaskMode) {
            case (int)MaskMode.Stencil: EndCreateStencilMask(cb, refCount); break;
        case (int)MaskMode.Alpha:   EndCreateAlphaMask(cb); break;
        }
    }
    
    private void ResetMask(CommandBuffer cb) {
        switch (effectiveMaskMode) {
        case (int)MaskMode.Stencil: ResetStencilMask(cb); break;
        case (int)MaskMode.Alpha:   ResetAlphaMask(cb); break;
        }
    }

    private void SetMeshDeformerArray(CommandBuffer cb, int w, int h, System.IntPtr image, int imageSize) {
        w /= 4;
        var tex = new Texture2D(w, h, TextureFormat.RGBAFloat, false);
        tex.filterMode = FilterMode.Point;
        tex.wrapMode = TextureWrapMode.Repeat;
        tex.LoadRawTextureData(image, imageSize);
        tex.Apply(false, true);
        cb.SetGlobalTexture(sID_MeshDeformerParamsTex, tex);
        cb.SetGlobalVector(sID_MeshDeformerParamsSampleArg, new Vector4(1.0f / w, 1.0f / w / h, 0.0f, 0.0f));
        MarkAsTemporaryTexture(tex);
    }

    private void SetMeshDeformerOffset(CommandBuffer cb, Vector2 offset) {
        cb.SetGlobalVector(sID_MeshDeformerOffset, offset);
    }

    private void SetMeshDeformerChain(CommandBuffer cb, float[] indexArray, int arraySize) {
        cb.SetGlobalFloatArray(sID_MeshDeformerIndexArray, indexArray);
        cb.SetGlobalFloat(sID_MeshDeformerIndexArraySize, arraySize);
        mMeshDeformationMode = 1;
    }

    private void ResetMeshDeformerChain(CommandBuffer cb) {
        mMeshDeformationMode = 0;
    }
    
    CommandBuffer CreateRenderCommandBuffer(byte[] cmdBuf, int cmdBufLength) {
        // M2DebugLog.printf("DrawCore({0}): begin", mPlayerID);

        CommandBuffer result = mCommandBufferCache.Require();
        result.name = "E-mote Render Command Buffer";
        result.SetGlobalFloat(sID_AlphaCutoff, EmotePlayer.alphaCutoff / 256.0f);
        SetColorFilter(result);

        mIsClipValid = false;
        mMaskType = MaskType.None;
        mMaskRefCount = 0;
        mMeshDeformationMode = 0;
        int idx = 0;
        if (mapToRenderTexture) 
            mE2WMatrix = mE2UMatrix;
        else
            mE2WMatrix = transform.localToWorldMatrix * mE2UMatrix;

        mAlphaMaskTextureAlloced[0] = false;
        mAlphaMaskTextureAlloced[1] = false;
        mRefAlphaIndex = 0;

#if EMOTE_SUPPORT_BBM
        mIsBBMBufferAlloced = false;
#endif //  EMOTE_SUPPORT_BBM
        
        while (cmdBufLength > idx) {
            System.UInt32 cmd = System.BitConverter.ToUInt32(cmdBuf, idx); idx += 4;
            System.UInt32 cmdBlockSize = System.BitConverter.ToUInt32(cmdBuf, idx); idx += 4;
            // M2DebugLog.printf("DrawCore({0}): len={3}, idx={1, 5}: cmd={2}", mPlayerID, idx-4, cmd, cmdBufLength);
            switch (cmd) {
            default:
                {
                    // M2DebugLog.printf("DrawCore({0}): *** Unknown Command: {1}, Size: {2} at Index: {3}", mPlayerID, cmd, cmdBlockSize, idx-8);
                    idx += (int)cmdBlockSize;
                    break;
                }
            case  1:  // M_MOTION_COMMAND_ID_BEGIN_RENDER
                {
                    #pragma warning disable 0219
                        System.UInt32 ver = System.BitConverter.ToUInt32(cmdBuf, idx); idx += 4;
                    #pragma warning restore 0219
                        // M2DebugLog.printf("DrawCore({0}): M_MOTION_COMMAND_ID_BEGIN_RENDER: ver={1}", mPlayerID, ver);
                        break;
                }
            case  2:  // M_MOTION_COMMAND_ID_END_RENDER
                {
                    // M2DebugLog.printf("DrawCore({0}): M_MOTION_COMMAND_ID_END_RENDER", mPlayerID);
                    break;
                }
            case  3:  // M_MOTION_COMMAND_ID_SET_BLEND_MODE
                {
                    System.UInt32 mode = System.BitConverter.ToUInt32(cmdBuf, idx); idx += 4;
                    mBlendMode = (int)mode;
#if EMOTE_SUPPORT_BBM
                    // pre multiply alpha shader replacement
                    if (pmaEnabled) {
                        int index = mBlendMode & M_BLEND_MODE_MASK;
                        bool modified = true;
                        switch (index) {
                            default: modified = false; break;
                            case M_BLEND_MODE_ALPHABLEND: index = M_BLEND_MODE_PMA; break;
                            case M_BLEND_MODE_ADD:        index = M_BLEND_MODE_BBM_RAWADD; break;
                            case M_BLEND_MODE_PSSUBTRACT: index = M_BLEND_MODE_BBM_RAWSUB; break;
                            case M_BLEND_MODE_MULTIPLY:   index = M_BLEND_MODE_BBM_RAWMULTIPLY; break;
                            case M_BLEND_MODE_SCREEN:     index = M_BLEND_MODE_BBM_RAWSCREEN; break;
                        }
                        if (modified)
                            mBlendMode = mBlendMode & (~M_BLEND_MODE_MASK) | index;
                    }
                    mIsBBM = (extract_blend_action(mBlendMode) == M_BLEND_ACTION_BUFFERED);
                    // M2DebugLog.printf("DrawCore({0}): M_MOTION_COMMAND_ID_SET_BLEND_MODE: blendmode={1:x}, alterBlendMode={2:x}, isBBM={3}", mPlayerID, mode, mBlendMode, mIsBBM);
#endif //  EMOTE_SUPPORT_BBM
                    break;
                }

#if EMOTE_SUPPORT_BBM
            case  15:  // M_MOTION_COMMAND_ID_SET_BLEND_PARAM
                {
                    float[] points = mFloat4Buffer;
                    GCHandle h = GCHandle.Alloc(points, GCHandleType.Pinned);
                    Marshal.Copy(cmdBuf, idx, h.AddrOfPinnedObject(), (int)4); idx += 4;
                    h.Free();
                    mBlendParam =  points[0];
                    // M2DebugLog.printf("DrawCore({0}): M_MOTION_COMMAND_ID_SET_BLEND_PARAM: blendparam={1}", mPlayerID, mBlendParam);
                    break;
                }
#endif // EMOTE_SUPPORT_BBM

            case  4:  // M_MOTION_COMMAND_ID_LOAD_TEX
                {
                    System.UInt32 id = System.BitConverter.ToUInt32(cmdBuf, idx); idx += 4;
                    mSpriteId = (int)id;
                    result.SetGlobalTexture(sID_MainTex, mTexture[mSpriteId].texture);
                    // M2DebugLog.printf("DrawCore({0}): M_MOTION_COMMAND_ID_LOAD_TEX: id={1}", mPlayerID, id);
                    break;
                }
            case  5:  // M_MOTION_COMMAND_ID_BEGIN_CREATE_MASK
                {
                    System.UInt32 value = System.BitConverter.ToUInt32(cmdBuf, idx); idx += 4;
                    // M2DebugLog.printf("DrawCore({0}): M_MOTION_COMMAND_ID_BEGIN_CREATE_MASK: value={1}", mPlayerID, value);
                    BeginCreateMask(result, (int)value);
                    break;
                }
            case  6:  // M_MOTION_COMMAND_ID_PREPARE_INNER_MASK
                {
                    System.UInt32 refCount = System.BitConverter.ToUInt32(cmdBuf, idx); idx += 4;
                    PrepareInnerMask(result, (int)refCount);
                    // M2DebugLog.printf("DrawCore({0}): M_MOTION_COMMAND_ID_PREPARE_INNER_MASK: ref={1}", mPlayerID, refCount);
                    break;

                }
            case  7:  // M_MOTION_COMMAND_ID_PREPARE_OUTER_MASK
                {
                    System.UInt32 refCount = System.BitConverter.ToUInt32(cmdBuf, idx); idx += 4;
                    PrepareOuterMask(result, (int)refCount);
                    // M2DebugLog.printf("DrawCore({0}): M_MOTION_COMMAND_ID_PREPARE_OUTER_MASK: ref={1}", mPlayerID, refCount);
                    break;
                }
            case  8:  // M_MOTION_COMMAND_ID_END_CREATE_MASK
                {
                    System.UInt32 refCount = System.BitConverter.ToUInt32(cmdBuf, idx); idx += 4;
                    EndCreateMask(result, (int)refCount);
                    // M2DebugLog.printf("DrawCore({0}): M_MOTION_COMMAND_ID_END_CREATE_MASK: ref={1}", mPlayerID, refCount);
                    break;
                }
            case  9:  // M_MOTION_COMMAND_ID_CLEAR_MASK
                {
                    // M2DebugLog.printf("DrawCore({0}): M_MOTION_COMMAND_ID_CLEAR_MASK", mPlayerID);
                    ResetMask(result);
                    break;
                }
            case 11: // M_MOTION_COMMAND_ID_SET_CLIP
                {
                    float[] points = mFloat4Buffer;
                    GCHandle h = GCHandle.Alloc(points, GCHandleType.Pinned);
                    Marshal.Copy(cmdBuf, idx, h.AddrOfPinnedObject(), (int)4*4); idx += 4*4;
                    h.Free();
                    mIsClipValid = true;
                    float width = points[2] - points[0];
                    float height = points[3] - points[1];
                    mClipRect.Set(points[0] - width * 0.1f, 
                                  points[1] - height * 0.1f,
                                  width * 1.2f,
                                  height * 1.2f);
                    break;
                }
            case 12: // M_MOTION_COMMAND_ID_RESET_CLIP
                {
                    mIsClipValid = false;
                    break;
                }

            case 13: // MOTION_COMMAND_ID_SET_COLOR_FILTER: 
                {
                    System.UInt32 filter = System.BitConverter.ToUInt32(cmdBuf, idx); idx += 4;
                    System.UInt32 color = System.BitConverter.ToUInt32(cmdBuf, idx); idx += 4;
                    // M2DebugLog.printf("DrawCore({0}): MOTION_COMMAND_ID_SET_COLOR_FILTER:: filter={1}, color={2}", mPlayerID, filter, color);
                    mColorFilter = (int)filter;
                    mFilterColor = (int)color;
                    SetColorFilter(result);
                    break;
                }

#if EMOTE_SUPPORT_BBM
            case 14:  // M_MOTION_COMMAND_ID_PREPARE_BUFFERED_BLEND_RECT
                {
#if UNITY_2017_1_OR_NEWER
                    if (! mapToRenderTexture) {
                        idx += 4*4;
                        PrepareBufferedBlend(result, 0, 0, 0, 0);
                        break;
                    }
                    float[] points = mFloat4Buffer;
                    GCHandle h = GCHandle.Alloc(points, GCHandleType.Pinned);
                    Marshal.Copy(cmdBuf, idx, h.AddrOfPinnedObject(), (int)4*4); idx += 4*4;
                    h.Free();
                    if (isLegacyTransform) {
                        var l = Mathf.Floor(points[0] + texWidth / 2);
                        var t = Mathf.Ceil(-points[1] + texHeight / 2);
                        var r = Mathf.Ceil(points[2] + texWidth / 2);
                        var b = Mathf.Floor(-points[3] + texHeight / 2);

                        PrepareBufferedBlend(result, l, b, r, t);

                    } else {
                        var lt = mE2UMatrix.MultiplyPoint3x4(new Vector2(points[0], points[1]));
                        var rb = mE2UMatrix.MultiplyPoint3x4(new Vector2(points[2], points[3]));
                        var l = Mathf.Floor((lt.x - mU_RenderTextureBounds.x) / mU_RenderTextureBounds.width * texWidth);
                        var t = Mathf.Ceil((lt.y - mU_RenderTextureBounds.y) / mU_RenderTextureBounds.height * texHeight);
                        var r = Mathf.Ceil((rb.x - mU_RenderTextureBounds.x) / mU_RenderTextureBounds.width * texWidth);
                        var b = Mathf.Floor((rb.y - mU_RenderTextureBounds.y) / mU_RenderTextureBounds.height * texHeight);
                        PrepareBufferedBlend(result, l, b, r, t);
                    }
#else // UNITY_2017_1_OR_NEWER
                    idx += 4*4;
                    PrepareBufferedBlend(result, 0, 0, texWidth, texHeight);
#endif // UNITY_2017_1_OR_NEWER

                    break;
                }
#endif // EMOTE_SUPPORT_BBM

                case 16: //   M_MOTION_COMMAND_ID_SET_MESH_DEFORMER_ARRAY,
                    {
                        int w = System.BitConverter.ToInt32(cmdBuf, idx); idx += 4;
                        int h = System.BitConverter.ToInt32(cmdBuf, idx); idx += 4;
                        int typeSize = sizeof(float);
                        int imageSize = (int)(w * h) * typeSize;
                        var gch = GCHandle.Alloc(cmdBuf, GCHandleType.Pinned);
                        System.IntPtr imagePtr = new System.IntPtr(gch.AddrOfPinnedObject().ToInt64() + idx);
                        SetMeshDeformerArray(result, w, h, imagePtr, imageSize);
                        gch.Free();
                        idx += imageSize;
//                        M2DebugLog.printf("DrawCore({0}): M_MOTION_COMMAND_ID_SET_MESH_DEFORMER_ARRAY: w={1}, h={2}", mPlayerID, w, h);
                        break;
                    }
                case 17: // M_MOTION_COMMAND_ID_RESET_MESH_DEFORMER_CHAIN,
                    {
                        ResetMeshDeformerChain(result);
                        break;
                    }
                case 18: // M_MOTION_COMMAND_ID_SET_MESH_DEFORMER_CHAIN,
                    {
                        int  meshDeformerIndexCount = System.BitConverter.ToInt32(cmdBuf, idx); idx += 4;
                        for (int i = 0; i < meshDeformerIndexCount; i++) {
                            mFloat16Buffer[i] = (float)System.BitConverter.ToUInt32(cmdBuf, idx); idx += 4;
                        }
                        SetMeshDeformerChain(result, mFloat16Buffer, meshDeformerIndexCount);
//                        M2DebugLog.printf("DrawCore({0}): M_MOTION_COMMAND_ID_SET_MESH_DEFORMER_CHAIN: meshDeformerIndexCount={1}", mPlayerID, meshDeformerIndexCount);
                        break;
                    }
                case 19: // M_MOTION_COMMAND_ID_SET_MESH_DEFORMER_OFFSET,
                    {
                        float[] points = mFloat4Buffer;
                        GCHandle h = GCHandle.Alloc(points, GCHandleType.Pinned);
                        Marshal.Copy(cmdBuf, idx, h.AddrOfPinnedObject(), (int)4*2); idx += 4*2;
                        h.Free();
                        SetMeshDeformerOffset(result, new Vector2(points[0], points[1]));
                        break;
                    }
                
            case 10:  // M_MOTION_COMMAND_ID_RENDER_MESH
                {
                    System.UInt32 prim = System.BitConverter.ToUInt32(cmdBuf, idx); idx += 4;
                    System.UInt32 attr = System.BitConverter.ToUInt32(cmdBuf, idx); idx += 4;
                    System.UInt32 vertex = System.BitConverter.ToUInt32(cmdBuf, idx); idx += 4;
                    System.UInt32 index = System.BitConverter.ToUInt32(cmdBuf, idx); idx += 4;
                    // M2DebugLog.printf("DrawCore({0}): M_MOTION_COMMAND_ID_RENDER_MESH: {6}: loop={7}: prim={1}, z={2}, attr={3}, vertex={4}, index={5}", mPlayerID, prim, z, attr, vertex, index, mPartsCount, mLoop);
                    Vector3[] drawVert;
                    Vector2[] drawUvs;
                    Color32[] drawColor;
                    int[] drawIndices;

                    {
                        drawVert = mVector3ArrayCache.Require((int)vertex);
                        GCHandle h = GCHandle.Alloc(drawVert, GCHandleType.Pinned);
                        Marshal.Copy(cmdBuf, idx, h.AddrOfPinnedObject(), (int)vertex*4*3); idx += (int)vertex*4*3;
                        h.Free();
                    }
                    {
                        drawUvs = mVector2ArrayCache.Require((int)vertex);
                        GCHandle h = GCHandle.Alloc(drawUvs, GCHandleType.Pinned);
                        Marshal.Copy(cmdBuf, idx, h.AddrOfPinnedObject(), (int)vertex*4*2); idx += (int)vertex*4*2;
                        h.Free();
                    }
                    {
                        drawColor = null;
                        if ((attr & M_TEXTURE_ATTRIBUTE_NO_DIFFUSE) == 0) {
                            drawColor = mColor32ArrayCache.Require((int)vertex);
                            GCHandle h = GCHandle.Alloc(drawColor, GCHandleType.Pinned);
                            Marshal.Copy(cmdBuf, idx, h.AddrOfPinnedObject(), (int)vertex*4); idx += (int)vertex*4;
                            h.Free();
                            // M2DebugLog.printf("DrawCore: color={0:X} {1:X} {2:X} {3:X} ", drawColor[0], drawColor[1], drawColor[2], drawColor[3]);
                        }
                    }
                    {
                        drawIndices = mIntArrayCache.Require((int)index);
                        GCHandle h = GCHandle.Alloc(drawIndices, GCHandleType.Pinned);
                        Marshal.Copy(cmdBuf, idx, h.AddrOfPinnedObject(), (int)index*4); idx += (int)index*4;
                        h.Free();
                    }

                    if ((mBlendMode & M_BLEND_MODE_MASK) == M_BLEND_MODE_EMBED
                        && ! mapToRenderTexture)
                        continue;
#if EMOTE_SUPPORT_BBM
                    if (mIsBBM
                        && ! mIsBBMBufferValid)
                        continue;
#else //  EMOTE_SUPPORT_BBM
                    if (extract_blend_action(mBlendMode) == M_BLEND_ACTION_BUFFERED)
                        continue;
#endif //  EMOTE_SUPPORT_BBM

                    switch (prim) {
                    case 1: // M_MOTION_COMMAND_PRIMITIVE_TRISTRIP
                        {
                            var newDrawIndices = mIntArrayCache.Require((int)((index - 2) * 3));
                            for (int i = 0; i < index - 2; i++) {
                                System.Array.Copy(drawIndices, i, newDrawIndices, i * 3, 3);
                            }
                            drawIndices = newDrawIndices;
                            break;
                        }
                    }

                    Mesh mesh = mMeshCache.Require();
                    mesh.MarkDynamic();
                    mesh.Clear();
                    mesh.subMeshCount = 1;
                    mesh.vertices = drawVert;
                    mesh.uv = drawUvs;
                    mesh.colors32 = drawColor;
                    
                    // M2DebugLog.printf("DrawCore: material={0}, shader={1}, texture={2}", mMaterial, sShader, mTexture[mSpriteId]);
                    switch (prim) {
                    default:
                    case 2:  // M_MOTION_COMMAND_PRIMITIVE_QUADLIST
                        mesh.SetIndices(drawIndices, MeshTopology.Quads, 0);
                        break;
                    case 1:  // M_MOTION_COMMAND_PRIMITIVE_TRISTRIP
                    case 3:  // M_MOTION_COMMAND_PRIMITIVE_TRIANGLELIST
                        mesh.SetIndices(drawIndices, MeshTopology.Triangles, 0);
                        break;
                    }

                    int modulate = ((mBlendMode & M_BLEND_COLOR_DOUBLE) != 0) ? 2 : 1;
                    if ((attr & M_TEXTURE_ATTRIBUTE_NO_DIFFUSE) != 0) modulate = 0;
                    ShaderIndex shaderIndex;
#if EMOTE_SUPPORT_BBM
                    BBMBlendIndex bbmBlendIndex = BBMBlendIndex.Invalid;
#endif //  EMOTE_SUPPORT_BBM
                    if (mMaskType == MaskType.None) {
#if EMOTE_SUPPORT_BBM
                        if (mIsBBM) {
                            shaderIndex = ShaderIndex.BBM;
                        } 
                        else 
#endif //  EMOTE_SUPPORT_BBM
                        {
                            switch (mBlendMode & M_BLEND_MODE_MASK) {
                            default:
                            case M_BLEND_MODE_ALPHABLEND: shaderIndex = ShaderIndex.Alpha;      break;
                            case M_BLEND_MODE_PMA:        shaderIndex = ShaderIndex.PMA;      break;
                            case M_BLEND_MODE_ADD:        shaderIndex = ShaderIndex.Add;        break;
                            case M_BLEND_MODE_PSSUBTRACT: shaderIndex = ShaderIndex.PSSubtract; break;
                            case M_BLEND_MODE_MULTIPLY:   shaderIndex = ShaderIndex.Multiply;   break;
                            case M_BLEND_MODE_SCREEN:     shaderIndex = ShaderIndex.Screen;     break;
                            case M_BLEND_MODE_EMBED:      shaderIndex = ShaderIndex.Embed;      break;
                            }
                        }
                    } else {
                        // M2DebugLog.printf("DrawCore: mMaskValue={0}, mBlendMode={1}, mMaskType={2}", mMaskValue, mBlendMode, mMaskType);
#if EMOTE_SUPPORT_BBM
                        if (mIsBBM) {
                            shaderIndex = ShaderIndex.BBMApplyMask;
                        } 
                        else 
#endif //  EMOTE_SUPPORT_BBM
                        {
                            switch (mMaskType) {
                            default:
                            case MaskType.Apply:
                                switch (mBlendMode & M_BLEND_MODE_MASK) {
                                default:
                                case M_BLEND_MODE_ALPHABLEND: shaderIndex = ShaderIndex.AlphaApplyMask;      break;
                                case M_BLEND_MODE_PMA:        shaderIndex = ShaderIndex.PMAApplyMask;        break;
                                case M_BLEND_MODE_ADD:        shaderIndex = ShaderIndex.AddAplyMask;         break;
                                case M_BLEND_MODE_PSSUBTRACT: shaderIndex = ShaderIndex.PSSubtractApplyMask; break;
                                case M_BLEND_MODE_MULTIPLY:   shaderIndex = ShaderIndex.MultiplyApplyMask;   break;
                                case M_BLEND_MODE_SCREEN:     shaderIndex = ShaderIndex.ScreenApplyMask;     break;
                                case M_BLEND_MODE_EMBED:      shaderIndex = ShaderIndex.EmbedApplyMask;      break;
                                }
                                break;
                                
                            case MaskType.Inner:  shaderIndex = (effectiveMaskMode == (int)MaskMode.Stencil) ? ShaderIndex.StencilInnerMask : ShaderIndex.AlphaInnerMask;  break;
                            case MaskType.Outer:  shaderIndex = (effectiveMaskMode == (int)MaskMode.Stencil) ? ShaderIndex.StencilOuterMask : ShaderIndex.AlphaOuterMask; break;
                            }
                        }
                    }
#if EMOTE_SUPPORT_BBM
                    if (mIsBBM) {
                        switch (mBlendMode & M_BLEND_MODE_MASK) {
                        case M_BLEND_MODE_BBM_RAWADD:      bbmBlendIndex = BBMBlendIndex.RawAdd;      break;
                        case M_BLEND_MODE_BBM_RAWSUB:      bbmBlendIndex = BBMBlendIndex.RawSub;      break;
                        case M_BLEND_MODE_BBM_RAWMULTIPLY: bbmBlendIndex = BBMBlendIndex.RawMultiply; break;
                        case M_BLEND_MODE_BBM_RAWSCREEN:   bbmBlendIndex = BBMBlendIndex.RawScreen;   break;
                        case M_BLEND_MODE_PSADDITIVE:    bbmBlendIndex = BBMBlendIndex.PsAdditive;    break;
                        case M_BLEND_MODE_PSSUBTRACTIVE: bbmBlendIndex = BBMBlendIndex.PsSubtractive; break;
                        case M_BLEND_MODE_PSOVERLAY:     bbmBlendIndex = BBMBlendIndex.PsOverlay;     break;
                        case M_BLEND_MODE_PSHARDLIGHT:   bbmBlendIndex = BBMBlendIndex.PsHardlight;   break;
                        case M_BLEND_MODE_PSSOFTLIGHT:   bbmBlendIndex = BBMBlendIndex.PsSoftlight;   break;
                        case M_BLEND_MODE_PSCOLORDODGE:  bbmBlendIndex = BBMBlendIndex.PsColordodge;  break;
                        case M_BLEND_MODE_PSCOLORBURN:   bbmBlendIndex = BBMBlendIndex.PsColorburn;   break;
                        case M_BLEND_MODE_PSLIGHTEN:     bbmBlendIndex = BBMBlendIndex.PsLighten;     break;
                        case M_BLEND_MODE_PSDARKEN:      bbmBlendIndex = BBMBlendIndex.PsDarken;      break;
                        case M_BLEND_MODE_PSDIFFERENCE:  bbmBlendIndex = BBMBlendIndex.PsDifference;  break;
                        case M_BLEND_MODE_PSEXCLUSION:   bbmBlendIndex = BBMBlendIndex.PsExclusion;   break;
                        case M_BLEND_MODE_FLTGRAYSCALE:  bbmBlendIndex = BBMBlendIndex.FltGrayscale;  break;
                        case M_BLEND_MODE_FLTMOSAIC:     bbmBlendIndex = BBMBlendIndex.FltMosaic;     break;
                        case M_BLEND_MODE_FLTBLUR: {
                            switch ((int)mBlendParam % M_BLUR_SHADER_COUNT) {
                            case 0: bbmBlendIndex = BBMBlendIndex.FltBlur1; break;
                            case 1: bbmBlendIndex = BBMBlendIndex.FltBlur2; break;
                            case 2: bbmBlendIndex = BBMBlendIndex.FltBlur3; break;
                            case 3: bbmBlendIndex = BBMBlendIndex.FltBlur6; break;
                            case 4: bbmBlendIndex = BBMBlendIndex.FltBlur9; break;
                            }
                            break;
                        }
                        }
                    }
#endif //  EMOTE_SUPPORT_BBM

#if UNITY_EDITOR                    
                    if (EmotePlayer.outlineTranslucentTexture
                        && ! mIsBBM
                        && (mBlendMode & M_BLEND_MODE_MASK) == M_BLEND_MODE_EMBED) {
                        ShaderIndex outlineShaderIndex;
                        if (mMaskType == MaskType.Apply)
                            outlineShaderIndex = ShaderIndex.OutlineApplyMask;
                        else
                            outlineShaderIndex = ShaderIndex.Outline;
                        
                        Material outlineMaterial = RequireSpriteMaterial(outlineShaderIndex, modulate, mMaskRefCount, mRefAlphaIndex, mMeshDeformationMode, mTexture[mSpriteId].isAst
#if EMOTE_SUPPORT_BBM
                    										, bbmBlendIndex
#endif // EMOTE_SUPPORT_BBM
                                                                         );
                        var vec = Vector4.zero;
                        vec.x = EmotePlayer.outlineWidth / mTexture[mSpriteId].texture.width;
                        vec.y = EmotePlayer.outlineWidth / mTexture[mSpriteId].texture.height;
                        result.SetGlobalVector(sID_OutlineParam, vec);
                        result.SetGlobalColor(sID_OutlineColor, EmotePlayer.outlineColor);
                        result.DrawMesh(mesh, mE2WMatrix, outlineMaterial);
                        
                    }
#endif // UNITY_EDITOR                    
                    
                    Material material = RequireSpriteMaterial(shaderIndex, modulate, mMaskRefCount, mRefAlphaIndex, mMeshDeformationMode, mTexture[mSpriteId].isAst
#if EMOTE_SUPPORT_BBM
                    										, bbmBlendIndex
                                                            , pmaEnabled
#endif // EMOTE_SUPPORT_BBM
                                                              );
                    result.DrawMesh(mesh, mE2WMatrix, material);

                    // M2DebugLog.printf("DrawCore({0}): after Draw", mPlayerID);
                    break;
                }
            }
        }

        return result;
    }

#if UNITY_EDITOR
    void DrawGizmoRect(Rect rect, Color color, bool drawX) {
        Gizmos.color = color;
        Vector2 v0 = new Vector2(rect.xMin, rect.yMin);
        Vector2 v1 = new Vector2(rect.xMin, rect.yMax);
        Vector2 v2 = new Vector2(rect.xMax, rect.yMax);
        Vector2 v3 = new Vector2(rect.xMax, rect.yMin);
        Gizmos.DrawLine(v0, v1);
        Gizmos.DrawLine(v1, v2);
        Gizmos.DrawLine(v2, v3);
        Gizmos.DrawLine(v3, v0);
        if (drawX) {
            Gizmos.DrawLine(v0, v2);
            Gizmos.DrawLine(v1, v3);
        }
    }

    void OnDrawGizmosSelected() {
        if (isLegacyTransform)
            return;
        Matrix4x4 matrixBak = Gizmos.matrix;
        Gizmos.matrix *= transform.localToWorldMatrix;
        DrawGizmoRect(mU_CharaBounds, Color.green, false);
        if (mapToRenderTexture) {
            if (! clipRenderTexture)
                DrawGizmoRect(mU_CharaMarginBounds, Color.blue, true);
            else
                DrawGizmoRect(mU_RenderTextureBounds, Color.blue, true);
        }
        Gizmos.matrix = matrixBak;
    }
#endif // UNITY_EDITOR
};


public class EmoteDeviceManager
{
    private bool _loaded = false;
    
    public EmoteDeviceManager() {
    }
    
    ~EmoteDeviceManager() {
        Unload();
    }

    public void Load() {
        if (_loaded)
            return;
        EmotePlayer.requireDevice();
        _loaded = true;
    }
    
    public void Unload() {
        if (! _loaded)
            return;
        EmotePlayer.releaseDevice();
        _loaded = false;
    }

    public bool loaded {
        get { return _loaded; }
    }

    public int activePlayerCount {
        get { return EmotePlayer.activePlayers.Count; }
    }

    public List<EmotePlayer> getActivePlayerList() {
        return new List<EmotePlayer>(EmotePlayer.activePlayers);
    }

    public int deviceRefCount {
        get { return EmotePlayer.deviceRefCount; }
    }

    public bool deviceResident {
        get { return deviceRefCount > 0; }
    }

    public int getMainMemTotalSize() {
        return EmotePlayer.getMainMemTotalSize();
    }

    public int getMainMemAllocatedSize() {
        return EmotePlayer.getMainMemAllocatedSize();
    }
};

public class EmoteAsset
{
    public byte[] rawFileImage;
    public List<TextAsset> files = new List<TextAsset>();
    public List<Texture2D> textures = new List<Texture2D>();
};

public class EmoteAssetRequest : CustomYieldInstruction
{
    private class FileRequest {
        EmoteAssetRequest owner;
        private string path;
        private int count;
        public List<TextAsset> files = new List<TextAsset>();
        public List<Texture2D> textures = new List<Texture2D>();
        private bool done;
        private bool async;
        private Object resultObject;
        private AsyncOperation requestObject;

        public bool isDone {
            get {
                if ( done)
                    return true;
                if (async) {
                    if (! owner.isRequestDone(requestObject))
                        return false;
                    resolveLoadResult();
                } 
                return done;
            }
        }
    
        public FileRequest(EmoteAssetRequest _owner, string _path, bool _async) {
            owner = _owner;
            count = 0;
            done = false;
            async = _async;
            path = _path.Replace(".bytes", "").Replace(".psb", "");;
        }

        private string generateBytesPath() {
            return string.Format("{0}{1}", path, owner.requireExtension ? ".bytes" : "");
        }
        
        private string generateTexturePath() {
            return string.Format("{0}_tex{1:000}{2}", path, count - 1, owner.requireExtension ? ".png" : "");
        }

        public void start() {
            requestLoad();
        }

        protected void requestLoad() {
            string filePath;
            System.Type fileType;
            if (count == 0) {
                filePath = generateBytesPath();
                fileType = typeof(TextAsset);
            } else {
                filePath = generateTexturePath();
                fileType = typeof(Texture2D);
            }
            //      M2DebugLog.printf("try load {0} -> {1}", filePath, count);

            if (! owner.contains(filePath)) {
                done = true;
                return;
            }
            if (async) {
                requestObject = owner.loadAsync(filePath, fileType);
            } else {
                resultObject = owner.load(filePath, fileType);
                resolveLoadResult();
            }
        }

        private void resolveLoadResult() {
            if (async)
                resultObject = owner.getRequestResult(requestObject);
            //      M2DebugLog.printf("result load {0}", resultObject);
            if (resultObject == null) {
                done = true;
                return;
            }
            if (count == 0) {
                TextAsset file = (TextAsset)resultObject;
                files.Add(file);
            } else {
                Texture2D texture = (Texture2D)resultObject;
                textures.Add(texture);
            }
            count++;
            requestLoad();
        }
    };

    List<FileRequest> requests;

    public override bool keepWaiting
    {
        get {
            return ! isDone;
        }
    }

    public bool isDone {
        get {
            foreach (FileRequest request in requests) {
                if (! request.isDone)
                    return false;
            }
            return true;
        }
    }
    
    public EmoteAsset asset {
        get {
            EmoteAsset result = new EmoteAsset();
            foreach (FileRequest request in requests) {
                result.files.AddRange(request.files);
                result.textures.AddRange(request.textures);
            }
            return result;
        }
    }

    public EmoteAssetRequest(string[] paths, bool async) {
        requests = new List<FileRequest>();
        foreach (string path in paths) {
            FileRequest request = new FileRequest(this, path, async);
            requests.Add(request);
        }
    }

    public void requestLoad() {
        foreach (FileRequest request in requests) {
            request.start();
        }
    }
    
    public virtual bool contains(string filePath) {
        return true;
    }
    
    public virtual AsyncOperation loadAsync(string filePath, System.Type fileType) {
        return null;
    }
    
    public virtual Object load(string filePath, System.Type fileType) {
        return null;
    }

    public virtual bool isRequestDone(AsyncOperation request) {
        return false;
    }

    public virtual Object getRequestResult(AsyncOperation request) {
        return null;
    }

    public virtual bool requireExtension {
        get {
            return true;
        }
    }
};

public class EmoteAssetFromResourcesRequest : EmoteAssetRequest
{
    public EmoteAssetFromResourcesRequest(string[] paths, bool _async) : base(paths, _async) {
        requestLoad();
    }

    public override bool requireExtension {
        get {
            return false;
        }
    }
    
    public override AsyncOperation loadAsync(string filePath, System.Type fileType) {
        return Resources.LoadAsync(filePath, fileType);
    }
    
    public override Object load(string filePath, System.Type fileType) {
        return Resources.Load(filePath, fileType);
    }
    
    public override bool isRequestDone(AsyncOperation request) {
        return ((ResourceRequest)request).isDone; 
    }
    
    public override Object getRequestResult(AsyncOperation request) {
        return ((ResourceRequest)request).asset;
    }
};

public class EmoteAssetFromAssetBundleRequest : EmoteAssetRequest
{
    private AssetBundle assetBundle;

    public EmoteAssetFromAssetBundleRequest(AssetBundle bundle, string[] paths, bool _async) : base(paths, _async) {
        assetBundle = bundle;
        requestLoad();
    }

    public override bool contains(string filePath) {
        return assetBundle.Contains(filePath);
    }

    public override AsyncOperation loadAsync(string filePath, System.Type fileType) {
        return assetBundle.LoadAssetAsync(filePath, fileType);
    }

    public override Object load(string filePath, System.Type fileType) {
        return assetBundle.LoadAsset(filePath, fileType);
    }

    public override bool isRequestDone(AsyncOperation request) {
        return ((AssetBundleRequest)request).isDone; 
    }

    public override Object getRequestResult(AsyncOperation request) {
        return ((AssetBundleRequest)request).asset;
    }
}

#if UNITY_EDITOR
public class EmoteAssetFromAssetDatabaseRequest : EmoteAssetRequest
{
    public EmoteAssetFromAssetDatabaseRequest(string[] paths, bool _async) : base(paths, _async) {
        requestLoad();
    }
    
    public override AsyncOperation loadAsync(string filePath, System.Type fileType) {
        return null;
    }
    
    public override Object load(string filePath, System.Type fileType) {
        return AssetDatabase.LoadAssetAtPath(filePath, fileType);
    }
    
    public override bool isRequestDone(AsyncOperation request) {
        return false;
    }
    
    public override Object getRequestResult(AsyncOperation request) {
        return null;
    }
};
#endif // UNITY_EDITOR


