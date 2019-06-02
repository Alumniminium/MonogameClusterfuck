using System.Threading.Tasks.Dataflow;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using One.Primitives;
using One.Systems;
using NoiseGen;
using System;
using One.SceneManagement.Primitives;
using Player = One.Entities.Player;

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
            lightMask = Engine.Instance.Content.Load<Texture2D>("Shaders/lightmask");
            effect1 = Engine.Instance.Content.Load<Effect>("Shaders/lighteffect");
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
            var left = ((viewbounds.Left / TileSet.TileSize) * TileSet.TileSize) - TileSet.TileSize;
            var top = ((viewbounds.Top / TileSet.TileSize) * TileSet.TileSize) - TileSet.TileSize;

            var lightLowerTile = TileSet.Tiles[102];
            var lightUpperTile = TileSet.Tiles[103];
            var floorTile = TileSet.Tiles[69];
            var wallTile = TileSet.Tiles[144];
            var upperWallTile = TileSet.Tiles[143];
            var wallTileLeft = TileSet.Tiles[176];
            var wallTileRight = TileSet.Tiles[112];
            var upperWallTileRight = TileSet.Tiles[110];
            var upperWallTileLeft = TileSet.Tiles[175];

            Sprite sprite = floorTile;

            Engine.Instance.GraphicsDevice.SetRenderTarget(mainTarget);
            if (InputState.DrawTileSet)
            {
                DrawTileset(destRect);
            }
            else
            {
                for (var x = left; x <= viewbounds.Right; x += TileSet.TileSize)
                {
                    for (var y = top; y <= viewbounds.Bottom; y += TileSet.TileSize)
                    {
                        var value = new TileInfo((x, y));
                        var a = new TileInfo((x, y - 32));
                        var b = new TileInfo((x, y + 32));
                        destRect.Location = new Point(x, y);
                        if (value.Type == TileType.Ground)
                        {
                            if (Core.Success(10))
                            {
                                SpriteBatch.Draw(lightLowerTile.Texture, destRect, lightLowerTile.Source, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1f);
                                destRect.Location = new Point(x, y-32);
                                SpriteBatch.Draw(lightUpperTile.Texture, destRect, lightUpperTile.Source, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1f);
                            }
                            SpriteBatch.Draw(floorTile.Texture, destRect, floorTile.Source, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1f);
                        }
                        else if (value.Type == TileType.Wall)
                        {
                            sprite = wallTile;
                            if (b.Type == TileType.Ground)
                                sprite = wallTile;
                            if (b.Type == TileType.Wall)
                                sprite = upperWallTile;

                            SpriteBatch.Draw(sprite.Texture, destRect, sprite.Source, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0.99f);
                        }
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
            SpriteBatch.Draw(lightMask, new Vector2(10000 * 32, 10000 * 32), null, Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            SpriteBatch.Draw(lightMask, new Vector2(10001 * 32, 10010 * 32), null, Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            SpriteBatch.Draw(lightMask, new Vector2(10010 * 32, 10001 * 32), null, Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            SpriteBatch.Draw(lightMask, new Vector2(10017 * 32, 10020 * 32), null, Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            SpriteBatch.Draw(lightMask, new Vector2(10020 * 32, 10004 * 32), null, Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
            SpriteBatch.End();

            Engine.Instance.GraphicsDevice.SetRenderTarget(null);
            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            effect1.Parameters["lightMask"].SetValue(lightsTarget);
            effect1.CurrentTechnique.Passes[0].Apply();
            SpriteBatch.Draw(mainTarget, Vector2.Zero, Color.White);
        }
    }
}