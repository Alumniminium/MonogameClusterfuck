using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using monogame.Db;
using monogame.Primitives;

namespace monogame
{
    public class GameCore : Game
    {
        int Height => GraphicsDevice.PresentationParameters.BackBufferHeight;
        int Width => GraphicsDevice.PresentationParameters.BackBufferWidth;
            
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        private bool TileMapDrawing;
        TileMap Map;
        public GameCore()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight= 960;
            graphics.PreferredBackBufferWidth=1024;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Map.LoadContent(Content);
            font = Content.Load<SpriteFont>("Font");
        }

        KeyboardState KeyboardState = Keyboard.GetState();
        protected override void Update(GameTime gameTime)
        {
            var lastState = KeyboardState;
            KeyboardState = Keyboard.GetState();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            if(Keyboard.GetState().IsKeyDown(Keys.H) && lastState.IsKeyUp(Keys.H))
                {
                    TileMapDrawing=!TileMapDrawing;
                }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            if(TileMapDrawing)
            DrawTileMap();
            else
            DrawBackground();

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawBackground()
        {
            for (int x = 0; x < Width; x += 32)
            {
                for (int y = 0; y < Height; y += 32)
                {
                    spriteBatch.Draw(AssetDb.Textures[109], new Vector2(x, y));
                }
            }
        }

        private void DrawTileMap()
        {
            var pos = new Vector2(0, 0);
            int count = 0;
            foreach (var kvp in AssetDb.Textures)
            {
                spriteBatch.Draw(kvp.Value, pos);
                spriteBatch.DrawString(font, count.ToString(), pos, Color.White);
                pos.X += 32;
                if (pos.X > Width)
                {
                    pos.Y += 32;
                    pos.X = 0;
                }
                count++;
            }
        }
    }
}
