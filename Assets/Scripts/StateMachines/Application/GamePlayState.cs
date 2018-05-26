using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class GamePlayState : IStateMachineState
    {
        public ApplicationContext Context { get; set; }
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
        }
    }
}
