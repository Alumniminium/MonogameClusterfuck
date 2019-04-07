using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck.Entities;
using MonoGameClusterFuck.Layers;
using MonoGameClusterFuck.Networking;
using MonoGameClusterFuck.Primitives;
using MonoGameClusterFuck.Settings;
using MonoGameClusterFuck.Systems;

namespace MonoGameClusterFuck
{
    public class Engine : Game
    {
        public static Engine Instance;
        public static InputManager InputManager;
        public static SpriteBatch SpriteBatch;
        public static GraphicsDeviceManager Graphics;
        public static NetworkClient NetworkClient;
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
            GameMap.Layers[LayerType.Ground].Add(new TileSet(32));
            GameMap.Layers[LayerType.Entity].Add(new Player(32));
            GameMap.Layers[LayerType.UI].Add(new FpsCounter(32));

            InputManager = new InputManager();
            NetworkClient = new NetworkClient();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Fonts.LoadContent(Content);
            //NetworkClient.ConnectAsync("192.168.0.3", 1337);
            //NetworkClient.Send(MsgLogin.Create("monogame","password"));
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

        private void DrawGame()
        {
            SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: Camera.Transform);

            foreach (var layer in GameMap.Layers.Where(k => k.Key != LayerType.UI))
                layer.Value.Draw();

            SpriteBatch.End();
        }
    }
}
