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
        public Color DefaultColor = new Color(0, 213, 255);
        public int RelativeSortOrder = 0;
        public List<SpriteRenderer> TilesInUse = new List<SpriteRenderer>(0);
        public GameObject Folder;
    }

    public class OverlaySystem : IOverlaySystem
    {
        private GameObject OverlayTilePrefab;
        private Queue<SpriteRenderer> _overlayTilePool = new Queue<SpriteRenderer>();
        private int _overlayTilePoolSize = 20;
        private GameObject OverlayFolder;
        private List<Overlay> Overlays = new List<Overlay>(0);
        private IGameStateSystem _gameStateSystem;

        public OverlaySystem(GameStateSystem gameStateSystem)
        {
            _gameStateSystem = gameStateSystem;
        }

        public void Init()
        {
            if (OverlayTilePrefab == null)
            {
                OverlayTilePrefab = Resources.Load<GameObject>("Overlay/OverlayTile");
            }
            OverlayFolder = MakeFolder("Overlays");
            PopulatePool();
        }

        public void Process()
        {
            foreach (var overlay in Overlays)
            {
                Draw(overlay);
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
            if (!Overlays.Contains(overlay))
            {
                Overlays.Add(overlay);
                foreach (var config in overlay.Configs)
                {
                    if (config.Folder == null)
                    {
                        config.Folder = MakeFolder(config.Name == null ? "Tiles" : string.Format("{0} Tiles", config.Name));
                        config.Folder.transform.SetParent(OverlayFolder.transform);
                    }
                }
                Draw(overlay);
            }
        }

        public void Deactivate(Overlay overlay)
        {
            if (Overlays.Contains(overlay))
            {
                Overlays.Remove(overlay);
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

        private static GameObject MakeFolder(string name)
        {
            var go = new GameObject();
            go.name = name;
            go.transform.position = new Vector3(0, 0, 0);
            go.transform.localScale = new Vector3(1, 1, 1);
            go.transform.localEulerAngles = new Vector3(0, 0, 0);
            return go;
        }

        private SpriteRenderer GetTileFromPoolAndActivate(OverlayConfig overlay)
        {
            if (_overlayTilePool.Count == 0)
            {
                AddNewEntryToPool();
            }
            var result = _overlayTilePool.Dequeue();
            result.transform.parent = overlay.Folder.transform;
            overlay.TilesInUse.Add(result);
            result.gameObject.SetActive(true);
            return result;
        }

        private void ReturnToPool(SpriteRenderer spriteRenderer)
        {
            spriteRenderer.sortingOrder = 0;
            spriteRenderer.gameObject.SetActive(false);
            spriteRenderer.gameObject.transform.parent = OverlayFolder.transform;
            spriteRenderer.transform.position = Vector3.zero;
            _overlayTilePool.Enqueue(spriteRenderer);
        }

        private void PopulatePool()
        {
            for (int i = 0; i < _overlayTilePoolSize; i++)
            {
                AddNewEntryToPool();
            }
        }

        private void AddNewEntryToPool()
        {
            var go = GameObject.Instantiate(OverlayTilePrefab);
            var spriteRenderer = go.GetComponent<SpriteRenderer>();
            ReturnToPool(spriteRenderer);
        }

        public void Clear()
        {
            var overlaysToRemove = new List<Overlay>(Overlays.Count);
            overlaysToRemove.AddRange(Overlays);
            foreach (var overlay in overlaysToRemove)
            {
                Deactivate(overlay);
            }
            Overlays.Clear();
            _overlayTilePool.Clear();
        }
    }
}
