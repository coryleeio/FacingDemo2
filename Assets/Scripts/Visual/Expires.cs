namespace Gamepackage
{
    using UnityEngine;

    public class Expires : MonoBehaviour
    {
        public float Lifetime;
        private float TimeElapsed = 0.0f;

        // Update is called once per frame
        void Update()
        {
            TimeElapsed += Time.deltaTime;
            if (TimeElapsed > Lifetime)
            {
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}
