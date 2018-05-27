using UnityEngine;
using UnityEngine.UI;
namespace Gamepackage
{
    public class HealthBar : MonoBehaviour
    {
        public Slider Slider;
        public Token Token;

        private void Start()
        {
            Slider = GetComponentInChildren<Slider>();
        }

        // Update is called once per frame
        public void Update()
        {
            if (Slider != null && Token != null)
            {
                Slider.minValue = 0;
                Slider.maxValue = Token.MaxHealth;
                Slider.value = Token.CurrentHealth;
            }
        }
    }
}

