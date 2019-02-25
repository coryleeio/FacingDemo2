using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gamepackage
{
    public class GameSceneRoot : MonoBehaviour
    {
        public bool Stopped = false;

        public void Start()
        {
            if (Context.Game == null)
            {
                SaveUtil.NewGame();
                DungeonGenerator.GenerateDungeon();
                Context.ResourceManager.LoadAllPrototypes();
                Stopped = false;
            }
            GameObject obj = new GameObject
            {
                tag = "MainCamera",
                name = "Camera"
            };
            var camera = obj.AddComponent<Camera>();
            var gameSceneCameraDriver = obj.AddComponent<GameSceneCameraDriver>();
            obj.transform.position = new Vector3(0, 0, 30);
            camera.orthographicSize = 4f;
            camera.orthographic = true;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.1f, 0.15f, 0.15f);

            ViewFactory.BuildMapTiles(Context.Game.CurrentLevel);
            foreach (var entity in Context.Game.CurrentLevel.Entitys)
            {
                if (entity.View != null)
                {
                    ViewFactory.RebuildView(entity);
                }
            }
            var player = Context.Game.CurrentLevel.Player;
            if (player.View != null && player.View.ViewGameObject != null)
            {
                gameSceneCameraDriver.JumpToTarget(player.Position);
            }
            CameraDriver = gameSceneCameraDriver;

            Context.EntitySystem.Init();
            Context.SpriteSortingSystem.Init();
            Context.VisibilitySystem.Init();
            Context.OverlaySystem.Init(Context.Game.CurrentLevel.Grid.Width, Context.Game.CurrentLevel.Grid.Height);
            Context.PathFinder.Init(DiagonalOptions.NoDiagonals, 5);
            Context.FlowSystem.Init();
            Context.PlayerController.Init();
            CameraDriver = GetCamera();
            CameraDriver.JumpToTarget(Context.Game.CurrentLevel.Player.Position);

            var EventSystemPrefab = Resources.Load<GameObject>("UI/EventSystem");
            var eventSYstem = GameObject.Instantiate(EventSystemPrefab);
            eventSYstem.name = "EventSystem";

            var UICanvasPrefab = Resources.Load<GameObject>("UI/UICanvas");
            var UICanvas = GameObject.Instantiate(UICanvasPrefab);
            UICanvas.name = "UICanvas";
            var uiController = UICanvas.GetComponent<UIController>();
            Context.UIController = uiController;
            Context.UIController.Init();
            Context.GameSceneRoot = this;
        }

        public void Update()
        {
            if (!Stopped)
            {
                Context.PlayerController.Process();
                // Player controller can cause a scene change which will cause these to error out
                // as the state will change.
                if (!Stopped)
                {
                    Context.OverlaySystem.Process();
                    Context.PathFinder.Process();
                    Context.FlowSystem.Process();
                    Context.VisibilitySystem.Process();
                    CameraDriver.MoveCamera();
                }
            }
        }

        public void OnDisable()
        {
            Context.OverlaySystem.Clear();
            Context.VisibilitySystem.Clear();
            Context.PathFinder.Cleanup();
            Context.UIController = null;
            Context.PathFinder.Cleanup();
        }

        public GameSceneCameraDriver CameraDriver { get; set; }
        public GameSceneCameraDriver GetCamera()
        {
            return CameraDriver;
        }
    }
}
