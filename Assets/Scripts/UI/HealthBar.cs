using UnityEngine;
using UnityEngine.UI;
namespace Gamepackage
{
    public class HealthBar : MonoBehaviour
    {
        public Slider Slider;
        public Entity Entity;

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
    }
}

