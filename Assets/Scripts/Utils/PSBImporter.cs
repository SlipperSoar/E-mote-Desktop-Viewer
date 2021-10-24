using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using FreeMote;
using FreeMote.Psb;
using FreeMote.Psb.Textures;
using FreeMote.Psb.Types;
using FreeMote.Plugins;
using FreeMote.Plugins.Images;
using FreeMote.Plugins.Audio;
using FreeMote.PsBuild;
using UnityEngine;

public class PsbInfo
{
    public string name;
    public string path;
    public byte[] bytes;
}

public static class PSBImporter
{
    private static string dataPath;
    private static List<PsbInfo> emotes;
    public static List<PsbInfo> Emotes => emotes;

    static PSBImporter()
    {
        emotes = new List<PsbInfo>();

        dataPath = Application.streamingAssetsPath;
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
            emotes.Add(new PsbInfo() { name = file.Name, path = path });
        }

        var dllDirPath = directoryInfo.Parent.CreateSubdirectory("Managed").FullName;
        Debug.Log($"dir parent: {dllDirPath}");
#if UNITY_EDITOR
        FreeMount.Init();
#elif UNITY_STANDALONE_WIN
        // FreeMount.Init($"{dataPath}/../Managed");
        // FreeMount.Init(dllDirPath:$"{dataPath}/../Managed");
        FreeMount.Init(dllDirPath, dllDirPath);
#endif

        foreach (var emote in emotes)
        {
            var path = emote.path;
            Debug.Log($"current PSB File Path: {path}");
            try
            {
                var psbFile = new PsbFile(path);
                var headerValid = psbFile.TestHeaderEncrypted();
                var bodyValid = psbFile.TestBodyEncrypted();
                Debug.Log($"header valid?: {headerValid}");
                Debug.Log($"body valid?: {bodyValid}");
                if (headerValid && bodyValid)
                {
                    emote.bytes = File.ReadAllBytes(path);
                }
                else
                {
                    LoadEmoteBytes(emote);
                }
            }
            catch (System.Exception e1)
            {
                try
                {
                    LoadEmoteBytes(emote);
                }
                catch (System.Exception e2)
                {
                    Debug.LogError($"error1: {e1.Message}, error2: {e2.Message}");
                    continue;
                }
            }
        }

        emotes = emotes.Where(info => info.bytes != null).ToList();
    }

    private static void LoadEmoteBytes(PsbInfo psbInfo)
    {
        var ctx = FreeMount.CreateContext();
        try
        {
            using var fs = File.OpenRead(psbInfo.path);
            string currentType = null;
            using var ms = ctx.OpenFromShell(fs, ref currentType);
            Debug.Log($"memory stream: {ms}");
            var psb = ms != null ? new PSB(ms) : new PSB(fs);
            if (psb.Platform == PsbSpec.krkr)
            {
                Debug.Log("Platform: Krkr");
                psb.SwitchSpec(PsbSpec.win, PsbSpec.win.DefaultPixelFormat());
            }
            psb.FixMotionMetadata();
            psb.Merge();
            psbInfo.bytes = psb.Build();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"error: {e.Message}");
        }
    }
}
