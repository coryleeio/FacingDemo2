using UnityEngine;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class SwapPositionsWithAlly : Action
    {
        public override int TimeCost
        {
            get
            {
                return 250;
            }
        }

        public override bool IsEndable
        {
            get
            {
                return isDoneInternal;
            }
        }

        private Vector2 LerpCurrentPositionForSource;
        private Vector2 LerpTargetPositionForSource;

        private Vector2 LerpCurrentPositionForTarget;
        private Vector2 LerpTargetPositionForTarget;

        private float ElapsedMovementTime;

        private bool isDoneInternal = false;

        public override void Enter()
        {
            base.Enter();
            Assert.IsTrue(Targets.Count == 1);
            LerpCurrentPositionForSource = MathUtil.MapToWorld(Source.Position);
            LerpTargetPositionForSource = MathUtil.MapToWorld(Targets[0].Position);

            LerpCurrentPositionForTarget = MathUtil.MapToWorld(Targets[0].Position);
            LerpTargetPositionForTarget = MathUtil.MapToWorld(Source.Position);
            Camera.main.GetComponent<GameSceneCameraDriver>().NewTarget(Targets[0].Position);
        }


        public override void Do()
        {
            base.Do();

            ElapsedMovementTime += Time.deltaTime;

            if (ElapsedMovementTime > Move.TimeBetweenTiles)
            {
                ElapsedMovementTime = Move.TimeBetweenTiles;
            }

            var lerpPercentarge = ElapsedMovementTime / Move.TimeBetweenTiles;

            var _lerpPosSrc = Vector2.Lerp(LerpCurrentPositionForSource, LerpTargetPositionForSource, lerpPercentarge);
            var _lerpPosTarget = Vector2.Lerp(LerpCurrentPositionForTarget, LerpTargetPositionForTarget, lerpPercentarge);

            if (Source.View != null && Source.View.ViewGameObject != null)
            {
                Source.View.ViewGameObject.transform.position = _lerpPosSrc;
            }
            if (Targets[0].View != null && Targets[0].View.ViewGameObject != null)
            {
                Targets[0].View.ViewGameObject.transform.position = _lerpPosTarget;
            }
            if (Vector2.Distance(_lerpPosSrc, LerpTargetPositionForSource) < 0.005f)
            {
                isDoneInternal = true;
            }
        }

        public override void Exit()
        {
            base.Exit();
            ServiceLocator.EntitySystem.Deregister(Source, ServiceLocator.GameStateManager.Game.CurrentLevel);
            ServiceLocator.EntitySystem.Deregister(Targets[0], ServiceLocator.GameStateManager.Game.CurrentLevel);
            var oldSourcePos = new Point(Source.Position.X, Source.Position.Y);
            // Move the view to the new position
            if (Source.View != null && Source.View.ViewGameObject != null)
            {
                Source.View.ViewGameObject.transform.position = MathUtil.MapToWorld(Targets[0].Position);
            }
            if (Targets[0].View != null && Targets[0].View.ViewGameObject != null)
            {
                Targets[0].View.ViewGameObject.transform.position = MathUtil.MapToWorld(oldSourcePos);
            }

            // Actually set new position

            Source.Position = Targets[0].Position;
            Targets[0].Position = oldSourcePos;

            ServiceLocator.GameStateManager.Game.CurrentLevel.Grid[Source.Position].Walkable = !Source.BlocksPathing;
            ServiceLocator.GameStateManager.Game.CurrentLevel.Grid[Targets[0].Position].Walkable = !Targets[0].BlocksPathing;

            ServiceLocator.EntitySystem.Register(Source, ServiceLocator.GameStateManager.Game.CurrentLevel);
            ServiceLocator.EntitySystem.Register(Targets[0], ServiceLocator.GameStateManager.Game.CurrentLevel);

            foreach (var potentialTrigger in ServiceLocator.GameStateManager.Game.CurrentLevel.Entitys)
            {
                if (potentialTrigger.Trigger != null && potentialTrigger.Trigger.Ability.TriggeredBy == Ability.TriggerType.OnTriggerStep)
                {
                    var points = MathUtil.GetPointsByOffset(potentialTrigger.Position, potentialTrigger.Trigger.Offsets);
                    if (points.Contains(Source.Position))
                    {
                        var ability = potentialTrigger.Trigger.Ability;
                        ability.Targets.Add(Source);
                        if (potentialTrigger.Trigger.Ability.CanPerform)
                        {
                            var step = new Step();
                            step.Actions.AddFirst(ability);
                            ServiceLocator.FlowSystem.Steps.AddAfter(ServiceLocator.FlowSystem.Steps.First, step);
                        }
                    }
                    if (points.Contains(Targets[0].Position))
                    {
                        var ability = potentialTrigger.Trigger.Ability;
                        ability.Targets.Add(Targets[0]);
                        if (potentialTrigger.Trigger.Ability.CanPerform)
                        {
                            var step = new Step();
                            step.Actions.AddFirst(ability);
                            ServiceLocator.FlowSystem.Steps.AddAfter(ServiceLocator.FlowSystem.Steps.First, step);
                        }
                    }
                }
            }
        }
    }
}
