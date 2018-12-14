using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class CombatContextCapability
    {
        public Entity Source;
        public CombatContext CombatContext;
        public Item MainHand;
        public Item Ammo;
        public List<Effect> AppliedEffects = new List<Effect>();
        public AttackParameters AttackParameters;
        public int Range;
        public int NumberOfTargetsToPierce;
        public bool CanPerform
        {
            get
            {
                if (CombatContext == CombatContext.NotSet)
                {
                    return false;
                }
                var hasBody = Source.Body != null && Source.Body.CanAttackInMelee;
                if (CombatContext == CombatContext.Melee)
                {
                    var hasWeapon = hasBody && Source.Inventory != null && MainHand != null && MainHand.CanBeUsedInMelee;
                    return hasBody || hasWeapon;
                }
                else if (CombatContext == CombatContext.Ranged)
                {
                    var hasRangedWeapon = hasBody && Source.Inventory != null && MainHand != null && MainHand.CanBeUsedForRanged;
                    var hasAmmo = Ammo != null && MainHand.AmmoType == Ammo.AmmoType;
                    return hasRangedWeapon && hasAmmo;
                }
                else if (CombatContext == CombatContext.Thrown)
                {
                    return hasBody && Source.Inventory != null && MainHand != null && MainHand.CanBeThrown;
                }
                else if (CombatContext == CombatContext.Zapped)
                {
                    return hasBody && Source.Inventory != null && MainHand != null && MainHand.CanBeUsedForZap;
                }
                else
                {
                    throw new NotImplementedException("Unimplemented attack type");
                }
            }
        }

        public bool IsInRange(Entity target)
        {
            return IsInRange(target.Position);
        }

        public bool IsInRange(Point target)
        {
            return Source.Position.IsOrthogonalTo(target) && Source.Position.Distance(target) <= Range;
        }

        public bool HasAClearShot(Point target)
        {
            var isOrthogonal = Source.Position.IsOrthogonalTo(target);
            var distance = (int)Source.Position.Distance(target); // whole number bc grid coords

            var coordsToCheck = MathUtil.LineInDirection(Source.Position, MathUtil.RelativeDirection(Source.Position, target), distance);
            foreach(var point in coordsToCheck)
            {
                var game = Context.GameStateManager.Game;
                var level = game.CurrentLevel;
                if (point != target)
                {
                    var entitiesInPosition = level.Grid[point].EntitiesInPosition;
                    foreach(var entityInPosition in entitiesInPosition)
                    {
                        if(entityInPosition.IsCombatant && entityInPosition.Behaviour != null && Source.Behaviour  != null && entityInPosition.Behaviour.Team == Source.Behaviour.Team)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public CombatContextCapability(Entity source, CombatContext combatContext, Item mainhandOverride = null)
        {
            this.Source = source;
            this.CombatContext = combatContext;

            if (mainhandOverride == null)
            {
                MainHand = source.Inventory.GetItemBySlot(ItemSlot.MainHand);
            }
            else
            {
                MainHand = mainhandOverride;
            }

            Ammo = source.Inventory.GetItemBySlot(ItemSlot.Ammo);

            if (combatContext == CombatContext.Melee)
            {
                if (MainHand != null && MainHand.CanBeUsedInMelee)
                {
                    Range = MainHand.MeleeRange;
                    NumberOfTargetsToPierce = MainHand.MeleeTargetsPierced;

                    AttackParameters = MathUtil.ChooseRandomElement<AttackParameters>(MainHand.MeleeParameters);
                    AppliedEffects.AddRange(MainHand.Effects.FindAll(CombatUtil.AppliedEffects));
                    AppliedEffects.AddRange(AttackParameters.AttackSpecificEffects.FindAll(CombatUtil.AppliedEffects));
                }
                else
                {
                    Range = source.Body.MeleeRange;
                    NumberOfTargetsToPierce = source.Body.MeleeTargetsPierced;

                    AttackParameters = MathUtil.ChooseRandomElement<AttackParameters>(source.Body.MeleeParameters);
                    AppliedEffects.AddRange(source.Body.Effects.FindAll(CombatUtil.AppliedEffects));
                    AppliedEffects.AddRange(AttackParameters.AttackSpecificEffects.FindAll(CombatUtil.AppliedEffects));
                }

            }

            else if (combatContext == CombatContext.Ranged)
            {
                if (MainHand != null && MainHand.CanBeUsedForRanged && Ammo != null)
                {
                    Range = MainHand.RangedRange;
                    NumberOfTargetsToPierce = MainHand.RangedTargetsPierced;
                    var rangedAttackParameters = MathUtil.ChooseRandomElement<AttackParameters>(MainHand.RangedParameters);
                    var ammoAttackParameters = MathUtil.ChooseRandomElement<AttackParameters>(Ammo.RangedParameters);
                    AttackParameters = new AttackParameters()
                    {
                        AttackMessage = ammoAttackParameters.AttackMessage,
                        Bonus = rangedAttackParameters.Bonus + ammoAttackParameters.Bonus,
                        DamageType = ammoAttackParameters.DamageType,
                        DyeNumber = ammoAttackParameters.DyeNumber,
                        DyeSize = ammoAttackParameters.DyeSize,
                        ExplosionParameters = ammoAttackParameters.ExplosionParameters,
                        ProjectileAppearanceIdentifier = ammoAttackParameters.ProjectileAppearanceIdentifier,
                    };

                    AppliedEffects.AddRange(MainHand.Effects.FindAll(CombatUtil.AppliedEffects));
                    AppliedEffects.AddRange(Ammo.Effects.FindAll(CombatUtil.AppliedEffects));
                    AppliedEffects.AddRange(rangedAttackParameters.AttackSpecificEffects.FindAll(CombatUtil.AppliedEffects));
                    AppliedEffects.AddRange(ammoAttackParameters.AttackSpecificEffects.FindAll(CombatUtil.AppliedEffects));
                }
            }
            else if (combatContext == CombatContext.Thrown)
            {
                if (MainHand != null && MainHand.CanBeThrown)
                {
                    AttackParameters = MathUtil.ChooseRandomElement<AttackParameters>(MainHand.ThrowParameters);
                    AppliedEffects.AddRange(MainHand.Effects.FindAll(CombatUtil.AppliedEffects));
                    AppliedEffects.AddRange(AttackParameters.AttackSpecificEffects.FindAll(CombatUtil.AppliedEffects));
                    Range = MainHand.ThrownRange;
                    NumberOfTargetsToPierce = MainHand.ThrownTargetsPierced;
                }
            }
            else if (combatContext == CombatContext.Zapped)
            {
                if (MainHand != null && MainHand.CanBeUsedForZap)
                {
                    AttackParameters = MathUtil.ChooseRandomElement<AttackParameters>(MainHand.ZapParameters);
                    AppliedEffects.AddRange(MainHand.Effects.FindAll(CombatUtil.AppliedEffects));
                    AppliedEffects.AddRange(AttackParameters.AttackSpecificEffects.FindAll(CombatUtil.AppliedEffects));
                    Range = MainHand.ZapRange;
                    NumberOfTargetsToPierce = MainHand.ZappedTargetsPierced;
                }
            }
            // If we fall through here we aren't dealing with a attack
        }

        public EntityStateChange AttackTarget(Entity target)
        {
            return AttackTargets(new List<Entity>() { target });
        }

        public EntityStateChange AttackTargets(List<Entity> targets)
        {
            var stateChange = new EntityStateChange
            {
                Source = Source,
                CombatContext = CombatContext,
            };

            stateChange.AttackParameters = AttackParameters;
            stateChange.AppliedEffects.AddRange(AppliedEffects);

            foreach (var target in targets)
            {
                stateChange.Targets.Add(target);
            }
            return stateChange;
        }
    }

    public class AttackCapabilities
    {
        // Inputs
        public Entity Source;
        private Dictionary<CombatContext, CombatContextCapability> AttackCapabilitiesPerType = new Dictionary<CombatContext, CombatContextCapability>();
        private AttackCapabilities() { }

        public AttackCapabilities(Entity source, Item mainhandOverride = null)
        {
            this.Source = source;
            foreach (var enumVal in System.Enum.GetValues(typeof(CombatContext)))
            {
                var combatContext = (CombatContext)enumVal;
                if (combatContext == CombatContext.NotSet)
                {
                    continue;
                }
                AttackCapabilitiesPerType[combatContext] = new CombatContextCapability(Source, combatContext, mainhandOverride);
            }
        }

        public CombatContextCapability this[CombatContext inp]
        {
            get { return this.AttackCapabilitiesPerType[inp]; }
            set { this.AttackCapabilitiesPerType[inp] = value; }
        }
    }
}
