using UnityEngine;

namespace Gamepackage
{
    public class DeathAnimation : MonoBehaviour
    {
        float ElapsedTime = 0.0f;
        private static Color DeathColor = new Color(Color.black.r, Color.black.g, Color.black.b, 0f);
        MaterialPropertyBlock mpb;
        public string colorPropertyName = "_Color";

        public void Start()
        {
            var healthBar = GetComponentInChildren<HealthBar>();
            if (healthBar != null)
            {
                healthBar.gameObject.SetActive(false);
            }
            var floating = GetComponentInChildren<Floating>();

            if (floating != null)
            {
                floating.gameObject.SetActive(false);
            }
            mpb = new MaterialPropertyBlock();
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
                    var color = Color.Lerp(Color.white, Color.black, firstPhasePercentage);
                    if (spriteRenderer == null)
                    {
                        var meshRenderer = GetComponentInChildren<MeshRenderer>();
                        meshRenderer.SetPropertyBlock(mpb);
                        mpb.SetColor(colorPropertyName, color);
                    }
                    else
                    {
                        spriteRenderer.color = color;
                    }

                }
                else if (secondPhasePErcentage < 1.0f)
                {
                    var color = Color.Lerp(Color.black, DeathColor, secondPhasePErcentage);
                    if (spriteRenderer == null)
                    {
                        var meshRenderer = GetComponentInChildren<MeshRenderer>();
                        mpb.SetColor(colorPropertyName, color);
                        meshRenderer.SetPropertyBlock(mpb);
                    }
                    else
                    {
                        spriteRenderer.color = color;
                    }
                }
                else
                {
                    GameObject.Destroy(this.gameObject);
                }
            }
        }
    }
}
