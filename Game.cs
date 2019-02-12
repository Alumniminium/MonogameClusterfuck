using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameClusterFuck.Layers;
using MonoGameClusterFuck.Primitives;
using MonoGameClusterFuck.Systems;
using System;

namespace MonoGameClusterFuck
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        public static Game Instance;
        public int Height => GraphicsDevice.PresentationParameters.BackBufferHeight;
        public int Width => GraphicsDevice.PresentationParameters.BackBufferWidth;
        public SpriteBatch SpriteBatch;
        public InputManager InputManager;
        public Camera Camera;

        public Cursor Cursor;
        GraphicsDeviceManager graphics;
        TileSet TileSet;
        KeyboardState KeyboardState;
        FpsCounter FpsCounter;

        public Game()
        {
            IsFixedTimeStep = false; //Allow >60fps
            graphics = new GraphicsDeviceManager(this)
            {
                SynchronizeWithVerticalRetrace = false, //Vsync
                PreferredBackBufferHeight = 480,
                PreferredBackBufferWidth = 854
            };
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Instance = this;
        }

        protected override void Initialize()
        {
            TileSet = new TileSet(32);
            Cursor = new Cursor(32);
            KeyboardState = Keyboard.GetState();
            InputManager = new InputManager();
            FpsCounter = new FpsCounter();
            Camera = new Camera(GraphicsDevice.Viewport);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Cursor.LoadContent();
            TileSet.LoadContent();
            Fonts.LoadContent(Content);

            GlobalState.Layers[LayerType.UI].Add(Cursor);
            GlobalState.Layers[LayerType.Ground].Add(TileSet);
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();
            Camera.Update(GraphicsDevice.Viewport, gameTime);
            Cursor.Update(gameTime);
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
