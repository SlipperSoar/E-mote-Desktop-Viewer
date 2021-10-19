using UnityEngine;

[AddComponentMenu("Emote Player/Emote API Logger")]
public class EmoteAPILogger : MonoBehaviour {
    public EmotePlayer player = null;
    public string log = "";

    void Start() {
        if (player == null)
            player = GetComponent<EmotePlayer>();
    }
}
