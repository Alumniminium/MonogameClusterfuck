using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using One.Systems;

namespace MonoGameClusterFuck.SceneManagement.Scenes
{
    public class Splash : Scene
    {
        private Texture2D _slashTexture;
        const string Footer = "powered by the AlumniEngine :3";
        public Splash()
        {
            ThreadedConsole.WriteLine("[Scene][Splash] Constructor called!");
        }

        public override void LoadContent()
        {
            ThreadedConsole.WriteLine("[Scene][Splash] Loading content...");
            _slashTexture = Engine.Instance.Content.Load<Texture2D>("splash");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (SceneActivatedTime.AddSeconds(5) < DateTime.UtcNow)
            {
                SceneManager.SetState(SceneEnum.InfiniteWorld);
                ThreadedConsole.WriteLine("[Scene][Splash] Transitioning to Scene 2");
            }
            base.Update(gameTime);
        }

        public override void DrawUI()
        {
            var x = (Engine.Graphics.PreferredBackBufferWidth / 2) - (_slashTexture.Width / 2);
            var y = (Engine.Graphics.PreferredBackBufferHeight / 2) - (_slashTexture.Height / 2);
            SpriteBatch.Draw(_slashTexture, new Vector2(x, y), Color.White);
            var size = Fonts.Generic.MeasureString(Footer);
            x = (Engine.Graphics.PreferredBackBufferWidth / 2) - (int)(size.X / 2);
            y = Engine.Graphics.PreferredBackBufferHeight - (int)size.Y;
            SpriteBatch.DrawString(Fonts.Generic, Footer, new Vector2(x, y), Color.White);
            base.DrawUI();
        }
    }
}