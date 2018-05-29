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
        }

        // Update is called once per frame
        public void Update()
        {
            if (Slider != null && Entity != null)
            {
                Slider.minValue = 0;
                Slider.maxValue = Entity.CombatantComponent.MaxHealth;
                Slider.value = Entity.CombatantComponent.CurrentHealth;
            }
        }
    }
}

