using UnityEngine;
using UnityEngine.UI;

namespace Gamepackage
{
    public class FloatingText : MonoBehaviour
    {
        public float LeftRightOffset = 0.0f; // set by manager. otherwise floats straight up.
        private float lifetime = 1.0f;
        public float elapsedTime = 0.0f;
        private Color StartingColor = Color.black;
        private Color EndingColor = Color.black;
        private Text TextComponent;
        private Vector3 StartingPosition;
        private Vector3 EndingPosition;

        void Start()
        {
            TextComponent = GetComponent<Text>();
            StartingPosition = TextComponent.rectTransform.position;
            EndingPosition = new Vector3(StartingPosition.x + LeftRightOffset, StartingPosition.y + 0.5f, StartingPosition.z);
            StartingColor = TextComponent.color;
            EndingColor = new Color(StartingColor.r, StartingColor.g, StartingColor.b, 0.0f);
        }

        void Update()
        {
            TextComponent.color = Color.Lerp(StartingColor, EndingColor, elapsedTime / lifetime);
            TextComponent.rectTransform.position = Vector3.Lerp(StartingPosition, EndingPosition, elapsedTime / lifetime);
            elapsedTime += Time.deltaTime;
            if(elapsedTime > lifetime)
            {
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}
