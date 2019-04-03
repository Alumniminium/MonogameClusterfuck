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
    public class Engine : Microsoft.Xna.Framework.Game
    {
        public static Client Client;
        public static Engine Instance;
        public static bool DrawTileSet { get; set; }
        public static int Frames { get; internal set; }
        public static SpriteBatch SpriteBatch;
        public static Camera Camera;
        public static KeyboardManager KeyboardManager;
        public static InputManager InputManager;
        public static Cursor Cursor;
        public static GraphicsDeviceManager Graphics;
        public static TileSet TileSet;
        public static FpsCounter FpsCounter;
        public static Player Player;

        public Engine()
        {
            IsFixedTimeStep = false; //Allow >60fps
            Graphics = new GraphicsDeviceManager(this)
            {
                SynchronizeWithVerticalRetrace = GraphicsSettings.Instance.VSync,
                PreferredBackBufferHeight = GraphicsSettings.Instance.Height,
                PreferredBackBufferWidth = GraphicsSettings.Instance.Width,
                IsFullScreen = GraphicsSettings.Instance.Fullscreen
            };
            //Graphics.PreferMultiSampling = true;
            Graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Instance = this;
        }

        protected override void Initialize()
        {
            TileSet = new TileSet(32);
            Cursor = new Cursor(32);
            InputManager = new InputManager();
            KeyboardManager = InputManager.KManager;
            FpsCounter = new FpsCounter();
            Camera = new Camera(GraphicsDevice.Viewport);
            Player = new Player(32);
            Player.Initialize();
            Client = new Client();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Player.LoadContent();
            Cursor.LoadContent();
            TileSet.LoadContent();
            Fonts.LoadContent(Content);
            GameMap.Layers[LayerType.Cursor].Add(Cursor);
            GameMap.Layers[LayerType.Ground].Add(TileSet);
            GameMap.Layers[LayerType.Entity].Add(Player);
            Client.ConnectAsync("192.168.0.3", 1337);
            Client.Send(MsgLogin.Create("monogame","password"));
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();
            Camera.Update(GraphicsDevice.Viewport, gameTime);
            Player.Update(gameTime);
            Cursor.Update(gameTime);
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Camera.Transform);

            foreach (var layer in GameMap.Layers)
                layer.Value.Draw();

            SpriteBatch.End();

            SpriteBatch.Begin();
            FpsCounter.Draw();
            SpriteBatch.End();
            base.Draw(gameTime);
            Frames++;
        }
    }
}
