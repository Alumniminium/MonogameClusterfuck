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
            Entities.Add(new Player(32, 0.1f));
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
        Sprite sprite = new Sprite(32, 1f);
        public override void DrawGame()
        {
            if (!Loaded)
                return;

            //var lightLowerTile = TileSet.Tiles[102];
            //var lightUpperTile = TileSet.Tiles[103];
            //var floorTile = TileSet.Tiles[69];
            //var wallTile = TileSet.Tiles[144];
            //var upperWallTile = TileSet.Tiles[143];
            //var wallTileLeft = TileSet.Tiles[176];
            //var wallTileRight = TileSet.Tiles[112];
            //var upperWallTileRight = TileSet.Tiles[110];
            //var upperWallTileLeft = TileSet.Tiles[175];


            Engine.Instance.GraphicsDevice.SetRenderTarget(mainTarget);

            if (InputState.DrawTileSet)
            {
                DrawTileset(new Rectangle(0, 0, Engine.Graphics.PreferredBackBufferWidth, Engine.Graphics.PreferredBackBufferHeight));
            }
            else
            {
                for (int x = Camera.VisibleArea.Left; x < Camera.VisibleArea.Right; x += 32)
                {
                    for (int y = Camera.VisibleArea.Top; y < Camera.VisibleArea.Bottom; y += 32)
                    {
                        var tile = NoiseGen.GetPerlin(x / 100, y / 100);
                        var spriteId = 52;

                        if (tile > 0.3)
                            spriteId = 35; //sand
                        if (tile > 0.4)
                            spriteId = 41; //grass
                        if (tile > 0.70)
                            spriteId = 227; //dirt
                        if (tile > 0.89)
                            spriteId = 323; //rock
                        if (tile == 1)
                            spriteId = 593; //snow

                        sprite = TileSet.Tiles[spriteId];
                        var destRect = new Rectangle(x, y, 32, 32);
                        SpriteBatch.Draw(sprite.Texture, destRect, sprite.Source, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1f);
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