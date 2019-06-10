using Newtonsoft.Json;
using UnityEngine;

namespace Gamepackage
{
    public class Explosion : Action
    {
        [JsonIgnore]
        public override Entity Source
        {
            get; set;
        }

        // Inputs
        private CalculatedCombatAction CalculatedCombatAction;

        // Derived
        private CombatActionParameters ExplosionParameters;
        private ProjectileAppearanceTemplate ExplosionProjectileAppearance;

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

        public Explosion(CalculatedCombatAction calculatedAttack)
        {
            this.CalculatedCombatAction = calculatedAttack;
            this.ExplosionParameters = calculatedAttack.ResolvedCombatActionParameters.ExplosionParameters;
            this.ExplosionProjectileAppearance = calculatedAttack.ResolvedCombatActionParameters.ExplosionParameters.ProjectileAppearance;
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
                ViewFactory.InstantiateProjectileAppearance(ExplosionParameters.ProjectileAppearance.OnSwingDefinition, CalculatedCombatAction.EndpointOfAttack, Direction.SouthEast, null);
            }
        }

        public override void Do()
        {
            base.Do();
            ElapsedTimeThisTile += Time.deltaTime;

            if(ExplosionProjectileAppearance.OnEnterDefinition == null)
            {
                Debug.LogWarning("This explosion had no OnEnterDefinition, so it wont show anything.");
                isDoneInternal = true;
                CombatUtil.ApplyAttackInstantly(CalculatedCombatAction);
                return;
            }
            if (ElapsedTimeThisTile > ExplosionProjectileAppearance.OnEnterDefinition.TimeSpentInEachTile)
            {
                if (Index == ExplosionParameters.Range)
                {
                    isDoneInternal = true;
                }
                else
                {
                    ElapsedTimeThisTile = 0.0f;
                    Index++;
                    var currentPoints = CalculatedCombatAction.ExplosionPointsByRadius[Index];
                    var currentStateChanges = CalculatedCombatAction.ExplosionStateChangesByRadius[Index];
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
                        var previousPoints = CalculatedCombatAction.ExplosionPointsByRadius[Index - 1];
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
