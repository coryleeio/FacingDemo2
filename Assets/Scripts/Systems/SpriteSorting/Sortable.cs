using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public class Sortable : MonoBehaviour, IComparable<Sortable>
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
                var oldPosition = _position;
                var newPosition = value;
                if (Context.SpriteSortingSystem != null)
                {
                    Context.SpriteSortingSystem.Move(this, oldPosition, newPosition);
                }
                _position = newPosition;
            }
        }

        public Point _positionRelativeToParent;
        public Point PositionRelativeToParent
        {
            get
            {
                return _positionRelativeToParent;
            }
            set
            {
                _positionRelativeToParent = value;
                RecalculatePositionRelativeToParent();
            }
        }

        public void RecalculatePositionRelativeToParent()
        {
            if (Parent != null)
            {
                Position = new Point(Parent.Position.X + _positionRelativeToParent.X, Parent.Position.Y + _positionRelativeToParent.Y);
            }
        }

        public int SpriteSortableId;
        public List<Sortable> Children;
        public Sortable Parent;
        public SortingLayer Layer;
        public int Weight;
        public int Height;
        public int SortOrder;
        public SpriteRenderer CachedSpriteRenderer;
        public MeshRenderer CachedMeshRenderer;
        public bool Registered = false;

        public void Start()
        {
            if(Position == null)
            {
                return;
            }
            if (Context.SpriteSortingSystem != null)
            {
                Context.SpriteSortingSystem.Register(this);
            }
        }

        public void OnDestroy()
        {
            if (Context.SpriteSortingSystem != null)
            {
                Context.SpriteSortingSystem.Deregister(this);
            }
        }

        public int CompareTo(Sortable other)
        {
            var heightCheck = Height.CompareTo(other.Height);
            if (heightCheck != 0)
            {
                return heightCheck;
            }
            else
            {
                // if heights are the same, fall back to weight
                return Weight.CompareTo(other.Weight);
            }
        }
    }
}
