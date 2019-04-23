using System;
using Microsoft.Xna.Framework;
using MonoGameClusterFuck.Entities;
using MonoGameClusterFuck.Primitives;
using MonoGameClusterFuck.Systems;
using NoiseGen;
using One.Systems;

namespace MonoGameClusterFuck.SceneManagement.Scenes
{
    public class InfiniteWorld : Scene
    {
        public TileSet TileSet;
        public FastNoise NoiseGen = new FastNoise();

        public InfiniteWorld()
        {
            ThreadedConsole.WriteLine("[Scene][InfiniteWorld] Constructor called!");
        }

        public override void Initialize()
        {
            ThreadedConsole.WriteLine("[Scene][InfiniteWorld] Initializing components...");
            TileSet = new TileSet(32);
            TileSet.Slice();
            Entities.Add(new Player(32,1));
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
            base.Update(gameTime);
        }

        public override void DrawUI()
        {
            FpsCounter.Draw();
            base.DrawUI();
        }

        public override void DrawGame()
        {
            var SpriteSize = TileSet.TileSize;
            var destRect = new Rectangle(Point.Zero, new Point(SpriteSize));
            var viewbounds = Camera.VisibleArea;

            var left = (viewbounds.Left / SpriteSize * SpriteSize) - SpriteSize;
            var top = (viewbounds.Top / SpriteSize * SpriteSize) - SpriteSize;
            var FloorTile = TileSet.Tiles[357];
            var WallTile = TileSet.Tiles[5];
            for (var x = left; x <= viewbounds.Right; x += SpriteSize)
            {
                for (var y = top; y <= viewbounds.Bottom; y += SpriteSize)
                {
                    var value = NoiseGen.GetCellular(x, y);

                    if (value > 0.45f)
                    {
                        destRect.Location = new Point(x, y);
                        SpriteBatch.Draw(FloorTile.Texture, destRect, FloorTile.Source, Color.White);
                    }
                    else
                    {
                        destRect.Location = new Point(x, y);
                        SpriteBatch.Draw(WallTile.Texture, destRect, WallTile.Source, Color.White);
                    }
                }
            }
            base.DrawGame();
        }
    }
}