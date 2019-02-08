using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
                PreferredBackBufferHeight = 720,
                PreferredBackBufferWidth = 1280
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
            Camera=new Camera(GraphicsDevice.Viewport);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Cursor.LoadContent();
            TileSet.LoadContent(Content);
            Fonts.LoadContent(Content);
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

            SpriteBatch.Begin(SpriteSortMode.Deferred,BlendState.AlphaBlend,SamplerState.PointClamp,DepthStencilState.Default,RasterizerState.CullNone,null,Camera.Transform);


            if (GlobalState.DrawTileSet)
                TileSet.Draw();
            else
                TileSet.Draw(default(Point));

            if (GlobalState.DisplayHelp)
                SpriteBatch.DrawString(Fonts.Generic, "Press H to toggle Tileset Display", new Vector2(Width - 235, 0), Color.Red);

            FpsCounter.Draw();


            Cursor.Draw();
            SpriteBatch.End();
            base.Draw(gameTime);
            GlobalState.Frames++;
        }

    }
}
