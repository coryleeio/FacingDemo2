using UnityEditor;
using UnityEngine;

namespace Gamepackage
{
    public class ImportPostProcessor : AssetPostprocessor
    {
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
                    textureImporter.SetTextureSettings(textureSettings);
                }
            }

        }
    }
}
