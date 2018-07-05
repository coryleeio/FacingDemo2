using UnityEngine;

namespace Gamepackage
{
    public class DeathAnimation : MonoBehaviour
    {
        float ElapsedTime = 0.0f;
        private static Color DeathColor = new Color(Color.black.r, Color.black.g, Color.black.b, 0f);

        public void Start()
        {
            var healthBar = GetComponentInChildren<HealthBar>();
            if (healthBar != null)
            {
                healthBar.gameObject.SetActive(false);
            }
        }

        public void Update()
        {
            ElapsedTime = ElapsedTime += Time.deltaTime;

            if (ElapsedTime > 1.0f)
            {
                var spriteRenderer = GetComponent<SpriteRenderer>();
                var firstPhasePercentage = (ElapsedTime - 1.0f) / 1f;
                var secondPhasePErcentage = (ElapsedTime - 2f) / 1f;

                if (firstPhasePercentage < 1.0f)
                {
                    spriteRenderer.color = Color.Lerp(Color.white, Color.black, firstPhasePercentage);
                }
                else if (secondPhasePErcentage < 1.0f)
                {
                    spriteRenderer.color = Color.Lerp(Color.black, DeathColor, secondPhasePErcentage);
                }
                else
                {
                    GameObject.Destroy(this.gameObject);
                }
            }
        }
    }
}
