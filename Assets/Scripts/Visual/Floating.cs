namespace Gamepackage
{
    using UnityEngine;
    public class Floating : MonoBehaviour
    {
        public float startingOffset;
        public float frequency = 3.0f;
        public float amplitude = 0.005f;
        public Shadow shadow;

        void Start()
        {
            startingOffset = Random.Range(0.0f, 1.0f);
            shadow = this.gameObject.transform.parent.GetComponentInChildren<Shadow>();
        }

        public void FixedUpdate()
        {
            var oldX = this.transform.position.x;
            var oldZ = this.transform.position.z;
            var oldY = this.transform.position.y;
            var adjustment = (Mathf.Sin((startingOffset + Time.fixedTime) * frequency) * amplitude);
            if (shadow != null)
            {
                var oldScaleX = shadow.transform.localScale.x;
                var oldScaleY = shadow.transform.localScale.y;
                var oldScaleZ = shadow.transform.localScale.z;
                shadow.transform.localScale = new Vector3(oldScaleX -adjustment, oldScaleY - adjustment, oldScaleZ - adjustment);
            }
            this.transform.position = new Vector3(oldX, oldY + adjustment, oldZ);
        }
    }
}
