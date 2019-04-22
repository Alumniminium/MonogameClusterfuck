using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck.Scenes;
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
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            SceneManager.SetState(0);
        }

        protected override void Initialize()
        {
            SceneManager.Initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Fonts.LoadContent();
            SceneManager.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();
            SceneManager.Update(gameTime);
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DimGray);

            SpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: Camera.Transform);
            SceneManager.DrawGame();
            SpriteBatch.End();

            SpriteBatch.Begin();
            SceneManager.DrawUI();
            SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
