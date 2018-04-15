using UnityEngine;

namespace Gamepackage
{
    public class GameScene : Scene
    {
        private ILogSystem _logger;
        private IPrototypeFactory _prototypeFactory;
        public GameScene(ILogSystem logger, IPrototypeFactory prototypeFactory)
        {
            _logger = logger;
            _prototypeFactory = prototypeFactory;
        }

        protected override void BuildScene()
        {
            GameObject obj = new GameObject();
            obj.name = "Camera";
            obj.AddComponent<Camera>();
        }
    }
}