using UnityEngine;
using UnityEngine.Profiling;

namespace Gamepackage
{
    public class GameSceneController : MonoBehaviour
    {
        public bool Stopped = false;

        public void Start()
        {
            if (Context.Game == null)
            {
                // So play button works in editor
                SaveUtil.NewGame();
                DungeonGenerator.GenerateDungeon();
                Context.ResourceManager.LoadAllPrototypes();
                Stopped = false;
            }

            SceneUtil.FindOrCreateEventSystem();
            SceneUtil.FindOrCreateGameSceneCamera();
            SceneUtil.FindOrCreatUIController();

            var gameSceneCameraDriver = Camera.main.gameObject.GetComponent<GameSceneCameraDriver>();

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

            Context.UIController.Init();
            Context.EntitySystem.Init();
            Context.SpriteSortingSystem.Init();
            Context.VisibilitySystem.Init();
            Context.OverlaySystem.Init(Context.Game.CurrentLevel.Grid.Width, Context.Game.CurrentLevel.Grid.Height);
            Context.PathFinder.Init(DiagonalOptions.NoDiagonals, 5);
            Context.FlowSystem.Init();
            Context.PlayerController.Init();
            CameraDriver = GetCamera();
            CameraDriver.JumpToTarget(Context.Game.CurrentLevel.Player.Position);
            Context.UIController.FloatingCombatTextManager.Show();
            Context.UIController.TextLog.ClearText();
            Context.UIController.TextLog.Show();
            Context.GameSceneRoot = this;
            Context.VisibilitySystem.UpdateVisibility();
        }

        public void Update()
        {
            Profiler.BeginSample("GameSceneControllerUpdate");
            if (!Stopped)
            {
                Profiler.BeginSample("PlayerController");
                Context.PlayerController.Process();
                Profiler.EndSample();
                // Player controller can cause a scene change which will cause these to error out
                // as the state will change.
                if (!Stopped)
                {
                    Profiler.BeginSample("OverlaySystem");
                    Context.OverlaySystem.Process();
                    Profiler.EndSample();

                    Profiler.BeginSample("PathFinder");
                    Context.PathFinder.Process();
                    Profiler.EndSample();

                    Profiler.BeginSample("FlowSystem");
                    Context.FlowSystem.Process();
                    Profiler.EndSample();

                    CameraDriver.MoveCamera();
                }
            }
            Profiler.EndSample();
        }

        public void OnDisable()
        {
            Context.OverlaySystem.Clear();
            Context.VisibilitySystem.Clear();
            Context.PathFinder.Cleanup();
            Context.PathFinder.Cleanup();
            Context.UIController = null;
        }

        public GameSceneCameraDriver CameraDriver { get; set; }
        public GameSceneCameraDriver GetCamera()
        {
            return CameraDriver;
        }
    }
}
