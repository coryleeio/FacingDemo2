using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class GamePlayState : IStateMachineState
    {
        private GameSceneCameraDriver CameraDriver;

        public void Enter()
        {
            ServiceLocator.EntitySystem.Init();
            ServiceLocator.SpriteSortingSystem.Init();
            ServiceLocator.GameScene.Load();
            ServiceLocator.VisibilitySystem.Init();
            ServiceLocator.OverlaySystem.Init(ServiceLocator.GameStateManager.Game.CurrentLevel.Grid.Width, ServiceLocator.GameStateManager.Game.CurrentLevel.Grid.Height);
            ServiceLocator.PathFinder.Init(DiagonalOptions.DiagonalsWithoutCornerCutting, 5);
            ServiceLocator.FlowSystem.Init();
            ServiceLocator.PlayerController.Init();
            CameraDriver = ServiceLocator.GameScene.GetCamera();
            CameraDriver.JumpToTarget(ServiceLocator.GameStateManager.Game.CurrentLevel.Player.Position);

            var EventSystemPrefab = Resources.Load<GameObject>("UI/EventSystem");
            var eventSYstem = GameObject.Instantiate(EventSystemPrefab);
            eventSYstem.name = "EventSystem";

            var UICanvasPrefab = Resources.Load<GameObject>("UI/UICanvas");
            var UICanvas = GameObject.Instantiate(UICanvasPrefab);
            UICanvas.name = "UICanvas";
            var uiController = UICanvas.GetComponent<UIController>();
            ServiceLocator.UIController = uiController;
            ServiceLocator.UIController.Init();
        }

        public void Process()
        {
            ServiceLocator.PlayerController.Process();
            ServiceLocator.OverlaySystem.Process();
            ServiceLocator.PathFinder.Process();
            ServiceLocator.FlowSystem.Process();
            ServiceLocator.SpriteSortingSystem.Process();
            ServiceLocator.VisibilitySystem.Process();
            CameraDriver.MoveCamera();
        }

        public void Exit()
        {
            ServiceLocator.OverlaySystem.Clear();
            ServiceLocator.VisibilitySystem.Clear();
            ServiceLocator.GameScene.Unload();
            ServiceLocator.PathFinder.Cleanup();
            ServiceLocator.UIController = null;
        }
    }
}
