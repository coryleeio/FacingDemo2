
using UnityEngine;

namespace Gamepackage
{
    public class LogSystem : ILogSystem
    {
        public void Error(string message)
        {
            Debug.LogError(message);
        }

        public void Log(string message)
        {
            Debug.Log(message);
        }

        public void Warn(string message)
        {
            Debug.LogWarning(message);
        }
    }
}