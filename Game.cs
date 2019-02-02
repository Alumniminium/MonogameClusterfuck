using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using monogame.Db;
using monogame.Primitives;

namespace monogame
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        public static Game Instance;
        public int Height => GraphicsDevice.PresentationParameters.BackBufferHeight;
        public int Width => GraphicsDevice.PresentationParameters.BackBufferWidth;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        private bool TileMapDrawing;
        TileMap Map;
        KeyboardState KeyboardState;
        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 960;
            graphics.PreferredBackBufferWidth = 1024;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Instance = this;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Map = new TileMap();
            KeyboardState = Keyboard.GetState();
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
            var lastState = KeyboardState;
            KeyboardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.H) && lastState.IsKeyUp(Keys.H))
            {
                TileMapDrawing = !TileMapDrawing;
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            if (TileMapDrawing)
                Map.DrawTileMap(spriteBatch);
            else
                Map.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
