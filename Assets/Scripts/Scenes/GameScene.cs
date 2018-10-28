using UnityEngine;

namespace Gamepackage
{
    public class GameScene : Scene
    {
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
            camera.orthographicSize = 4f;
            camera.orthographic = true;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.1f, 0.15f, 0.15f);

            Context.ViewFactory.BuildMapTiles(Context.GameStateManager.Game.CurrentLevel);
            foreach (var entity in Context.GameStateManager.Game.CurrentLevel.Entitys)
            {
                if(entity.View != null)
                {
                    Context.ViewFactory.BuildView(entity);
                }
            }
            var player = Context.GameStateManager.Game.CurrentLevel.Player;
            if(player.View != null && player.View.ViewGameObject != null)
            {
                gameSceneCameraDriver.JumpToTarget(player.Position);
            }
            CameraDriver = gameSceneCameraDriver;
        }

        public GameSceneCameraDriver GetCamera()
        {
            return CameraDriver;
        }
    }
}