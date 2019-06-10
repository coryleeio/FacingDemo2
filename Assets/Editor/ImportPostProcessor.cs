using UnityEditor;
using UnityEngine;

namespace Gamepackage
{
    public class ImportPostProcessor : AssetPostprocessor
    {
        void OnPreprocessAsset()
        {
            if (assetImporter != null)
            {
                if (assetPath.Contains("Prefabs/") && !assetPath.Contains("UI/"))
                {
                    assetImporter.assetBundleName = "Core";
                }
            }
        }

        void OnPreprocessTexture()
        {
            if (assetPath.Contains(".png"))
            {
                TextureImporter textureImporter = (TextureImporter)assetImporter;
                if (assetPath.Contains("Sprites/"))
                {
                    // Spine needs this
                    textureImporter.isReadable = true;

                    textureImporter.textureType = TextureImporterType.Sprite;
                    textureImporter.spriteImportMode = SpriteImportMode.Single;
                    textureImporter.spritePixelsPerUnit = 128;

                    TextureImporterSettings textureSettings = new TextureImporterSettings();
                    textureImporter.ReadTextureSettings(textureSettings);
                    textureSettings.spriteMeshType = SpriteMeshType.FullRect;
                    textureImporter.assetBundleName = "Core";
                    textureImporter.SetTextureSettings(textureSettings);
                }

                if (assetPath.Contains("Spine/Export"))
                {
                    // Spine needs this
                    textureImporter.isReadable = true;

                    TextureImporterSettings textureSettings = new TextureImporterSettings();
                    textureImporter.ReadTextureSettings(textureSettings);
                    textureImporter.assetBundleName = "Core";
                    textureImporter.SetTextureSettings(textureSettings);
                }
            }

        }
    }
}
