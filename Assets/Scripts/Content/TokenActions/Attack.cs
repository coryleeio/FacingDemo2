using Newtonsoft.Json;

namespace Gamepackage
{
    public class Attack : TokenAction
    {
        private float TimeStart;
        private float Duration = 0.5f;

        public int TargetTokenId;
        [JsonIgnore]
        public Token TargetToken
        {
            get
            {
                return Context.TokenSystem.GetTokenById(TargetTokenId);
            }
        }

        public override int TimeCost
        {
            get
            {
                return 250;
            }
        }

        public override void Enter()
        {
            base.Enter();
            TimeStart = UnityEngine.Time.deltaTime;
        }

        public override void Exit()
        {
            base.Exit();
            Context.CombatSystem.DealDamage(Token, TargetToken, 1);
        }

        public override bool IsEndable
        {
            get
            {
                return TimeStart + Duration >= UnityEngine.Time.deltaTime;
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
                return Token.Position.IsOrthogonalTo(TargetToken.Position);
            }
        }
    }
}
