using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameClusterFuck.Primitives
{
    public class TileSet : Sprite
    {
        public TileSet(int size) : base(size)
        {

        }

        public override void LoadContent()
        {
            Texture = Game.Instance.Content.Load<Texture2D>("terrain");
        }

        public override void Draw()
        {
                int count = 0;
            if (GlobalState.DrawTileSet)
            {
                var location = Point.Zero;
                var labelPos = Vector2.Zero;
                var sourceRect = new Rectangle(location, Size);
                var destRect = new Rectangle(location, Size);
                var viewbounds = Game.Instance.Camera.VisibleArea;

                var xbounds = viewbounds.X / 32 * 32;
                var ybounds = viewbounds.Y / 32 * 32;
                for (int y = 0; y < Game.Instance.Height; y += Size.Y)
                {
                    for (int x = 0; x < Game.Instance.Width; x += Size.X)
                    {
                        location.X = x;
                        location.Y = y;
                        sourceRect.Location = location;
                        destRect.Location = location;
                        labelPos.X = location.X;
                        labelPos.Y = location.Y;

                        Game.Instance.SpriteBatch.Draw(Texture, destRect, sourceRect, Color.White);
                        //Game.Instance.SpriteBatch.DrawString(Fonts.Generic, count.ToString(), labelPos, Color.White);
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
                var viewbounds = Game.Instance.Camera.VisibleArea;

                var xbounds = viewbounds.X / 32 * 32;
                var ybounds = viewbounds.Y / 32 * 32;
                for (int x = 0; x < Game.Instance.Width; x += Size.X)
                {
                    for (int y = 0; y < Game.Instance.Height; y += Size.Y)
                    {
                        location.X = x;
                        location.Y = y;
                        destRect.Location = location;
                        Game.Instance.SpriteBatch.Draw(Texture, destRect, sourceRect, Color.White);
                        count++;
                    }
                }
            }
        }
    }
}