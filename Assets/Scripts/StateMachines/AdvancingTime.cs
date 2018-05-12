using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gamepackage
{
    public class AdvancingTime : IStateMachineState<Root>
    {
        public ILogSystem LogSystem { get; set; }
        public IGameStateSystem GameStateSystem { get; set; }

        public void Enter(Root owner)
        {
            var game = GameStateSystem.Game;
            
            foreach(var token in GameStateSystem.Game.CurrentLevel.Tokens)
            {
                
            }
        }

        public void Exit(Root owner)
        {
            
        }

        public void HandleMessage(Message messageToHandle)
        {
            
        }

        public void Process(Root owner)
        {

        }
    }
}
