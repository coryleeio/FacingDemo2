using System.Collections.Generic;

namespace Gamepackage
{
    public class MovementComponent : Component
    {
        public MovementComponent()
        {

        }

        public MovementComponent(MovementComponent other)
        {

        }

        public Pointf LerpCurrentPosition;

        public Pointf LerpTargetPosition;

        public float ElapsedMovementTime;

        public bool HasMovedSinceLastTriggerCheck;

        public bool IsMoving;

        public Queue<Point> CurrentPath = new Queue<Point>(0);

        public Point TargetPosition;

        public override void InjectContext(ApplicationContext context)
        {
            base.InjectContext(context);
        }
    }
}
