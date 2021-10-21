using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoteLoader : MonoBehaviour
{
    [SerializeField] private EmotePlayer emotePlayer;

    private List<EmoteAsset> emoteAssets;

    void Start()
    {
        emoteAssets = new List<EmoteAsset>();
        foreach (var bytes in PSBImporter.Emotes)
        {
            EmoteAsset emoteAsset = new EmoteAsset();
            emoteAsset.files.Add(bytes);
            emoteAssets.Add(emoteAsset);
        }
        LoadToEmotePlayer();
    }

    #region Private Method

    private void LoadToEmotePlayer(int index = 0)
    {
        emotePlayer.LoadData(emoteAssets[index]);
    }

    #endregion
}