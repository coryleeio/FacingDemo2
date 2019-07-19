using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public static class AIUtil
    {
        [JsonIgnore]
        private static readonly List<CombatActionType> MeleeAIRelevantContexts = new List<CombatActionType>()
        {
            CombatActionType.Melee,
        };

        [JsonIgnore]
        private static readonly List<CombatActionType> RangedAIRelevantContexts = new List<CombatActionType>()
        {
            CombatActionType.Melee,
            CombatActionType.Zapped,
            CombatActionType.Thrown,
            CombatActionType.Ranged,
        };

        public static void ReceivePath(Entity entity, Path path)
        {
            entity.PathsReturned++;
            if (path.Nodes.Count > 0)
            {
                var move = new Move
                {
                    Source = entity,
                    TargetPosition = new Point(path.Nodes[0].Position.X, path.Nodes[0].Position.Y)
                };
                entity.NextAction = move;
            }
        }

        public static void CommonAIAttackWithWeapon(Level level, Entity entity, Item item, List<CombatActionType> contexts)
        {
            var hostileTarget = FindVisibleTargets(entity);
            if (hostileTarget == null)
            {
                AIUtil.EnqueueDefaultBehaviour(entity);
                return;
            }
            else
            {
                entity.LastKnownTargetPosition = new Point(hostileTarget.Position);
                var shoutRadius = entity.CalculateValueOfAttribute(Attributes.ShoutRadius);
                AIUtil.ShoutAboutHostileTarget(entity, level, hostileTarget, shoutRadius);
            }
            EnqueueBasicAttackOrApproach(entity, item, contexts, hostileTarget);
        }

        private static bool AbleAndWillingToPerformInteractionTypeOnTarget(Entity source, CombatActionType InteractionType, Item item, Entity target)
        {
            var resolvedParams = CombatUtil.CombatActionParametersResolve(source, InteractionType, item);
            var grid = Context.Game.CurrentLevel.Grid;

            var isAbleToPerform = CombatUtil.CanAttackWithItem(source, InteractionType, item) &&
                CombatUtil.InRangeOfAttack(grid, source.Position, resolvedParams.CombatActionParameters.Range, resolvedParams.CombatActionParameters.TargetingType, target.Position) &&
                CombatUtil.HasAClearShot(source, resolvedParams.CombatActionParameters.TargetingType, target.Position);

            if (!isAbleToPerform)
            {
                return isAbleToPerform;
            }

            // There are certain things the AI CAN do, but it should NOT do.
            // those go here.
            var isWillingToPerform = true;
            if (InteractionType == CombatActionType.Thrown)
            {
                // Dont throw my only weapon
                isWillingToPerform = item.NumberOfItems > 1;
            }
            return isAbleToPerform && isWillingToPerform;
        }

        private static void EnqueueInteractionType(Entity entity, CombatActionType InteractionType, Item item, Entity target)
        {
            var grid = Context.Game.CurrentLevel.Grid;
            var InteractionTypeParameters = CombatUtil.CombatActionParametersResolve(entity, InteractionType, item);
            var calculatedAttack = CombatUtil.CalculateAttack(grid, entity, InteractionType, item, target.Position, InteractionTypeParameters);
            var attack = new CombatAction(calculatedAttack);
            entity.NextAction = attack;
        }

        public static void EnqueueBasicAttackOrApproach(Entity entity, Item item, List<CombatActionType> relevantContexts, Entity target)
        {
            foreach (var combatContext in relevantContexts)
            {
                if (AbleAndWillingToPerformInteractionTypeOnTarget(entity, combatContext, item, target))
                {
                    EnqueueInteractionType(entity, combatContext, item, target);
                }
            }

            if (entity.NextAction == null) // was never able to resolve a thing to do
            {
                Context.GameSceneRoot.StartCoroutine(MoveToward(entity, target.Position));
            }
        }

        public static void ShoutAboutHostileTarget(Entity source, Level level, Entity hostileTarget, int shoutRadius)
        {
            MakeNoiseIndicatingLocation(level, source.Position, hostileTarget.Position, shoutRadius, (possibleEntity) => { return possibleEntity.IsCombatant && possibleEntity.ActingTeam == source.ActingTeam; });
        }

        // Make a noise at a location, hearable by all entities that are within the radius and match the predicate.
        private static void MakeNoiseIndicatingLocation(Level level, Point soundOrigin, Point targetLocation, int radius, Predicate<Entity> predicate)
        {
            if (level.BoundingBox.Contains(soundOrigin))
            {
                var possibleLocations = level.Grid[soundOrigin].CachedFloorFloodFills[radius];
                foreach (var posibleLocation in possibleLocations)
                {
                    var entitiesInLocation = level.Grid[posibleLocation].EntitiesInPosition;
                    foreach (var entityInLocation in entitiesInLocation)
                    {
                        if (predicate == null || predicate(entityInLocation))
                        {
                            if (entityInLocation.IsCombatant && entityInLocation.LastKnownTargetPosition == null)
                            {
                                entityInLocation.LastKnownTargetPosition = targetLocation;
                            }
                        }
                    }
                }
            }
        }

        private static Entity FindVisibleTargets(Entity entity)
        {
            var entitySystem = Context.EntitySystem;
            var level = Context.Game.CurrentLevel;
            var grid = level.Grid;
            var points = Context.VisibilitySystem.PlacesVisibleFromLocation(level, entity.Position, entity.CalculateValueOfAttribute(Attributes.VisionRadius));
            var targets = new List<Entity>();
            var player = level.Player;

            foreach (var pos in points)
            {
                var entitiesInPos = grid[pos].EntitiesInPosition;
                foreach (var entityInPos in entitiesInPos)
                {
                    if (entityInPos == entity)
                    {
                        continue;
                    }
                    if ((entityInPos.IsCombatant && entityInPos.IsDead) || !entityInPos.IsCombatant)
                    {
                        continue;
                    }
                    if (entity.ActingTeam == Team.PLAYER)
                    {
                        // If I am a member of the player's team - search for enemies.
                        if (entityInPos.IsCombatant && (entityInPos.ActingTeam == Team.Enemy || entityInPos.ActingTeam == Team.EnemyOfAll))
                        {
                            targets.Add(entityInPos);
                        }
                    }
                    else if (entity.ActingTeam == Team.Enemy)
                    {
                        // If I am a member of the enemy's team - search for enemies
                        if (entityInPos.IsCombatant && (entityInPos.ActingTeam == Team.PLAYER || entityInPos.ActingTeam == Team.EnemyOfAll))
                        {
                            targets.Add(entityInPos);
                        }
                    }
                    else if (entity.ActingTeam == Team.EnemyOfAll)
                    {
                        // If I am a member of the ENEMY_OF_ALL team - everyone is an enemy, except neutrals
                        if (entityInPos.IsCombatant && entityInPos.ActingTeam != Team.Neutral)
                        {
                            targets.Add(entityInPos);
                        }
                    }

                    else
                    {
                        throw new NotImplementedException("Cant find targets for entity with team: " + entity.Id.ToString() + " " + entity.ActingTeam.ToString());
                    }
                }
            }

            if (targets == null || targets.Count == 0)
            {
                return null;
            }
            targets.Sort(delegate (Entity e1, Entity e2)
            {
                return Point.Distance(entity.Position, e1.Position).CompareTo(Point.Distance(entity.Position, e2.Position));
            });
            foreach (var target in targets)
            {
                return target;
            }
            return null;
        }

        public static IEnumerator MoveToward(Entity entity, Point targetPosition)
        {
            entity.NextAction = null;
            var level = Context.Game.CurrentLevel;
            var PointsAroundMe = MathUtil.OrthogonalPoints(entity.Position).FindAll(Filters.NonoccupiedTiles);
            var PointsAroundTarget = MathUtil.OrthogonalPoints(targetPosition).FindAll(Filters.NonoccupiedTiles);
            if (PointsAroundMe.Count == 0)
            {
                // If there are no positions around me available, I can just give up now, as I cannot move.
                var wait = new Wait
                {
                    Source = entity
                };
                entity.NextAction = wait;
                yield break;
            }
            PointsAroundMe = PointsAroundMe.FindAll((poi) => { return Point.DistanceSquared(poi, targetPosition) < Point.DistanceSquared(entity.Position, targetPosition); });

            entity.PathsExpected = PointsAroundTarget.Count + PointsAroundMe.Count;
            PointsAroundMe.Sort(new PointDistanceComparer()
            {
                Source = entity.Position
            });

            foreach (var pointAroundTarget in PointsAroundTarget)
            {
                Context.PathFinder.StartPath(entity.Position, pointAroundTarget, Context.Game.CurrentLevel.Grid, (returnedPath) => { ReceivePath(entity, returnedPath); });
            }

            foreach (var pointAroundMe in PointsAroundMe)
            {
                Context.PathFinder.StartPath(entity.Position, pointAroundMe, Context.Game.CurrentLevel.Grid, (returnedPath) => { ReceivePath(entity, returnedPath); });
            }

            while (entity.NextAction == null)
            {
                if (entity.PathsReturned == entity.PathsExpected)
                {
                    var wait = new Wait
                    {
                        Source = entity
                    };
                    entity.NextAction = wait;
                    break; // done
                }
                yield return new WaitForEndOfFrame();
            }
        }

        public static void EnqueueDefaultBehaviour(Entity entity)
        {
            if (entity.ActingTeam == Team.PLAYER)
            {
                // Default for allies is follow player
                var player = Context.Game.CurrentLevel.Player;
                if (player.Position.IsAdjacentTo(entity.Position))
                {
                    // Wait if we are already adjacent
                    var wait = new Wait
                    {
                        Source = entity
                    };
                    entity.NextAction = wait;
                }
                else
                {
                    // Otherwise move toward player
                    Context.GameSceneRoot.StartCoroutine(MoveToward(entity, player.Position));
                }
                return;
            }
            else
            {
                if (entity.LastKnownTargetPosition != null)
                {
                    if (entity.LastKnownTargetPosition.Distance(entity.Position) < 1.0f)
                    {
                        entity.LastKnownTargetPosition = null;
                        var wait = new Wait
                        {
                            Source = entity
                        };
                        entity.NextAction = wait;
                    }
                    else
                    {
                        Context.GameSceneRoot.StartCoroutine(MoveToward(entity, entity.LastKnownTargetPosition));
                        return;
                    }
                }
                else
                {
                    // Default for monsters is waiting
                    var wait = new Wait
                    {
                        Source = entity
                    };
                    entity.NextAction = wait;
                }
            }
        }
    }
}
