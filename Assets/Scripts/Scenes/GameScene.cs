using UnityEngine;

namespace Gamepackage
{
    public class GameScene : Scene
    {
        private ILogSystem _logger;
        public GameScene(ILogSystem logger)
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