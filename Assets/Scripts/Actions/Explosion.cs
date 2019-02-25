using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class Explosion : Action
    {
        // Inputs
        private CalculatedAttack CalculatedAttack;

        // Derived
        private ExplosionParameters ExplosionParameters;
        private ProjectileAppearance ExplosionProjectileAppearance;

        private bool isDoneInternal = false;
        private int Index = 0;
        private float ElapsedTimeThisTile = 0.0f;

        public override bool IsEndable
        {
            get
            {
                return isDoneInternal;
            }
        }

        private Explosion() { }

        public Explosion(CalculatedAttack calculatedAttack)
        {
            this.CalculatedAttack = calculatedAttack;
            this.ExplosionParameters = calculatedAttack.AttackParameters.ExplosionParameters;
            this.ExplosionProjectileAppearance = calculatedAttack.AttackParameters.ExplosionParameters.ProjectileAppearance;
        }

        public override bool IsValid()
        {
            return base.IsValid();
        }

        public override void Enter()
        {
            base.Enter();
            if (ExplosionParameters.ProjectileAppearance == null)
            {
                Debug.LogWarning("Invisible explosion?");
            }

            if (ExplosionParameters.ProjectileAppearance != null && ExplosionParameters.ProjectileAppearance.OnSwingDefinition != null)
            {
                ViewFactory.InstantiateProjectileAppearance(ExplosionParameters.ProjectileAppearance.OnSwingDefinition, CalculatedAttack.EndpointOfAttack, Direction.SouthEast, null);
            }
        }

        public override void Do()
        {
            base.Do();
            ElapsedTimeThisTile += Time.deltaTime;
            if (ElapsedTimeThisTile > ExplosionProjectileAppearance.OnEnterDefinition.PerTileTravelTime)
            {
                if (Index == ExplosionParameters.Radius)
                {
                    isDoneInternal = true;
                }
                else
                {
                    ElapsedTimeThisTile = 0.0f;
                    Index++;
                    var currentPoints = CalculatedAttack.ExplosionPointsByRadius[Index];
                    var currentStateChanges = CalculatedAttack.ExplosionStateChangesByRadius[Index];
                    foreach(var stateChange in currentStateChanges)
                    {
                        CombatUtil.ApplyEntityStateChange(stateChange);
                    }

                    foreach (var point in currentPoints)
                    {
                        if (ExplosionProjectileAppearance != null && ExplosionProjectileAppearance.OnEnterDefinition != null)
                        {
                            ViewFactory.InstantiateProjectileAppearance(ExplosionProjectileAppearance.OnEnterDefinition, point, Direction.SouthEast, null);
                        }
                    }

                    if (Index - 1 > 0)
                    {
                        var previousPoints = CalculatedAttack.ExplosionPointsByRadius[Index - 1];
                        foreach (var point in previousPoints)
                        {
                            if (ExplosionProjectileAppearance != null && ExplosionProjectileAppearance.OnLeaveDefinition != null)
                            {
                                ViewFactory.InstantiateProjectileAppearance(ExplosionProjectileAppearance.OnLeaveDefinition, point, Direction.SouthEast, null);
                            }
                        }
                    }
                }
            }
        }
    }
}
