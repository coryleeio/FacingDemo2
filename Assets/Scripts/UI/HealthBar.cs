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
            if(Entity != null && Entity.ViewGameObject != null)
            {
                var newPos = new Vector3(Entity.ViewGameObject.transform.position.x, Entity.ViewGameObject.transform.position.y + 0.35f, 0.0f);
                this.transform.position = newPos;

                if (Entity.IsCombatant && Entity.CurrentHealth != LastHealth)
                {
                    LastHealth = Entity.CurrentHealth;
                    Entity.HealthBar.UpdateHealth(Entity.CurrentHealth, Entity.CalculateValueOfAttribute(Attributes.MaxHealth));
                }
            }
        }

        public void UpdateHealth(int value, int max)
        {
            if (Slider != null && Entity != null && Entity.IsCombatant)
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

