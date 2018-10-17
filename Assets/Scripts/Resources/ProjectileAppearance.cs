namespace Gamepackage
{
    public class ProjectileAppearance : IResource
    {
        public UniqueIdentifier UniqueIdentifier { get; set; }
        public ProjectileAppearanceDefinition OnEnterDefinition;
        public ProjectileAppearanceDefinition OnLeaveDefinition;
        public ProjectileAppearanceDefinition OnHitDefinition;
        public ProjectileAppearanceDefinition OnSwingDefinition;
        public ProjectileAppearanceDefinition ProjectileDefinition;
    }
}
