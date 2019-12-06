using UnityEngine;

namespace Gamepackage
{
    public class GameSceneCameraDriver : MonoBehaviour
    {
        private const float _cameraZOffset = -30;
        private Vector3 targetPosition;
        private const float _totalMoveTime = 0.25f;

        public void JumpToTarget(Point p)
        {
            var targetp = MathUtil.MapToWorld(p);
            targetPosition = new Vector3(targetp.x, targetp.y, _cameraZOffset);
            transform.position = targetPosition;
        }

        public void Init()
        {
            this.transform.position = new Vector3(0, 0, _cameraZOffset);
            targetPosition = new Vector2(0, 0);
        }

        public void MoveCamera()
        {
            if (targetPosition != null && (targetPosition.x != this.transform.position.x || targetPosition.y != this.transform.position.y))
            {
                var percentToMove = Time.deltaTime/_totalMoveTime;
                var nextPos = Vector3.Slerp(this.transform.position, targetPosition, percentToMove);
                transform.position = nextPos;
            }
        }

        public void NewTarget(Point p)
        {
            var targetp = MathUtil.MapToWorld(p);
            targetPosition = new Vector3(targetp.x, targetp.y, _cameraZOffset);
        }
    }
}
