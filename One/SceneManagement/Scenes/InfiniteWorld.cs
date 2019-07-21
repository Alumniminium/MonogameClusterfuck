using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using One.Primitives;
using One.Systems;
using NoiseGen;
using One.SceneManagement.Primitives;
using Player = One.Entities.Player;
using System;
using One.UI;
using System.Collections.Generic;
using One.Primitives.WorldGen;

namespace One.SceneManagement.Scenes
{
    public class InfiniteWorld : Scene
    {
        public TileSet TileSet;
        public static FastNoise NoiseGen = new FastNoise();

        public static Texture2D lightMask;
        RenderTarget2D lightsTarget;
        RenderTarget2D mainTarget;
        Effect effect1;

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
            lightsTarget = new RenderTarget2D(Engine.Graphics.GraphicsDevice, Engine.Graphics.PreferredBackBufferWidth, Engine.Graphics.PreferredBackBufferHeight);
            mainTarget = new RenderTarget2D(Engine.Graphics.GraphicsDevice, Engine.Graphics.PreferredBackBufferWidth, Engine.Graphics.PreferredBackBufferHeight);
            ThreadedConsole.WriteLine("[Scene][InfiniteWorld] Initialization handed over to base class.");
            base.Initialize();
        }

        public override void LoadContent()
        {
            ThreadedConsole.WriteLine("[Scene][InfiniteWorld] Loading content...");
            UIRect.Initialize();
            //lightMask = Engine.Instance.Content.Load<Texture2D>("Shaders/lightmask");
            //effect1 = Engine.Instance.Content.Load<Effect>("Shaders/lighteffect");
            ThreadedConsole.WriteLine("[Scene][InfiniteWorld] Loading handed over to base class.");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (!Loaded)
                return;
            var viewbounds = Camera.VisibleArea;
            var left = ((viewbounds.Left / TileSet.TileSize) * TileSet.TileSize) - TileSet.TileSize;
            var top = ((viewbounds.Top / TileSet.TileSize) * TileSet.TileSize) - TileSet.TileSize;
            for (var x = left; x <= viewbounds.Right; x += Chunk.ChunkSize * TileSet.TileSize)
            {
                for (var y = top; y <= viewbounds.Bottom; y += Chunk.ChunkSize * TileSet.TileSize)
                {
                    SimulationManager.LoadArea(viewbounds);
                }
            }

            base.Update(gameTime);
        }

        UIRectangle UIRect = new UIRectangle(100, 40, Color.Purple);
        public override void DrawUI()
        {
            if (!Loaded)
                return;
            FpsCounter.Draw();
            UIRect.Position = new Vector2(350, 200);
            UIRect.Draw();
            base.DrawUI();
        }
        public override void DrawGame()
        {
            if (!Loaded)
                return;

            var destRect = new Rectangle(Point.Zero, new Point(TileSet.TileSize));

            //var lightLowerTile = TileSet.Tiles[102];
            //var lightUpperTile = TileSet.Tiles[103];
            //var floorTile = TileSet.Tiles[69];
            //var wallTile = TileSet.Tiles[144];
            //var upperWallTile = TileSet.Tiles[143];
            //var wallTileLeft = TileSet.Tiles[176];
            //var wallTileRight = TileSet.Tiles[112];
            //var upperWallTileRight = TileSet.Tiles[110];
            //var upperWallTileLeft = TileSet.Tiles[175];

            Sprite sprite = TileSet.Tiles[69];

            Engine.Instance.GraphicsDevice.SetRenderTarget(mainTarget);

            if (InputState.DrawTileSet)
            {
                DrawTileset(destRect);
            }
            else
            {
                foreach (var (location, chunk) in SimulationManager.LoadedChunks)
                {
                    for (int x = 0; x < Chunk.ChunkSize; x++)
                    {
                        for (int y = 0; y < Chunk.ChunkSize; y++)
                        {
                            sprite = TileSet.Tiles[chunk.Cells[x, y].SpriteId];

                            destRect = new Rectangle(x * 32, y * 32, 32, 32);
                            destRect.Location += location.ToPoint();
                            SpriteBatch.Draw(sprite.Texture, destRect, sprite.Source, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1f);
                        }
                    }
                }
                base.DrawGame();
                SpriteBatch.End();

                if (InputState.UseLighting)
                {
                    DrawLights();
                }
                else
                {
                    Engine.Instance.GraphicsDevice.SetRenderTarget(null);
                    SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
                    SpriteBatch.Draw(mainTarget, Vector2.Zero, Color.White);
                }
            }
        }

        private void DrawTileset(Rectangle destRect)
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
                Engine.SpriteBatch.DrawString(Fonts.ProFont, count.ToString(), stringDest, Color.Magenta, 0, Vector2.One, 1, SpriteEffects.None, 1.0f);
                Engine.SpriteBatch.Draw(TileSet.Atlas, destRect, TileSet.Tiles[count].Source, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1.0f);
                count++;
                xoffset++;
                if (xoffset == 12)
                {
                    xoffset = 0;
                    yoffset++;
                }
            }
        }

        private void DrawLights()
        {
            Engine.Instance.GraphicsDevice.SetRenderTarget(lightsTarget);
            Engine.Instance.GraphicsDevice.Clear(Color.Black);
            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, transformMatrix: Camera.Transform);
            SpriteBatch.Draw(lightMask, new Vector2(1 * 32, 1 * 32), null, Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            SpriteBatch.Draw(lightMask, new Vector2(1 * 32, 10 * 32), null, Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            SpriteBatch.Draw(lightMask, new Vector2(10 * 32, 1 * 32), null, Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            SpriteBatch.Draw(lightMask, new Vector2(17 * 32, 20 * 32), null, Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            SpriteBatch.Draw(lightMask, new Vector2(2 * 32, 29 * 32), null, Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            SpriteBatch.End();

            Engine.Instance.GraphicsDevice.SetRenderTarget(null);
            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            effect1.Parameters["lightMask"].SetValue(lightsTarget);
            effect1.CurrentTechnique.Passes[0].Apply();
            SpriteBatch.Draw(mainTarget, Vector2.Zero, Color.White);
        }
    }
}