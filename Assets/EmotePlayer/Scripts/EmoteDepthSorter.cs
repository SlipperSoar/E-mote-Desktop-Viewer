using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Emote Player/Emote Depth Sorter")]
public class EmoteDepthSorter : MonoBehaviour
{
    private List<EmotePlayer> players;

    void OnPreRender() {
        players = new List<EmotePlayer>(EmotePlayer.activePlayers);
        foreach (EmotePlayer player in players)
            player.skipNextDrawCall = true;
    }

    public class EmotePlayerDepthComarere : IComparer<EmotePlayer>
    {
        public int Compare(EmotePlayer p1, EmotePlayer p2) {
            if (p1.cameraDepth < p2.cameraDepth)
                return -1;
            if (p1.cameraDepth > p2.cameraDepth)
                return 1;
            return 0;
        }
    }
    
    void OnPostRender() {
        Matrix4x4 mat = GetComponent<Camera>().worldToCameraMatrix;
        foreach (EmotePlayer player in players)
            player.cameraDepth = mat.MultiplyPoint(player.transform.position).z;
        players.Sort(new EmotePlayerDepthComarere());
        foreach (EmotePlayer player in players)
            player.DrawCore();
    }
}