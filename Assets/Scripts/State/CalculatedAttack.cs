using System.Collections.Generic;

namespace Gamepackage
{
    public class CalculatedAttack
    {
        public Entity Source;
        public AttackType AttackType;
        public AttackParameters AttackParameters;
        public AttackTypeParameters AttackTypeParameters;
        public Item Item;
        public Direction DirectionOfAttack;
        public Point TargetPosition;
        public Dictionary<int, List<Point>> ExplosionPointsByRadius = new Dictionary<int, List<Point>>();
        public Dictionary<int, List<EntityStateChange>> ExplosionStateChangesByRadius = new Dictionary<int, List<EntityStateChange>>();
        public List<EntityStateChange> ExplosionStateChanges = new List<EntityStateChange>();
        public List<EntityStateChange> AttackStateChanges = new List<EntityStateChange>();
        public List<EntityStateChange> SourceStateChanges = new List<EntityStateChange>();
        public List<Point> PointsPossiblyAffectedBeforeTargetPiercing = new List<Point>();
        public List<Point> PointsAffectedByAttack = new List<Point>();
        public int NumberOfTargetsPierced;
        public Point EndpointOfAttack;
        public List<ItemStateChange> ItemStateChanges = new List<ItemStateChange>(0);
        public List<GroundDropSpawn> GroundDropsToSpawn = new List<GroundDropSpawn>(0);
    }
}
