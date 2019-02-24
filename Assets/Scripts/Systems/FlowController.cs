
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class FlowController
    {
        public LinkedList<Step> Steps = new LinkedList<Step>();
        public Team CurrentlyActingTeam = Team.PLAYER; // always start on player when changing scenes

        public void Init()
        {
            ChangeCurrentlyActingTeam(Team.PLAYER); // always start on player when changing scenes
        }

        public void Process()
        {
            // If any steps have been enqueued we perform them until there are no more
            if (Steps.Count != 0)
            {
                Steps.First.Value.Do();
                if (Steps.First.Value.Actions.Count == 0)
                {
                    Steps.RemoveFirst();
                }
            }
            // Once there are no more steps...
            else
            {
                // Take stock of who from the current team still needs to act
                var entities = Context.Game.CurrentLevel.Entitys;
                var entitiesDoneThinking = 0;
                var entitiesNotDoneThinking = 0;
                var entitesThatStillNeedToACtBeforePhaseEnds = 0;
                foreach (var entity in entities)
                {
                    if (entity.Behaviour != null)
                    {
                        if (entity.Behaviour.IsPlayer && entity.Behaviour.AI == AIType.None)
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
                            else
                            {
                                entitiesNotDoneThinking++;
                            }
                        }
                    }
                }

                // If noone from the current team needs to act, end the phase
                // you need to return here, because you must reevaluate who needs to act in the new phase
                if (entitesThatStillNeedToACtBeforePhaseEnds == 0)
                {
                    var vals = Enum.GetValues(typeof(Team));
                    var numTeams = vals.Length;
                    var currentTeamInt = (int)CurrentlyActingTeam;

                    // Choose the next team.. if you are at the end loop back around and choose the first one again
                    if(currentTeamInt == numTeams - 1)
                    {
                        ChangeCurrentlyActingTeam((Team)vals.GetValue(0));
                    }
                    else
                    {
                        ChangeCurrentlyActingTeam((Team)vals.GetValue(currentTeamInt + 1));
                    }
                    return;
                }

                // If everyone is done thinking (handled by another system)
                // Enqueue their actions into steps and return to the beginning
                // this will cause the steps to be processed before anything else happens.
                // otherwise do nothing until everyone is done thinking.
                if (entitiesDoneThinking == entitesThatStillNeedToACtBeforePhaseEnds)
                {
                    // At this point all the actors have reevaluted their situation and know their next action.
                    // and we have actors that have actions to take
                    List<Entity> waiters = new List<Entity>(0);
                    List<Entity> movers = new List<Entity>(0);
                    List<Entity> combatants = new List<Entity>(0);
                    List<Entity> swappers = new List<Entity>(0);

                    foreach (var entity in entities)
                    {
                        if (entity.Behaviour != null && !entity.Behaviour.IsDoneThisTurn)
                        {
                            if(entity.Behaviour.NextAction == null)
                            {
                                Debug.Log("it");
                            }
                            Assert.IsNotNull(entity.Behaviour.NextAction, "Any entity that is not done at this point should know what they would like to do, somehow an action was not chosen for: " + entity.PrototypeIdentifier.ToString());
                            if (!entity.Behaviour.NextAction.IsValid())
                            {
                                if(entity.Behaviour.NextAction.GetType() == typeof(Move))
                                {
                                    var castMove = (Move)entity.Behaviour.NextAction;
                                    Debug.Log(string.Format("Entity {0} of type {1} discarded its next action of {2}", entity.Id.ToString(), entity.PrototypeIdentifier.ToString(), entity.Behaviour.NextAction.ToString()));
                                    Debug.Log(string.Format("It was in position {0} trying to move to: {1}", entity.Position, castMove.TargetPosition));
                                }
                                else
                                {
                                    Debug.Log(string.Format("Entity {0} of type {1} discarded its next action of {2}", entity.Id.ToString(), entity.PrototypeIdentifier.ToString(), entity.Behaviour.NextAction.ToString()));
                                }
                                if(entity.IsPlayer)
                                {
                                    Debug.Log("Since it is the player the action will be cleared.");
                                    entity.Behaviour.NextAction = null;
                                    entity.Behaviour.IsThinking = false;
                                    continue;
                                }
                                else
                                {
                                    Debug.Log("Since it was not the player, it will wait a turn...");
                                    // Force it to wait, because it couldnt figure out 
                                    // what to do.
                                    var wait = new Wait
                                    {
                                        Source = entity
                                    };
                                    entity.Behaviour.NextAction = wait;
                                    return;
                                }
                            }
                            if (entity.Behaviour.NextAction.GetType() == typeof(Wait))
                            {
                                waiters.Add(entity);
                            }
                            else if (entity.Behaviour.NextAction.GetType() == typeof(Move))
                            {
                                movers.Add(entity);
                            }
                            else if (entity.Behaviour.NextAction.GetType() == typeof(SwapPositionsWithAlly))
                            {
                                swappers.Add(entity);
                            }
                            else
                            {
                                combatants.Add(entity);
                            }
                        }
                    }
                    if (combatants.Count != 0)
                    {
                        // Enqueue a combat action
                        var step = new Step();
                        var endTurnStep = new Step();

                        var entityToAct = combatants[0];
                        var nextAction = entityToAct.Behaviour.NextAction;
                        step.Actions.AddLast(nextAction);
                        var endTurn = new EndTurn
                        {
                            Source = entityToAct
                        };
                        endTurnStep.Actions.AddLast(endTurn);

                        Steps.AddLast(step);
                        Steps.AddLast(endTurnStep);
                        ForceAIsToRecalculateMoves(entities);
                    }
                    else if (swappers.Count != 0)
                    {
                        var step = new Step();
                        var endTurnStep = new Step();
                        foreach (var swapper in swappers)
                        {
                            step.Actions.AddFirst(swapper.Behaviour.NextAction);
                        }
                        foreach (var swapper in swappers)
                        {
                            var endTurn = new EndTurn
                            {
                                Source = swapper
                            };
                            endTurnStep.Actions.AddFirst(endTurn);
                        }
                        Steps.AddLast(step);
                        Steps.AddLast(endTurnStep);
                        ForceAIsToRecalculateMoves(entities);
                    }
                    else if (waiters.Count != 0)
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
                            var endTurn = new EndTurn
                            {
                                Source = waiter
                            };
                            endTurnStep.Actions.AddFirst(endTurn);
                        }
                        Steps.AddLast(step);
                        Steps.AddLast(endTurnStep);
                    }
                    else if (movers.Count != 0)
                    {
                        // Batch the non conflicting moves
                        var listOfMovedToPoints = new List<Point>();
                        var moveStep = new Step();
                        var endTurnSTep = new Step();

                        foreach (var entity in movers)
                        {
                            var desiredMove = entity.Behaviour.NextAction as Move;
                            if (!listOfMovedToPoints.Contains(desiredMove.TargetPosition))
                            {
                                listOfMovedToPoints.Add(desiredMove.TargetPosition);
                                moveStep.Actions.AddLast(entity.Behaviour.NextAction);
                                var endTurn = new EndTurn
                                {
                                    Source = entity
                                };
                                endTurnSTep.Actions.AddLast(endTurn);
                            }
                        }
                        Steps.AddLast(moveStep);
                        Steps.AddLast(endTurnSTep);
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

        public void ChangeCurrentlyActingTeam(Team nextPhase)
        {
            CurrentlyActingTeam = nextPhase;
            var entities = Context.Game.CurrentLevel.Entitys;
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
            var canAct = entity.Body != null && !entity.Body.IsDead && entity.Behaviour != null;
            return canAct && (entity.Behaviour.ActingTeam == CurrentlyActingTeam);
        }
    }
}
