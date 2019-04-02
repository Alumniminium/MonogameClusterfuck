using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NoiseGen;

namespace MonoGameClusterFuck.Primitives
{
    public class TileSet : Sprite
    {
        public static FastNoise Noise = new FastNoise();
        public TileSet(int size) : base(size)
        {

        }

        public override void LoadContent()
        {
            Texture = Engine.Instance.Content.Load<Texture2D>("terrain");
        }

        public override void Draw(Layers.LayerType type)
        {
            int count = 0;
            if (Engine.DrawTileSet)
            {
                var location = Point.Zero;
                var labelPos = Vector2.Zero;
                var sourceRect = new Rectangle(location, Size);
                var destRect = new Rectangle(location, Size);
                var viewbounds = Engine.Camera.VisibleArea;

                var left = (viewbounds.Left / Size.X * Size.X) - Size.X;
                var top = (viewbounds.Top / Size.Y * Size.Y) - Size.Y;
                for (int y = top; y <= Math.Min(Texture.Height,viewbounds.Bottom); y += Size.Y)
                {
                    for (int x = left; x <= Math.Min(Texture.Width,viewbounds.Right); x += Size.X)
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
                var tile = new Point(Size.X * 13, Size.Y * 3);

                var sourceRect = new Rectangle(tile, Size);

                var location = Point.Zero;
                var destRect = new Rectangle(Point.Zero, Size);
                var viewbounds = Engine.Camera.VisibleArea;

                var left = (viewbounds.Left / Size.X * Size.X) - Size.X;
                var top = (viewbounds.Top / Size.Y * Size.Y) - Size.Y;
                
                for (int x = left; x <= viewbounds.Right; x += Size.X)
                {
                    for (int y = top; y <= viewbounds.Bottom; y += Size.Y)
                    {
                        float value = Noise.GetCellular(x, y);
                        if (value > 0.45f)
                        {
                            tile = new Point(Size.X * 13, Size.Y * 3);
                            sourceRect = new Rectangle(tile, Size);
                            destRect.Location = new Point(x, y);
                            Engine.SpriteBatch.Draw(Texture, destRect, sourceRect, Color.White);
                        }
                        else
                        {
                            tile = new Point(Size.X * 4, Size.Y * 15);

                            sourceRect = new Rectangle(tile, Size);
                            destRect.Location = new Point(x, y);
                            Engine.SpriteBatch.Draw(Texture, destRect, sourceRect, Color.White);
                        }
                    }
                }
            }
        }
    }
}