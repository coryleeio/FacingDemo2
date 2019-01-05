using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class Behaviour : Component
    {
        public Team OriginalTeam;
        public Team ActingTeam;
        public AIType AI;
        public bool IsDoneThisTurn = false;
        public int TimeAccrued = 0;
        public bool IsThinking = false;
        public bool IsPlayer;

        public Point LastKnownTargetPosition = null;

        [JsonIgnore]
        public Action NextAction = null;

        [JsonIgnore]
        private static List<CombatContext> MeleeAIRelevantContexts = new List<CombatContext>()
        {
            CombatContext.Melee,
        };

        [JsonIgnore]
        private static List<CombatContext> RangedAIRelevantContexts = new List<CombatContext>()
        {
            CombatContext.Melee,
            CombatContext.Zapped,
            CombatContext.Thrown,
            CombatContext.Ranged,
        };

        [JsonIgnore]
        public int PathsExpected = 0;

        [JsonIgnore]
        public int PathsReturned = 0;

        [JsonIgnore]
        Coroutine runningRoutine;

        public void FigureOutNextAction()
        {
            if(AI == AIType.None)
            {
                throw new NotImplementedException("AI with undefined behaviour: " + entity.PrototypeIdentifier.ToString());
            }

            var level = Context.GameStateManager.Game.CurrentLevel;
            var capabilities = new AttackCapabilities(entity);
            NextAction = null;

            if (entity.Behaviour.AI == AIType.DumbMelee)
            {
                CommonAIAttackWithWeapon(level, capabilities, MeleeAIRelevantContexts);
            }
            else if (entity.Behaviour.AI == AIType.Archer)
            {
                CommonAIAttackWithWeapon(level, capabilities, RangedAIRelevantContexts);
            }
        }

        private void CommonAIAttackWithWeapon(Level level, AttackCapabilities capabilities, List<CombatContext> contexts)
        {
            var hostileTarget = FindVisibleTargets(entity);
            if (hostileTarget == null)
            {
                EnqueueDefaultBehaviour();
                return;
            }
            else
            {
                LastKnownTargetPosition = new Point(hostileTarget.Position);
                var shoutRadius = entity.CalculateValueOfAttribute(Attributes.SHOUT_RADIUS);
                ShoutAboutHostileTarget(level, hostileTarget, shoutRadius);
            }
            EnqueueBasicAttackOrApproach(capabilities, contexts, hostileTarget);
        }

        public void ShoutAboutHostileTarget(Level level, Entity hostileTarget, int shoutRadius)
        {
            MakeNoiseIndicatingLocation(level, entity.Position, hostileTarget.Position, shoutRadius, (possibleEntity) => { return possibleEntity.Behaviour != null && possibleEntity.Behaviour.ActingTeam == entity.Behaviour.ActingTeam; });
        }

        // Make a noise at a location, hearable by all entities that are within the radius and match the predicate.
        private void MakeNoiseIndicatingLocation(Level level, Point soundOrigin, Point targetLocation, int radius, Predicate<Entity> predicate)
        {
            if(level.BoundingBox.Contains(soundOrigin))
            {
                var possibleLocations = level.Grid[soundOrigin].CachedFloorFloodFills[radius];
                foreach(var posibleLocation in possibleLocations)
                {
                    var entitiesInLocation = level.Grid[posibleLocation].EntitiesInPosition;
                    foreach(var entityInLocation in entitiesInLocation)
                    {
                        if(predicate == null || predicate(entityInLocation))
                        {
                            if(entityInLocation.Behaviour != null && entityInLocation.Behaviour.LastKnownTargetPosition == null)
                            {
                                entityInLocation.Behaviour.LastKnownTargetPosition = targetLocation;
                            }
                        }
                    }
                }
            }
        }

        private void EnqueueBasicAttackOrApproach(AttackCapabilities capabilities, List<CombatContext> relevantContexts, Entity target)
        {
            foreach (var combatContext in relevantContexts)
            {
                if (AbleAndWillingToPerformAttackTypeOnTarget(capabilities, combatContext, target))
                {
                    EnqueueAttackType(capabilities, combatContext, target);
                }
            }

            if (NextAction == null) // was never able to resolve a thing to do
            {
                Context.Application.StartCoroutine(MoveToward(target.Position));
            }
        }

        private void EnqueueAttackType(AttackCapabilities capabilities, CombatContext combatContext, Entity target)
        {
            var capability = capabilities[combatContext];
            var direction = MathUtil.RelativeDirection(entity.Position, target.Position);
            var attack = new Attack(capabilities, combatContext, direction, target.Position);
            NextAction = attack;
        }

        private bool AbleAndWillingToPerformAttackTypeOnTarget(AttackCapabilities capabilities, CombatContext combatContext, Entity target)
        {
            var capability = capabilities[combatContext];
            var isAbleToPerform = capability.CanPerform && capability.IsInRange(target) && capability.HasAClearShot(target.Position);

            if (!isAbleToPerform)
            {
                return isAbleToPerform;
            }

            // There are certain things the AI CAN do, but it should NOT do.
            // those go here.
            var isWillingToPerform = true;
            if (combatContext == CombatContext.Thrown)
            {
                // Dont throw my only weapon
                isWillingToPerform = capability.MainHand.NumberOfItems > 1;
            }
            return isAbleToPerform && isWillingToPerform;
        }

        private static Entity FindVisibleTargets(Entity entity)
        {
            var entitySystem = Context.EntitySystem;
            var level = Context.GameStateManager.Game.CurrentLevel;
            var grid = level.Grid;
            var points = Context.VisibilitySystem.PlacesVisibleFromLocation(level, entity.Position, entity.CalculateValueOfAttribute(Attributes.VISION_RADIUS));
            var targets = new List<Entity>();
            var player = level.Player;

            foreach (var pos in points)
            {
                var entitiesInPos = grid[pos].EntitiesInPosition;
                foreach(var entityInPos in entitiesInPos)
                {
                    if(entityInPos == entity)
                    {
                        continue;
                    }
                    if((entityInPos.Body != null && entityInPos.Body.IsDead) || !entityInPos.IsCombatant)
                    {
                        continue;
                    }
                    if (entity.Behaviour.ActingTeam == Team.PLAYER)
                    {
                        // If I am a member of the player's team - search for enemies.
                        if(entityInPos.Behaviour != null && (entityInPos.Behaviour.ActingTeam == Team.Enemy || entityInPos.Behaviour.ActingTeam == Team.EnemyOfAll))
                        {
                            targets.Add(entityInPos);
                        }
                    }
                    else if (entity.Behaviour.ActingTeam == Team.Enemy)
                    {
                        // If I am a member of the enemy's team - search for enemies
                        if (entityInPos.Behaviour != null && (entityInPos.Behaviour.ActingTeam == Team.PLAYER || entityInPos.Behaviour.ActingTeam == Team.EnemyOfAll))
                        {
                            targets.Add(entityInPos);
                        }
                    }
                    else if (entity.Behaviour.ActingTeam == Team.EnemyOfAll)
                    {
                        // If I am a member of the ENEMY_OF_ALL team - everyone is an enemy, except neutrals
                        if (entityInPos.Behaviour != null && entityInPos.Behaviour.ActingTeam != Team.Neutral)
                        {
                            targets.Add(entityInPos);
                        }
                    }

                    else
                    {
                        throw new NotImplementedException("Cant find targets for entity with team: " + entity.PrototypeIdentifier.ToString() + " " + entity.Behaviour.ActingTeam.ToString());
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

        private void EnqueueDefaultBehaviour()
        {
            if (entity.Behaviour.ActingTeam == Team.PLAYER)
            {
                // Default for allies is follow player
                var player = Context.GameStateManager.Game.CurrentLevel.Player;
                Context.Application.StartCoroutine(MoveToward(player.Position));
                return;
            }
            else
            {
                if(LastKnownTargetPosition != null)
                {
                    if(LastKnownTargetPosition.Distance(entity.Position) < 1.0f)
                    {
                        LastKnownTargetPosition = null;
                        var wait = new Wait
                        {
                            Source = entity
                        };
                        NextAction = wait;
                    }
                    else
                    {
                        Context.Application.StartCoroutine(MoveToward(LastKnownTargetPosition));
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
                    NextAction = wait;
                }
            }
        }

        private IEnumerator MoveToward(Point targetPosition)
        {
            NextAction = null;
            PathsReturned = 0;

            var level = Context.GameStateManager.Game.CurrentLevel;
            var PointsAroundTarget = MathUtil.OrthogonalPoints(targetPosition).FindAll((p) => { return level.Grid[p].Walkable; });
            var PointsAroundMe = MathUtil.OrthogonalPoints(entity.Position).FindAll((p) => { return level.Grid[p].Walkable && Point.DistanceSquared(p, targetPosition) < Point.DistanceSquared(entity.Position, targetPosition); });

            PathsExpected = PointsAroundTarget.Count + PointsAroundMe.Count;
            PointsAroundMe.Sort(new PointDistanceComparer()
            {
                Source = entity.Position
            });

            foreach (var pointAroundTarget in PointsAroundTarget)
            {
                Context.PathFinder.StartPath(entity.Position, pointAroundTarget, Context.GameStateManager.Game.CurrentLevel.Grid, ReceivePath);
            }

            foreach (var pointAroundMe in PointsAroundMe)
            {
                Context.PathFinder.StartPath(entity.Position, pointAroundMe, Context.GameStateManager.Game.CurrentLevel.Grid, ReceivePath);
            }

            while (NextAction == null)
            {
                if (PathsReturned == PathsExpected)
                {
                    var wait = new Wait
                    {
                        Source = entity
                    };
                    NextAction = wait;
                    break; // done
                }
                yield return new WaitForEndOfFrame();
            }
        }

        public void ReceivePath(Path path)
        {
            PathsReturned++;
            if (path.Nodes.Count > 0)
            {
                if (runningRoutine != null)
                {
                    Context.Application.StopCoroutine(runningRoutine);
                    runningRoutine = null;
                }
                var move = new Move
                {
                    Source = entity,
                    TargetPosition = new Point(path.Nodes[0].Position.X, path.Nodes[0].Position.Y)
                };
                NextAction = move;
            }
        }
    }
}
