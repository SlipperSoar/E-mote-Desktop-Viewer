using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class PSBImporter
{
    private static List<string> psbPathes;
    public static List<byte[]> emoteBytes;

    static PSBImporter()
    {
        psbPathes = new List<string>();
        emoteBytes = new List<byte[]>();

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
            emoteBytes.Add(File.ReadAllBytes(path));
        }
    }
}
