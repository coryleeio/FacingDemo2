namespace Gamepackage
{
    public static class ServiceLocator
    {
        public static GameStateManager GameStateManager = new GameStateManager();
        public static VisibilitySystem VisibilitySystem = new VisibilitySystem();
        public static ResourceManager ResourceManager = new ResourceManager();
        public static PrototypeFactory PrototypeFactory = new PrototypeFactory();
        public static EntityManager EntitySystem = new EntityManager();
        public static DungeonGenerator DungeonGenerator = new DungeonGenerator();
        public static SpriteSortingSystem SpriteSortingSystem = new SpriteSortingSystem();
        public static OverlaySystem OverlaySystem = new OverlaySystem();
        public static PathFinder PathFinder = new PathFinder();
        public static CombatSystem CombatSystem = new CombatSystem();
        public static GameScene GameScene = new GameScene();
        public static LoadingScene LoadingScene = new LoadingScene();
        public static MainMenuScene MainMenuScene = new MainMenuScene();
        public static FlowController FlowSystem = new FlowController();
        public static PlayerController PlayerController = new PlayerController();

        public static Application Application; // mono
        public static UIController UIController; // mono
    }
}
