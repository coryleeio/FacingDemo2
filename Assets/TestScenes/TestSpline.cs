using UnityEngine;
using System.Collections.Generic;
using Gamepackage;

public class TestSpline : MonoBehaviour
{
    public Material sourceMaterial;

    void Start()
    {
        var lis = new List<Sprite>();
        var longswordSprite = Resources.Load<Sprite>("Sprites/Longsword");
        var spritesToEquip = new Dictionary<SpriteAttachment, Sprite>()
        {
            {
                SpriteAttachment.MainHandFront, longswordSprite
            }
        };

        var skeletonDataAsset = Resources.Load<Spine.Unity.SkeletonDataAsset>("Spine/Export/Humanoid_SkeletonData");
        ViewFactory.BuildSplineView(skeletonDataAsset, spritesToEquip, "Template");
    }
}
