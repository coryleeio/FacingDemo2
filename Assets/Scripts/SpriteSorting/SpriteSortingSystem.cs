using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class SpriteSortingSystem
    {
        private Level level;
        private int nextSortableId;
        private Dictionary<int, Sortable> sortablesById;
        private Dictionary<SortingLayer, int> gameSortingLayerToUnitySortingLayerMap = new Dictionary<SortingLayer, int>();

        public void Init()
        {
            nextSortableId = 0;
            level = Context.Game.CurrentLevel;
            var sortables = GameObject.FindObjectsOfType<Sortable>();
            sortablesById = new Dictionary<int, Sortable>();
            var uncastLayers = Enum.GetValues(typeof(SortingLayer));
            {
                foreach (var uncastLayer in uncastLayers)
                {
                    var layer = (SortingLayer)uncastLayer;
                    gameSortingLayerToUnitySortingLayerMap[layer] = UnityEngine.SortingLayer.NameToID(layer.ToString());
                }
            }

            for (var x = 0; x < level.Grid.Width; x++)
            {
                for (var y = 0; y < level.Grid.Height; y++)
                {
                    foreach (var uncastLayer in uncastLayers)
                    {
                        var layer = (SortingLayer)uncastLayer;
                        if (!level.Grid[x, y].SortablesInPositionByLayer.ContainsKey(layer))
                        {
                            level.Grid[x, y].SortablesInPositionByLayer[layer] = new List<Sortable>();
                        }
                    }
                }
            }
            foreach (var sortable in sortables)
            {
                Register(sortable);
            }
        }

        public void Move(Sortable spriteSortable, Point oldPosition, Point newPosition)
        {
            if (!spriteSortable.Registered)
            {
                return;
            }
            spriteSortable._position = newPosition;
            if (oldPosition != null)
            {
                // Remove
                var sortables = level.Grid[oldPosition].SortablesInPositionByLayer[spriteSortable.Layer];
                sortables.Remove(spriteSortable);

                foreach(var child in spriteSortable.Children)
                {
                    child.RecalculatePositionRelativeToParent();
                }

                // You dont need to resort here because removal doesnt affect the existing order
                // and the order has a known min/max due to the position
            }
            var layer = spriteSortable.Layer;
            level.Grid[newPosition].SortablesInPositionByLayer[layer].Add(spriteSortable);
            ResortPosition(newPosition, spriteSortable.Layer);
        }

        public void Register(Sortable spriteSortable)
        {
            if(spriteSortable.Position == null)
            {
                spriteSortable.Position = new Point(0, 0);
                if(spriteSortable.PositionRelativeToParent == null)
                {
                    spriteSortable.PositionRelativeToParent = new Point(0, 0);
                }
            }
            if (spriteSortable != null || spriteSortable.Registered)
            {
                if (sortablesById.ContainsKey(spriteSortable.SpriteSortableId) &&
                    sortablesById[spriteSortable.SpriteSortableId] == spriteSortable)
                {
                    // Already registered, ignore.
                    // This is just to make it work in cases when sortables are added 
                    // either before or after the system has init.
                }
                else
                {
                    spriteSortable.SpriteSortableId = nextSortableId;
                    nextSortableId++;
                    sortablesById[spriteSortable.SpriteSortableId] = spriteSortable;
                    spriteSortable.Children = new List<Sortable>();
                    spriteSortable.CachedSpriteRenderer = spriteSortable.GetComponent<SpriteRenderer>();
                    spriteSortable.CachedMeshRenderer = spriteSortable.GetComponentInChildren<MeshRenderer>();
                    spriteSortable.Registered = true;

                    if(spriteSortable.Parent == null)
                    {
                        var possibleChildren = spriteSortable.gameObject.GetComponentsInChildren<Sortable>();
                        foreach (var possibleChild in possibleChildren)
                        {
                            if (possibleChild != spriteSortable)
                            {
                                spriteSortable.Children.Add(possibleChild);
                            }
                        }
                        foreach (var child in spriteSortable.Children)
                        {
                            child.Parent = spriteSortable;
                            if(!child.Registered)
                            {
                                Register(child);
                            }

                            child.RecalculatePositionRelativeToParent();
                        }
                    }

                    Move(spriteSortable, null, spriteSortable.Position);
                }
            }
        }

        public void Deregister(Sortable spriteSortable)
        {
            if (spriteSortable != null || !spriteSortable.Registered)
            {
                if (sortablesById.ContainsKey(spriteSortable.SpriteSortableId) &&
                    sortablesById[spriteSortable.SpriteSortableId] == spriteSortable)
                {
                    sortablesById.Remove(spriteSortable.SpriteSortableId);
                    foreach (var child in spriteSortable.Children)
                    {
                        spriteSortable.Parent = null;
                    }
                }
            }
        }

        private void ResortPosition(Point p, SortingLayer layer)
        {
            ResortPosition(p.X, p.Y, layer);
        }

        private void ResortPosition(int x, int y, SortingLayer layer)
        {
            if (layer != SortingLayer.Overlays)
            {
                level.Grid[x, y].SortablesInPositionByLayer[layer].Sort();
            }
            var sortOrdersAvailablePerTile = 40;  //  (32767 / 40 / 40 ) * 2 (because range is -32767 - 32767)
            var minimumSortingOrderForLayer = (y * (level.BoundingBox.Width * sortOrdersAvailablePerTile) + (x * sortOrdersAvailablePerTile));
            minimumSortingOrderForLayer = minimumSortingOrderForLayer - 32767; // adjust for negative values so we use the whole range for each layer
            var maxSortingOrderForLayer = minimumSortingOrderForLayer + sortOrdersAvailablePerTile;
            var startingSortOrder = minimumSortingOrderForLayer;
            var sortOrder = minimumSortingOrderForLayer;
            var sortables = level.Grid[x, y].SortablesInPositionByLayer[layer];
            foreach (var sortable in sortables)
            {
                // There are too many overlay sprites, just use their weight
                if (layer == SortingLayer.Overlays)
                {
                    sortOrder = minimumSortingOrderForLayer + sortable.Weight;
                }

                if (sortable.CachedSpriteRenderer != null)
                {
                    sortable.CachedSpriteRenderer.sortingOrder = sortOrder;
                    sortable.CachedSpriteRenderer.sortingLayerID = gameSortingLayerToUnitySortingLayerMap[layer];
                    sortable.SortOrder = sortOrder;
                }
                else if (sortable.CachedMeshRenderer != null)
                {
                    sortable.CachedMeshRenderer.sortingOrder = sortOrder;
                    sortable.CachedMeshRenderer.sortingLayerID = gameSortingLayerToUnitySortingLayerMap[layer];
                    sortable.SortOrder = sortOrder;
                }
                if (sortOrder < maxSortingOrderForLayer)
                {
                    if (layer != SortingLayer.Overlays)
                    {
                        sortOrder += 1;
                    }
                }
                else
                {
                    // This happens for large amounts of ground items, since we stop incrementing, they might be sorted at the same index as other things in the layer
                    Debug.LogWarning("sorting out of bounds in: " + x + ", " + y + " for layer: " + layer + " " + minimumSortingOrderForLayer + " < " + sortOrder + " < " + maxSortingOrderForLayer);
                    Debug.Log(level.Grid[x, y].SortablesInPositionByLayer[layer].Count);
                }
            }
        }
    }
}
