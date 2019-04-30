using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck.SceneManagement;
using MonoGameClusterFuck.Settings;
using MonoGameClusterFuck.Systems;

namespace MonoGameClusterFuck
{
    public class Engine : Game
    {
        public static Engine Instance;
        public static SpriteBatch SpriteBatch;
        public static GraphicsDeviceManager Graphics;
        public Engine()
        {
            ThreadedConsole.WriteLine("[Engine] Initializing Engine...");
            IsFixedTimeStep = false;
            Graphics = new GraphicsDeviceManager(this)
            {
                SynchronizeWithVerticalRetrace = GraphicsSettings.Instance.VSync,
                PreferredBackBufferHeight = GraphicsSettings.Instance.Height,
                PreferredBackBufferWidth = GraphicsSettings.Instance.Width,
                IsFullScreen = GraphicsSettings.Instance.Fullscreen,
            };
            ThreadedConsole.WriteLine("[Engine] Applying Graphics Settings...");
            Graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Instance = this;

            ThreadedConsole.WriteLine("[Engine] Creating SpriteBatch...");
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            ThreadedConsole.WriteLine("[Engine] Starting Scene 0...");
            SceneManager.SetState(0);
        }

        protected override void Initialize()
        {
            ThreadedConsole.WriteLine("[Engine] Initializing components...");
            ThreadedConsole.WriteLine("[Engine] Further initialization handed over to SceneManager...");
            SceneManager.Initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            ThreadedConsole.WriteLine("[Engine] Loading Fonts...");
            Fonts.LoadContent();
            ThreadedConsole.WriteLine("[Engine] Further content loading handed over to SceneManager...");
            SceneManager.LoadContent();
            Sw.Start();
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();
            SceneManager.Update(gameTime);
            base.Update(gameTime);
        }
        public static Stopwatch Sw = new Stopwatch();
        protected override void Draw(GameTime gameTime)
        {
            Sw.Restart();
            GraphicsDevice.Clear(Color.White);
            SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: Camera.Transform);
            SceneManager.DrawGame();
            SpriteBatch.End();

            SpriteBatch.Begin();
            SceneManager.DrawUI();
            SpriteBatch.End();
            base.Draw(gameTime);
            Sw.Stop();
            FpsCounter.Frametime = Sw.Elapsed.TotalMilliseconds;
        }
    }
}
