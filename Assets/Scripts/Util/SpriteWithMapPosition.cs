using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Gamepackage
{
    public class SpriteWithMapPosition : MonoBehaviour
    {
        public Point _position;
        public Point Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                if(Sortable != null)
                {
                    Sortable.Position = new Point(_position);
                }
            }
        }


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

        private Sortable _sortable;
        public Sortable Sortable
        {
            get
            {
                if (_sortable == null)
                {
                    _sortable = GetComponent<Sortable>();
                }
                return _sortable;
            }
            set
            {
                _sortable = value;
            }
        }
    }
}


