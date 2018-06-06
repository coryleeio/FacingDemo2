using Gamepackage;
using Newtonsoft.Json;
using System.Collections.Generic;

public class TryMoveToward : EntityAction
{
    public int TargetId = -1;
    public Point TargetPoint;
    public List<Point> PointsAroundTarget = new List<Point>();
    public List<Point> PointsAroundMe = new List<Point>();
    private bool waitingOnPath = false;
    public Path Path;

    [JsonIgnore]
    private Entity _target;
    [JsonIgnore]
    public Entity Target
    {
        get
        {
            if (_target == null)
            {
                _target = ServiceLocator.EntitySystem.GetEntityById(TargetId);
            }
            return _target;
        }
    }

    public override int TimeCost
    {
        get
        {
            return 0; // this action will either wait or move, the cost depends on that
        }
    }

    public override bool IsEndable
    {
        get
        {
            return Path != null || Target == null;
        }
    }

    public override bool IsAMovementAction
    {
        get
        {
            return false;
        }
    }

    public override bool IsStartable
    {
        get
        {
            return TargetId != -1;
        }
    }

    public override void Enter()
    {
        if (Target != null)
        {
            var level = ServiceLocator.GameStateManager.Game.CurrentLevel;
            PointsAroundTarget = MathUtil.OrthogonalPoints(Target.Position).FindAll((p) => { return level.TilesetGrid[p].Walkable; });
            PointsAroundMe = MathUtil.OrthogonalPoints(Entity.Position).FindAll((p) => { return level.TilesetGrid[p].Walkable && Point.DistanceSquared(p, Target.Position) < Point.DistanceSquared(Entity.Position, Target.Position); });

            PointsAroundMe.Sort(new PointDistanceComparer()
            {
                Source = Entity.Position
            });

            FindTargetPosition();
        }
        base.Enter();
    }

    private void FindTargetPosition()
    {
        if (PointsAroundTarget.Count > 0)
        {
            TargetPoint = MathUtil.ChooseRandomElement<Point>(PointsAroundTarget);
        }
        else if (PointsAroundMe.Count > 0)
        {
            TargetPoint = PointsAroundMe[0];
        }
        else
        {
            TargetPoint = Target.Position;
        }
    }

    [JsonIgnore]
    public Grid<Tile> Grid
    {
        get
        {
            return ServiceLocator.GameStateManager.Game.CurrentLevel.TilesetGrid;
        }
    }

    public override void Process()
    {
        base.Process();
        if (!waitingOnPath)
        {
            ServiceLocator.PathFinder.StartPath(Entity.Position, TargetPoint, Grid, this.PathComplete);
            waitingOnPath = true;
        }
    }

    public void PathComplete(Path path)
    {
        waitingOnPath = false;
        Path = path;
        var thisAction = Entity.Behaviour.ActionList.Find(this);
        if (thisAction == null)
        {
            var wait = ServiceLocator.PrototypeFactory.BuildEntityAction<Wait>(Entity);
            Entity.Behaviour.ActionList.AddFirst(wait);
            return;
        }
        if (path.Nodes.Count == 0)
        {
            if (PointsAroundTarget.Contains(TargetPoint))
            {
                PointsAroundTarget.Remove(TargetPoint);
            }

            if(PointsAroundMe.Contains(TargetPoint))
            {
                PointsAroundMe.Remove(TargetPoint);
            }

            if (PointsAroundTarget.Count > 0)
            {
                TargetPoint = MathUtil.ChooseRandomElement<Point>(PointsAroundTarget);
                ServiceLocator.PathFinder.StartPath(Entity.Position, TargetPoint, Grid, this.PathComplete);
                return;
            }
            else if(PointsAroundMe.Count > 0)
            {
                TargetPoint = PointsAroundMe[0];
                ServiceLocator.PathFinder.StartPath(Entity.Position, TargetPoint, Grid, this.PathComplete);
                return;
            }
            else
            {
                var wait = ServiceLocator.PrototypeFactory.BuildEntityAction<Wait>(Entity);
                Entity.Behaviour.ActionList.AddAfter(thisAction, wait);
            }
        }
        else if (!Grid[path.Nodes[0].Position].Walkable)
        {
            Path = null; // Keep searching - someone moved into this tile while we were looking for a path
        }
        else
        {
            var move = ServiceLocator.PrototypeFactory.BuildEntityAction<Move>(Entity);
            move.TargetLocation = new Point(path.Nodes[0].Position.X, path.Nodes[0].Position.Y);
            thisAction = Entity.Behaviour.ActionList.Find(this);

            Entity.Behaviour.ActionList.AddAfter(thisAction, move);
            // Do a tick of the move so that we reserve the spot we want to move to.
            move.Do();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
