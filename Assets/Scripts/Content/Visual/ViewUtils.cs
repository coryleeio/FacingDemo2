using Spine.Unity;
using UnityEngine;

namespace Gamepackage
{
    public class ViewUtils
    {
        public static void UpdateHealthBarColor(Entity entity)
        {
            if(entity.View != null && entity.View.HealthBar != null)
            {
                entity.View.HealthBar.UpdateColor(ColorForHealthBar(entity.Behaviour.ActingTeam, entity.IsPlayer));
            }
        }

        private static Color ColorForHealthBar(Team team, bool isPlayer)
        {
            if (isPlayer)
            {
                return Color.green;
            }
            else if (team == Team.Enemy || team == Team.EnemyOfAll)
            {
                return Color.red;
            }
            else if (team == Team.Neutral || (team == Team.PLAYER && !isPlayer))
            {
                return Color.blue;
            }
            else throw new NotImplementedException();
        }

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
