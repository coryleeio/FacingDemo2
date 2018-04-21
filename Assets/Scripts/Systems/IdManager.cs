using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gamepackage
{
    public class IdManager
    {
        private int _nextId = 0;
        public int NextId
        {
            get
            {
                _nextId = _nextId + 1;
                return _nextId;
            }
        }
    }
}
