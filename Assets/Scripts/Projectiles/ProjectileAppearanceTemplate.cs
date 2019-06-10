using System.Collections.Generic;

namespace Gamepackage
{
    public class ProjectileAppearanceTemplate
    {
        public string Identifier { get; set; }
        public Dictionary<ProjectileComponentTypes, ProjectileAppearanceTemplateComponent> ProjectileAppearanceComponents;

        public ProjectileAppearanceTemplateComponent ProjectileDefinition
        {
            get
            {
                if (ProjectileAppearanceComponents.ContainsKey(ProjectileComponentTypes.Projectile))
                {
                    return ProjectileAppearanceComponents[ProjectileComponentTypes.Projectile];
                }
                else
                {
                    return null;
                }
            }
        }
        public ProjectileAppearanceTemplateComponent OnEnterDefinition
        {
            get
            {
                if (ProjectileAppearanceComponents.ContainsKey(ProjectileComponentTypes.OnEnter))
                {
                    return ProjectileAppearanceComponents[ProjectileComponentTypes.OnEnter];
                }
                else
                {
                    return null;
                }
            }
        }
        public ProjectileAppearanceTemplateComponent OnLeaveDefinition
        {
            get
            {
                if (ProjectileAppearanceComponents.ContainsKey(ProjectileComponentTypes.OnLeave))
                {
                    return ProjectileAppearanceComponents[ProjectileComponentTypes.OnLeave];
                }
                else
                {
                    return null;
                }
            }
        }
        public ProjectileAppearanceTemplateComponent OnHitDefinition
        {
            get
            {
                if (ProjectileAppearanceComponents.ContainsKey(ProjectileComponentTypes.OnHit))
                {
                    return ProjectileAppearanceComponents[ProjectileComponentTypes.OnHit];
                }
                else
                {
                    return null;
                }
            }
        }
        public ProjectileAppearanceTemplateComponent OnSwingDefinition
        {
            get
            {
                if (ProjectileAppearanceComponents.ContainsKey(ProjectileComponentTypes.OnSwing))
                {
                    return ProjectileAppearanceComponents[ProjectileComponentTypes.OnSwing];
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
