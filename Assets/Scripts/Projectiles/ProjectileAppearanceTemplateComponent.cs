using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class ProjectileAppearanceTemplateComponent
    {
        // List of sprites for this appearance, if there are more than 1 it is considered animated.
        public List<Sprite> Sprites = new List<Sprite>();

        // Total time the projectile is visible, 
        // does NOT affect Projectile component types because they just travel until they hit the target
        public float TotalTimeVisible = 0.5f;

        // If you are a non projectile type, do you want to keep the projectiles rotation when you hit the target?
        // Useful with spinning projectiles to inherit the rotation OnEnter, OnHit, and OnLeave etc.
        // rarely true.
        public bool InheritRotation = false;

        // Should the sprite face the direction it is moving.
        public SpriteFacingBehavior SpriteFacingBehavior;

        // How long to spend in each tile. For projectiles this determines the speed.
        // For explosions this determines the expansion rate.
        public float TimeSpentInEachTile = 0.5f;

        // Scale of the projectile.
        public float ScaleX;
        public float ScaleY;
        public float ScaleZ;
        public float AnimationTimeSpentShowingEachSprite = 0.0f;
        public ChangeMethodType AnimationChangeType = ChangeMethodType.NotSet;

        public bool Animated
        {
            get
            {
                return Sprites.Count > 1;
            }
        }
    }
}
