using System.Collections.Concurrent;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck.Entities;
using MonoGameClusterFuck.Layers;
using MonoGameClusterFuck.Networking;
using MonoGameClusterFuck.Networking.Packets;
using MonoGameClusterFuck.Primitives;
using MonoGameClusterFuck.Settings;
using MonoGameClusterFuck.Systems;

namespace MonoGameClusterFuck
{
    public static class Collections
    {
        public static ConcurrentDictionary<uint, Entity> Entities = new ConcurrentDictionary<uint, Entity>();
    }
    public class Engine : Game
    {
        public static Engine Instance;
        public static InputManager InputManager;
        public static SpriteBatch SpriteBatch;
        public static GraphicsDeviceManager Graphics;
        public static TileSet TileSet;
        public static FastNoise NoiseGen = new FastNoise();
        public Engine()
        {
            IsFixedTimeStep = false;
            Graphics = new GraphicsDeviceManager(this)
            {
                SynchronizeWithVerticalRetrace = GraphicsSettings.Instance.VSync,
                PreferredBackBufferHeight = GraphicsSettings.Instance.Height,
                PreferredBackBufferWidth = GraphicsSettings.Instance.Width,
                IsFullScreen = GraphicsSettings.Instance.Fullscreen,
            };
            Graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Instance = this;
        }

        protected override void Initialize()
        {
            GameMap.Layers[LayerType.Cursor].Add(new Cursor(32));
            GameMap.Layers[LayerType.Entity].Add(new Player(32));
            GameMap.Layers[LayerType.UI].Add(new FpsCounter(32));
            TileSet = new TileSet(32);
            TileSet.Slice();
            chunk = new Chunk(Vector2.Zero);
            chunk2 = new Chunk(new Vector2(32*16,0));
            InputManager = new InputManager();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Fonts.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();
            foreach (var layer in GameMap.Layers)
                layer.Value.Update(gameTime);
            
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            DrawGame();
            DrawUI();

            base.Draw(gameTime);
        }

        private void DrawUI()
        {
            SpriteBatch.Begin();
            foreach (var layer in GameMap.Layers.Where(k => k.Key == LayerType.UI))
                layer.Value.Draw();
            SpriteBatch.End();
        }

        private Chunk chunk;
        private Chunk chunk2;
        private void DrawGame()
        {
            SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: Camera.Transform);

            foreach (var layer in GameMap.Layers.Where(k => k.Key != LayerType.UI))
                layer.Value.Draw();

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
            SpriteBatch.End();
        }
    }
}
