using UnityEngine;
using System.Collections.Generic;
using Gamepackage;

public class TestSpline : MonoBehaviour {

    public Material sourceMaterial; // This will be used as the basis for shader and material property settings.

    // Use this for initialization
    void Start ()
    {
        var lis = new List<Sprite>();
        var longswordSprite = Resources.Load<Sprite>("Sprites/Longsword");
        var spritesToEquip = new Dictionary<SpriteAttachment, Sprite>()
        {
            {
                SpriteAttachment.MainHandFront, longswordSprite
            }
        };

        ViewFactory.BuildSplineView("Spine/Humanoid/Export/Humanoid_SkeletonData", spritesToEquip, "HumanBlack");
    }
}
