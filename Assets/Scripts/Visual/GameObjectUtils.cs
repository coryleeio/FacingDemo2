using UnityEngine;

namespace Gamepackage
{
    public static class GameObjectUtils
    {
        public static GameObject MakeFolder(string name)
        {
            var go = new GameObject();
            go.name = name;
            go.transform.position = new Vector3(0, 0, 0);
            go.transform.localScale = new Vector3(1, 1, 1);
            go.transform.localEulerAngles = new Vector3(0, 0, 0);
            return go;
        }
    }
}
