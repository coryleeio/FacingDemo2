using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Gamepackage
{
    public class SpriteWithMapPosition : MonoBehaviour
    {
        public Point Position;
        private SpriteRenderer _spriteRenderer;
        public SpriteRenderer SpriteRenderer
        {
            get
            {
                if(_spriteRenderer == null)
                {
                    _spriteRenderer = GetComponent<SpriteRenderer>();
                }
                return _spriteRenderer;
            }
            set
            {
                _spriteRenderer = value;
            }
        }
    }
}


