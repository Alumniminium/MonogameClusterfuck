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

        public override void DrawGame()
        {
            if (!Loaded)
                return;

            var destRect = new Rectangle(Point.Zero, new Point(TileSet.TileSize));
            var viewbounds = Camera.VisibleArea;

            var left = (viewbounds.Left / TileSet.TileSize * TileSet.TileSize) - TileSet.TileSize;
            var top = (viewbounds.Top / TileSet.TileSize * TileSet.TileSize) - TileSet.TileSize;
            var floorTile = TileSet.Tiles[357];
            var wallTile = TileSet.Tiles[5];
            for (var x = left; x <= viewbounds.Right; x += TileSet.TileSize)
            {
                for (var y = top; y <= viewbounds.Bottom; y += TileSet.TileSize)
                {
                    var value = NoiseGen.GetCellular(x, y);

                    if (value > 0.25f)
                    {
                        destRect.Location = new Point(x, y);
                        SpriteBatch.Draw(floorTile.Texture, destRect, floorTile.Source, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1.0f);
                    }
                    else
                    {
                        destRect.Location = new Point(x, y);
                        SpriteBatch.Draw(wallTile.Texture, destRect, wallTile.Source, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1.0f);
                    }
                }
            }
            base.DrawGame();
        }
    }
}