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

            var floating = GetComponentInChildren<Floating>();

            if (floating != null)
            {
                floating.enabled = false;
            }
            mpb = new MaterialPropertyBlock();
        }

        public void Update()
        {
            ElapsedTime = ElapsedTime += Time.deltaTime;

            if (ElapsedTime > 1.0f)
            {
                var spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
                var firstPhasePercentage = (ElapsedTime - 1.0f) / 1f;
                var secondPhasePErcentage = (ElapsedTime - 2f) / 1f;

                if (firstPhasePercentage < 1.0f)
                {
                    var color = Color.Lerp(Color.white, Color.black, firstPhasePercentage);
                    if (spriteRenderers.Length == 0)
                    {
                        var meshRenderer = GetComponentInChildren<MeshRenderer>();
                        meshRenderer.SetPropertyBlock(mpb);
                        mpb.SetColor(colorPropertyName, color);
                    }
                    else
                    {
                        foreach(var spriteRenderer in spriteRenderers)
                        {
                            spriteRenderer.color = color;
                        }
                    }

                }
                else if (secondPhasePErcentage < 1.0f)
                {
                    var color = Color.Lerp(Color.black, DeathColor, secondPhasePErcentage);
                    if (spriteRenderers.Length == 0)
                    {
                        var meshRenderer = GetComponentInChildren<MeshRenderer>();
                        mpb.SetColor(colorPropertyName, color);
                        meshRenderer.SetPropertyBlock(mpb);
                    }
                    else
                    {
                        foreach (var spriteRenderer in spriteRenderers)
                        {
                            spriteRenderer.color = color;
                        }
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
