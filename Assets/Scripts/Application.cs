using UnityEngine;

namespace Gamepackage
{
    public class Application : MonoBehaviour
    {
        public ApplicationStateMachine StateMachine = new ApplicationStateMachine();

        void Start()
        {
            DontDestroyOnLoad(this);
            ServiceLocator.Application = this;
            StateMachine.ChangeState(ApplicationStateMachine.MainMenuState);
        }

        void Update()
        {
            StateMachine.Process();
        }

        void OnDisable()
        {
            ServiceLocator.PathFinder.Cleanup();
        }

        void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 100, 50), "Start New game"))
            {
                ServiceLocator.GameStateManager.Clear();
                StateMachine.ChangeState(ApplicationStateMachine.LoadingResourcesState);
            }

            if (GUI.Button(new Rect(10, 75, 100, 50), "Exit game"))
            {
                ServiceLocator.GameStateManager.Clear();
                StateMachine.ChangeState(ApplicationStateMachine.MainMenuState);
            }

            if (GUI.Button(new Rect(10, 140, 100, 50), "Reload current"))
            {
                StateMachine.ChangeState(ApplicationStateMachine.GamePlayState);
            }

            if (GUI.Button(new Rect(10, 215, 100, 50), "Save"))
            {
                ServiceLocator.GameStateManager.SaveGame();
            }

            if (GUI.Button(new Rect(10, 290, 100, 50), "Load"))
            {
                ServiceLocator.GameStateManager.LoadGame();
                StateMachine.ChangeState(ApplicationStateMachine.LoadingResourcesState);
            }

            if (GUI.Button(new Rect(10, 350, 100, 50), "Reveal"))
            {
                var newVis = new bool[40, 40];
                for (var x = 0; x < 40; x++)
                {
                    for (var y = 0; y < 40; y++)
                    {
                        newVis[x, y] = true;
                    }
                }
                ServiceLocator.VisibilitySystem.UpdateVisibility(newVis);
            }

            if (Camera.main != null)
            {
                var mouseMapPosition = MathUtil.GetMousePositionOnMap(Camera.main);
                if (GUI.Button(new Rect(150, 10, 100, 50), mouseMapPosition.ToString()))
                {

                }
            }
        }
    }
}