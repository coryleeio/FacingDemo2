using UnityEngine;

namespace Gamepackage
{
    public class TokenViewPrototype : IResource
    {
        public string UniqueIdentifier { get; set; }
        public string ClassName { get; set; }
        public Sprite Sprite { get; set; }
    }
}
