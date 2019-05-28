using UnityEngine;
using UnityEngine.UI;
namespace Gamepackage
{
    public class HealthBar : MonoBehaviour
    {
        public Slider Slider;
        public Entity Entity;
        public int LastHealth = 0;

        private void Start()
        {
            Slider = GetComponentInChildren<Slider>();

            // Required bc prefab cannot set its type to inherit
            // without an extra useless canvas
            GetComponent<Canvas>().overrideSorting = true;
        }

        public void Update()
        {
            if(Entity != null && Entity.View != null && Entity.View.ViewGameObject != null)
            {
                var newPos = new Vector3(Entity.View.ViewGameObject.transform.position.x, Entity.View.ViewGameObject.transform.position.y + 0.35f, 0.0f);
                this.transform.position = newPos;

                if (Entity.Body != null && Entity.Body.CurrentHealth != LastHealth)
                {
                    LastHealth = Entity.Body.CurrentHealth;
                    Entity.View.HealthBar.UpdateHealth(Entity.Body.CurrentHealth, Entity.CalculateValueOfAttribute(Attributes.MAX_HEALTH));
                }
            }
        }

        public void UpdateHealth(int value, int max)
        {
            if (Slider != null && Entity != null && Entity.Body != null)
            {
                Slider.minValue = 0;
                Slider.maxValue = max;
                Slider.value = value;
            }
        }

        public void UpdateColor(Color color)
        {
            var findImage = transform.Find("Slider").transform.Find("Fill Area").transform.Find("Fill").transform.GetComponent<Image>();
            if(findImage != null)
            {
                findImage.color = color;
            }
        }
    }
}

