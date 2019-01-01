using KDSharp.KDTree;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class AIController : Behaviour
    {
        public override bool IsPlayer
        {
            get
            {
                return false;
            }
        }

        private static List<CombatContext> MeleeAIRelevantContexts = new List<CombatContext>()
        {
            CombatContext.Melee,
        };

        private static List<CombatContext> RangedAIRelevantContexts = new List<CombatContext>()
        {
            CombatContext.Melee,
            CombatContext.Zapped,
            CombatContext.Thrown,
            CombatContext.Ranged,
        };

        public int PathsExpected = 0;
        public int PathsReturned = 0;

        [JsonIgnore]
        Coroutine runningRoutine;

        public override void FigureOutNextAction()
        {
            var level = Context.GameStateManager.Game.CurrentLevel;
            var capabilities = new AttackCapabilities(entity);
            NextAction = null;

            if(entity.Behaviour.AI == AIType.DumbMelee)
            {
                DoMeleeAI(level, capabilities);
            }
            else if (entity.Behaviour.AI == AIType.Archer)
            {
                DoRangedAI(level, capabilities);
            }
        }


        private void DoMeleeAI(Level level, AttackCapabilities capabilities)
        {
            var hostileTarget = FindTarget(entity);
            if (hostileTarget == null)
            {
                EnqueueDefaultBehaviour();
                return;
            }

            if (!Context.VisibilitySystem.CanSee(level, entity, hostileTarget))
            {
                EnqueueDefaultBehaviour();
            }
            else
            {
                EnqueueBasicAttackOrApproach(capabilities, MeleeAIRelevantContexts, hostileTarget);
            }
        }

        private void DoRangedAI(Level level, AttackCapabilities capabilities)
        {
            var hostileTarget = FindTarget(entity);
            if (hostileTarget == null)
            {
                EnqueueDefaultBehaviour();
                return;
            }

            if (!Context.VisibilitySystem.CanSee(level, entity, hostileTarget))
            {
                EnqueueDefaultBehaviour();
            }
            else
            {
                EnqueueBasicAttackOrApproach(capabilities, RangedAIRelevantContexts, hostileTarget);
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

        public bool AbleAndWillingToPerformAttackTypeOnTarget(AttackCapabilities capabilities, CombatContext combatContext, Entity target)
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

        public static NearestNeighbour<Entity> NearestTargets(Entity entity)
        {
            var entitySystem = Context.EntitySystem;
            KDTree<Entity> relevantTree = null;

            if (entity.Behaviour.Team == Team.PLAYER)
            {
                // If I am a member of the player's team - search for enemies.
                relevantTree = entitySystem.EnemyTeamTree;
            }
            else if (entity.Behaviour.Team == Team.ENEMY)
            {
                // If I am a member of the enemy team - search for players.
                relevantTree = entitySystem.PlayerTeamTree;
            }
            else if (entity.Behaviour.Team == Team.NEUTRAL)
            {
                // Neutrals dont have enemies.
            }
            else
            {
                throw new NotImplementedException();
            }
            return relevantTree.NearestNeighbors(new double[2] { entity.Position.X, entity.Position.Y }, 10);
        }

        private static Entity FindTarget(Entity entity)
        {
            var entitySystem = Context.EntitySystem;
            var possibleTargets = NearestTargets(entity);

            if (possibleTargets == null)
            {
                return null;
            }
            foreach (var target in possibleTargets)
            {
                return target;
            }
            return null;
        }

        private void EnqueueDefaultBehaviour()
        {
            if (entity.Behaviour.Team == Team.PLAYER)
            {
                // Default for allies is follow player
                var player = Context.GameStateManager.Game.CurrentLevel.Player;
                Context.Application.StartCoroutine(MoveToward(player.Position));
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

        IEnumerator MoveToward(Point targetPosition)
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
