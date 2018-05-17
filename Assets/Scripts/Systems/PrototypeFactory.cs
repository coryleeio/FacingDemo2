using System;
using System.Collections.Generic;
using TinyIoC;
using UnityEngine;

namespace Gamepackage
{
    public class PrototypeFactory
    {
        public ResourceManager ResourceManager { get; set; }
        public TinyIoCContainer Container { get; set; }
        public TokenSystem TokenSystem { get; set; }
        public SpriteSortingSystem SpriteSortingSystem { get; set; }
        private Material DefaultSpriteMaterial;
        private Sprite MissingSprite;

        public PrototypeFactory()
        {
            if (DefaultSpriteMaterial == null)
            {
                DefaultSpriteMaterial = Resources.Load<Material>("Materials/DefaultSpriteMaterial");
            }
            if (MissingSprite == null)
            {
                MissingSprite = Resources.Load<Sprite>("Missing");
            }
        }

        public Token BuildToken(UniqueIdentifier identifier)
        {
            var prototype = ResourceManager.GetPrototype<TokenPrototype>(identifier);
            var token = new Token
            {
                Position = new Point(0, 0),
                PrototypeIdentifier = identifier,
                BehaviorIdentifier = prototype.BehaviorIdentifier,
                ViewType = prototype.ViewType,
                TriggerPrototypeUniqueIdentifier = prototype.TriggerPrototypeUniqueIdentifier,
                ViewUniqueIdentifier = prototype.ViewUniqueIdentifier
            };
            token.Tags.AddRange(prototype.Tags);
            return token;
        }

        public GameObject BuildView(Token token)
        {
            var tokenPrototype = ResourceManager.GetPrototype<TokenPrototype>(token.PrototypeIdentifier);
            var go = new GameObject();
            go.name = tokenPrototype.UniqueIdentifier.ToString();
            go.transform.position = MathUtil.MapToWorld(token.Position);
            return go;
        }

