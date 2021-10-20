using System.Collections;
using System.Collections.Generic;
using System.IO;
using FreeMote;
using FreeMote.Psb;
using FreeMote.Psb.Textures;
using FreeMote.Psb.Types;
using FreeMote.Plugins;
using FreeMote.Plugins.Images;
using FreeMote.Plugins.Audio;
using UnityEngine;

public static class PSBImporter
{
    private static List<string> psbPathes;
    private static List<byte[]> emotes;

    static PSBImporter()
    {
        psbPathes = new List<string>();

        var dataPath = Application.streamingAssetsPath;
        DirectoryInfo directoryInfo = new DirectoryInfo(dataPath);
        var files = directoryInfo.GetFiles();
        Debug.Log($"dataPath: {dataPath}, fileCount: {files.Length}");
        foreach (var file in files)
        {
            if (!file.Extension.Equals(".psb"))
            {
                continue;
            }
            Debug.Log($"File Extension: {file.Extension} Name: {file.Name}");
            var path = $"{dataPath}/{file.Name}";
            psbPathes.Add(path);
        }

        foreach (var path in psbPathes)
        {
            PSB psb = new PSB(path);
            PsbFile psbFile = new PsbFile(path);
            
        }
    }
}
