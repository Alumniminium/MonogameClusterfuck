using System.Linq;
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

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        TileSet TileSet;
        KeyboardState KeyboardState;
        InputManager InputManager;
        FpsCounter FpsCounter;
        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.SynchronizeWithVerticalRetrace = true;
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Instance = this;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            TileSet = new TileSet(32);
            KeyboardState = Keyboard.GetState();
            InputManager = new InputManager();
            FpsCounter = new FpsCounter();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
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

            spriteBatch.Begin();

            if (GlobalState.DrawTileSet)
                TileSet.DrawTileSet(spriteBatch);
            else
                TileSet.DrawBgTile(spriteBatch);
            if (GlobalState.DisplayHelp)
            {
                spriteBatch.DrawString(Fonts.Generic, "Press H to toggle Tileset Display", new Vector2(Width - 235, 0), Color.Red);
            }
            FpsCounter.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
            GlobalState.Frames++;
        }

    }
}
