using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoiseGen;

namespace MonoGameClusterFuck.Primitives
{
    public class TileSet : DrawableComponent
    {
        public static FastNoise Noise = new FastNoise();
        public Tile FloorTile;
        public Tile WallTile;

        public TileSet(int size)
        {

        }

        public override void LoadContent()
        {
            Texture = Engine.Instance.Content.Load<Texture2D>("terrain");
            FloorTile = new Tile(32);
            WallTile = new Tile(32);
            FloorTile.Texture = Texture;
            WallTile.Texture = Texture;
            FloorTile.Source.Location = new Point(Size.X * 13, Size.Y * 3);
            WallTile.Source.Location = new Point(Size.X * 4, Size.Y * 15);
        }

        public override void Update(GameTime deltaTime)
        {

        }

        public override void Draw(Layers.LayerType type)
        {
            var count = 0;
            if (Engine.DrawTileSet)
            {
                var location = Point.Zero;
                var labelPos = Vector2.Zero;
                var sourceRect = new Rectangle(location, Size);
                var destRect = new Rectangle(location, Size);
                var viewbounds = Engine.Camera.VisibleArea;

                var left = (viewbounds.Left / Size.X * Size.X) - Size.X;
                var top = (viewbounds.Top / Size.Y * Size.Y) - Size.Y;
                for (var y = top; y <= Math.Min(Texture.Height,viewbounds.Bottom); y += Size.Y)
                {
                    for (var x = left; x <= Math.Min(Texture.Width,viewbounds.Right); x += Size.X)
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
                var destRect = new Rectangle(Point.Zero, Size);
                var viewbounds = Engine.Camera.VisibleArea;

                var left = (viewbounds.Left / Size.X * Size.X) - Size.X;
                var top = (viewbounds.Top / Size.Y * Size.Y) - Size.Y;
                
                for (var x = left; x <= viewbounds.Right; x += Size.X)
                {
                    for (var y = top; y <= viewbounds.Bottom; y += Size.Y)
                    {
                        var value = Noise.GetCellular(x, y);
                        
                        if (value > 0.45f)
                        {
                            destRect.Location = new Point(x, y);
                            Engine.SpriteBatch.Draw(Texture, destRect, FloorTile.Source, Color.White);
                        }
                        else
                        {
                            destRect.Location = new Point(x, y);
                            Engine.SpriteBatch.Draw(Texture, destRect, WallTile.Source, Color.White);
                        }
                    }
                }
            }
        }
    }
}