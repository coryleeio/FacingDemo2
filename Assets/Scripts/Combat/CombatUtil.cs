
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public static class CombatUtil
    {
        public static List<Entity> HittableEntitiesInPositionsOnLevel(Point point, Level level)
        {
            return HittableEntitiesInPositionsOnLevel(new List<Point>() { point }, level);
        }

        public static List<Entity> HittableEntitiesInPositionsOnLevel(List<Point> points, Level level)
        {
            List<Entity> entities = new List<Entity>();
            foreach (var point in points)
            {
                entities.AddRange(level.Grid[point].EntitiesInPosition.FindAll(Filters.HittableEntities));
            }
            return entities;
        }

        public static List<Effect> AppliedEffectsResolve(CombatActionType InteractionType, Item item)
        {
            List<Effect> appliedEffects = new List<Effect>();
            if (item == null)
            {
                // Simple damage will cause this.
                return appliedEffects;
            }
            if (item.IsEnchanted && item.Enchantment.Template.AppliedEffects.ContainsKey(InteractionType) && item.Enchantment.HasCharges)
            {
                appliedEffects.Add(EffectFactory.Build(item.Enchantment.Template.AppliedEffects[InteractionType]));
            }
            return appliedEffects;
        }

        public static Point EndpointOfAttack(Entity source, CombatActionType InteractionType, Item item, Point targetPosition, Direction direction, CombatActionParameters InteractionTypeParameters)
        {
            Point retval;
            if (InteractionType == CombatActionType.ApplyToSelf)
            {
                retval = source.Position;
            }
            else if (InteractionTypeParameters.TargetingType == CombatActionTargetingType.Line)
            {
                retval = CombatUtil.CalculateEndpointOfLineSkillshot(source.Position, InteractionTypeParameters, direction);
            }
            else if (InteractionTypeParameters.TargetingType == CombatActionTargetingType.SelectTarget)
            {
                retval = targetPosition;
            }
            else
            {
                throw new NotImplementedException();
            }
            return retval;
        }

        public static List<Point> PointsHitByAttack(Entity source, CombatActionType InteractionType, Item item, Point endpointOfAttack, Direction direction, CombatActionParameters InteractionTypeParameters)
        {
            var retVal = new List<Point>();
            if (InteractionTypeParameters.TargetingType == CombatActionTargetingType.Line)
            {
                var distance = (int)source.Position.Distance(endpointOfAttack);
                retVal.AddRange(MathUtil.LineInDirection(source.Position, direction, distance));
            }
            else if (InteractionTypeParameters.TargetingType == CombatActionTargetingType.SelectTarget)
            {
                retVal.Add(endpointOfAttack);
            }
            else
            {
                throw new NotImplementedException();
            }
            return retVal;
        }

        public static void ApplyAttackInstantly(CalculatedCombatAction calculated)
        {
            foreach (var stateChange in calculated.AttackStateChanges)
            {
                CombatUtil.ApplyEntityStateChange(stateChange);
            }
            foreach (var stateChange in calculated.ExplosionStateChanges)
            {
                CombatUtil.ApplyEntityStateChange(stateChange);
            }

            foreach (var stateChange in calculated.SourceStateChanges)
            {
                CombatUtil.ApplyEntityStateChange(stateChange);
            }

            CombatUtil.ApplyItemStateChanges(calculated);
            CombatUtil.ApplyGroundSpawnStateChange(calculated);
            if (Context.UIController)
            {
                Context.UIController.Refresh();
            }
        }

        public static CalculatedCombatAction CalculateSimpleDamage(Entity target, string i18nString, int damage, DamageTypes damageType)
        {
            var resolved = new ResolvedCombatActionDescriptor()
            {
                CombatActionParameters = new DerivedCombatActionParameters()
                {
                    Range = 1,
                    AttackMessagePrefix = i18nString,
                    ClusteringFactor = 1,
                    Damage = damage,
                    DamageType = damageType,
                    ProjectileAppearanceIdentifier = "PROJECTILE_APPEARANCE_NONE",
                    NumberOfTargetsToPierce = 1,
                    TargetingType = CombatActionTargetingType.SelectTarget,
                    InteractionProperties = new List<InteractionProperties>()
                    {
                        InteractionProperties.Unavoidable,
                    },
                },
                ExplosionParameters = null
            };
            var calculated = CombatUtil.CalculateAttack(Context.Game.CurrentLevel.Grid,
                    null,
                    CombatActionType.ApplyToOther,
                    null,
                    target.Position,
                    resolved
            );
            return calculated;
        }

        public static CalculatedCombatAction CalculateAttack(Grid<Tile> grid,
            Entity source, CombatActionType InteractionType, Item item, Point targetPosition,
            ResolvedCombatActionDescriptor InteractionTypeParameters)
        {
            CalculatedCombatAction calculatedAttack = new CalculatedCombatAction
            {
                Source = source,
                CombatActionType = InteractionType,
                Item = item,
                TargetPosition = targetPosition,
                ResolvedCombatActionParameters = InteractionTypeParameters,
                NumberOfTargetsPierced = 0,
            };
            if (source != null)
            {
                calculatedAttack.DirectionOfAttack = MathUtil.RelativeDirection(source.Position, targetPosition);
            }

            calculatedAttack.EndpointOfAttack = EndpointOfAttack(source, calculatedAttack.CombatActionType, item, targetPosition, calculatedAttack.DirectionOfAttack, calculatedAttack.ResolvedCombatActionParameters.CombatActionParameters);
            calculatedAttack.PointsPossiblyAffectedBeforeTargetPiercing.AddRange(PointsHitByAttack(source, InteractionType, item, calculatedAttack.EndpointOfAttack, calculatedAttack.DirectionOfAttack, calculatedAttack.ResolvedCombatActionParameters.CombatActionParameters));
            var entitiesInPositionsHitByAttack = new List<Entity>();

            foreach (var point in calculatedAttack.PointsPossiblyAffectedBeforeTargetPiercing)
            {
                var entitiesInThisPosition = grid[point].EntitiesInPosition.FindAll(Filters.HittableEntities);
                calculatedAttack.PointsAffectedByAttack.Add(point);
                if (entitiesInThisPosition.Count > 0)
                {
                    entitiesInPositionsHitByAttack.AddRange(entitiesInThisPosition);
                    calculatedAttack.NumberOfTargetsPierced++; // This means that it is really tiles pierced, but entities are rarely stacked
                    if (calculatedAttack.NumberOfTargetsPierced >= calculatedAttack.ResolvedCombatActionParameters.CombatActionParameters.NumberOfTargetsToPierce)
                    {
                        break;
                    }
                }
            }
            foreach (var target in entitiesInPositionsHitByAttack)
            {
                var blockPercent = Context.RulesEngine.CalculateBlockPercentage(calculatedAttack, target);
                var blocked = MathUtil.PercentageChanceEventOccurs(blockPercent);

                var toHitPercent = Context.RulesEngine.CalculateToHitPercentage(calculatedAttack, target);
                var hit = MathUtil.PercentageChanceEventOccurs(toHitPercent);

                var attackStateChange = new EntityStateChange
                {
                    Source = calculatedAttack.Source,
                    InteractionType = calculatedAttack.CombatActionType,
                    Target = target,
                    Item = calculatedAttack.Item,
                    TargetPositionOfInteraction = calculatedAttack.TargetPosition,
                    WasBlocked = blocked,
                    Missed = !hit,
                };
                attackStateChange.CombatActionParameters = calculatedAttack.ResolvedCombatActionParameters.CombatActionParameters;
                attackStateChange.AppliedEffects.AddRange(CombatUtil.AppliedEffectsResolve(InteractionType, item));
                calculatedAttack.AttackStateChanges.Add(attackStateChange);
                if (calculatedAttack.ResolvedCombatActionParameters.CombatActionParameters != null)
                {
                    CalculateDamageForCombatActionParameters(attackStateChange.CombatActionParameters, attackStateChange);
                }
                CombatUtil.CalculateAffectOutgoingAttack(calculatedAttack, attackStateChange);
                CombatUtil.CalculateAffectIncomingAttackEffects(calculatedAttack, attackStateChange);
                CombatUtil.CalculateBlockIncomingAttackEffects(calculatedAttack, attackStateChange);
                CalculateOnUseMessages(calculatedAttack, attackStateChange);
            }
            CalculateExplosion(grid, calculatedAttack, calculatedAttack.EndpointOfAttack);
            CalculateProjectileItemChanges(calculatedAttack);
            CalculateChargeItemChanges(calculatedAttack);
            CalculateCost(calculatedAttack);
            return calculatedAttack;
        }

        private static void CalculateCost(CalculatedCombatAction calculatedAttack)
        {
            if (calculatedAttack.ResolvedCombatActionParameters.Cost != null)
            {
                if (calculatedAttack.ResolvedCombatActionParameters.Cost.Health > 0)
                {
                    var costStateChange = new EntityStateChange
                    {
                        Source = calculatedAttack.Source,
                        InteractionType = CombatActionType.ApplyToOther,
                        Target = calculatedAttack.Source,
                        HealthChange = calculatedAttack.ResolvedCombatActionParameters.Cost.Health,
                        TargetPositionOfInteraction = calculatedAttack.Source.Position,
                        WasBlocked = false,
                        Missed = false,
                    };
                    costStateChange.LogMessages.AddLast(string.Format("cost.health.damage".Localize(), calculatedAttack.Item.Name.Localize(), calculatedAttack.ResolvedCombatActionParameters.Cost.Health, calculatedAttack.Source.Name));
                    calculatedAttack.SourceStateChanges.Add(costStateChange);
                }
            }
        }

        private static void CalculateOnUseMessages(CalculatedCombatAction calculatedAttack, EntityStateChange change)
        {
            if (calculatedAttack.CombatActionType == CombatActionType.ApplyToSelf && calculatedAttack.Item != null)
            {
                if (calculatedAttack.Source.IsPlayer)
                {
                    change.LogMessages.AddFirst(calculatedAttack.Item.Template.ApplyToSelfPlayerText.Localize());
                }
                else
                {
                    change.LogMessages.AddFirst(string.Format(calculatedAttack.Item.Template.ApplyToSelfOtherText.Localize(), calculatedAttack.Source.Name));
                }
            }
        }

        public static void ApplyGroundSpawnStateChange(CalculatedCombatAction calculatedAttack)
        {
            foreach (var groundDropStateChange in calculatedAttack.GroundDropsToSpawn)
            {
                var groundDrop = EntityFactory.Build("ENTITY_GROUND_DROP", Team.Neutral);
                groundDrop.Position = new Point(groundDropStateChange.SpawnPosition);
                groundDrop.Name = groundDropStateChange.Name;

                var level = Context.Game.CurrentLevel;
                Context.EntitySystem.Register(groundDrop, level);
                var itemCopy = ItemFactory.Build(groundDropStateChange.UniqueIdentifier);
                itemCopy.NumberOfItems = 1;

                InventoryUtil.AddItem(groundDrop, itemCopy);
                ViewFactory.RebuildView(groundDrop);
            }
        }

        private static Item GetItemBeingLaunched(CalculatedCombatAction calculatedAttack)
        {
            var ammo = CombatUtil.AmmoResolve(calculatedAttack.Source, calculatedAttack.CombatActionType, calculatedAttack.Item);
            return calculatedAttack.CombatActionType == CombatActionType.Ranged ? ammo : calculatedAttack.Item;
        }

        public static void ApplyItemStateChanges(CalculatedCombatAction calculatedAttack)
        {
            foreach (var itemStateChange in calculatedAttack.ItemStateChanges)
            {
                if (itemStateChange.NumberOfChargesConsumed > 0 && itemStateChange.Item.Enchantment != null)
                {
                    itemStateChange.Item.Enchantment.NumberOfChargesRemaining = itemStateChange.Item.Enchantment.NumberOfChargesRemaining - 1;
                }
                if (itemStateChange.NumberOfItemsConsumed > 0)
                {
                    itemStateChange.Item.NumberOfItems -= itemStateChange.NumberOfItemsConsumed;
                    if (itemStateChange.Item.NumberOfItems <= 0)
                    {
                        InventoryUtil.RemoveWholeItemStack(itemStateChange.Owner, itemStateChange.Item);
                    }
                }
            }
        }

        private static void CalculateChargeItemChanges(CalculatedCombatAction calculatedAttack)
        {
            // Handle the swinging weapon
            var item = calculatedAttack.Item;
            if (item != null && item.HasCharges)
            {
                var itemStateChange = new ItemStateChange();
                itemStateChange.Item = item;
                itemStateChange.Owner = calculatedAttack.Source;
                if (item.Template.DestroyOnUse)
                {
                    itemStateChange.NumberOfItemsConsumed = 1;
                }
                if (item.IsEnchanted && !item.Enchantment.HasUnlimitedCharges)
                {
                    itemStateChange.NumberOfChargesConsumed = 1;
                }
                calculatedAttack.ItemStateChanges.Add(itemStateChange);
            }
        }

        private static void CalculateProjectileItemChanges(CalculatedCombatAction calculatedAttack)
        {
            if (calculatedAttack.CombatActionType == CombatActionType.Ranged || calculatedAttack.CombatActionType == CombatActionType.Thrown)
            {
                Item itemBeingLaunched = GetItemBeingLaunched(calculatedAttack);
                var shouldSpawnItemOnGround = MathUtil.PercentageChanceEventOccurs(itemBeingLaunched.Template.ChanceToSurviveLaunch);
                if (shouldSpawnItemOnGround)
                {
                    var groundDropSpawn = new GroundDropSpawn();
                    groundDropSpawn.UniqueIdentifier = itemBeingLaunched.TemplateIdentifier;
                    groundDropSpawn.Name = itemBeingLaunched.Name;

                    var grid = Context.Game.CurrentLevel.Grid;
                    Point locationForSpawn = null;
                    if (grid[calculatedAttack.EndpointOfAttack].TileType == TileType.Wall)
                    {
                        // Spawn one tile back the way it came
                        var oppositeDirection = MathUtil.OppositeDirection(calculatedAttack.DirectionOfAttack);
                        var offset = MathUtil.OffsetForDirection(oppositeDirection);
                        locationForSpawn = MathUtil.GetPointByOffset(calculatedAttack.EndpointOfAttack, offset);
                    }
                    else
                    {
                        // Spawn exactly at endpoint
                        locationForSpawn = new Point(calculatedAttack.EndpointOfAttack.X, calculatedAttack.EndpointOfAttack.Y);
                    }

                    groundDropSpawn.SpawnPosition = new Point(locationForSpawn);
                    groundDropSpawn.NumberToSpawn = 1;
                    calculatedAttack.GroundDropsToSpawn.Add(groundDropSpawn);
                }
                var itemStateChange = new ItemStateChange()
                {
                    Owner = calculatedAttack.Source,
                    Item = itemBeingLaunched,
                    NumberOfItemsConsumed = 1
                };
                calculatedAttack.ItemStateChanges.Add(itemStateChange);
            }
        }

        public static void CalculateExplosion(Grid<Tile> grid, CalculatedCombatAction calculatedAttack, Point explosionPosition)
        {
            var explosionParams = calculatedAttack.ResolvedCombatActionParameters.ExplosionParameters;
            if (explosionParams != null)
            {
                Assert.IsNotNull(calculatedAttack.Source);
                Assert.IsNotNull(explosionPosition);
                Assert.IsTrue(explosionParams.Range > 0, "An explosion with a radius of 0 does nothing");

                var pointsHitOnAnyPass = new List<Point>();
                for (var index = 0; index <= explosionParams.Range; index++)
                {
                    EnsureExplosionDictsForRadius(calculatedAttack, index);

                    var pointsHitOnPass = calculatedAttack.ExplosionPointsByRadius[index];
                    var stateChangesOnPass = calculatedAttack.ExplosionStateChangesByRadius[index];

                    if (index == 0)
                    {
                        // The first tile in the flood fill might be a wall node if the shot hit the wall
                        // so dont filter for floor tiles, just add this node.
                        pointsHitOnPass.Add(explosionPosition);
                    }
                    else
                    {
                        var pointsInFloodfill = Context.Game.CurrentLevel.Grid[explosionPosition].CachedFloorFloodFills[index];
                        foreach (var point in pointsInFloodfill)
                        {
                            if (!pointsHitOnAnyPass.Contains(point))
                            {
                                pointsHitOnPass.Add(point);
                                pointsHitOnAnyPass.Add(point);
                            }
                        }

                    }
                    foreach (var pointHitOnPass in pointsHitOnPass)
                    {
                        var entitiesInPosition = grid[pointHitOnPass].EntitiesInPosition;
                        foreach (var entityInPosition in entitiesInPosition)
                        {
                            if (entityInPosition.IsCombatant)
                            {
                                var entityStateChange = new EntityStateChange
                                {
                                    Source = calculatedAttack.Source,
                                    InteractionType = calculatedAttack.CombatActionType,
                                    CombatActionParameters = explosionParams,
                                    Target = entityInPosition,
                                };
                                CalculateDamageForCombatActionParameters(explosionParams, entityStateChange);
                                if (calculatedAttack.Item != null)
                                {
                                    var item = calculatedAttack.Item;
                                    entityStateChange.AppliedEffects.AddRange(CombatUtil.AppliedEffectsResolve(CombatActionType.Explosion, item));
                                }

                                stateChangesOnPass.Add(entityStateChange);
                                calculatedAttack.ExplosionStateChanges.Add(entityStateChange);
                            }
                        }
                    }

                }
            }
        }

        private static void CalculateDamageForCombatActionParameters(CombatActionParameters attackParameters, EntityStateChange attackStateChange)
        {
            for (var numDyeRolled = 0; numDyeRolled < attackParameters.ClusteringFactor; numDyeRolled++)
            {
                attackStateChange.HealthChange += UnityEngine.Random.Range(1, attackParameters.Damage + 1);
            }

            if (attackParameters.DamageType == DamageTypes.HEALING)
            {
                attackStateChange.HealthChange *= -1;
            }
        }

        private static void EnsureExplosionDictsForRadius(CalculatedCombatAction calculatedAttack, int index)
        {
            if (!calculatedAttack.ExplosionPointsByRadius.ContainsKey(index))
            {
                calculatedAttack.ExplosionPointsByRadius[index] = new List<Point>();
            }
            if (!calculatedAttack.ExplosionStateChangesByRadius.ContainsKey(index))
            {
                calculatedAttack.ExplosionStateChangesByRadius[index] = new List<EntityStateChange>();
            }
        }

        public static ResolvedCombatActionDescriptor CombatActionParametersResolve(Entity source, CombatActionType InteractionType, Item item)
        {
            Assert.IsNotNull(item);
            var resolved = new ResolvedCombatActionDescriptor();
            if (item.CanBeUsedInInteractionType(InteractionType))
            {
                if (item.CombatActionDescriptor.ContainsKey(InteractionType))
                {
                    Flatten(resolved, item.CombatActionDescriptor[InteractionType]);
                }
                if (item.IsEnchanted && item.Enchantment.Template.CombatActionDescriptor.ContainsKey(InteractionType))
                {
                    if (InteractionType == CombatActionType.Melee || InteractionType == CombatActionType.Ranged)
                    {
                        // for melee and ranged we want the weapon damage, but the effects from the enchantment.
                        // we got the weapon damage above, then we get the explosion and ability costs if-any from the enchantment
                        resolved.Cost = item.Enchantment.Template.CombatActionDescriptor[InteractionType].Cost;
                        resolved.ExplosionParameters = item.Enchantment.Template.CombatActionDescriptor[InteractionType].ExplosionParameters;
                    }
                    else
                    {
                        // for most things we dumb copy the params, of the enchantment onto the params 
                        // of the item
                        Flatten(resolved, item.Enchantment.Template.CombatActionDescriptor[InteractionType]);
                    }

                }
            }
            return resolved;
        }

        // The same as CombatActionDescriptor but resolved from multiple sources.
        public static ResolvedCombatActionDescriptor Flatten(ResolvedCombatActionDescriptor res, CombatActionDescriptor inp)
        {
            // lay one over the other
            if (inp.CombatActionParameters != null)
            {
                res.CombatActionParameters = new DerivedCombatActionParameters(inp.CombatActionParameters);
            }

            if (inp.ExplosionParameters != null)
            {
                res.ExplosionParameters = inp.ExplosionParameters;
            }

            if (inp.Cost != null)
            {
                res.Cost = inp.Cost;
            }
            return res;
        }


        public static void ShowMessages(EntityStateChange result)
        {
            foreach (var msg in result.LogMessages)
            {
                Context.UIController.TextLog.AddText(msg);
            }
            foreach (var msg in result.LateMessages)
            {
                Context.UIController.TextLog.AddText(msg);
            }
            foreach (var msg in result.FloatingTextMessage)
            {
                ShowFloatingMessage(result.Target.Position, msg);
            }

            // Because in some circumstances this can be caused twice, clear the buffer.
            result.LogMessages.Clear();
            result.LateMessages.Clear();
            result.FloatingTextMessage.Clear();
        }

        public static void ShowFloatingMessage(Point pos, FloatingTextMessage msg)
        {
            if (pos != null && msg != null && Context.UIController != null && Context.UIController.FloatingCombatTextManager != null)
            {
                Context.UIController.FloatingCombatTextManager.ShowCombatText(msg.Message, msg.Color, msg.FontSize, MathUtil.MapToWorld(pos), msg.AllowLeftRightDrift);
            }
        }

        public static void ApplyEntityStateChange(EntityStateChange result)
        {
            Assert.IsNotNull(result.Target);
            Assert.IsNotNull(result.AppliedEffects);
            var target = result.Target;

            Assert.IsNotNull(target);
            if (result.Source != null)
            {
                var newTargetingDirection = MathUtil.RelativeDirection(target.Position, result.Source.Position);
                target.Direction = newTargetingDirection;
                if (target.SkeletonAnimation != null)
                {
                    var skeletonAnimation = target.SkeletonAnimation;
                    skeletonAnimation.AnimationState.ClearTracks();
                    skeletonAnimation.Skeleton.SetToSetupPose();
                    skeletonAnimation.AnimationState.SetAnimation(0, DisplayUtil.GetAnimationNameForDirection(Animations.GetHit, newTargetingDirection), false);
                    skeletonAnimation.AnimationState.AddAnimation(0, DisplayUtil.GetAnimationNameForDirection(Animations.Idle, newTargetingDirection), true, 0.0f);
                }
            }
            if (result.CombatActionParameters != null && result.CombatActionParameters.DamageType == DamageTypes.NOT_SET)
            {
                throw new NotImplementedException("You forgot to set the damage type on this attack");
            }

            if (!target.IsCombatant)
            {
                return;
            }

            if (target.CurrentHealth <= 0)
            {
                // if you keep hitting him he doesn't get dead-er..
                return;
            }


            var sourceName = "";
            if (result.Source != null)
            {
                sourceName = result.Source.Name;
            }
            var targetName = target.Name;

            if (result.Missed)
            {
                result.LogMessages.AddLast(string.Format(result.CombatActionParameters.AttackMissMessage.Localize(), sourceName, targetName));
                result.FloatingTextMessage.AddLast(new FloatingTextMessage()
                {
                    Message = "attacks.miss.floating.text".Localize(),
                    Color = DisplayUtil.DamageDisplayColor(true),
                    target = target,
                    AllowLeftRightDrift = true,
                });
            }
            else if (result.WasShortCircuited)
            {
                result.LogMessages.AddLast(string.Format(result.CombatActionParameters.AttackBlockedMessage.Localize(), sourceName, targetName, result.HealthChange, DisplayUtil.DamageTypeToDisplayString(result.CombatActionParameters.DamageType)));
            }
            else if (result.WasBlocked)
            {
                result.LogMessages.AddLast(string.Format(result.CombatActionParameters.AttackBlockedMessage.Localize(), sourceName, targetName));
                result.FloatingTextMessage.AddLast(new FloatingTextMessage()
                {
                    Message = "attacks.block.floating.text".Localize(),
                    Color = DisplayUtil.DamageDisplayColor(true),
                    target = target,
                    AllowLeftRightDrift = true,
                });
            }
            else
            {
                if (result.HealthChange != 0)
                {
                    if (result.CombatActionParameters != null)
                    {
                        result.LogMessages.AddLast(string.Format(result.CombatActionParameters.AttackHitMessage.Localize(), sourceName, targetName, result.HealthChange < 0 ? result.HealthChange * -1 : result.HealthChange, DisplayUtil.DamageTypeToDisplayString(result.CombatActionParameters.DamageType)));
                    }
                    target.CurrentHealth = target.CurrentHealth - result.HealthChange;
                    var healthChangeDisplay = result.HealthChange > 0 ? "-" : "+";
                    healthChangeDisplay += Math.Abs(result.HealthChange).ToString();
                    Color healthChangeColor = DisplayUtil.DamageDisplayColor(result.HealthChange < 0);
                    result.FloatingTextMessage.AddLast(new FloatingTextMessage()
                    {
                        Message = string.Format("{0}", healthChangeDisplay),
                        Color = healthChangeColor,
                        target = target,
                        AllowLeftRightDrift = true,
                    });

                    // Go ahead and show messages so that onApply messages appear in the correct order
                    // onapply lacks a combat context so its messages appear immediately
                    // We do this here so that the messages appear before the effect messages
                    ShowMessages(result);
                }

                HandleAppliedEffects(result);
                HandleRemovedEffects(result);

                if (target.IsNPC)
                {
                    if ((result.AppliedEffects.Count > 0 || result.HealthChange > 0) && result.Target.IsCombatant && result.Source != null)
                    {
                        var level = Context.Game.CurrentLevel;
                        AIUtil.ShoutAboutHostileTarget(result.Target, level, result.Source, result.Target.CalculateValueOfAttribute(Attributes.ShoutRadius));
                        if (result.Target.LastKnownTargetPosition == null)
                        {
                            result.Target.LastKnownTargetPosition = new Point(result.Source.Position);
                        }
                    }
                }
                CombatUtil.CapAttributes(target);

                if (target.CurrentHealth <= 0)
                {
                    var player = Context.Game.CurrentLevel.Player;
                    if (player != null && !target.IsPlayer)
                    {
                        var xp = Context.RulesEngine.CalculateXpForKill(result, target);
                        player.Xp += xp;

                        var campaignTemplate = Context.Game.CampaignTemplate;
                        int.TryParse(campaignTemplate.Settings[Settings.MaxLevel.ToString()], out int maxLevel);

                        if (player.Level < maxLevel)
                        {
                            var nextLevel = player.Level + 1;
                            var xpNeededForNextLevel = campaignTemplate.XpForLevel[nextLevel];

                            if(player.Xp >= xpNeededForNextLevel)
                            {
                                // We are over the threshold for atleast the next level
                                // figure out what level to stop at
                                for (var curItrLevel = nextLevel; curItrLevel <= maxLevel; curItrLevel++)
                                {
                                    var xpNeededForItrLevel = campaignTemplate.XpForLevel[curItrLevel];
                                    if (player.Xp >= xpNeededForItrLevel)
                                    {
                                        player.Level += 1;
                                        result.LogMessages.AddLast(string.Format("level.up".Localize(), player.Level));
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }


                            if (player.Xp > xpNeededForNextLevel)
                            {
                                player.Level += 1;
                            }
                        }
                        result.LogMessages.AddLast("xp.gain".Localize());
                    }

                    if (target.SkeletonAnimation != null)
                    {
                        var skeletonAnimation = target.SkeletonAnimation;
                        skeletonAnimation.AnimationState.ClearTracks();
                        skeletonAnimation.Skeleton.SetToSetupPose();
                        skeletonAnimation.AnimationState.SetAnimation(0, DisplayUtil.GetAnimationNameForDirection(Animations.FallDown, target.Direction), false);
                    }
                    result.FloatingTextMessage.AddLast(new FloatingTextMessage()
                    {
                        Message = string.Format("Dead!", result.HealthChange),
                        Color = Color.black,
                        target = target,
                        AllowLeftRightDrift = false,
                    });
                    result.LogMessages.AddLast(string.Format("{0} has been slain!", targetName));
                    target.Name = string.Format("( Corpse ) {0}", target.Name);
                    target.IsDead = true;
                    // Is not deregistered because the corpse should still be available
                    target.IsCombatant = false;
                    var game = Context.Game;
                    var level = game.CurrentLevel;

                    foreach (var pair in target.Inventory.EquippedItemBySlot)
                    {
                        var item = pair.Value;
                        if (item.IsEnchanted)
                        {
                            foreach (var effect in item.Enchantment.WornEffects)
                            {
                                effect.EffectImpl.RemovePersistantVisualEffects(target);
                            }
                        }
                    }

                    if (target.BlocksPathing)
                    {
                        // You cant deregister to ReleasePathfindingAtPosition bc we want to keep the corpse
                        level.ReleasePathfindingAtPosition(target, target.Position);
                        target.BlocksPathing = false;
                    }

                    if (target.ViewGameObject != null)
                    {
                        var deathAnim = target.ViewGameObject.AddComponent<DeathAnimation>();
                        if (target.HealthBar != null)
                        {
                            GameObject.Destroy(target.HealthBar.gameObject);
                        }
                    }

                    if (InventoryUtil.HasAnyItems(target))
                    {
                        target.ViewTemplateIdentifier = "VIEW_CORPSE";
                        ViewFactory.RebuildView(target, false);
                        var sortable = target.ViewGameObject.GetComponent<Sortable>();
                        sortable.Position = target.Position;
                        Context.SpriteSortingSystem.Register(sortable);

                        target.Trigger = TriggerFactory.Build("TRIGGER_LOOTABLE");
                    }
                    if (!target.IsPlayer)
                    {
                        game.MonstersKilled++;
                    }
                }

                if (target.IsPlayer && Context.UIController.InventoryWindow.isActiveAndEnabled)
                {
                    Context.UIController.Refresh();
                }
            }
            ShowMessages(result);
        }

        public static List<Effect> GetWornEffectsFromInventory(Entity entity)
        {
            var effectsAggregate = new List<Effect>();
            if (entity.Inventory != null)
            {
                foreach (var pair in entity.Inventory.EquippedItemBySlot)
                {
                    var item = pair.Value;
                    if (item.IsEnchanted)
                    {
                        effectsAggregate.AddRange(item.Enchantment.WornEffects);
                    }
                }

                foreach (var item in entity.Inventory.Items)
                {
                    if (item != null)
                    {
                        if (item.Template.TagsThatDescribeThisItem.Contains("ItemEffectsApplyFromInventory"))
                        {
                            if (item.IsEnchanted)
                            {
                                effectsAggregate.AddRange(item.Enchantment.WornEffects);
                            }
                        }
                    }
                }
            }
            return effectsAggregate;
        }

        public static List<Effect> GetEntityAllTypesOfEffects(Entity entity, Predicate<Effect> filter = null)
        {
            var effectsAggregate = new List<Effect>();
            effectsAggregate.AddRange(GetWornEffectsFromInventory(entity));

            if (entity.IsCombatant)
            {
                effectsAggregate.AddRange(entity.TemporaryEffects);
            }
            if (filter == null)
            {
                return effectsAggregate;
            }
            else
            {
                return effectsAggregate.FindAll(filter);
            }
        }

        // Aggregate all the tags an entity has applied to it via itself, items, and effects.
        // does not include tags that describe the specific item or effect, only the tags
        // applied to the entity itself.
        public static List<string> GetTagsOnEntity(Entity entity)
        {
            var aggregate = new List<string>();
            aggregate.AddRange(entity.EntityInnateTags);
            aggregate.AddRange(entity.EntityAcquiredTags);
            var items = InventoryUtil.GetEquippedItems(entity);
            foreach (var item in items)
            {
                aggregate.AddRange(item.Template.TagsAppliedToEntity);
            }
            var effects = entity.GetAllTypesOfEffects();
            foreach (var effect in effects)
            {
                aggregate.AddRange(effect.Template.TagsAppliedToEntity);
            }
            return aggregate;
        }

        public static void RemoveEntityEffects(Entity entity, List<Effect> effectsThatShouldExpire)
        {
            Predicate<Effect> IsAnAffectThatShouldExpire = (eff) => { return effectsThatShouldExpire.Contains(eff); };
            if (entity.IsCombatant)
            {
                var expiring = entity.TemporaryEffects.FindAll(IsAnAffectThatShouldExpire);
                foreach (var expire in expiring)
                {
                    expire.EffectImpl.OnRemove(expire, entity);
                }
                entity.TemporaryEffects.RemoveAll(IsAnAffectThatShouldExpire);
            }
        }

        private static void HandleRemovedEffects(EntityStateChange outcome)
        {
            if (!outcome.WasShortCircuited)
            {
                foreach (var effect in outcome.RemovedEffects)
                {
                    RemoveEntityEffects(outcome.Target, outcome.RemovedEffects);
                }
            }
        }

        public static void CapAttributes(Entity entity)
        {
            if (entity.IsCombatant)
            {
                if (entity.CurrentHealth > entity.CalculateValueOfAttribute(Gamepackage.Attributes.MaxHealth))
                {
                    entity.CurrentHealth = entity.CalculateValueOfAttribute(Gamepackage.Attributes.MaxHealth);
                }
            }
        }

        private static void HandleAppliedEffects(EntityStateChange outcome)
        {
            if (!outcome.WasShortCircuited)
            {
                foreach (var effect in outcome.AppliedEffects)
                {
                    effect.EffectImpl.HandleStacking(effect, outcome);
                }
            }
        }

        public static void DoStepTriggersForMover(Entity Mover)
        {
            var stepTriggers = Context.Game.CurrentLevel.Entitys.FindAll(Filters.StepTriggers);
            foreach (var trigger in stepTriggers)
            {
                var points = MathUtil.GetPointsByOffset(trigger.Position, trigger.Trigger.Offsets);

                if (points.Contains(Mover.Position))
                {
                    trigger.Trigger.Perform(Mover, trigger);
                }
            }
        }

        public static bool CanAttackWithItem(Entity source, CombatActionType InteractionType, Item item)
        {
            if (InteractionType == CombatActionType.NotSet)
            {
                return false;
            }
            var ammo = AmmoResolve(source, InteractionType, item);
            var hasBody = source.IsCombatant && CanAttackInMeleeWithoutWeapon(source);
            if (InteractionType == CombatActionType.Melee)
            {
                var hasWeapon = hasBody && source.Inventory != null && item != null && item.CanBeUsedInInteractionType(CombatActionType.Melee);
                return hasBody || hasWeapon;
            }
            else if (InteractionType == CombatActionType.Ranged)
            {
                var hasRangedWeapon = hasBody && source.Inventory != null && item != null && item.CanBeUsedInInteractionType(CombatActionType.Ranged);
                var hasAmmo = false;
                if (hasRangedWeapon)
                {
                    hasAmmo = ammo != null && item.Template.AmmoType == ammo.Template.AmmoType;
                }
                return hasRangedWeapon && hasAmmo;
            }
            else if (InteractionType == CombatActionType.Thrown)
            {
                return hasBody && source.Inventory != null && item != null && item.CanBeUsedInInteractionType(CombatActionType.Thrown);
            }
            else if (InteractionType == CombatActionType.Zapped)
            {
                return hasBody && source.Inventory != null && item != null && item.CanBeUsedInInteractionType(CombatActionType.Zapped);
            }
            else if (InteractionType == CombatActionType.ApplyToSelf || InteractionType == CombatActionType.ApplyToOther)
            {
                return true;
            }
            else
            {
                throw new NotImplementedException("Unimplemented attack type");
            }
        }

        public static Item AmmoResolve(Entity source, CombatActionType InteractionType, Item item)
        {
            if (source == null || InteractionType != CombatActionType.Ranged || source.Inventory == null)
            {
                return null;
            }
            return InventoryUtil.GetItemBySlot(source, ItemSlot.Ammo);
        }

        public static List<Point> PointsInRange(Grid<Tile> grid, Point position, int range, CombatActionTargetingType interactionTargetingType)
        {
            List<Point> outputRange = new List<Point>();
            if (interactionTargetingType == CombatActionTargetingType.Line)
            {
                outputRange.AddRange(MathUtil.LineInDirection(position, Direction.SouthEast, range));
                outputRange.AddRange(MathUtil.LineInDirection(position, Direction.SouthWest, range));
                outputRange.AddRange(MathUtil.LineInDirection(position, Direction.NorthEast, range));
                outputRange.AddRange(MathUtil.LineInDirection(position, Direction.NorthWest, range));
            }
            else if (interactionTargetingType == CombatActionTargetingType.SelectTarget)
            {
                outputRange.AddRange(grid[position].CachedFloorFloodFills[range]);
            }
            else
            {
                throw new NotImplementedException("AttackTargetingType not implemented: " + interactionTargetingType);
            }
            outputRange.AddRange(outputRange);
            return outputRange;
        }

        public static bool InRangeOfAttack(Grid<Tile> grid, Point position, int range, CombatActionTargetingType attackTargetingType, Point target)
        {
            var pointsInRange = PointsInRange(grid, position, range, attackTargetingType);
            return pointsInRange.Contains(target);
        }

        public static bool HasAClearShot(Entity source, CombatActionTargetingType attackTargetingType, Point target)
        {
            var canHitFromHere = source.Position.IsOrthogonalTo(target) || attackTargetingType != CombatActionTargetingType.Line;

            if (!canHitFromHere)
            {
                return false;
            }

            var distance = (int)source.Position.Distance(target); // whole number bc grid coords
            var coordsToCheck = MathUtil.LineInDirection(source.Position, MathUtil.RelativeDirection(source.Position, target), distance);
            foreach (var point in coordsToCheck)
            {
                var game = Context.Game;
                var level = game.CurrentLevel;
                if (point != target)
                {
                    if (level.Grid[point].TileType == TileType.Wall)
                    {
                        return false; // dont shoot through walls
                    }
                    var entitiesInPosition = level.Grid[point].EntitiesInPosition;
                    foreach (var entityInPosition in entitiesInPosition)
                    {
                        if (entityInPosition.IsCombatant && entityInPosition.IsCombatant && source.IsCombatant && entityInPosition.ActingTeam == source.ActingTeam)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        public static bool CanAttackInMeleeWithoutWeapon(Entity source)
        {
            return source.IsCombatant && source.DefaultAttackItem.CombatActionDescriptor.ContainsKey(CombatActionType.Melee);
        }

        public static List<Point> PointsInExplosionRange(CombatActionParameters ExplosionParameters, Point placementPosition)
        {
            List<Point> outputRange = new List<Point>();
            if (ExplosionParameters != null)
            {
                outputRange.AddRange(Context.Game.CurrentLevel.Grid[placementPosition].CachedFloorFloodFills[ExplosionParameters.Range]);
            }
            return outputRange;
        }

        public static void CalculateAffectOutgoingAttack(CalculatedCombatAction calculatedAttack, EntityStateChange change)
        {
            if (change.Source != null)
            {
                var sourceEffects = change.Source.GetAllTypesOfEffects();
                foreach (var effect in sourceEffects)
                {
                    effect.EffectImpl.CalculateAffectOutgoingAttack(effect, calculatedAttack, change);
                }
            }
        }

        public static void CalculateAffectIncomingAttackEffects(CalculatedCombatAction calculatedAttack, EntityStateChange change)
        {
            var targetEffects = change.Target.GetAllTypesOfEffects();
            foreach (var effect in targetEffects)
            {
                effect.EffectImpl.CalculateAffectIncomingAttackEffects(effect, calculatedAttack, change);
            }
        }
        public static void CalculateBlockIncomingAttackEffects(CalculatedCombatAction calculatedAttack, EntityStateChange change)
        {
            var tagsTargetHas = change.Target.Tags;
            var effectsBlocked = new List<Effect>();
            foreach (var effect in change.AppliedEffects)
            {
                foreach (var tagThatMayBlockThisEffect in effect.Template.TagsThatBlockThisEffect)
                {
                    if (tagsTargetHas.Contains(tagThatMayBlockThisEffect))
                    {
                        effectsBlocked.Add(effect);
                    }
                }
            }
            foreach (var effectBlocked in effectsBlocked)
            {
                change.AppliedEffects.Remove(effectBlocked);
                change.LateMessages.AddLast(string.Format((effectBlocked.Template.LocalizationPrefix + ".block").Localize(), change.Target.Name));
            }
        }

        public static void ShortCiruitAttack(EntityStateChange change)
        {
            change.HealthChange = 0;
            change.WasShortCircuited = true;
            change.LogMessages.Clear();
            change.LateMessages.Clear();
            change.FloatingTextMessage.Clear();
            change.AppliedEffects.Clear();
            change.RemovedEffects.Clear();
        }

        public static Point[] TriggerShapeToOffsets(TriggerShape shape)
        {
            if (shape == TriggerShape.Diagonal)
            {
                return MathUtil.DiagonalOffsets;
            }
            else if (shape == TriggerShape.Orthogonal)
            {
                return MathUtil.OrthogonalOffsets;
            }
            else if (shape == TriggerShape.OrthogonalOrDiagonal)
            {
                return MathUtil.SurroundingOffsets;
            }
            else if (shape == TriggerShape.SingleSquare)
            {
                return new Point[] { new Point(0, 0) };
            }
            else throw new NotImplementedException();
        }

        public static Point CalculateEndpointOfLineSkillshot(Point position, CombatActionParameters InteractionTypeParameters, Direction direction)
        {
            var pointsInLine = MathUtil.LineInDirection(position, direction, InteractionTypeParameters.Range);
            var numberOfThingsCanPierce = InteractionTypeParameters.NumberOfTargetsToPierce;
            var numberOfThingsPierced = 0;

            var game = Context.Game;
            var level = game.CurrentLevel;
            var grid = level.Grid;
            var previouslyTraversedPoint = position;
            foreach (var point in pointsInLine)
            {
                if (grid[point].EntitiesInPosition.FindAll(Filters.HittableEntities).Count > 0)
                {
                    numberOfThingsPierced += 1;
                }
                if (numberOfThingsPierced == numberOfThingsCanPierce)
                {
                    return point;
                }
                if (grid[point].TileType != TileType.Floor)
                {
                    return point;
                }
                previouslyTraversedPoint = point;
            }
            return pointsInLine[pointsInLine.Count - 1];
        }
    }
}
