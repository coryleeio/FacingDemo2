
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

        public static List<Effect> AppliedEffectsResolve(Entity source, AttackType attackType, Item item, AttackTypeParameters attackTypeParameters, AttackParameters attackParameters)
        {
            List<Effect> appliedEffects = new List<Effect>();
            if (attackTypeParameters == null)
            {
                return appliedEffects;
            }
            var ammo = CombatUtil.AmmoResolve(source, attackType, item);
            if (item != null)
            {
                appliedEffects.AddRange(item.EffectsGlobal.FindAll(Filters.AppliedEffects));
            }
            appliedEffects.AddRange(attackParameters.AttackSpecificEffects.FindAll(Filters.AppliedEffects));
            if (attackType == AttackType.Ranged)
            {
                var ammoAttackParameters = MathUtil.ChooseRandomElement<AttackParameters>(ammo.AttackTypeParameters[AttackType.Ranged].AttackParameters);
                appliedEffects.AddRange(ammo.EffectsGlobal.FindAll(Filters.AppliedEffects));
                appliedEffects.AddRange(ammoAttackParameters.AttackSpecificEffects.FindAll(Filters.AppliedEffects));
            }
            return appliedEffects;
        }

        public static Point EndpointOfAttack(Entity source, AttackType attackType, Item item, Point targetPosition, Direction direction, AttackTypeParameters attackTypeParameters)
        {
            Point retval;
            if (attackTypeParameters.AttackTargetingType == AttackTargetingType.Line)
            {
                retval = CombatUtil.CalculateEndpointOfLineSkillshot(source.Position, attackTypeParameters, direction);
            }
            else if (attackTypeParameters.AttackTargetingType == AttackTargetingType.SelectTarget)
            {
                retval = targetPosition;
            }
            else
            {
                throw new NotImplementedException();
            }
            return retval;
        }

        public static List<Point> PointsHitByAttack(Entity source, AttackType attackType, Item item, Point endpointOfAttack, Direction direction, AttackTypeParameters attackTypeParameters)
        {
            var retVal = new List<Point>();
            if (attackTypeParameters.AttackTargetingType == AttackTargetingType.Line)
            {
                var distance = (int)source.Position.Distance(endpointOfAttack);
                retVal.AddRange(MathUtil.LineInDirection(source.Position, direction, distance));
            }
            else if (attackTypeParameters.AttackTargetingType == AttackTargetingType.SelectTarget)
            {
                retVal.Add(endpointOfAttack);
            }
            else
            {
                throw new NotImplementedException();
            }
            return retVal;
        }

        public static void ApplyAttackInstantly(CalculatedAttack calculated)
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

        public static CalculatedAttack CalculateSimpleDamage(Entity target, string i18nString, int damage, DamageTypes damageType)
        {
            var attackTypeParameters = new AttackTypeParameters()
            {
                Range = 1,
                AttackParameters = new List<AttackParameters>() {
                    new AttackParameters()
                    {
                            AttackMessage = i18nString.Localize(),
                            Bonus = 0,
                            DyeNumber = 1,
                            DyeSize = damage,
                            DamageType = damageType,
                            ProjectileAppearanceIdentifier = UniqueIdentifier.PROJECTILE_APPEARANCE_NONE,
                    }
                },
                NumberOfTargetsToPierce = 1,
                AttackTargetingType = AttackTargetingType.SelectTarget,
            };

            var calculated = CombatUtil.CalculateAttack(Context.Game.CurrentLevel.Grid,
                    null,
                    AttackType.ApplyToOther,
                    null,
                    target.Position,
                    attackTypeParameters,
                    attackTypeParameters.AttackParameters[0]
            );
            return calculated;
        }

        public static CalculatedAttack CalculateAttack(Grid<Tile> grid,
            Entity source, AttackType attackType, Item item, Point targetPosition,
            AttackTypeParameters attackTypeParameters,
            AttackParameters attackParameters)
        {
            CalculatedAttack calculatedAttack = new CalculatedAttack
            {
                Source = source,
                AttackType = attackType,
                Item = item,
                TargetPosition = targetPosition,
                AttackParameters = attackParameters,
                AttackTypeParameters = attackTypeParameters,
                NumberOfTargetsPierced = 0,
            };
            if (source != null)
            {
                calculatedAttack.DirectionOfAttack = MathUtil.RelativeDirection(source.Position, targetPosition);
            }

            calculatedAttack.EndpointOfAttack = EndpointOfAttack(source, calculatedAttack.AttackType, item, targetPosition, calculatedAttack.DirectionOfAttack, calculatedAttack.AttackTypeParameters);
            calculatedAttack.PointsPossiblyAffectedBeforeTargetPiercing.AddRange(PointsHitByAttack(source, attackType, item, calculatedAttack.EndpointOfAttack, calculatedAttack.DirectionOfAttack, calculatedAttack.AttackTypeParameters));
            var entitiesToHit = new List<Entity>();

            foreach (var point in calculatedAttack.PointsPossiblyAffectedBeforeTargetPiercing)
            {
                var entitiesInThisPosition = grid[point].EntitiesInPosition.FindAll(Filters.HittableEntities);
                calculatedAttack.PointsAffectedByAttack.Add(point);
                if (entitiesInThisPosition.Count > 0)
                {
                    entitiesToHit.AddRange(entitiesInThisPosition);
                    calculatedAttack.NumberOfTargetsPierced++; // This means that it is really tiles pierced, but entities are rarely stacked
                    if (calculatedAttack.NumberOfTargetsPierced >= calculatedAttack.AttackTypeParameters.NumberOfTargetsToPierce)
                    {
                        break;
                    }
                }
            }
            foreach (var target in entitiesToHit)
            {
                var attackStateChange = new EntityStateChange
                {
                    Source = calculatedAttack.Source,
                    AttackType = calculatedAttack.AttackType,
                    Target = target,
                    AttackingItem = calculatedAttack.Item,
                    TargetPositionOfAttack = calculatedAttack.TargetPosition,
                };
                attackStateChange.AttackParameters = calculatedAttack.AttackParameters;
                attackStateChange.AppliedEffects.AddRange(CombatUtil.AppliedEffectsResolve(source, attackType, item, calculatedAttack.AttackTypeParameters, calculatedAttack.AttackParameters));
                calculatedAttack.AttackStateChanges.Add(attackStateChange);
                if (calculatedAttack.AttackParameters != null)
                {
                    CalculateDamageForAttackParameters(attackStateChange.AttackParameters, attackStateChange);
                }
                CombatUtil.CalculateAffectOutgoingAttack(calculatedAttack, attackStateChange);
                CombatUtil.CalculateAffectIncomingAttackEffects(calculatedAttack, attackStateChange);
            }
            CalculateExplosion(grid, calculatedAttack, calculatedAttack.EndpointOfAttack);
            CalculateProjectileItemChanges(calculatedAttack);
            CalculateChargeItemChanges(calculatedAttack);
            return calculatedAttack;
        }


        public static void ApplyGroundSpawnStateChange(CalculatedAttack calculatedAttack)
        {
            foreach (var groundDropStateChange in calculatedAttack.GroundDropsToSpawn)
            {
                var groundDrop = EntityFactory.Build(UniqueIdentifier.ENTITY_GROUND_DROP);
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

        private static Item GetItemBeingLaunched(CalculatedAttack calculatedAttack)
        {
            var ammo = CombatUtil.AmmoResolve(calculatedAttack.Source, calculatedAttack.AttackType, calculatedAttack.Item);
            return calculatedAttack.AttackType == AttackType.Ranged ? ammo : calculatedAttack.Item;
        }

        public static void ApplyItemStateChanges(CalculatedAttack calculatedAttack)
        {
            foreach (var itemStateChange in calculatedAttack.ItemStateChanges)
            {
                if (itemStateChange.NumberOfChargesConsumed > 0)
                {
                    itemStateChange.Item.ExactNumberOfChargesRemaining = itemStateChange.Item.ExactNumberOfChargesRemaining - 1;
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

        private static void CalculateChargeItemChanges(CalculatedAttack calculatedAttack)
        {
            // Handle the swinging weapon
            var item = calculatedAttack.Item;
            if (item != null && !item.HasUnlimitedCharges && item.HasCharges)
            {
                var itemStateChange = new ItemStateChange();
                itemStateChange.Item = item;
                itemStateChange.Owner = calculatedAttack.Source;
                if (item.ExactNumberOfChargesRemaining - 1 <= 0 && item.DestroyWhenAllChargesAreConsumed)
                {
                    itemStateChange.NumberOfItemsConsumed = 1;
                }
                itemStateChange.NumberOfChargesConsumed = 1;
                calculatedAttack.ItemStateChanges.Add(itemStateChange);
            }
        }

        private static void CalculateProjectileItemChanges(CalculatedAttack calculatedAttack)
        {
            if (calculatedAttack.AttackType == AttackType.Ranged || calculatedAttack.AttackType == AttackType.Thrown)
            {
                Item itemBeingLaunched = GetItemBeingLaunched(calculatedAttack);
                var shouldSpawnItemOnGround = MathUtil.PercentageChanceEventOccurs(itemBeingLaunched.ChanceToSurviveLaunch);
                if (shouldSpawnItemOnGround)
                {
                    var groundDropSpawn = new GroundDropSpawn();
                    groundDropSpawn.UniqueIdentifier = itemBeingLaunched.UniqueIdentifier;
                    groundDropSpawn.Name = itemBeingLaunched.DisplayName;

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

        public static void CalculateExplosion(Grid<Tile> grid, CalculatedAttack calculatedAttack, Point explosionPosition)
        {
            if (calculatedAttack.AttackParameters.ExplosionParameters != null)
            {
                Assert.IsNotNull(calculatedAttack.Source);
                Assert.IsNotNull(explosionPosition);
                Assert.IsTrue(calculatedAttack.AttackParameters.ExplosionParameters.Radius > 0, "An explosion with a radius of 0 does nothing");

                var pointsHitOnAnyPass = new List<Point>();
                for (var index = 0; index <= calculatedAttack.AttackParameters.ExplosionParameters.Radius; index++)
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
                                    AttackType = calculatedAttack.AttackType,
                                    AttackParameters = calculatedAttack.AttackParameters.ExplosionParameters,
                                    Target = entityInPosition,
                                };
                                CalculateDamageForAttackParameters(calculatedAttack.AttackParameters.ExplosionParameters, entityStateChange);
                                entityStateChange.AppliedEffects.AddRange(calculatedAttack.AttackParameters.ExplosionParameters.AppliedEffects);
                                stateChangesOnPass.Add(entityStateChange);
                                calculatedAttack.ExplosionStateChanges.Add(entityStateChange);
                            }
                        }
                    }

                }
            }
        }

        private static void CalculateDamageForAttackParameters(AttackParameters attackParameters, EntityStateChange attackStateChange)
        {
            for (var numDyeRolled = 0; numDyeRolled < attackParameters.DyeNumber; numDyeRolled++)
            {
                attackStateChange.HealthChange += UnityEngine.Random.Range(1, attackParameters.DyeSize + 1);
            }
            attackStateChange.HealthChange += attackParameters.Bonus;

            if (attackParameters.DamageType == DamageTypes.HEALING)
            {
                attackStateChange.HealthChange *= -1;
            }
        }

        private static void EnsureExplosionDictsForRadius(CalculatedAttack calculatedAttack, int index)
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


        public static AttackParameters AttackParametersResolve(Entity source, AttackType attackType, Item item)
        {
            var attackTypeParameters = CombatUtil.AttackTypeParametersResolve(source, attackType, item);
            AttackParameters resolved = null;
            if (attackTypeParameters != null)
            {
                if (attackType == AttackType.Ranged)
                {
                    var ammo = CombatUtil.AmmoResolve(source, attackType, item);
                    resolved = MathUtil.ChooseRandomElement<AttackParameters>(ammo.AttackTypeParameters[AttackType.Ranged].AttackParameters);
                }
                else
                {
                    resolved = MathUtil.ChooseRandomElement<AttackParameters>(attackTypeParameters.AttackParameters);
                }
            }
            return resolved;
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
                Context.UIController.FloatingCombatTextManager.ShowCombatText(msg.Message, msg.Color, msg.FontSize, MathUtil.MapToWorld(result.Target.Position));
            }
            // Because in some circumstances this can be caused twice, clear the buffer.
            result.LogMessages.Clear();
            result.LateMessages.Clear();
            result.FloatingTextMessage.Clear();
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
                if (target.View != null && target.View.SkeletonAnimation != null)
                {
                    var skeletonAnimation = target.View.SkeletonAnimation;
                    skeletonAnimation.AnimationState.ClearTracks();
                    skeletonAnimation.Skeleton.SetToSetupPose();
                    skeletonAnimation.AnimationState.SetAnimation(0, StringUtil.GetAnimationNameForDirection(Animations.GetHit, newTargetingDirection), false);
                    skeletonAnimation.AnimationState.AddAnimation(0, StringUtil.GetAnimationNameForDirection(Animations.Idle, newTargetingDirection), true, 0.0f);
                }
            }
            if (result.AttackParameters != null && result.AttackParameters.DamageType == DamageTypes.NOT_SET)
            {
                throw new NotImplementedException("You forgot to set the damage type on this attack");
            }

            if (!target.IsCombatant)
            {
                throw new NotImplementedException("Cannot deal damage to non combatants");
            }

            if (target.Body.CurrentHealth <= 0)
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

            if (result.WasShortCircuited)
            {
                result.LogMessages.AddLast(string.Format(result.AttackParameters.AttackMessage, sourceName, targetName, result.HealthChange, StringUtil.DamageTypeToDisplayString(result.AttackParameters.DamageType)));
            }
            else
            {
                if (result.AttackParameters != null && result.HealthChange != 0)
                {
                    result.LogMessages.AddLast(string.Format(result.AttackParameters.AttackMessage, sourceName, targetName, result.HealthChange < 0 ? result.HealthChange * -1 : result.HealthChange, StringUtil.DamageTypeToDisplayString(result.AttackParameters.DamageType)));
                    target.Body.CurrentHealth = target.Body.CurrentHealth - result.HealthChange;

                    var healthChangeDisplay = result.HealthChange > 0 ? "-" : "+";
                    healthChangeDisplay += Math.Abs(result.HealthChange).ToString();
                    Color healthChangeColor = Color.black;

                    if (target.IsPlayer)
                    {
                        if (result.HealthChange > 0)
                        {
                            // Damage to player
                            healthChangeColor = Color.red;
                        }
                        else
                        {
                            // Healing to player
                            healthChangeColor = Color.green;
                        }
                    }
                    else
                    {
                        if (result.HealthChange > 0)
                        {
                            // Damage to NPC
                            healthChangeColor = Color.magenta;
                        }
                        else
                        {
                            // Healing to NPC
                            healthChangeColor = Color.blue;
                        }
                    }

                    result.FloatingTextMessage.AddLast(new FloatingTextMessage()
                    {
                        Message = string.Format("{0}", healthChangeDisplay),
                        Color = healthChangeColor,
                        target = target,
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
                    if ((result.AppliedEffects.Count > 0 || result.HealthChange > 0) && result.Target.Behaviour != null && result.Source != null)
                    {
                        var level = Context.Game.CurrentLevel;
                        AIUtil.ShoutAboutHostileTarget(result.Target, level, result.Source, result.Target.CalculateValueOfAttribute(Attributes.SHOUT_RADIUS));
                        if (result.Target.Behaviour.LastKnownTargetPosition == null)
                        {
                            result.Target.Behaviour.LastKnownTargetPosition = new Point(result.Source.Position);
                        }
                    }
                }
                CombatUtil.CapAttributes(target);

                if (target.Body.CurrentHealth <= 0)
                {
                    if (target.View != null && target.View.SkeletonAnimation != null)
                    {
                        var skeletonAnimation = target.View.SkeletonAnimation;
                        skeletonAnimation.AnimationState.ClearTracks();
                        skeletonAnimation.Skeleton.SetToSetupPose();
                        skeletonAnimation.AnimationState.SetAnimation(0, StringUtil.GetAnimationNameForDirection(Animations.FallDown, target.Direction), false);
                    }
                    result.FloatingTextMessage.AddLast(new FloatingTextMessage()
                    {
                        Message = string.Format("Dead!", result.HealthChange),
                        Color = Color.black,
                        target = target,
                    });
                    result.LogMessages.AddLast(string.Format("{0} has been slain!", targetName));
                    target.Name = string.Format("( Corpse ) {0}", target.Name);
                    target.Body.IsDead = true;
                    // Is not deregistered because the corpse should still be available
                    target.Behaviour = null;
                    var game = Context.Game;
                    var level = game.CurrentLevel;

                    foreach (var pair in target.Inventory.EquippedItemBySlot)
                    {
                        var item = pair.Value;
                        foreach (var effect in item.EffectsGlobal)
                        {
                            effect.RemovePersistantVisualEffects(target);
                        }
                    }

                    if (target.BlocksPathing)
                    {
                        // You cant deregister to ReleasePathfindingAtPosition bc we want to keep the corpse
                        level.ReleasePathfindingAtPosition(target, target.Position);
                        target.BlocksPathing = false;
                    }

                    if (target.View.ViewGameObject != null)
                    {
                        target.View.ViewGameObject.AddComponent<DeathAnimation>();

                    }

                    if (InventoryUtil.HasAnyItems(target))
                    {
                        target.View.ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_CORPSE;
                        ViewFactory.RebuildView(target, false);
                        var sortable = target.View.ViewGameObject.GetComponent<Sortable>();
                        sortable.Position = target.Position;
                        Context.SpriteSortingSystem.Register(sortable);
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

        public static List<Effect> GetEntityEffectsByType(Entity entity, Predicate<Effect> filter = null)
        {
            var effectsAggregate = new List<Effect>();
            if (entity.Trigger != null && entity.Trigger.Effects.Count > 0)
            {
                effectsAggregate.AddRange(entity.Trigger.Effects);
            }

            foreach (var pair in entity.Inventory.EquippedItemBySlot)
            {
                var item = pair.Value;
                effectsAggregate.AddRange(item.EffectsGlobal);
            }

            foreach (var item in entity.Inventory.Items)
            {
                if (item != null)
                {
                    effectsAggregate.AddRange(item.EffectsGlobal);

                }
            }

            if (entity.Body != null)
            {
                effectsAggregate.AddRange(entity.Body.Effects);
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

        public static void RemoveEntityEffects(Entity entity, List<Effect> effectsThatShouldExpire)
        {
            Predicate<Effect> IsAnAffectThatShouldExpire = (eff) => { return effectsThatShouldExpire.Contains(eff); };

            // Note that we never remove attack specific effects
            if (entity.Trigger != null && entity.Trigger.Effects.Count > 0)
            {
                var expiring = entity.Trigger.Effects.FindAll(IsAnAffectThatShouldExpire);
                foreach (var expire in expiring)
                {
                    expire.OnRemove(entity);
                }
                entity.Trigger.Effects.RemoveAll(IsAnAffectThatShouldExpire);
            }

            foreach (var pair in entity.Inventory.EquippedItemBySlot)
            {
                var item = pair.Value;
                var expiring = item.EffectsGlobal.FindAll(IsAnAffectThatShouldExpire);
                foreach (var expire in expiring)
                {
                    expire.OnRemove(entity);
                }
                item.EffectsGlobal.RemoveAll(IsAnAffectThatShouldExpire);
            }

            foreach (var item in entity.Inventory.Items)
            {
                if (item != null)
                {
                    var expiring = item.EffectsGlobal.FindAll(IsAnAffectThatShouldExpire);
                    foreach (var expire in expiring)
                    {
                        expire.OnRemove(entity);
                    }
                    item.EffectsGlobal.RemoveAll(IsAnAffectThatShouldExpire);
                }
            }

            if (entity.Body != null)
            {
                var expiring = entity.Body.Effects.FindAll(IsAnAffectThatShouldExpire);
                foreach (var expire in expiring)
                {
                    expire.OnRemove(entity);
                }
                entity.Body.Effects.RemoveAll(IsAnAffectThatShouldExpire);
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
            if (entity.Body != null)
            {
                if (entity.Body.CurrentHealth > entity.CalculateValueOfAttribute(Gamepackage.Attributes.MAX_HEALTH))
                {
                    entity.Body.CurrentHealth = entity.CalculateValueOfAttribute(Gamepackage.Attributes.MAX_HEALTH);
                }
            }
        }

        private static void HandleAppliedEffects(EntityStateChange outcome)
        {
            if (!outcome.WasShortCircuited)
            {
                foreach (var effect in outcome.AppliedEffects)
                {
                    if (effect is AppliedEffect)
                    {
                        var appliedEffect = (AppliedEffect)effect;
                        if (appliedEffect.CanApply(outcome))
                        {
                            appliedEffect.Apply(outcome);
                        }
                    }
                    else
                    {
                        effect.HandleStacking(outcome);
                    }
                }
            }
        }

        public static void PerformTriggerStepAbilityIfSteppedOn(Entity Source, Entity potentialTrigger, List<Point> points)
        {
            if (points.Contains(Source.Position))
            {
                var abilities = potentialTrigger.Trigger.Effects;
                foreach (var effect in abilities)
                {
                    var attack = new EntityStateChange();
                    attack.Source = potentialTrigger;
                    attack.Target = Source;
                    effect.TriggerOnStep(attack);
                }
            }
        }

        public static void ConsumeItemCharges(Entity owner, Item item, int NumberConsumed = 1)
        {
            if (!item.HasUnlimitedCharges && item.HasCharges)
            {
                item.ExactNumberOfChargesRemaining = item.ExactNumberOfChargesRemaining - NumberConsumed;
                if (item.ExactNumberOfChargesRemaining < 0)
                {
                    item.ExactNumberOfChargesRemaining = 0;
                }
                if (item.DestroyWhenAllChargesAreConsumed)
                {
                    InventoryUtil.RemoveWholeItemStack(owner, item);
                    Context.UIController.Refresh();
                    ViewFactory.RebuildView(owner);
                }
            }
        }

        public static bool CanAttackWithItem(Entity source, AttackType attackType, Item item)
        {
            if (attackType == AttackType.NotSet)
            {
                return false;
            }
            var ammo = AmmoResolve(source, attackType, item);
            var hasBody = source.Body != null && CanAttackInMeleeWithoutWeapon(source);
            if (attackType == AttackType.Melee)
            {
                var hasWeapon = hasBody && source.Inventory != null && item != null && item.CanBeUsedInAttackType(AttackType.Melee);
                return hasBody || hasWeapon;
            }
            else if (attackType == AttackType.Ranged)
            {
                var hasRangedWeapon = hasBody && source.Inventory != null && item != null && item.CanBeUsedInAttackType(AttackType.Ranged);
                var hasAmmo = false;
                if (hasRangedWeapon)
                {
                    hasAmmo = ammo != null && item.AmmoType == ammo.AmmoType;
                }
                return hasRangedWeapon && hasAmmo;
            }
            else if (attackType == AttackType.Thrown)
            {
                return hasBody && source.Inventory != null && item != null && item.CanBeUsedInAttackType(AttackType.Thrown);
            }
            else if (attackType == AttackType.Zapped)
            {
                return hasBody && source.Inventory != null && item != null && item.CanBeUsedInAttackType(AttackType.Zapped);
            }
            else if (attackType == AttackType.ApplyToSelf || attackType == AttackType.ApplyToOther)
            {
                return true;
            }
            else
            {
                throw new NotImplementedException("Unimplemented attack type");
            }
        }

        public static Item AmmoResolve(Entity source, AttackType attackType, Item item)
        {
            if (source == null || attackType != AttackType.Ranged || source.Inventory == null)
            {
                return null;
            }
            return InventoryUtil.GetItemBySlot(source, ItemSlot.Ammo);
        }

        public static List<Point> PointsInRange(Grid<Tile> grid, Point position, int range, AttackTargetingType attackTargetingType)
        {
            List<Point> outputRange = new List<Point>();
            if (attackTargetingType == AttackTargetingType.Line)
            {
                outputRange.AddRange(MathUtil.LineInDirection(position, Direction.SouthEast, range));
                outputRange.AddRange(MathUtil.LineInDirection(position, Direction.SouthWest, range));
                outputRange.AddRange(MathUtil.LineInDirection(position, Direction.NorthEast, range));
                outputRange.AddRange(MathUtil.LineInDirection(position, Direction.NorthWest, range));
            }
            else if (attackTargetingType == AttackTargetingType.SelectTarget)
            {
                outputRange.AddRange(grid[position].CachedFloorFloodFills[range]);
            }
            else
            {
                throw new NotImplementedException("AttackTargetingType not implemented: " + attackTargetingType);
            }
            outputRange.AddRange(outputRange);
            return outputRange;
        }

        public static bool InRangeOfAttack(Grid<Tile> grid, Point position, int range, AttackTargetingType attackTargetingType, Point target)
        {
            var pointsInRange = PointsInRange(grid, position, range, attackTargetingType);
            return pointsInRange.Contains(target);
        }

        public static bool HasAClearShot(Entity source, AttackTargetingType attackTargetingType, Point target)
        {
            var canHitFromHere = source.Position.IsOrthogonalTo(target) || attackTargetingType != AttackTargetingType.Line;

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
                        if (entityInPosition.IsCombatant && entityInPosition.Behaviour != null && source.Behaviour != null && entityInPosition.Behaviour.ActingTeam == source.Behaviour.ActingTeam)
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
             return source.Body != null && source.Body.MeleeAttackTypeParameters != null && source.Body.MeleeAttackTypeParameters.AttackParameters.Count > 0;
        }

        public static List<Point> PointsInExplosionRange(ExplosionParameters ExplosionParameters, Point placementPosition)
        {
            List<Point> outputRange = new List<Point>();
            if (ExplosionParameters != null)
            {
                outputRange.AddRange(Context.Game.CurrentLevel.Grid[placementPosition].CachedFloorFloodFills[ExplosionParameters.Radius]);
            }
            return outputRange;
        }

        public static AttackTypeParameters AttackTypeParametersResolve(Entity source, AttackType attackType, Item item)
        {
            AttackTypeParameters attackTypeParameters = null;
            var ammo = CombatUtil.AmmoResolve(source, attackType, item);
            var hasAmmoOrDoesntNeedIt = attackType != AttackType.Ranged || ammo != null;
            if ((item == null || !item.CanBeUsedInAttackType(AttackType.Melee) && CanAttackInMeleeWithoutWeapon(source)))
            {
                attackTypeParameters = source.Body.MeleeAttackTypeParameters;
            }
            else if (item != null && item.CanBeUsedInAttackType(attackType) && hasAmmoOrDoesntNeedIt)
            {
                attackTypeParameters = item.AttackTypeParameters[attackType];
            }

            return attackTypeParameters;
        }

        public static void CalculateAffectOutgoingAttack(CalculatedAttack calculatedAttack, EntityStateChange change)
        {
            if (change.Source != null)
            {
                var sourceEffects = change.Source.GetEffects();
                foreach (var effect in sourceEffects)
                {
                    if (effect.CanAffectOutgoingAttack(calculatedAttack, change))
                    {
                        effect.CalculateAffectOutgoingAttack(calculatedAttack, change);
                    }
                }
            }
        }

        public static void CalculateAffectIncomingAttackEffects(CalculatedAttack calculatedAttack, EntityStateChange change)
        {
            var targetEffects = change.Target.GetEffects();
            foreach (var effect in targetEffects)
            {
                if (effect.CanAffectIncomingAttack(calculatedAttack, change))
                {
                    effect.CalculateAffectIncomingAttackEffects(calculatedAttack, change);
                }
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

        public static Point CalculateEndpointOfLineSkillshot(Point position, AttackTypeParameters attackTypeParameters, Direction direction)
        {
            var pointsInLine = MathUtil.LineInDirection(position, direction, attackTypeParameters.Range);
            var numberOfThingsCanPierce = attackTypeParameters.NumberOfTargetsToPierce;
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
