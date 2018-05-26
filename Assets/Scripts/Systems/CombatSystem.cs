
using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class CombatSystem
    {
        public ApplicationContext Context { get; set; }
        private List<Token> DyingTokens = new List<Token>(0);
        private Color DeathColor = new Color(Color.black.r, Color.black.g, Color.black.b, 0f);

        public CombatSystem()
        {

        }

        public void Process()
        {
            foreach(var token in DyingTokens)
            {
                if(token.View != null)
                {
                    token.ElapsedTimeDead = token.ElapsedTimeDead += Time.deltaTime;
                    if(token.ElapsedTimeDead > 1.0f)
                    {
                        if(token.ViewType == ViewType.StaticSprite)
                        {
                            var spriteRenderer = token.View.GetComponent<SpriteRenderer>();

                            var firstPhasePercentage = (token.ElapsedTimeDead - 1.0f) / 1f;

                            if(firstPhasePercentage < 1.0f)
                            {
                                spriteRenderer.color = Color.Lerp(Color.white, Color.black, firstPhasePercentage);
                            }
                            else
                            {
                                var secondPhasePErcentage = (token.ElapsedTimeDead - 2f) / 1f;
                                spriteRenderer.color = Color.Lerp(Color.black, DeathColor, secondPhasePErcentage);
                            }
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }
                    }
                }
            }
            DyingTokens.RemoveAll((t) => { return t.ElapsedTimeDead > 9.0f; });
        }

        public void DealDamage(Token source, Token target, int damage)
        {
            if(!target.IsCombatant)
            {
                return;
            }
            target.CurrentHealth = target.CurrentHealth - damage;
            if(target.CurrentHealth <= 0)
            {
                var level = Context.GameStateManager.Game.CurrentLevel;
                Context.TokenSystem.Deregister(target, level);
                if(target.TokenPrototype.BlocksPathing)
                {
                    Context.GameStateManager.Game.CurrentLevel.TilesetGrid[target.Position].Walkable = true;
                }
                DyingTokens.Add(target);
            }
        }
    }
}
