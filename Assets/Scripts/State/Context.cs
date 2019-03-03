namespace Gamepackage
{
    public static class Context
    {
        public static Game Game;
        public static VisibilitySystem VisibilitySystem = new VisibilitySystem();
        public static ResourceManager ResourceManager = new ResourceManager();
        public static EntityManager EntitySystem = new EntityManager();
        public static Localizer Localizer = new Localizer();
        public static SpriteSortingSystem SpriteSortingSystem = new SpriteSortingSystem();
        public static OverlaySystem OverlaySystem = new OverlaySystem();
        public static PathFinder PathFinder = new PathFinder();
        public static FlowController FlowSystem = new FlowController();
        public static PlayerController PlayerController = new PlayerController();

        public static UIController UIController;
        public static GameSceneController GameSceneRoot;
    }
}
