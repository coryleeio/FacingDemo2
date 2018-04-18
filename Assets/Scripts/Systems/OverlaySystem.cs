using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public class Overlay
    {
        public string Name;
        public Shape Shape;
        public bool PositionFollowsMouse = false;
        public bool RotationFollowsMouse = false;
        public Color DefaultColor = new Color(0, 213, 255);
        public Point LastDrawnPosition;
        public Direction LastDrawnRotation;
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
            if(OverlayTilePrefab == null)
            {
                OverlayTilePrefab = Resources.Load<GameObject>("Overlay/OverlayTile");
            }
            OverlayFolder = MakeFolder("Overlays");
            PopulatePool();
        }

        public void Process()
        {
            var mouseMapPosition = MathUtil.GetMousePositionOnMap(Camera.main);
            foreach (var overlay in Overlays)
            {
                if (overlay.PositionFollowsMouse)
                {
                    overlay.Shape.Position = mouseMapPosition;
                    Draw(overlay);
                }
                if(overlay.RotationFollowsMouse)
                {
                    overlay.Shape.Direction = MathUtil.RelativeDirection(overlay.Shape.Position, mouseMapPosition);
                    Draw(overlay);
                }
            }
        }

        public void Activate(Overlay overlay)
        {
            if (!Overlays.Contains(overlay))
            {
                Overlays.Add(overlay);
                if (overlay.Folder == null)
                {
                    overlay.Folder = MakeFolder(overlay.Name == null ? "Tiles" : string.Format("{0} Tiles", overlay.Name));
                    overlay.Folder.transform.SetParent(OverlayFolder.transform);
                }
                Draw(overlay);
            }
        }

        public void Deactivate(Overlay overlay)
        {
            if(Overlays.Contains(overlay))
            {
                Overlays.Remove(overlay);
                overlay.LastDrawnPosition = null;
                overlay.LastDrawnRotation = Direction.SouthEast;
            }
        }

        private void Draw(Overlay overlay)
        {
            foreach (var tileInUse in overlay.TilesInUse)
            {
                ReturnToPool(tileInUse);
            }
            overlay.TilesInUse.Clear();
            foreach(var point in overlay.Shape.Points)
            {
                var tile = GetTileFromPoolAndActivate(overlay);
                tile.transform.position = MathUtil.MapToWorld(point.X, point.Y);
                tile.color = overlay.DefaultColor;
            }
            overlay.LastDrawnPosition = overlay.Shape.Position;
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

        private SpriteRenderer GetTileFromPoolAndActivate(Overlay overlay)
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
