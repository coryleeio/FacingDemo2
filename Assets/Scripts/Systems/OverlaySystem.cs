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

    public enum SpriteType
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
        public SpriteType SpriteType = SpriteType.Square;
        public Color DefaultColor = new Color(0, 213, 255);
        public int RelativeSortOrder = 0;
        public List<SpriteRenderer> TilesInUse = new List<SpriteRenderer>(0);
        public GameObject Folder;
    }

    public class OverlaySystem : IOverlaySystem
    {
        private Sprite _squareSprite;
        private Sprite _circleSprite;
        private GameObjectPool<SpriteRenderer> _pool;
        private GameObject _overlayFolder;
        private List<Overlay> _overlays = new List<Overlay>(0);
        private IGameStateSystem _gameStateSystem;

        public OverlaySystem(GameStateSystem gameStateSystem)
        {
            _gameStateSystem = gameStateSystem;
        }

        public void Init()
        {
            if(_squareSprite == null)
            {
                _squareSprite = Resources.Load<Sprite>("Overlay/Square");
            }
            if (_circleSprite == null)
            {
                _circleSprite = Resources.Load<Sprite>("Overlay/Circle");
            }
            _overlayFolder = GameObjectUtils.MakeFolder("Overlays");
            _pool = new GameObjectPool<SpriteRenderer>("Overlay/OverlayPrefab", _overlayFolder);
            _pool.Init();
        }

        public void Process()
        {
            foreach (var overlay in _overlays)
            {
                Draw(overlay);
            }
        }

        private Sprite GetSpriteForType(SpriteType spriteType)
        {
            if(spriteType == SpriteType.Square)
            {
                return _squareSprite;
            }
            else if(spriteType == SpriteType.Circle)
            {
                return _circleSprite;
            }
            else
            {
                throw new Exception("Not implemented");
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
                throw new System.Exception("Not implemented");
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
                if(ShouldDraw(config, previous, mouseMapPosition))
                {
                    foreach (var point in config.Shape.Points)
                    {
                        var tile = GetTileFromPoolAndActivate(config);
                        tile.sortingOrder = overlay.SortOrder + config.RelativeSortOrder;
                        tile.transform.position = MathUtil.MapToWorld(point.X, point.Y);
                        tile.color = config.DefaultColor;
                    }
                }
                previous = config;
            }
        }

        private bool ShouldDraw(OverlayConfig config, OverlayConfig previousConfig, Point mouseMapPosition)
        {
            if(config.OverlayConstraint == OverlayConstraints.AllTiles)
            {
                return true;
            }
            else if(config.OverlayConstraint == OverlayConstraints.Diagonals)
            {
                return config.Shape.Position.IsDiagonalTo(mouseMapPosition);
            }
            else if(config.OverlayConstraint == OverlayConstraints.Orthogonals)
            {
                return config.Shape.Position.IsOrthogonalTo(mouseMapPosition);
            }
            else if(config.OverlayConstraint == OverlayConstraints.ConstraintedToShapeOfPreviousConfig)
            {
                return config.Shape.Intersects(previousConfig.Shape);
            }
            else
            {
                throw new Exception("Not implemented");
            }
        }

        private SpriteRenderer GetTileFromPoolAndActivate(OverlayConfig overlay)
        {
            var result = _pool.CheckOut();
            result.transform.parent = overlay.Folder.transform;
            overlay.TilesInUse.Add(result);
            var sprite = GetSpriteForType(overlay.SpriteType);
            if (result.sprite != sprite)
            {
                result.sprite = sprite;
            }
            return result;
        }

        private void ReturnToPool(SpriteRenderer spriteRenderer)
        {
            _pool.CheckIn(spriteRenderer);
            spriteRenderer.sortingOrder = 0;
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
    }
}
