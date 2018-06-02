using UnityEngine;

namespace Gamepackage
{
    public class GameScene : Scene
    {
        public Logger Logger { get; set; }
        public PrototypeFactory PrototypeFactory { get; set; }
        public GameStateManager GameStateManager { get; set; }
        public GameSceneCameraDriver CameraDriver { get; set; }

        public GameScene()
        {
        }

        protected override void BuildScene()
        {
            GameObject obj = new GameObject
            {
                tag = "MainCamera",
                name = "Camera"
            };
            var camera = obj.AddComponent<Camera>();
            var gameSceneCameraDriver = obj.AddComponent<GameSceneCameraDriver>();
            obj.transform.position = new Vector3(0, 0, -30);
            camera.orthographicSize = 3f;
            camera.orthographic = true;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.1f, 0.15f, 0.15f);

            PrototypeFactory.BuildMapTiles(GameStateManager.Game.CurrentLevel);
            foreach (var entity in GameStateManager.Game.CurrentLevel.Entitys)
            {
                if(entity.View != null)
                {
                    entity.View.ViewGameObject = PrototypeFactory.BuildView(entity);
                }
            }
            var player = GameStateManager.Game.CurrentLevel.Player;
            if(player.View != null && player.View.ViewGameObject != null)
            {
                gameSceneCameraDriver.target = player.View.ViewGameObject;
            }
            CameraDriver = gameSceneCameraDriver;
        }

        public GameSceneCameraDriver GetCamera()
        {
            return CameraDriver;
        }
    }
}