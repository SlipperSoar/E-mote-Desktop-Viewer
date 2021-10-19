using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

[AddComponentMenu("Emote Player/Emote Logo")]
public class EmoteLogo : MonoBehaviour
{
    public enum Resolution {
        D2,
        D4,
        D5
    };
    public string nextScene;
    public bool canSkip = true;
    public Resolution resolution = Resolution.D4;
    private float elapsedTime = 0;
    private bool active = false;
    private const float logoPlayingTime = 1.5f;

    void Start() {
        StartCoroutine("LoadLogoAsync");
    }

    public IEnumerator LoadLogoAsync() {
        int texWidth = 0, texHeight = 0;
        string filename = "";
        switch (resolution) {
        case Resolution.D2: texWidth = 1024; texHeight = 512; filename = "emote/emote_logo_d2"; break;
        case Resolution.D4: texWidth = 1024; texHeight = 1024; filename = "emote/emote_logo_d4"; break;
        case Resolution.D5: texWidth = 2048; texHeight = 1024; filename = "emote/emote_logo_d5"; break;
        }

        EmoteAssetRequest req = EmotePlayer.LoadAssetAsync(filename);
        while (! req.isDone)
            yield return 0;

        EmotePlayer player = this.GetComponent(typeof(EmotePlayer)) as EmotePlayer;
        player.texWidth = texWidth;
        player.texHeight = texHeight;
        player.LoadData(req.asset);

        active = true;
    }
    
    void Update() {
        if (! active)
            return;
        if (canSkip
            & (Input.GetKey(KeyCode.Space)
               || Input.GetKey(KeyCode.Return)
               || Input.GetMouseButtonDown(0)
               || Input.GetMouseButtonDown(1)
               || (Input.touchCount > 0
                   && Input.touches[0].phase == TouchPhase.Began))) {
                GoToNextScene();
        }
        elapsedTime += Time.deltaTime;
        if (elapsedTime > logoPlayingTime)
            GoToNextScene();
    }

    void GoToNextScene() {
        if (nextScene != "")
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene);
        active = false;
    }
};