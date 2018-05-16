using System.Collections.Generic;

namespace Gamepackage
{
    public class TokenPrototype : IResource
    {
        public UniqueIdentifier UniqueIdentifier { get; set; }
        public bool BlocksPathing { get; set; }
    }
}
