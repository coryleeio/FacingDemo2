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
        public GameObject ProjectilePrefab;
        public List<GameObject> OnHitPrefabs = new List<GameObject>();
        public List<GameObject> OnSwingPrefabs = new List<GameObject>();
        public List<Effect> AppliedEffects = new List<Effect>();
        public AttackParameters AttackParameters;
        public float ProjectileTravelTime;
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

                    ProjectilePrefab = MainHand.ItemAppearance.MeleeProjectilePrefab;
                    OnHitPrefabs.Add(MainHand.ItemAppearance.MeleeOnHitPrefab);
                    OnSwingPrefabs.Add(MainHand.ItemAppearance.MeleeOnSwingPrefab);
                    ProjectileTravelTime = MainHand.ItemAppearance.MeleeProjectileTravelTime;

                    AttackParameters = MathUtil.ChooseRandomElement<AttackParameters>(MainHand.MeleeParameters);
                    AppliedEffects.AddRange(MainHand.Effects.FindAll(CombatUtil.AppliedEffects));
                    AppliedEffects.AddRange(AttackParameters.AttackSpecificEffects.FindAll(CombatUtil.AppliedEffects));
                }
                else
                {
                    Range = source.Body.MeleeRange;
                    NumberOfTargetsToPierce = source.Body.MeleeTargetsPierced;

                    ProjectilePrefab = source.Body.MeleeProjectilePrefab;
                    OnHitPrefabs.Add(source.Body.MeleeOnHitPrefab);
                    OnSwingPrefabs.Add(source.Body.MeleeOnSwingPrefab);
                    ProjectileTravelTime = source.Body.MeleeProjectileTravelTime;

                    AttackParameters = MathUtil.ChooseRandomElement<AttackParameters>(source.Body.MeleeParameters);
                    AppliedEffects.AddRange(source.Body.Effects.FindAll(CombatUtil.AppliedEffects));
                    AppliedEffects.AddRange(AttackParameters.AttackSpecificEffects.FindAll(CombatUtil.AppliedEffects));
                }

            }

            else if (combatContext == CombatContext.Ranged)
            {
                if (MainHand != null && MainHand.CanBeUsedForZap)
                {
                    Range = MainHand.RangedRange;
                    NumberOfTargetsToPierce = MainHand.RangedTargetsPierced;

                    if (MainHand.AmmoType != AmmoType.None)
                    {
                        ProjectilePrefab = Ammo.ItemAppearance.RangedProjectilePrefab;
                        ProjectileTravelTime = Ammo.ItemAppearance.RangedProjectileTravelTime;
                    }
                    else
                    {
                        ProjectilePrefab = MainHand.ItemAppearance.RangedProjectilePrefab;
                        ProjectileTravelTime = MainHand.ItemAppearance.RangedProjectileTravelTime;

                    }

                    OnHitPrefabs.Add(MainHand.ItemAppearance.RangedOnHitPrefab);
                    OnSwingPrefabs.Add(MainHand.ItemAppearance.RangedOnSwingPrefab);
                    AttackParameters = MathUtil.ChooseRandomElement<AttackParameters>(MainHand.MeleeParameters);
                    AppliedEffects.AddRange(MainHand.Effects.FindAll(CombatUtil.AppliedEffects));
                    AppliedEffects.AddRange(AttackParameters.AttackSpecificEffects.FindAll(CombatUtil.AppliedEffects));
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

                    ProjectilePrefab = MainHand.ItemAppearance.ThrownProjectilePrefab;
                    OnHitPrefabs.Add(MainHand.ItemAppearance.ThrownOnHitPrefab);
                    OnSwingPrefabs.Add(MainHand.ItemAppearance.ThrownOnSwingPrefab);
                    ProjectileTravelTime = MainHand.ItemAppearance.ThrownProjectileTravelTime;
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

                    ProjectilePrefab = MainHand.ItemAppearance.ZappedProjectilePrefab;
                    OnHitPrefabs.Add(MainHand.ItemAppearance.ZappedOnHitPrefab);
                    OnSwingPrefabs.Add(MainHand.ItemAppearance.ZappedOnSwingPrefab);
                    ProjectileTravelTime = MainHand.ItemAppearance.ZappedProjectileTravelTime;
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
        private Entity Source;
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
