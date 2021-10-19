#if (UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN)
#define EMOTE_PLATFORM_WIN
#endif

#if (UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX)
#define EMOTE_PLATFORM_OSX
#endif

#if (UNITY_IOS && ! UNITY_EDITOR)
#define EMOTE_PLATFORM_IPHONE
#endif

#if (UNITY_ANDROID && ! UNITY_EDITOR)
#define EMOTE_PLATFORM_ANDROID
#endif

#if (UNITY_WEBGL && ! UNITY_EDITOR)
#define EMOTE_PLATFORM_WEBGL
#endif

#if (UNITY_SWITCH && ! UNITY_EDITOR)
#define EMOTE_PLATFORM_SWITCH
#endif

#if (UNITY_PS4 && ! UNITY_EDITOR)
#define EMOTE_PLATFORM_PS4
#endif

#if (EMOTE_PLATFORM_IPHONE || EMOTE_PLATFORM_ANDROID || EMOTE_PLATFORM_WEBGL || EMOTE_PLATFORM_PS4 || EMOTE_PLATFORM_SWITCH || EMOTE_PLATFORM_WIN || EMOTE_PLATFORM_OSX)
#define EMOTE_SUPPORT_OWNHEAP
#endif


using UnityEngine;
using System.Collections;

// An FPS counter.
// It calculates frames/second over each updateInterval,
// so the display does not keep changing wildly.
[AddComponentMenu("Emote Player/Emote Benchmarks")]
public class EmoteBenchmarks : MonoBehaviour {
    public float updateInterval = 0.5F;
    private double lastInterval;
    private int frames;
    private double elapsedTime;
    private float fps, maxFps, minFps, sumFps, cntFps;
#if EMOTE_SUPPORT_OWNHEAP
    private float mmem, maxMmem, minMmem, sumMmem, cntMmem;
#endif //  EMOTE_SUPPORT_OWNHEAP

    void Reset() {
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
        elapsedTime = 0;
        fps = 0; maxFps = float.MinValue; minFps = float.MaxValue; sumFps = 0; cntFps = 0;
#if EMOTE_SUPPORT_OWNHEAP
        mmem = 0; maxMmem = float.MinValue; minMmem = float.MaxValue; sumMmem = 0; cntMmem = 0;
#endif // EMOTE_SUPPORT_OWNHEAP
    }

    void Start() {
        Reset();
    }
    void OnGUI() {
        Vector2 guiScreenSize = new Vector2(800, 600);
        float scale = System.Math.Max(Screen.width / guiScreenSize.x, Screen.height / guiScreenSize.y);
        GUIUtility.ScaleAroundPivot(new Vector2(scale, scale), Vector2.zero);

        GUILayout.Label(System.String.Format("{0} sec elapsed", (int)elapsedTime));
        GUILayout.Label(System.String.Format("{0:f2} fps (ave:{3:f2}/min:{1:f2}/max:{2:f2})", fps, minFps, maxFps, sumFps / cntFps));
#if EMOTE_SUPPORT_OWNHEAP
        GUILayout.Label(System.String.Format("{0:f2}% mmem (ave:{3:f2}/min:{1:f2}/max:{2:f2})", mmem, minMmem, maxMmem, sumMmem / cntMmem));
#endif // EMOTE_SUPPORT_OWNHEAP
        if (GUILayout.Button("Reset"))
            Reset();
    }

    void Update() {
        ++frames;
        float timeNow = Time.realtimeSinceStartup;
        if (timeNow > lastInterval + updateInterval) {
            fps = (float) (frames / (timeNow - lastInterval));
            frames = 0;
            elapsedTime += timeNow - lastInterval;
            lastInterval = timeNow;
            maxFps = System.Math.Max(maxFps, fps);
            minFps = System.Math.Min(minFps, fps);
            sumFps += fps;
            cntFps += 1;   
        }
#if EMOTE_SUPPORT_OWNHEAP
        mmem = (float)(EmotePlayer.getMainMemUsage() * 100.0);
        maxMmem = System.Math.Max(maxMmem, mmem);
        minMmem = System.Math.Min(minMmem, mmem);
        sumMmem += mmem;
        cntMmem += 1;
#endif
    }
}
