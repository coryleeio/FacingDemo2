using System;
using System.Collections.Generic;

namespace Gamepackage
{
    public static class EntityPrototypes
    {
        public static Entity Build(UniqueIdentifier uniqueIdentifier)
        {
            var entity = new Entity();
            entity.PrototypeIdentifier = uniqueIdentifier;

            if (entity.PrototypeIdentifier == UniqueIdentifier.ENTITY_PONCY)
            {
                entity.Name = "Poncy";
                entity.Body = new Body()
                {
                    CurrentHealth = 10,
                    MaxHealth = 10,
                };
                entity.Motor = new Motor();
                entity.BlocksPathing = true;
                entity.View = new View()
                {
                    ViewType = ViewType.StaticSprite,
                    ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_MARKER_BLUE,
                };
                entity.Behaviour = new PlayerBehaviour()
                {
                    Phase = FlowController.Phase.Player
                };
            }
            else if (entity.PrototypeIdentifier == UniqueIdentifier.ENTITY_GIANT_BEE)
            {
                entity.Name = "Giant Bee";
                entity.Body = new Body()
                {
                    CurrentHealth = 1,
                    MaxHealth = 1,
                };
                entity.Motor = new Motor();
                entity.BlocksPathing = true;
                entity.View = new View()
                {
                    ViewType = ViewType.StaticSprite,
                    ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_MARKER_RED,
                };
                entity.Behaviour = new AIBehaviour()
                {
                    Phase = FlowController.Phase.Enemies
                };
            }
            else if (entity.PrototypeIdentifier == UniqueIdentifier.ENTITY_QUEEN_BEE)
            {
                entity.Name = "Queen Bee";
                entity.Body = new Body()
                {
                    CurrentHealth = 3,
                    MaxHealth = 3,
                };
                entity.Motor = new Motor();
                entity.BlocksPathing = true;
                entity.View = new View()
                {
                    ViewType = ViewType.StaticSprite,
                    ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_MARKER_RED,
                };
                entity.Behaviour = new AIBehaviour()
                {
                    Phase = FlowController.Phase.Enemies
                };
            }
            else if (entity.PrototypeIdentifier == UniqueIdentifier.ENTITY_STAIRS_UP)
            {
                entity.Name = "Stairs (Up)";
                entity.BlocksPathing = false;
                entity.View = new View()
                {
                    ViewType = ViewType.StaticSprite,
                    ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_MARKER_GREEN,
                };
                entity.Trigger = new Trigger()
                {
                    // params filled out by dungeon generator
                    Ability = new TraverseStaircase(),
                    Offsets = new List<Point>() { new Point(0,0) }
                };
            }
            else if (entity.PrototypeIdentifier == UniqueIdentifier.ENTITY_STAIRS_DOWN)
            {
                entity.Name = "Stairs (Down)";
                entity.BlocksPathing = false;
                entity.View = new View()
                {
                    ViewType = ViewType.StaticSprite,
                    ViewPrototypeUniqueIdentifier = UniqueIdentifier.VIEW_MARKER_GREEN,
                };
                entity.Trigger = new Trigger()
                {
                    // params filled out by dungeon generator
                    Ability = new TraverseStaircase(),
                    Offsets = new List<Point>() { new Point(0, 0) }
                };
            }
            else
            {
                throw new NotImplementedException();
            }
            return entity;
        }
    }
}