        public void BuildMapTiles(Level level)
        {
            var folder = GameObjectUtils.MakeFolder("Grid");
            for (int x = 0; x < level.BoundingBox.Width; x++)
            {
                for (var y = 0; y < level.BoundingBox.Height; y++)
                {
                    var point = new Point(x, y);
                    var tileInfo = level.TilesetGrid[x, y];
                    var tileSet = ResourceManager.GetPrototype<Tileset>(tileInfo.TilesetIdentifier);

                    if (tileInfo.TileType == TileType.Floor)
                    {
                        SpriteRenderer renderer = BuildTileSpriteRenderer(folder, tileSet.FloorSprite, point);
                    }
                    else if (tileInfo.TileType == TileType.Wall)
                    {
                        var northEastPointTileType = GetTileInfoForOffset(level, point, MathUtil.NorthEastOffset);
                        var southEastPointTileType = GetTileInfoForOffset(level, point, MathUtil.SouthEastOffset);
                        var southWestPointTileType = GetTileInfoForOffset(level, point, MathUtil.SouthWestOffset);
                        var northWestPointTileType = GetTileInfoForOffset(level, point, MathUtil.NorthWestOffset);

                        if (
                            northEastPointTileType == TileType.Wall &&
                            southEastPointTileType == TileType.Wall &&
                            southWestPointTileType == TileType.Wall &&
                            northWestPointTileType == TileType.Wall
                        )
                        {
                            BuildTileSpriteRenderer(folder, tileSet.TeeSprite, point);
                        }
                        else if (
                             northEastPointTileType == TileType.Wall &&
                             southEastPointTileType == TileType.Wall &&
                             southWestPointTileType == TileType.Wall &&
                             (northWestPointTileType == TileType.Floor || northWestPointTileType == TileType.Empty)
                        )
                        {
                            BuildTileSpriteRenderer(folder, tileSet.SouthEastTeeSprite, point);
                        }
                        else if (
                             northEastPointTileType == TileType.Wall &&
                             southEastPointTileType == TileType.Wall &&
                             (southWestPointTileType == TileType.Floor || southWestPointTileType == TileType.Empty) &&
                             northWestPointTileType == TileType.Wall
                        )
                        {
                            BuildTileSpriteRenderer(folder, tileSet.NorthEastTeeSprite, point);
                        }
                        else if (
                             (northEastPointTileType == TileType.Floor || northEastPointTileType == TileType.Empty) &&
                             southEastPointTileType == TileType.Wall &&
                             southWestPointTileType == TileType.Wall &&
                             northWestPointTileType == TileType.Wall
                        )
                        {
                            BuildTileSpriteRenderer(folder, tileSet.SouthWestTeeSprite, point);
                        }
                        else if (
                             northEastPointTileType == TileType.Wall &&
                             (southEastPointTileType == TileType.Floor || southEastPointTileType == TileType.Empty) &&
                             southWestPointTileType == TileType.Wall &&
                             northWestPointTileType == TileType.Wall
                        )
                        {
                            BuildTileSpriteRenderer(folder, tileSet.NorthWestTeeSprite, point);
                        }

                        else if (
                             northEastPointTileType == TileType.Wall &&
                             southEastPointTileType == TileType.Floor &&
                             southWestPointTileType == TileType.Wall &&
                             northWestPointTileType == TileType.Empty
                        )
                        {
                            BuildTileSpriteRenderer(folder, tileSet.NorthWestWallSprite, point);
                        }

                        else if (
                            northEastPointTileType == TileType.Empty &&
                            southEastPointTileType == TileType.Wall &&
                            southWestPointTileType == TileType.Floor &&
                            northWestPointTileType == TileType.Wall
                        )
                        {
                            BuildTileSpriteRenderer(folder, tileSet.NorthEastWallSprite, point);
                        }

                        // We default to SE and SW incase we later want to shorten the walls on the southern side 
                        // if we cant determine which wall should be used opt for the SE and SW ones, as they
                        // might be shorter to prevent vision from being obscured.
                        else if (
                             (northEastPointTileType == TileType.Wall || northEastPointTileType == TileType.Floor) &&
                             (southEastPointTileType == TileType.Floor || southEastPointTileType == TileType.Empty) &&
                             (southWestPointTileType == TileType.Wall || southWestPointTileType == TileType.Floor) &&
                             (northWestPointTileType == TileType.Floor || northWestPointTileType == TileType.Empty)
                        )
                        {
                            BuildTileSpriteRenderer(folder, tileSet.SouthEastWallSprite, point);
                        }

                        else if (
                             (northEastPointTileType == TileType.Floor || northEastPointTileType == TileType.Empty) &&
                             (southEastPointTileType == TileType.Wall || southEastPointTileType == TileType.Floor) &&
                             (southWestPointTileType == TileType.Floor || southWestPointTileType == TileType.Empty) &&
                             (northWestPointTileType == TileType.Wall || northWestPointTileType == TileType.Floor)
                        )
                        {
                            BuildTileSpriteRenderer(folder, tileSet.SouthWestWallSprite, point);
                        }
                        else if (
                             northEastPointTileType == TileType.Wall &&
                             southEastPointTileType == TileType.Wall &&
                             (southWestPointTileType == TileType.Empty || southWestPointTileType == TileType.Floor) &&
                             (northWestPointTileType == TileType.Empty || northWestPointTileType == TileType.Floor)
                         )
                        {
                            BuildTileSpriteRenderer(folder, tileSet.WestCornerSprite, point);
                        }
                        else if (
                             (northEastPointTileType == TileType.Empty || northEastPointTileType == TileType.Floor) &&
                             southEastPointTileType == TileType.Wall &&
                             southWestPointTileType == TileType.Wall &&
                             (northWestPointTileType == TileType.Empty || northWestPointTileType == TileType.Floor)
                         )
                        {
                            BuildTileSpriteRenderer(folder, tileSet.NorthCornerSprite, point);
                        }
                        else if (
                             (northEastPointTileType == TileType.Empty || northEastPointTileType == TileType.Floor) &&
                             (southEastPointTileType == TileType.Empty || southEastPointTileType == TileType.Floor) &&
                             southWestPointTileType == TileType.Wall &&
                             northWestPointTileType == TileType.Wall
                         )
                        {
                            BuildTileSpriteRenderer(folder, tileSet.EastCornerSprite, point);
                        }
                        else if (
                             northEastPointTileType == TileType.Wall &&
                             (southEastPointTileType == TileType.Empty || southEastPointTileType == TileType.Floor) &&
                             (southWestPointTileType == TileType.Empty || southWestPointTileType == TileType.Floor) &&
                             northWestPointTileType == TileType.Wall
                         )
                        {
                            BuildTileSpriteRenderer(folder, tileSet.SouthCornerSprite, point);
                        }
                        else
                        {
                            BuildTileSpriteRenderer(folder, MissingSprite, point);
                        }
                    }
                }
            }
        }

        private static TileType GetTileInfoForOffset(Level level, Point position, Point offset)
        {
            var offsetPoint = MathUtil.GetPointByOffset(position, offset);
            if (!level.BoundingBox.Contains(offsetPoint))
            {
                return TileType.Empty;
            }
            return level.TilesetGrid[offsetPoint.X, offsetPoint.Y].TileType;
        }

        private SpriteRenderer BuildTileSpriteRenderer(GameObject folder, Sprite sprite, Point position)
        {
            GameObject o = new GameObject();
            o.name = "Tile";
            var renderer = o.AddComponent<SpriteRenderer>();
            renderer.material = DefaultSpriteMaterial;
            o.transform.SetParent(folder.transform);
            renderer.sprite = sprite;
            o.transform.localPosition = MathUtil.MapToWorld(position);
            SpriteSortingSystem.RegisterTile(renderer, position);
            return renderer;
        }
    }
}

