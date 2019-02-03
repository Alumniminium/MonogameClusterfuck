using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace monogame.Primitives
{
    public class TileSet
    {
        public Point TileSize = new Point(32, 32);
        public Texture2D Texture;

        public TileSet(int tileSize)
        {
            TileSize = new Point(tileSize, tileSize);
        }
        internal void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("terrain");
        }

        internal void Draw()
        {
            int count = 0;
            var location = Point.Zero;
            var labelPos = Vector2.Zero;
            var sourceRect = new Rectangle(location, TileSize);
            var destRect = new Rectangle(location, TileSize);

            for (int y = 0; y < Texture.Height; y += TileSize.Y)
            {
                for (int x = 0; x < Texture.Width; x += TileSize.X)
                {
                    location.X = x;
                    location.Y = y;
                    sourceRect.Location = location;
                    destRect.Location = location;
                    labelPos.X = location.X;
                    labelPos.Y = location.Y;

                    Game.Instance.SpriteBatch.Draw(Texture, destRect, sourceRect, Color.White);
                    Game.Instance.SpriteBatch.DrawString(Fonts.Generic, count.ToString(), labelPos, Color.White);
                    count++;
                }
            }
        }

        internal void Draw(Point tile = default(Point))
        {
            if(tile == default(Point))
                tile = new Point(TileSize.X * 13, TileSize.Y * 3);

            var sourceRect = new Rectangle(tile, TileSize);

            var location = Point.Zero;
            var destRect = new Rectangle(Point.Zero,TileSize);

            for (int x = 0; x < Game.Instance.Width; x += TileSize.X)
            {
                for (int y = 0; y < Game.Instance.Height; y += TileSize.Y)
                {
                    location.X = x;
                    location.Y = y;
                    destRect.Location = location;
                    Game.Instance.SpriteBatch.Draw(Texture, destRect, sourceRect, Color.White);
                }
            }
        }
    }
}