using UnityEngine;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class Move : Action
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

        private float TimeBetweenTiles = 0.25f;

        private Vector2 LerpCurrentPosition;
        private Vector2 LerpTargetPosition;

        private float ElapsedMovementTime;

        public Point TargetPosition;
        private bool isDoneInternal = false;

        public override void Enter()
        {
            base.Enter();


            LerpCurrentPosition = MathUtil.MapToWorld(Source.Position);
            LerpTargetPosition = MathUtil.MapToWorld(TargetPosition);
        }


        public override void Do()
        {
            base.Do();
            Assert.IsTrue(TargetPosition != null);
            ElapsedMovementTime += Time.deltaTime;
            if (ElapsedMovementTime > TimeBetweenTiles)
            {
                ElapsedMovementTime = TimeBetweenTiles;
            }

            var lerpPercentarge = ElapsedMovementTime / TimeBetweenTiles;
            var targetVectorPos = LerpTargetPosition;
            var _lerpPos = Vector2.Lerp(LerpCurrentPosition, targetVectorPos, lerpPercentarge);

            if (Source.View != null && Source.View.ViewGameObject != null)
            {
                Source.View.ViewGameObject.transform.position = _lerpPos;
            }
            if (Vector2.Distance(_lerpPos, targetVectorPos) < 0.005f)
            {
                isDoneInternal = true;
            }
        }

        public override void Exit()
        {
            base.Exit();
            // Release old position
            if (Source.BlocksPathing)
            {
                ServiceLocator.GameStateManager.Game.CurrentLevel.Grid[Source.Position].Walkable = true;
            }
            ServiceLocator.GameStateManager.Game.CurrentLevel.UnindexEntity(Source, Source.Position);

            // Move the view to the new position
            if (Source.View != null && Source.View.ViewGameObject != null)
            {
                Source.View.ViewGameObject.transform.position = MathUtil.MapToWorld(TargetPosition);
            }
            
            // Actually set new position
            Source.Position = TargetPosition;

            // Lock new position
            if (Source.BlocksPathing)
            {
                ServiceLocator.GameStateManager.Game.CurrentLevel.Grid[Source.Position].Walkable = false;
            }
            ServiceLocator.GameStateManager.Game.CurrentLevel.IndexEntity(Source, Source.Position);

            foreach (var potentialTrigger in ServiceLocator.GameStateManager.Game.CurrentLevel.Entitys)
            {
                if(potentialTrigger.Trigger != null)
                {
                    var points = MathUtil.GetPointsByOffset(potentialTrigger.Position, potentialTrigger.Trigger.Offsets);
                    if (points.Contains(Source.Position))
                    {
                        var ability = potentialTrigger.Trigger.Ability;
                        ability.Targets.Add(Source);
                        if(ability.CanPerform)
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
