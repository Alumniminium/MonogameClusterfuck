using System.Threading.Tasks.Dataflow;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck.Entities;
using MonoGameClusterFuck.Primitives;
using MonoGameClusterFuck.Systems;
using NoiseGen;

namespace MonoGameClusterFuck.SceneManagement.Scenes
{
    public class InfiniteWorld : Scene
    {
        public TileSet TileSet;
        public static FastNoise NoiseGen = new FastNoise();
        public static FastNoise NoiseGen2 = new FastNoise();

        public InfiniteWorld()
        {
            ThreadedConsole.WriteLine("[Scene][InfiniteWorld] Constructor called!");
        }

        public override void Initialize()
        {
            ThreadedConsole.WriteLine("[Scene][InfiniteWorld] Initializing components...");
            TileSet = new TileSet(32);
            TileSet.Slice();
            Entities.Add(new Player(32, 0.01f));
            Entities.Add(new Cursor(32));
            ThreadedConsole.WriteLine("[Scene][InfiniteWorld] Initialization handed over to base class.");
            base.Initialize();
        }

        public override void LoadContent()
        {
            ThreadedConsole.WriteLine("[Scene][InfiniteWorld] Loading content...");
            ThreadedConsole.WriteLine("[Scene][InfiniteWorld] Loading handed over to base class.");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (!Loaded)
                return;
            base.Update(gameTime);
        }

        public override void DrawUI()
        {
            if (!Loaded)
                return;
            FpsCounter.Draw();
            base.DrawUI();
        }
        enum TileType
        {
            Ground,
            Wall
        }
        struct TileInfo
        {
            public TileType Type;
            public TileInfo((float, float) location)
            {
                var value = NoiseGen.GetCubic(location.Item1, location.Item2);
                if (value > 0.10f)
                    Type = TileType.Wall;
                else
                    Type = TileType.Ground;
            }
        }
        public override void DrawGame()
        {
            if (!Loaded)
                return;

            var destRect = new Rectangle(Point.Zero, new Point(TileSet.TileSize));
            var viewbounds = Camera.VisibleArea;
            var left = ((viewbounds.Left / TileSet.TileSize) * TileSet.TileSize) - TileSet.TileSize;
            var top = ((viewbounds.Top / TileSet.TileSize) * TileSet.TileSize) - TileSet.TileSize;

            if (InputState.DrawTileSet)
            {
                int count = 0;
                int yoffset = 0;
                int xoffset = 0;
                destRect.Width = 64;
                destRect.Height = 64;
                foreach (var tile in TileSet.Tiles)
                {
                    if (TileSet.Tiles.Count == count)
                        count = 0;
                    destRect.Location = new Point(xoffset * 64, yoffset * 128);
                    var stringDest = new Vector2(destRect.Location.X, destRect.Y - 32);
                    Engine.SpriteBatch.DrawString(Fonts.ProFont, count.ToString(), stringDest, Color.Black, 0, Vector2.One, 1, SpriteEffects.None, 1.0f);
                    Engine.SpriteBatch.Draw(TileSet.Atlas, destRect, TileSet.Tiles[count].Source, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1.0f);
                    count++;
                    xoffset++;
                    if (xoffset == 12)
                    {
                        xoffset = 0;
                        yoffset++;
                    }
                }
                base.DrawGame();
                return;
            }

            var floorTile = TileSet.Tiles[69];
            var wallTile = TileSet.Tiles[144];
            var upperWallTile = TileSet.Tiles[143];
            var wallTileLeft = TileSet.Tiles[176];
            var wallTileRight = TileSet.Tiles[112];
            var upperWallTileRight = TileSet.Tiles[110];
            var upperWallTileLeft = TileSet.Tiles[175];
            
                    Sprite sprite = floorTile;
            for (var x = left; x <= viewbounds.Right; x += TileSet.TileSize)
            {
                for (var y = top; y <= viewbounds.Bottom; y += TileSet.TileSize)
                {
                        destRect.Location = new Point(x, y);
                         SpriteBatch.Draw(sprite.Texture, destRect, sprite.Source, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1f);
                }
            }

            for (var x = left; x <= viewbounds.Right; x += TileSet.TileSize)
            {
                for (var y = top; y <= viewbounds.Bottom; y += TileSet.TileSize)
                {
                    var value = new TileInfo((x, y));
                    var a = new TileInfo((x, y - 32));
                    var b = new TileInfo((x, y + 32));

                    if(value.Type==TileType.Wall)
                    {
                        sprite=wallTile;
                        if (b.Type == TileType.Ground)
                            sprite = wallTile;
                        if(b.Type== TileType.Wall)
                            sprite=upperWallTile;

                        destRect.Location = new Point(x, y);
                        SpriteBatch.Draw(sprite.Texture, destRect, sprite.Source, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.99f);
                    }
                }
            }
            base.DrawGame();
        }
    }
}