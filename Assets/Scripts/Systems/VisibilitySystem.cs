
using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Gamepackage
{
    public enum MapVisibilityState
    {
        Hidden, // player has never been here, and cant see it
        Revealed, // player can currently see this
        Obfuscated // player has been here, but cant see it currently.
    }

    public class VisibilitySystem : IVisibilitySystem
    {
        public IGameStateSystem _gameStateSystem { get; set; }

        public VisibilitySystem()
        {

        }

        public Texture2D RevealMask;
        public LayerMask FogLayer;
        private const int _numberOfPixelsPerTile = 64;
        private Texture2D _texture;
        private int _mapSize;
        private Point[,] _tileCenterPointForCoordinates;

        private Color HiddenColor = Color.black;
        private Color VisibleColor = Color.clear;
        private Color ObfuscatedColor = new Color(0, 0, 0, .75f);
        private bool[,] _updatedVisibilityGrid;
        private GameObject FogOfWarGameObject;
        public IGameStateSystem GameStateSystem { get; set; }

        public void Init()
        {
            FogLayer = LayerMask.GetMask(new string[] { "Fog" });
            if (FogOfWarGameObject == null)
            {
                var prefab = Resources.Load<GameObject>("Fog/Fog of War");
                FogOfWarGameObject = GameObject.Instantiate(prefab);
            }
            if(RevealMask == null)
            {
                RevealMask = Resources.Load<Texture2D>("Fog/CubeMask");
            }

            _mapSize = _gameStateSystem.Game.CurrentLevel.Domain.Width;

            _updatedVisibilityGrid = new bool[_mapSize, _mapSize];
            var textureX = _mapSize * _numberOfPixelsPerTile;
            var textureY = _mapSize * _numberOfPixelsPerTile;
            FogOfWarGameObject.transform.localScale = new Vector3(_mapSize, _mapSize, 1f);
            FogOfWarGameObject.transform.position = new Vector3(0, -_mapSize / 4, 0f);

            _texture = new Texture2D(textureX, textureY, TextureFormat.ARGB32, false);
            _texture.wrapMode = TextureWrapMode.Clamp;

            var renderer = FogOfWarGameObject.GetComponent<MeshRenderer>();
            renderer.material.mainTexture = _texture;
            _texture = (Texture2D)renderer.sharedMaterial.mainTexture;
            BuildIndexes();
            SetEntireTextureToHiddenColor();
            PaintInitialVisibility();
            _texture.Apply();
        }

        private void PaintInitialVisibility()
        {
            var revealed = new List<Point>();
            var obfuscated = new List<Point>();

            for (var x = 0; x < _mapSize; x++)
            {
                for (var y = 0; y < _mapSize; y++)
                {
                    if (_gameStateSystem.Game.CurrentLevel.VisibilityGrid[x, y] == MapVisibilityState.Obfuscated)
                    {
                        obfuscated.Add(new Point(x, y));
                    }
                    else if (_gameStateSystem.Game.CurrentLevel.VisibilityGrid[x, y] == MapVisibilityState.Revealed)
                    {
                        revealed.Add(new Point(x, y));
                    }
                }
            }

            PaintVisibility(obfuscated, MapVisibilityState.Obfuscated);
            PaintVisibility(revealed, MapVisibilityState.Revealed);
        }

        public void UpdateVisibility(bool[,] newVisibility)
        {
            Assert.AreEqual(newVisibility.GetLength(0), _mapSize);
            Assert.AreEqual(newVisibility.GetLength(1), _mapSize);
            var revealed = new List<Point>();
            var obfuscated = new List<Point>();

            for (var x = 0; x < _mapSize; x++)
            {
                for (var y = 0; y < _mapSize; y++)
                {
                    var changedToObfuscated = !newVisibility[x, y] && _gameStateSystem.Game.CurrentLevel.VisibilityGrid[x, y] == MapVisibilityState.Revealed;
                    if (newVisibility[x, y])
                    {
                        _gameStateSystem.Game.CurrentLevel.VisibilityGrid[x, y] = MapVisibilityState.Revealed;
                        revealed.Add(new Point(x, y));
                    }
                    else if (changedToObfuscated)
                    {

                        _gameStateSystem.Game.CurrentLevel.VisibilityGrid[x, y] = MapVisibilityState.Obfuscated;
                        obfuscated.Add(new Point(x, y));
                    }
                }
            }
            PaintVisibility(obfuscated, MapVisibilityState.Obfuscated);
            PaintVisibility(revealed, MapVisibilityState.Revealed);
            _texture.Apply();
        }

        private void BuildIndexes()
        {
            _tileCenterPointForCoordinates = new Point[_mapSize, _mapSize];
            if (_gameStateSystem.Game.CurrentLevel.VisibilityGrid == null)
            {
                _gameStateSystem.Game.CurrentLevel.VisibilityGrid = new Grid<MapVisibilityState>(_mapSize, _mapSize);
            }
            for (var x = 0; x < _mapSize; x++)
            {
                for (var y = 0; y < _mapSize; y++)
                {
                    Point p = GetCenterPointForTile(x, y);
                    _tileCenterPointForCoordinates[x, y] = p;
                }
            }
        }

        private void PaintVisibility(List<Point> listOfTilePositions, MapVisibilityState visibility)
        {
            var color = GetTargetColorForVisibilityType(visibility);
            foreach (var p in listOfTilePositions)
            {
                var tilePosX = p.X;
                var tilePosY = p.Y;
                Point center = _tileCenterPointForCoordinates[tilePosX, tilePosY];
                var bottomLeftX = center.X - _numberOfPixelsPerTile / 2;
                var bottomLeftY = center.Y - _numberOfPixelsPerTile / 2;
                var maskPixels = RevealMask.GetPixels(0, 0, _numberOfPixelsPerTile, _numberOfPixelsPerTile);
                var texturePixels = _texture.GetPixels(bottomLeftX, bottomLeftY, _numberOfPixelsPerTile, _numberOfPixelsPerTile);
                for (var i = 0; i < maskPixels.Length; i++)
                {
                    if (maskPixels[i].a != VisibleColor.a)
                    {
                        maskPixels[i] = color;
                    }
                    else
                    {
                        maskPixels[i] = texturePixels[i];
                    }
                }
                _texture.SetPixels(bottomLeftX, bottomLeftY, _numberOfPixelsPerTile, _numberOfPixelsPerTile, maskPixels);
            }
        }

        private void SetEntireTextureToHiddenColor()
        {
            var pixels = _texture.GetPixels();
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = HiddenColor;
            }
            _texture.SetPixels(pixels);
        }

        private Point GetCenterPointForTile(int tilePosX, int tilePosY)
        {
            var center = MathUtil.MapToWorld(tilePosX, tilePosY);
            var screenPoint = Camera.main.WorldToScreenPoint(new Vector3(center.x, center.y));
            var ray = Camera.main.ScreenPointToRay(screenPoint);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 20000, FogLayer))
            {
                Vector2 uv;
                uv.x = (hit.point.x - hit.collider.bounds.min.x) / hit.collider.bounds.size.x;
                uv.y = (hit.point.y - hit.collider.bounds.min.y) / hit.collider.bounds.size.y;

                var pixelX = (int)(uv.x * _texture.width);
                var pixelY = (int)(uv.y * _texture.height);
                Point p = new Point(pixelX, pixelY);
                _tileCenterPointForCoordinates[tilePosX, tilePosY] = p;
                return p;
            }
            else
            {
                throw new System.Exception("Shouldn't happen");
            }
        }

        private Color GetTargetColorForVisibilityType(MapVisibilityState mapVisibilityState)
        {
            if (mapVisibilityState == MapVisibilityState.Hidden)
            {
                return HiddenColor;
            }
            if (mapVisibilityState == MapVisibilityState.Obfuscated)
            {
                return ObfuscatedColor;
            }
            if (mapVisibilityState == MapVisibilityState.Revealed)
            {
                return VisibleColor;
            }
            throw new NotImplementedException("Not implemented");
        }

        public void UpdateVisibility()
        {
            for (int x = 0; x < _mapSize; x++)
            {
                for (int y = 0; y < _mapSize; y++)
                {
                    _updatedVisibilityGrid[x, y] = false;
                }
            }

            var level = GameStateSystem.Game.CurrentLevel;
            var player = level.Player;

            var visiblePoints = new List<Point>();
            visiblePoints.AddRange(MathUtil.FloodFill(player.Position, 5, ref visiblePoints, MathUtil.FloodFillType.Orthogonal, (pointOnLevel) => { return level.Domain.Contains(pointOnLevel) && level.TilesetGrid[pointOnLevel.X, pointOnLevel.Y].TileType != TileType.Empty; }));

            foreach (var tile in visiblePoints)
            {
               _updatedVisibilityGrid[tile.X, tile.Y] = true;
            }

            /*
            foreach (var pawn in level.Tokens)
            {
                if (!pawn.IsPlayer)
                {
                    pawn.Visible = _updatedVisibilityGrid[pawn.Position.x, pawn.Position.y];
                }
            }

            foreach (var itemDropActor in Current.SceneItemDropActors)
            {
                itemDropActor.Visible = _updatedVisibilityGrid[itemDropActor.Position.x, itemDropActor.Position.y];
            }
            */

            UpdateVisibility(_updatedVisibilityGrid);
        }

        public void Clear()
        {
            FogOfWarGameObject = null;
        }
    }

}