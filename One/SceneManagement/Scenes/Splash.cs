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
        private bool ShipRight = true;
        private DateTime LastUpdate;
        private Texture2D _splashTexture;
        private Texture2D _shipTexture;
        const string Footer = "powered by the AlumniEngine :3";
        Vector2 FooterDimensions, FooterPosition, FooterDestination;
        Point InvaderPosition, invaderDimensions, ShipPosition, ShipDimensions;
        Rectangle invaderRect, ShipRect;
        public Splash()
        {
            ThreadedConsole.WriteLine("[Scene][Splash] Constructor called!");
        }

        public override void LoadContent()
        {
            ThreadedConsole.WriteLine("[Scene][Splash] Loading content...");
            _splashTexture = Engine.Instance.Content.Load<Texture2D>("splash");
            _shipTexture = Engine.Instance.Content.Load<Texture2D>("ship");
            base.LoadContent();
            CreateLayout();
        }

        public void CreateLayout()
        {
            FooterDimensions = Fonts.Generic.MeasureString(Footer) * 2;
            FooterDestination = UIPlacementHelper.Position(FooterDimensions, UIElementPositioEnEnum.BottomCenter);
            FooterPosition = UIPlacementHelper.Position(FooterDimensions, UIElementPositioEnEnum.BottomCenter);
            FooterPosition.Y += 100;

            ShipDimensions = new Point(_shipTexture.Width/8,_shipTexture.Height/8);
            ShipPosition = UIPlacementHelper.Position(ShipDimensions.ToVector2(), UIElementPositioEnEnum.BottomCenter).ToPoint();
            ShipPosition.Y -= 32;
            ShipRect = new Rectangle(ShipPosition, ShipDimensions);

            invaderDimensions = new Point(_splashTexture.Width / 8, _splashTexture.Height / 8);
            InvaderPosition = UIPlacementHelper.Position(invaderDimensions.ToVector2(), UIElementPositioEnEnum.TopCenter).ToPoint();
            invaderRect = new Rectangle(InvaderPosition, invaderDimensions);
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

            if (SceneActivatedTime.AddSeconds(1) < DateTime.UtcNow)
            {
                if (SceneActivatedTime.AddSeconds(1) < DateTime.UtcNow)
                {
                    if (FooterPosition.Y != FooterDestination.Y)
                    {
                        FooterPosition.Y -= 1 * (float) gameTime.TotalGameTime.TotalSeconds;
                        if (FooterPosition.Y < FooterDestination.Y)
                        {
                            FooterPosition.Y = FooterDestination.Y;
                        }
                    }
                }

                if (LastUpdate.AddMilliseconds(150) < DateTime.UtcNow)
                {
                    if (invaderRect.X + (invaderRect.Width * 1.5) > Engine.Graphics.PreferredBackBufferWidth)
                    {
                        invaderRect.X = (int) (Engine.Graphics.PreferredBackBufferWidth - (invaderRect.Width * 1.5));
                        invaderRect.Y += 96;
                        GoRight = false;
                    }
                    else if (GoRight)
                        invaderRect.X += 48;
                    else if (invaderRect.X < invaderRect.Width * 0.5)
                    {
                        invaderRect.X = (int) (invaderRect.Width * 0.5);
                        invaderRect.Y += 96;
                        GoRight = true;
                    }
                    else if (!GoRight)
                        invaderRect.X -= 48;

                    LastUpdate = DateTime.UtcNow;
                }

                if (ShipRight)
                {
                    if (ShipRect.X + (ShipRect.Width * 1.5) < Engine.Graphics.PreferredBackBufferWidth)
                    {
                        ShipRect.X += 2;
                    }
                    else
                    {
                        ShipRight = false;
                    }
                }
                else
                {
                    if (ShipRect.X > (ShipRect.Width * 0.5))
                    {
                        ShipRect.X -= 2;
                    }
                    else
                    {
                        ShipRight = true;
                    }
                }
            }

            base.Update(gameTime);
        }

        public override void DrawUI()
        {
            if (!Loaded)
                return;

            SpriteBatch.Draw(_splashTexture, invaderRect, Color.White);
            SpriteBatch.Draw(_shipTexture, ShipRect, Color.White);
            SpriteBatch.DrawString(Fonts.ProFont, Footer, FooterPosition, Color.Black);
            base.DrawUI();
        }
    }
}