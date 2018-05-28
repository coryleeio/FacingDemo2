using TinyIoC;

namespace Gamepackage
{
    public class ApplicationContext
    {
        public Application Application { get; set; }
        public GamePlayState GamePlayState { get; set; }
        public MainMenuState MainMenuState { get; set; }
        public LoadingResourcesState LoadingResourcesState { get; set; }
        public GameStateManager GameStateManager { get; set; }
        public VisibilitySystem VisibilitySystem { get; set; }
        public ResourceManager ResourceManager { get; set; }
        public PrototypeFactory PrototypeFactory { get; set; }
        public EntityManager EntitySystem { get; set; }
        public DungeonGenerator DungeonGenerator { get; set; }
        public SpriteSortingSystem SpriteSortingSystem { get; set; }
        public OverlaySystem OverlaySystem { get; set; }
        public PathFinder PathFinder { get; set; }
        public CombatSystem CombatSystem { get; set; }
        public GameScene GameScene { get; set; }
        public LoadingScene LoadingScene { get; set; }
        public MainMenuScene MainMenuScene { get; set; }
        public MovementSystem MovementSystem { get; set; }
        public FlowSystem FlowSystem { get; set; }
        public DoTurn DoTurn { get; set; }
        public DoTriggers DoTriggers { get; set; }
        public PlayerController PlayerController { get; set; }
    }
}
