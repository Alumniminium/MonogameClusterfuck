using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameClusterFuck.Entities;
using MonoGameClusterFuck.Layers;
using MonoGameClusterFuck.Primitives;
using MonoGameClusterFuck.Settings;
using MonoGameClusterFuck.Systems;
using System;
namespace MonoGameClusterFuck
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        public SpriteBatch SpriteBatch;
        public InputManager InputManager;
        public Camera Camera;

        public Cursor Cursor;
        GraphicsDeviceManager graphics;
        TileSet TileSet;
        KeyboardState KeyboardState;
        FpsCounter FpsCounter;
        Player Player;

        public Game()
        {
            IsFixedTimeStep = false; //Allow >60fps
            graphics = new GraphicsDeviceManager(this)
            {
                SynchronizeWithVerticalRetrace = GraphicsSettings.Instance.VSync,
                PreferredBackBufferHeight = GraphicsSettings.Instance.Height,
                PreferredBackBufferWidth = GraphicsSettings.Instance.Width,
                IsFullScreen = GraphicsSettings.Instance.Fullscreen
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            GlobalState.Game=this;
        }

        protected override void Initialize()
        {
            TileSet = new TileSet(32);
            Cursor = new Cursor(32);
            KeyboardState = Keyboard.GetState();
            InputManager = new InputManager();
            FpsCounter = new FpsCounter();
            Camera = new Camera(GraphicsDevice.Viewport);
            Player=new Player(32);
            Player.Initialize();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Player.LoadContent();
            Cursor.LoadContent();
            TileSet.LoadContent();
            Fonts.LoadContent(Content);
            GlobalState.Layers[LayerType.UI].Add(Cursor);
            GlobalState.Layers[LayerType.Ground].Add(TileSet);
            GlobalState.Layers[LayerType.Entity].Add(Player);
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();
            Camera.Update(GraphicsDevice.Viewport, gameTime);
            Cursor.Update(gameTime);
            Player.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Camera.Transform);

            for (int i = GlobalState.Layers.Count-1; i > 0; i--)
            {
                GlobalState.Layers[(Layers.LayerType)i].Draw();
            }
            SpriteBatch.End();
            SpriteBatch.Begin();
            FpsCounter.Draw();
            SpriteBatch.End();
            base.Draw(gameTime);
            GlobalState.Frames++;
        }
    }
}
