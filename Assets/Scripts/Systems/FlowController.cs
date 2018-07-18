
using System.Collections.Generic;

namespace Gamepackage
{
    public class FlowController
    {
        public LinkedList<Step> Steps = new LinkedList<Step>();
        public Phase CurrentPhase = Phase.Player; // always start on player when changing scenes

        public void Init()
        {
            ChangePhase(Phase.Player);
        }

        public void Process()
        {
            if (Steps.Count != 0)
            {
                Steps.First.Value.Do();
                if (Steps.First.Value.Actions.Count == 0)
                {
                    Steps.RemoveFirst();
                }
            }
            else
            {
                var entities = Context.GameStateManager.Game.CurrentLevel.Entitys;
                var entitiesDoneThinking = 0;
                var entitesThatStillNeedToACtBeforePhaseEnds = 0;
                foreach (var entity in entities)
                {
                    if (entity.Behaviour != null)
                    {
                        if (entity.Behaviour.IsPlayer)
                        {
                            entity.Behaviour.IsThinking = true;
                        }
                        if (!entity.Behaviour.IsThinking)
                        {
                            entity.Behaviour.NextAction = null;
                            entity.Behaviour.IsThinking = true;
                            entity.Behaviour.FigureOutNextAction();
                        }
                        if (!entity.Behaviour.IsDoneThisTurn)
                        {
                            entitesThatStillNeedToACtBeforePhaseEnds++;
                            if (entity.Behaviour.IsThinking && entity.Behaviour.NextAction != null)
                            {
                                entitiesDoneThinking++;
                            }
                        }
                    }
                }

                if(entitesThatStillNeedToACtBeforePhaseEnds == 0)
                {
                    // phase is over, nobbody else needs to act
                    if(CurrentPhase == Phase.Player)
                    {
                        ChangePhase(Phase.Enemies);
                    }
                    else if(CurrentPhase == Phase.Enemies)
                    {
                        ChangePhase(Phase.Player);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                    return;
                }

                if(entitiesDoneThinking == entitesThatStillNeedToACtBeforePhaseEnds)
                {
                    // At this point all the actors have reevaluted their situation and know their next action.
                    // and we have actors that have actions to take
                    List<Entity> waiters = new List<Entity>(0);
                    List<Entity> movers = new List<Entity>(0);
                    List<Entity> everyoneElse = new List<Entity>(0);

                    foreach(var entity in entities)
                    {
                        if(entity.Behaviour != null && !entity.Behaviour.IsDoneThisTurn)
                        {
                            if(entity.Behaviour.NextAction.GetType() == typeof(Wait))
                            {
                                waiters.Add(entity);
                            }
                            else if(entity.Behaviour.NextAction.GetType() == typeof(Move))
                            {
                                movers.Add(entity);
                            }
                            else
                            {
                                everyoneElse.Add(entity);
                            }
                        }
                    }
                    if(waiters.Count != 0)
                    {
                        // Enqueue waiters
                        var step = new Step();
                        var endTurnStep = new Step();
                        foreach (var waiter in waiters)
                        {
                            step.Actions.AddFirst(waiter.Behaviour.NextAction);
                        }
                        foreach (var waiter in waiters)
                        {
                            endTurnStep.Actions.AddFirst(Context.PrototypeFactory.BuildEntityAction<EndTurn>(waiter));
                        }
                        Steps.AddLast(step);
                        Steps.AddLast(endTurnStep);
                    }
                    else if(movers.Count != 0)
                    {
                        // Batch the non conflicting moves
                        var listOfMovedToPoints = new List<Point>();
                        var moveStep = new Step();
                        var endTurnSTep = new Step();

                        foreach(var entity in movers)
                        {
                            var desiredMove = entity.Behaviour.NextAction as Move;
                            if (!listOfMovedToPoints.Contains(desiredMove.TargetPosition))
                            {
                                listOfMovedToPoints.Add(desiredMove.TargetPosition);
                                moveStep.Actions.AddLast(entity.Behaviour.NextAction);
                                endTurnSTep.Actions.AddLast(Context.PrototypeFactory.BuildEntityAction<EndTurn>(entity));
                            }
                        }
                        Steps.AddLast(moveStep);
                        Steps.AddLast(endTurnSTep);
                        ForceAIsToRecalculateMoves(entities);
                    }
                    else if(everyoneElse.Count != 0)
                    {
                        // Enqueue a combat action
                        var step = new Step();
                        var endTurnStep = new Step();

                        var entityToAct = everyoneElse[0];
                        var nextAction = entityToAct.Behaviour.NextAction;
                        step.Actions.AddLast(nextAction);
                        endTurnStep.Actions.AddLast(Context.PrototypeFactory.BuildEntityAction<EndTurn>(entityToAct));

                        Steps.AddLast(step);
                        Steps.AddLast(endTurnStep);
                        ForceAIsToRecalculateMoves(entities);
                    }
                }
                else
                {
                    // still waiting for entities or player to finish thinking...
                }
            }

        }

        private static void ForceAIsToRecalculateMoves(List<Entity> entities)
        {
            foreach (var entity in entities)
            {
                if (entity.Behaviour != null)
                {
                    if (!entity.Behaviour.IsPlayer)
                    {
                        entity.Behaviour.NextAction = null;
                        entity.Behaviour.IsThinking = false;
                    }
                }
            }
        }

        public void ChangePhase(Phase nextPhase)
        {
            CurrentPhase = nextPhase;
            var entities = Context.GameStateManager.Game.CurrentLevel.Entitys;
            foreach (var entity in entities)
            {
                if (entity.Behaviour != null)
                {
                    entity.Behaviour.IsDoneThisTurn = !ActsInPhase(entity);
                    entity.Behaviour.IsThinking = false;
                    entity.Behaviour.NextAction = null;
                }
            }
        }

        public bool ActsInPhase(Entity entity)
        {
            var playerOrPlayerAllyOnPlayerPhase = entity.Behaviour.Team == Team.PLAYER && CurrentPhase == Phase.Player;
            var enemyOrNeutralOnEnemyPhase = (entity.Behaviour.Team == Team.ENEMY || entity.Behaviour.Team == Team.NEUTRAL) && CurrentPhase == Phase.Enemies;
            return playerOrPlayerAllyOnPlayerPhase || enemyOrNeutralOnEnemyPhase;
        }
    }
}
