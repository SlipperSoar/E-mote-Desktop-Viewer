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
// using FreeMote.PsBuild;
using UnityEngine;

public static class PSBImporter
{
    private static List<string> psbPathes;
    private static List<byte[]> emotes;
    public static List<byte[]> Emotes => emotes;

    static PSBImporter()
    {
        psbPathes = new List<string>();
        emotes = new List<byte[]>();

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

        FreeMount.Init();
        var ctx = FreeMount.CreateContext();
        foreach (var path in psbPathes)
        {
            using var fs = File.OpenRead(path);
            string currentType = null;
            using var ms = ctx.OpenFromShell(fs, ref currentType);
            var psb = ms != null ? new PSB(ms) : new PSB(fs);
            if (psb.Platform == PsbSpec.krkr)
            {
                Debug.Log("Platform: Krkr");
                // psb.SwitchSpec(PsbSpec.win, PsbSpec.win.DefaultPixelFormat());
            }
            psb.FixMotionMetadata();
            psb.Merge();
            emotes.Add(psb.Build());
            // emotes.Add(File.ReadAllBytes(path));
        }
    }
}
