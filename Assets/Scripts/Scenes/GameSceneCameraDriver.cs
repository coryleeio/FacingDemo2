using UnityEngine;

namespace Gamepackage
{
    public class GameSceneCameraDriver : MonoBehaviour
    {
        private const float _cameraZOffset = -30;
        public GameObject target;

        public void JumpToTarget(GameObject newTarget)
        {
            target = newTarget;
            var targetPos = new Vector3(target.transform.position.x, target.transform.position.y, _cameraZOffset);
            transform.position = targetPos;
        }

        public void Init()
        {
            this.transform.position = new Vector3(0, 0, _cameraZOffset);
        }

        public void MoveCamera()
        {
            if (target != null)
            {
                var targetPos = new Vector3(target.transform.position.x, target.transform.position.y, _cameraZOffset);
                var nextPos = Vector3.Lerp(this.transform.position, targetPos, Time.fixedDeltaTime);
                transform.position = nextPos;
            }
        }
    }
}
