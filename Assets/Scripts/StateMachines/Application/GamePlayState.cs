using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class GamePlayState : IStateMachineState
    {
        private GameSceneCameraDriver CameraDriver;

        public void Enter()
        {
            Context.SpriteSortingSystem.Init();
            Context.GameScene.Load();
            Context.VisibilitySystem.Init();
            Context.OverlaySystem.Init(Context.GameStateManager.Game.CurrentLevel.TilesetGrid.Width, Context.GameStateManager.Game.CurrentLevel.TilesetGrid.Height);
            Context.PathFinder.Init(DiagonalOptions.DiagonalsWithoutCornerCutting, 5);
            Context.FlowSystem.Init();
            Context.PlayerController.Init();
            CameraDriver = Context.GameScene.GetCamera();
            CameraDriver.JumpToTarget();

            var EventSystemPrefab = Resources.Load<GameObject>("UI/EventSystem");
            var eventSYstem = GameObject.Instantiate(EventSystemPrefab);
            eventSYstem.name = "EventSystem";

            var UICanvasPrefab = Resources.Load<GameObject>("UI/UICanvas");
            var UICanvas = GameObject.Instantiate(UICanvasPrefab);
            UICanvas.name = "UICanvas";
            var uiController = UICanvas.GetComponent<UIController>();
            Context.UIController = uiController;
            Context.UIController.Init();
        }

        public void Process()
        {
            Context.PlayerController.Process();
            Context.OverlaySystem.Process();
            Context.PathFinder.Process();
            Context.FlowSystem.Process();
            Context.MovementSystem.Process();
            Context.SpriteSortingSystem.Process();
            Context.VisibilitySystem.Process();
            Context.CombatSystem.Process();
            CameraDriver.MoveCamera();
        }

        public void Exit()
        {
            Context.OverlaySystem.Clear();
            Context.VisibilitySystem.Clear();
            Context.GameScene.Unload();
            Context.PathFinder.Cleanup();
            Context.UIController = null;
        }
    }
}
