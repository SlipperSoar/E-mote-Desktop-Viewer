using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmoteLoader : MonoBehaviour
{
    [SerializeField] private EmotePlayer emotePlayer;
    [SerializeField] private Text log;

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
        log.text = $"path: {Application.streamingAssetsPath}\n count: {emoteAssets.Count}";
        emotePlayer.LoadData(emoteAssets[index]);
    }

    #endregion
}
