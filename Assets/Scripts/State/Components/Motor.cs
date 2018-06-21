using System.Collections.Generic;

namespace Gamepackage
{
    public class Motor : Component
    {
        public Motor() {}

        public Pointf LerpCurrentPosition;

        public Pointf LerpTargetPosition;

        public float ElapsedMovementTime;

        public bool HasMovedSinceLastTriggerCheck;

        public bool IsMoving;

        public Queue<Point> CurrentPath = new Queue<Point>(0);

        public Point MoveTargetPosition;
    }
}
