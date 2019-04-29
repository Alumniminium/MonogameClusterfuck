using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck.Helpers;
using MonoGameClusterFuck.Systems;

namespace MonoGameClusterFuck.SceneManagement.Scenes
{
    public class Splash : Scene
    {
        private bool GoRight = true;
        private DateTime LastUpdate;
        private Texture2D _splashTexture;
        const string Footer = "powered by the AlumniEngine :3";
        Vector2 FooterDimensions, FooterPosition;
        Point LogoPosition, LogoDimensions;
        Rectangle LogoRect;
        public Splash()
        {
            ThreadedConsole.WriteLine("[Scene][Splash] Constructor called!");
        }

        public override void LoadContent()
        {
            ThreadedConsole.WriteLine("[Scene][Splash] Loading content...");
            _splashTexture = Engine.Instance.Content.Load<Texture2D>("splash");
            base.LoadContent();
            CreateLayout();
        }

        public void CreateLayout()
        {
            FooterDimensions = Fonts.Generic.MeasureString(Footer) * 2;
            FooterPosition = UIPlacementHelper.Position(FooterDimensions, UIElementPositioEnEnum.BottomCenter);
            LogoDimensions = new Point(_splashTexture.Width / 8, _splashTexture.Height / 8);
            LogoPosition = UIPlacementHelper.Position(LogoDimensions.ToVector2(), UIElementPositioEnEnum.TopLeftCorner).ToPoint();
            LogoRect = new Rectangle(LogoPosition, LogoDimensions);
        }

        public override void Update(GameTime gameTime)
        {
            if (!Loaded)
                return;

            if (SceneActivatedTime.AddSeconds(5) < DateTime.UtcNow)
            {
                SceneManager.SetState(SceneEnum.InfiniteWorld);
                ThreadedConsole.WriteLine("[Scene][Splash] Transitioning to Scene 2");
            }
            if (LastUpdate.AddMilliseconds(150) < DateTime.Now)
            {
                if (LogoRect.X + (LogoRect.Width * 1.5) > Engine.Graphics.PreferredBackBufferWidth)
                {
                    LogoRect.X = (int)(Engine.Graphics.PreferredBackBufferWidth - (LogoRect.Width * 1.5));
                    LogoRect.Y += 96;
                    GoRight = false;
                }
                else if (GoRight)
                    LogoRect.X += 48;
                else if (LogoRect.X < LogoRect.Width * 0.5)
                {
                    LogoRect.X = (int)(LogoRect.Width * 0.5);
                    LogoRect.Y += 96;
                    GoRight = true;
                }
                else if (!GoRight)
                    LogoRect.X -= 48;

                LastUpdate = DateTime.Now;
            }
            base.Update(gameTime);
        }

        public override void DrawUI()
        {
            if (!Loaded)
                return;

            SpriteBatch.Draw(_splashTexture, LogoRect, Color.White);
            SpriteBatch.DrawString(Fonts.ProFont, Footer, FooterPosition, Color.White);
            base.DrawUI();
        }
    }
}