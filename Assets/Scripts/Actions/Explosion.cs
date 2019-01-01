using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class Explosion : Action
    {
        private bool isDoneInternal = false;
        private Entity Source;
        private ExplosionParameters ExplosionParameters;
        private ProjectileAppearance ExplosionAppearance;
        private Point Position;
        private int Index;
        private List<Point> NextPoints = new List<Point>();
        private List<Point> DonePoints = new List<Point>();
        private List<Point> RemnantPoints = new List<Point>();
        private float ElapsedTimeThisTile = 0.0f;

        public override bool IsEndable
        {
            get
            {
                return isDoneInternal;
            }
        }

        public override int TimeCost
        {
            get
            {
                return 0;
            }
        }

        private Explosion() { }

        public Explosion(Entity source, AttackParameters triggeringAttackParameters, Point Location)
        {
            this.Source = source;
            this.ExplosionParameters = triggeringAttackParameters.ExplosionParameters;
            if (ExplosionParameters.ProjectileAppearance != null)
            {
                ExplosionAppearance = ExplosionParameters.ProjectileAppearance;
            }
            this.Position = Location;
        }

        public override bool IsValid()
        {
            return base.IsValid();
        }

        public override void Enter()
        {
            base.Enter();
            Assert.IsNotNull(Source);
            Assert.IsNotNull(ExplosionParameters);
            Assert.IsNotNull(ExplosionAppearance);
            Assert.IsNotNull(Position);
            Assert.IsTrue(ExplosionParameters.Radius > 0, "An explosion with a radius of 0 does nothing");
            
            if(ExplosionAppearance != null && ExplosionAppearance.OnSwingDefinition != null)
            {
                ExplosionAppearance.OnSwingDefinition.Instantiate(Position, Direction.SouthEast);
            }

            MathUtil.FloodFill(Position, Index, ref NextPoints, MathUtil.FloodFillType.Surrounding);
        }

        public override void Do()
        {
            base.Do();
            ElapsedTimeThisTile += Time.deltaTime;
            if(ElapsedTimeThisTile > ExplosionAppearance.OnEnterDefinition.PerTileTravelTime)
            {
                if(Index >= ExplosionParameters.Radius)
                {
                    isDoneInternal = true;
                }
                else
                {
                    ElapsedTimeThisTile = 0.0f;
                    Index++;
                    MathUtil.FloodFill(Position, Index, ref NextPoints, MathUtil.FloodFillType.Surrounding, CombatUtil.FloorTiles);
                    foreach(var point in NextPoints)
                    {
                        if (!DonePoints.Contains(point))
                        {
                            if (ExplosionAppearance != null && ExplosionAppearance.OnEnterDefinition != null)
                            {
                                ExplosionAppearance.OnEnterDefinition.Instantiate(point, Direction.SouthEast);
                            }

                            var game = Context.GameStateManager.Game;
                            var level = game.CurrentLevel;

                            var entitiesInPosition = level.Grid[point].EntitiesInPosition;

                            foreach (var target in entitiesInPosition)
                            {
                                if(target.IsCombatant)
                                {
                                    var esc = new EntityStateChange();
                                    esc.Source = Source;
                                    esc.CombatContext = CombatContext.Zapped;
                                    esc.AttackParameters = ExplosionParameters;
                                    esc.AppliedEffects.AddRange(ExplosionParameters.AppliedEffects);
                                    esc.Target = target;
                                    CombatUtil.ApplyEntityStateChange(esc);
                                }
                            }
                            DonePoints.Add(point);
                        }
                    }

                    foreach(var donePoint in DonePoints)
                    {
                        if(!RemnantPoints.Contains(donePoint))
                        {
                            if (ExplosionAppearance != null && ExplosionAppearance.OnLeaveDefinition != null)
                            {
                                ExplosionAppearance.OnLeaveDefinition.Instantiate(donePoint, Direction.SouthEast);
                            }
                            RemnantPoints.Add(donePoint);
                        }
                    }
                }
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
