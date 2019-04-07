using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck.Systems;
using NoiseGen;

namespace MonoGameClusterFuck.Primitives
{
    public class TileSetExt
    {
        int _tileSize;
        public Texture2D Atlas;

        public List<Tile> Tiles;

        public TileSetExt(int tilesize)
        {
            _tileSize = tilesize;
            Tiles = new List<Tile>();
        }
        //1.2% ram
        public List<Tile> Slice(string textureName)
        {
            Atlas = Engine.Instance.Content.Load<Texture2D>("terrain");

            for (int x = 0; x < Atlas.Width; x += _tileSize)
            {
                for (int y = 0; y < Atlas.Height; y += _tileSize)
                {
                    var tile = new Tile(_tileSize);
                    tile.Texture = Atlas;
                    tile.Source = new Rectangle(x, y, _tileSize, _tileSize);
                    Tiles.Add(tile);
                }
            }

            return Tiles;
        }
    }
    public class TileSet : DrawableComponent
    {
        public TileSetExt Set;
        public static FastNoise Noise = new FastNoise();

        public Tile[,] Map = new Tile[2048, 2048];

        public TileSet(int size) : base(size)
        {
            Set = new TileSetExt(size);
        }

        public override void LoadContent()
        {
            Set.Slice("terrain");
            Texture = Engine.Instance.Content.Load<Texture2D>("terrain");
            base.LoadContent();

            for (int x = 1; x < 2048; x++)
            {
                for (int y = 1; y < 2048; y++)
                {
                    var value = Noise.GetCellular(x, y);
                    if (value > 0.45f)
                    {
                        Map[x, y] = Set.Tiles[515];
                    }
                    else
                    {
                        Map[x, y] = Set.Tiles[419];
                    }
                }
            }
            for (int x = 1; x < 2048; x++)
            {
                for (int y = 1; y < 2048; y++)
                {
                    var value = Noise.GetCellular(x, y);
                    if (value > 0.45f)
                    {
                        Map[x, y] = Set.Tiles[515];
                        if (Map[x, y - 1] == Set.Tiles[419])
                            Map[x, y] = Set.Tiles[514];
                    }
                    else
                    {
                        Map[x, y] = Set.Tiles[419];
                        if (Map[x-1, y] == Set.Tiles[515])
                            Map[x, y] = Set.Tiles[418];
                    }
                }
            }

        }

        public override void Update(GameTime deltaTime)
        {

        }

        public override void Draw(Layers.LayerType type)
        {
            var count = 0;
            if (InputState.DrawTileSet)
            {
                var location = Point.Zero;
                var labelPos = Vector2.Zero;
                var sourceRect = new Rectangle(location, SpriteSize);
                var destRect = new Rectangle(location, SpriteSize);
                var viewbounds = Camera.VisibleArea;

                var left = (viewbounds.Left / SpriteSize.X * SpriteSize.X) - SpriteSize.X;
                var top = (viewbounds.Top / SpriteSize.Y * SpriteSize.Y) - SpriteSize.Y;
                for (var x = left; x <= Math.Min(Texture.Width, viewbounds.Right); x += SpriteSize.X)
                {
                    for (var y = top; y <= Math.Min(Texture.Height, viewbounds.Bottom); y += SpriteSize.Y)
                    {
                        if (x < 0 || y < 0)
                            continue;
                        location.X = x;
                        location.Y = y;
                        sourceRect.Location = location;
                        destRect.Location = location;
                        labelPos.X = location.X;
                        labelPos.Y = location.Y;

                        Engine.SpriteBatch.Draw(Texture, destRect, sourceRect, Color.White);
                        Engine.SpriteBatch.DrawString(Fonts.Generic, count.ToString(), labelPos, Color.White);
                        count++;
                    }
                }
            }
            else
            {
                var destRect = new Rectangle(Point.Zero, SpriteSize);
                var viewbounds = Camera.VisibleArea;

                var left = (viewbounds.Left / SpriteSize.X * SpriteSize.X) - SpriteSize.X;
                var top = (viewbounds.Top / SpriteSize.Y * SpriteSize.Y) - SpriteSize.Y;

                for (var x = left; x <= viewbounds.Right; x += SpriteSize.X)
                {
                    for (var y = top; y <= viewbounds.Bottom; y += SpriteSize.Y)
                    {
                        if (x < 1 || y < 1)
                            continue;

                        destRect.Location = new Point(x, y);
                        Engine.SpriteBatch.Draw(Texture, destRect, Map[x, y].Source, Color.White);
                    }
                }
            }
        }
    }
}