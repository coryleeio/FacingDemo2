using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class Equals : ConditionalImpl
    {
        public enum Parameters
        {
            Key,
            Value,
        }

        public bool Satisfied(Game game, Dialog state, Dictionary<string, string> parameters)
        {
            return SatisfiedShared(game, state, parameters);
        }

        public static bool SatisfiedShared(Game game, Dialog state, Dictionary<string, string> parameters)
        {
            Assert.IsTrue(parameters.ContainsKey(Parameters.Key.ToString()));
            Assert.IsTrue(parameters.ContainsKey(Parameters.Value.ToString()));

            if (state.Data.ContainsKey(parameters[Parameters.Key.ToString()]))
            {
                var value = state.Data[parameters[Parameters.Key.ToString()]];
                var expectedValue = parameters[Parameters.Value.ToString()];
                return value == expectedValue;
            }
            else
            {
                return false;
            }
        }
    }
}
