using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

[InitializeOnLoad]
public class EmoteWebGLPluginsSetting
{
    static EmoteWebGLPluginsSetting() {
#if UNITY_2019_1_OR_NEWER
        Regex includePat = new Regex("EmotePlayer/Plugins/WebGL-1\\.38");
        Regex excludePat = new Regex("EmotePlayer/Plugins/WebGL-(1\\.35|1\\.36)");
#elif UNITY_5_5_OR_NEWER
        Regex includePat = new Regex("EmotePlayer/Plugins/WebGL-1\\.36");
        Regex excludePat = new Regex("EmotePlayer/Plugins/WebGL-(1\\.35|1\\.38)");
#else
        Regex includePat = new Regex("EmotePlayer/Plugins/WebGL-1\\.35");
        Regex excludePat = new Regex("EmotePlayer/Plugins/WebGL-(1\\.36|1\\.38)");
#endif 

        PluginImporter[] importers = PluginImporter.GetAllImporters();
        bool modified = false;

        foreach (PluginImporter importer in importers) {
            if (includePat.IsMatch(importer.assetPath)) {
                if (! importer.GetCompatibleWithPlatform(BuildTarget.WebGL)) {
                    importer.SetCompatibleWithPlatform(BuildTarget.WebGL, true);
                    importer.SaveAndReimport();
                    modified = true;
                }
            } else if (excludePat.IsMatch(importer.assetPath)) {
                if (importer.GetCompatibleWithPlatform(BuildTarget.WebGL)) {
                    importer.SetCompatibleWithPlatform(BuildTarget.WebGL, false);
                    importer.SaveAndReimport();
                    modified = true;
                }
            }
        }
        if (modified)
            EditorUtility.DisplayDialog("E-mote WebGL Plugins", "activated plugins suitable for this version of Unity!", "ok");
    }
}
