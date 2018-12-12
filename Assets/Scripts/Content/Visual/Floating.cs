namespace Gamepackage
{
    using UnityEngine;
    public class Floating : MonoBehaviour
    {
        public float startingOffset;
        public float frequency = 3.0f;
        public float amplitude = 0.005f;
        void Start()
        {
            startingOffset = Random.Range(0.0f, 1.0f);
        }
        public void Update()
        {
            var oldX = this.transform.position.x;
            var oldZ = this.transform.position.z;
            var oldY = this.transform.position.y;
            this.transform.position = new Vector3(oldX, oldY + (Mathf.Sin((startingOffset + Time.time) * frequency) * amplitude), oldZ);
        }
    }
}
