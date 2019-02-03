using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using monogame.Primitives;
using monogame.Systems;

namespace monogame
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        public static Game Instance;
        public int Height => GraphicsDevice.PresentationParameters.BackBufferHeight;
        public int Width => GraphicsDevice.PresentationParameters.BackBufferWidth;
        public SpriteBatch SpriteBatch;

        GraphicsDeviceManager graphics;
        TileSet TileSet;
        KeyboardState KeyboardState;
        InputManager InputManager;
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
            KeyboardState = Keyboard.GetState();
            InputManager = new InputManager();
            FpsCounter = new FpsCounter();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            TileSet.LoadContent(Content);
            Fonts.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            InputManager.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            SpriteBatch.Begin();

            if (GlobalState.DrawTileSet)
                TileSet.Draw();
            else
                TileSet.Draw(default(Point));

            if (GlobalState.DisplayHelp)
                SpriteBatch.DrawString(Fonts.Generic, "Press H to toggle Tileset Display", new Vector2(Width - 235, 0), Color.Red);

            FpsCounter.Draw();

            SpriteBatch.End();
            base.Draw(gameTime);
            GlobalState.Frames++;
        }

    }
}
