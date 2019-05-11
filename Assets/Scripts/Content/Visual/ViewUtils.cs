using Spine.Unity;
using UnityEngine;

namespace Gamepackage
{
    public class ViewUtils
    {
        public static void ApplyColorToEntity(Entity owner, Color color)
        {
            if (owner.View != null && owner.View.ViewGameObject != null)
            {
                var go = owner.View.ViewGameObject;
                var mySkeletonAnimation = go.GetComponentInChildren<SkeletonAnimation>(true);
                if (mySkeletonAnimation != null)
                {
                    mySkeletonAnimation.skeleton.SetColor(color);
                }
                else
                {
                    var mySpriteImages = go.GetComponentsInChildren<SpriteRenderer>(true);
                    foreach (var mySpriteImage in mySpriteImages)
                    {
                        var isShadow = mySpriteImage.gameObject.transform.GetComponent<Shadow>() != null;
                        if (!isShadow)
                        {
                            mySpriteImage.color = color;
                        }
                    }
                }
            }
        }
    }
}
