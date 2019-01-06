using System.Collections.Generic;

namespace Gamepackage
{
    public class AttackCapability
    {
        public Entity Source;
        public CombatContext CombatContext;
        public AttackTargetingType AttackTargetingType;
        public Item MainHand;
        public Item Ammo;
        public List<Effect> AppliedEffects = new List<Effect>();
        public AttackParameters AttackParameters;
        public int Range;
        public int NumberOfTargetsToPierce;
        public List<Point> CachedPointsInRange = null;
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
                    var hasAmmo = false;
                    if (hasRangedWeapon)
                    {
                        hasAmmo = Ammo != null && MainHand.AmmoType == Ammo.AmmoType;
                    }
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
            return PointsInRange().Contains(target);
        }
        public List<Point> PointsInExplosionRange(Point placementPosition)
        {
            List<Point> outputRange = new List<Point>();
            if (AttackParameters.ExplosionParameters != null)
            {
                outputRange.AddRange(Context.GameStateManager.Game.CurrentLevel.Grid[placementPosition].CachedFloorFloodFills[AttackParameters.ExplosionParameters.Radius]);
            }
            return outputRange;
        }
        public List<Point> PointsInRange()
        {
            if (CachedPointsInRange == null)
            {
                CachedPointsInRange = new List<Point>();
            }
            else
            {
                return CachedPointsInRange;
            }
            List<Point> outputRange = new List<Point>();
            if (AttackTargetingType == AttackTargetingType.Line)
            {
                outputRange.AddRange(MathUtil.LineInDirection(Source.Position, Direction.SouthEast, Range));
                outputRange.AddRange(MathUtil.LineInDirection(Source.Position, Direction.SouthWest, Range));
                outputRange.AddRange(MathUtil.LineInDirection(Source.Position, Direction.NorthEast, Range));
                outputRange.AddRange(MathUtil.LineInDirection(Source.Position, Direction.NorthWest, Range));
            }
            else if (AttackTargetingType == AttackTargetingType.SelectTarget)
            {
                outputRange.AddRange(Context.GameStateManager.Game.CurrentLevel.Grid[Source.Position].CachedFloorFloodFills[Range]);
            }
            else
            {
                throw new NotImplementedException("AttackTargetingType not implemented: " + AttackTargetingType);
            }
            CachedPointsInRange.AddRange(outputRange);
            return outputRange;
        }

        public bool HasAClearShot(Point target)
        {
            var canHitFromHere = Source.Position.IsOrthogonalTo(target) || AttackTargetingType != AttackTargetingType.Line;

            if (!canHitFromHere)
            {
                return false;
            }

            var distance = (int)Source.Position.Distance(target); // whole number bc grid coords
            var coordsToCheck = MathUtil.LineInDirection(Source.Position, MathUtil.RelativeDirection(Source.Position, target), distance);
            foreach (var point in coordsToCheck)
            {
                var game = Context.GameStateManager.Game;
                var level = game.CurrentLevel;
                if (point != target)
                {
                    if(level.Grid[point].TileType == TileType.Wall)
                    {
                        return false; // dont shoot through walls
                    }
                    var entitiesInPosition = level.Grid[point].EntitiesInPosition;
                    foreach (var entityInPosition in entitiesInPosition)
                    {
                        if (entityInPosition.IsCombatant && entityInPosition.Behaviour != null && Source.Behaviour != null && entityInPosition.Behaviour.ActingTeam == Source.Behaviour.ActingTeam)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public AttackCapability(Entity source, CombatContext combatContext, Item mainhandOverride = null)
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
                    AttackTargetingType = AttackParameters.AttackTargetingType;
                    AppliedEffects.AddRange(MainHand.Effects.FindAll(CombatUtil.AppliedEffects));
                    AppliedEffects.AddRange(AttackParameters.AttackSpecificEffects.FindAll(CombatUtil.AppliedEffects));
                }
                else
                {
                    Range = source.Body.MeleeRange;
                    NumberOfTargetsToPierce = source.Body.MeleeTargetsPierced;

                    AttackParameters = MathUtil.ChooseRandomElement<AttackParameters>(source.Body.MeleeParameters);
                    AttackTargetingType = AttackParameters.AttackTargetingType;
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
                        AttackTargetingType = rangedAttackParameters.AttackTargetingType,
                    };
                    AttackTargetingType = AttackTargetingType.Line;
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
                    AttackTargetingType = AttackParameters.AttackTargetingType;
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
                    AttackTargetingType = AttackParameters.AttackTargetingType;
                    NumberOfTargetsToPierce = MainHand.ZappedTargetsPierced;
                }
            }
            // If we fall through here we aren't dealing with a attack
        }

        public ActionOutcome PerformOnTarget(Entity target)
        {
            var stateChange = new ActionOutcome
            {
                Source = Source,
                CombatContext = CombatContext,
            };

            stateChange.AttackParameters = AttackParameters;
            stateChange.AppliedEffects.AddRange(AppliedEffects);

            stateChange.Target = target;
            return stateChange;
        }
    }

    public class AttackCapabilities
    {
        public Entity Source;
        public Item MainHandOverride = null;
        private AttackCapabilities() { }

        public AttackCapabilities(Entity source, Item mainhandOverride = null)
        {
            this.Source = source;
            this.MainHandOverride = mainhandOverride;
        }

        public AttackCapability this[CombatContext inp]
        {
            get { return new AttackCapability(Source, inp, MainHandOverride); }
        }
    }
}
