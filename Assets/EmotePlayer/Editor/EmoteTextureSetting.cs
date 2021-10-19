// ◇UTF-8

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

// 「/resources/emote/」以下のテクスチャのmipmapを外す
class EmoteTextureSetting : AssetPostprocessor
{
	void OnPreprocessTexture ()
	{
		var path = assetPath.ToLower ();
		if (path.IndexOf("/emote/") < 0)
            return;

		var imp = (assetImporter as TextureImporter);
#if ! UNITY_5_5_OR_NEWER
		imp.textureType = TextureImporterType.Advanced;
        imp.textureFormat = TextureImporterFormat.AutomaticCompressed;
#else //  ! UNITY_5_5_OR_NEWER
		imp.textureType = TextureImporterType.Default;
        imp.textureCompression = TextureImporterCompression.Compressed;
#endif //  ! UNITY_5_5_OR_NEWER         

		imp.mipmapEnabled = false;
        imp.wrapMode = TextureWrapMode.Clamp;
	}
}