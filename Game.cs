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
        TileMap Map;
        KeyboardState KeyboardState;
        InputManager InputManager;
        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Instance = this;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Map = new TileMap(32);
            KeyboardState = Keyboard.GetState();
            InputManager = new InputManager();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Map.LoadContent(Content);
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

            if (GlobalState.TileMapDrawing)
                Map.DrawTileMap(spriteBatch);
            else
                Map.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
