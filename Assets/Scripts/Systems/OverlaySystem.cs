using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gamepackage
{
    public enum OverlayBehaviour
    {
        StationaryNoRotation,
        StationaryRotationFollowsCursor,
        PositionFollowsCursor,
    }

    public enum OverlayConstraints
    {
        AllTiles,
        Orthogonals,
        Diagonals,
        ConstraintedToShapeOfPreviousConfig,
    }

    public enum OverlaySpriteType
    {
        Square,
        Circle,
    }

    public class Overlay
    {
        public List<OverlayConfig> Configs = new List<OverlayConfig>();
        public int SortOrder = 0;
    }

    public class OverlayConfig
    {
        public string Name;
        public Shape Shape;
        public OverlayBehaviour OverlayBehaviour;
        public OverlayConstraints OverlayConstraint = OverlayConstraints.AllTiles;
        public OverlaySpriteType SpriteType = OverlaySpriteType.Square;
        public Color DefaultColor = new Color(0, 213, 255);
        public int RelativeSortOrder = 0;
        public bool WalkableTilesOnly = false;
        public bool ConstrainToLevel = false;
        public List<SpriteWithMapPosition> TilesInUse = new List<SpriteWithMapPosition>(0);
        public GameObject Folder;
    }

    public class OverlaySystem : IOverlaySystem
    {
        private Sprite _squareSprite;
        private Sprite _circleSprite;
        private GameObjectPool<SpriteWithMapPosition> _pool;
        private GameObject _overlayFolder;
        private List<Overlay> _overlays = new List<Overlay>(0);
        public IGameStateSystem GameStateSystem { get; set; }
        private ListGrid<SpriteRenderer> AllOverlayTilesInUse;
        private Rectangle BoundingBox;

        public OverlaySystem()
        {
        }

        public void Init(int mapWidth, int mapHeight)
        {
            BoundingBox = new Rectangle()
            {
                Position = new Point(0, 0),
                Width = mapWidth,
                Height = mapHeight
            };
            if (_squareSprite == null)
            {
                _squareSprite = Resources.Load<Sprite>("Overlay/Square");
            }
            if (_circleSprite == null)
            {
                _circleSprite = Resources.Load<Sprite>("Overlay/Circle");
            }
            _overlayFolder = GameObjectUtils.MakeFolder("Overlays");
            _pool = new GameObjectPool<SpriteWithMapPosition>("Overlay/OverlayPrefab", _overlayFolder);
            AllOverlayTilesInUse = new ListGrid< SpriteRenderer>(mapWidth, mapHeight);
            _pool.Init();
        }

        public void Process()
        {
            foreach (var overlay in _overlays)
            {
                Draw(overlay);
            }
        }

        private Sprite GetSpriteForType(OverlaySpriteType spriteType)
        {
            if (spriteType == OverlaySpriteType.Square)
            {
                return _squareSprite;
            }
            else if (spriteType == OverlaySpriteType.Circle)
            {
                return _circleSprite;
            }
            else
            {
                throw new NotImplementedException("Not implemented");
            }
        }

        private static void UpdateDirectionToFollowCursorIfNeeded(Point mouseMapPosition, OverlayConfig config)
        {
            if (config.OverlayBehaviour == OverlayBehaviour.PositionFollowsCursor)
            {
                config.Shape.Position = mouseMapPosition;
            }
            else if (config.OverlayBehaviour == OverlayBehaviour.StationaryRotationFollowsCursor)
            {
                config.Shape.Direction = MathUtil.RelativeDirection(config.Shape.Position, mouseMapPosition);
            }
            else if (config.OverlayBehaviour == OverlayBehaviour.StationaryNoRotation)
            {
                // do nothing
            }
            else
            {
                throw new NotImplementedException("Not implemented");
            }
        }

        public void Activate(Overlay overlay)
        {
            if (!_overlays.Contains(overlay))
            {
                _overlays.Add(overlay);
                foreach (var config in overlay.Configs)
                {
                    if (config.Folder == null)
                    {
                        config.Folder = GameObjectUtils.MakeFolder(config.Name == null ? "Tiles" : string.Format("{0} Tiles", config.Name));
                        config.Folder.transform.SetParent(_overlayFolder.transform);
                    }
                }
                Draw(overlay);
            }
        }

        public void Deactivate(Overlay overlay)
        {
            if (_overlays.Contains(overlay))
            {
                _overlays.Remove(overlay);
            }
        }

        private void Draw(Overlay overlay)
        {
            var mouseMapPosition = MathUtil.GetMousePositionOnMap(Camera.main);
            OverlayConfig previous = null;
            foreach (var config in overlay.Configs)
            {
                UpdateDirectionToFollowCursorIfNeeded(mouseMapPosition, config);
                foreach (var tileInUse in config.TilesInUse)
                {
                    ReturnToPool(tileInUse);
                }
                config.TilesInUse.Clear();
                if (ShouldDraw(config, previous, mouseMapPosition))
                {
                    foreach (var point in config.Shape.Points)
                    {
                        if (config.ConstrainToLevel && !GameStateSystem.Game.CurrentLevel.Domain.Contains(point))
                        {
                            continue;
                        }
                        // This duplication of the contains check is necessary 
                        if (config.WalkableTilesOnly && GameStateSystem.Game.CurrentLevel.Domain.Contains(point) && GameStateSystem.Game.CurrentLevel.TilesetGrid[point.X, point.Y].TileType != TileType.Floor)
                        {
                            continue;
                        }
                        var tile = GetTileFromPoolAndActivate(config);
                        tile.SpriteRenderer.sortingOrder = overlay.SortOrder + config.RelativeSortOrder;
                        tile.transform.position = MathUtil.MapToWorld(point.X, point.Y);
                        tile.Position = new Point(point.X, point.Y);
                        tile.SpriteRenderer.color = config.DefaultColor;

                        if (BoundingBox.Contains(point))
                        {
                            AllOverlayTilesInUse[tile.Position.X, tile.Position.Y].Add(tile.SpriteRenderer);
                        }

                    }
                }
                previous = config;
            }
        }

        private bool ShouldDraw(OverlayConfig config, OverlayConfig previousConfig, Point mouseMapPosition)
        {
            if (config.OverlayConstraint == OverlayConstraints.AllTiles)
            {
                return true;
            }
            else if (config.OverlayConstraint == OverlayConstraints.Diagonals)
            {
                return config.Shape.Position.IsDiagonalTo(mouseMapPosition);
            }
            else if (config.OverlayConstraint == OverlayConstraints.Orthogonals)
            {
                return config.Shape.Position.IsOrthogonalTo(mouseMapPosition);
            }
            else if (config.OverlayConstraint == OverlayConstraints.ConstraintedToShapeOfPreviousConfig)
            {
                return config.Shape.Intersects(previousConfig.Shape);
            }
            else
            {
                throw new NotImplementedException("Not implemented");
            }
        }

        private SpriteWithMapPosition GetTileFromPoolAndActivate(OverlayConfig overlay)
        {
            var result = _pool.CheckOut();
            result.transform.parent = overlay.Folder.transform;
            overlay.TilesInUse.Add(result);
            var sprite = GetSpriteForType(overlay.SpriteType);
            if (result.SpriteRenderer.sprite != sprite)
            {
                result.SpriteRenderer.sprite = sprite;
            }
            return result;
        }

        private void ReturnToPool(SpriteWithMapPosition sprite)
        {
            if (BoundingBox.Contains(sprite.Position))
            {
                AllOverlayTilesInUse[sprite.Position.X, sprite.Position.Y].Remove(sprite.SpriteRenderer);
            }
            _pool.CheckIn(sprite);
            sprite.SpriteRenderer.sortingOrder = 0;
        }

        public void Clear()
        {
            var overlaysToRemove = new List<Overlay>(_overlays.Count);
            overlaysToRemove.AddRange(_overlays);
            foreach (var overlay in overlaysToRemove)
            {
                Deactivate(overlay);
            }
            _overlays.Clear();
        }

        public List<SpriteRenderer> GetTilesInPosition(int x, int y)
        {
            return AllOverlayTilesInUse[x, y];
        }
    }
}
