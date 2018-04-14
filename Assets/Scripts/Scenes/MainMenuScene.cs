using UnityEngine;

namespace Gamepackage
{
    public class MainMenuScene : Scene
    {
        private ILogSystem _logger;
        public MainMenuScene(ILogSystem logger)
        {
            _logger = logger;
        }

        protected override void BuildScene()
        {
            GameObject obj = new GameObject();
            obj.name = "Camera";
            obj.AddComponent<Camera>();
        }
    }
}