using Spine.Unity;
using UnityEngine;

namespace Gamepackage
{
    public class Poison : Effect
    {
        public override string DisplayName
        {
            get
            {
                return "effect.poison.name".Localize();
            }
        }

        public override string Description
        {
            get
            {
                return "effect.poison.description".Localize();
            }
        }

        public int PoisonAmount;

        public override void OnApply(EntityStateChange outcome)
        {
            base.OnApply(outcome);
            Context.UIController.TextLog.AddText(string.Format("effect.poison.apply.message".Localize(), outcome.Target.Name));
        }

        public override void OnRemove(Entity entity)
        {
            base.OnRemove(entity);
            Context.UIController.TextLog.AddText(string.Format("effect.poison.remove.message".Localize(), entity.Name));
        }

        public override void Tick(Entity entity)
        {
            base.Tick(entity);
            var calculated = CombatUtil.CalculateSimpleDamage(
                entity,
                "effect.poison.tick.message",
                PoisonAmount,
                DamageTypes.POISON
            );
            CombatUtil.ApplyAttackInstantly(calculated);
        }

        public override void HandleStacking(EntityStateChange outcome)
        {
            StackingStrategies.AddDuration(outcome, this);
        }

        public override void ApplyPersistentVisualEffects(Entity owner)
        {
            base.ApplyPersistentVisualEffects(owner);
            if(owner.View != null && owner.View.ViewGameObject != null)
            {
                var go = owner.View.ViewGameObject;
                var mySkeletonAnimation = go.GetComponentInChildren<SkeletonAnimation>(true);
                if(mySkeletonAnimation!= null)
                {
                    mySkeletonAnimation.skeleton.SetColor(Color.green);
                }
                var mySpriteImages = go.transform.GetComponentsInChildren<SpriteRenderer>(true);
                foreach( var mySpriteImage in mySpriteImages)
                {
                    mySpriteImage.color = Color.green;
                }
            }
        }

        public override void RemovePersistantVisualEffects(Entity owner)
        {
            base.RemovePersistantVisualEffects(owner);
            if (owner.View != null && owner.View.ViewGameObject != null)
            {
                var go = owner.View.ViewGameObject;
                var mySkeletonAnimation = go.GetComponentInChildren<SkeletonAnimation>(true);
                if (mySkeletonAnimation != null)
                {
                    mySkeletonAnimation.skeleton.SetColor(Color.white);
                }
                var mySpriteImages = go.transform.GetComponentsInChildren<SpriteRenderer>(true);
                foreach (var mySpriteImage in mySpriteImages)
                {
                    mySpriteImage.color = Color.white;
                }
            }
        }
    }
}
