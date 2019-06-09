using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using One.Helpers;
using One.Systems;

namespace One.SceneManagement.Scenes
{
    public enum StateEnum
    {
        Zero = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5
    }
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
        StateEnum State = StateEnum.Zero;
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

            ShipDimensions = new Point(_shipTexture.Width / 8, _shipTexture.Height / 8);
            ShipPosition = UIPlacementHelper.Position(ShipDimensions.ToVector2(), UIElementPositioEnEnum.BottomCenter).ToPoint();
            ShipPosition.Y += 64;
            ShipRect = new Rectangle(ShipPosition, ShipDimensions);

            invaderDimensions = new Point(_splashTexture.Width / 8, _splashTexture.Height / 8);
            InvaderPosition = UIPlacementHelper.Position(invaderDimensions.ToVector2(), UIElementPositioEnEnum.TopCenter).ToPoint();
            invaderRect = new Rectangle(InvaderPosition, invaderDimensions);
        }

        public override void Update(GameTime gameTime)
        {
            if (!Loaded)
                return;


            switch (State)
            {
                case StateEnum.Zero:
                    {
                        MoveSpaceInvader();
                        SlideShipIntoView(gameTime);
                        break;
                    }
                case StateEnum.One:
                    {
                        MoveSpaceInvader();
                        SlideFooterIntoView(gameTime);
                        break;
                    }
                case StateEnum.Two:
                    {
                        MoveSpaceInvader();
                        MoveShip();
                        break;
                    }
                case StateEnum.Three:
                case StateEnum.Four:
                case StateEnum.Five:
                    break;
            }

            if (SceneActivatedTime.AddSeconds(0) < DateTime.UtcNow)
            {
                SceneManager.SetState(SceneEnum.InfiniteWorld);
                ThreadedConsole.WriteLine("[Scene][Splash] Transitioning to Scene 2");
            }
            base.Update(gameTime);
        }

        private void SlideShipIntoView(GameTime gameTime)
        {
            var destination = UIPlacementHelper.Position(ShipDimensions.ToVector2(), UIElementPositioEnEnum.BottomCenter).Y - 48;
            if (ShipPosition.Y != destination)
            {
                ShipPosition.Y -= (int)(3 * (float)gameTime.TotalGameTime.TotalSeconds);

                if (ShipPosition.Y <= destination)
                {
                    ShipPosition.Y = (int)destination;
                    State = StateEnum.One;
                }
                ShipRect.X = ShipPosition.X;
                ShipRect.Y = ShipPosition.Y;
            }
        }

        private void MoveShip()
        {
            if (ShipRight)
            {
                if (ShipRect.X + (ShipRect.Width * 1.5) < Engine.Graphics.PreferredBackBufferWidth)
                    ShipRect.X += 6;
                else
                ShipRight = false;
            }
            else
            {
                if (ShipRect.X > (ShipRect.Width * 0.5))
                    ShipRect.X -= 6;
                else
                    ShipRight = true;
            }
        }

        private void MoveSpaceInvader()
        {
            if (LastUpdate.AddMilliseconds(150) < DateTime.UtcNow)
            {
                if (invaderRect.X + (invaderRect.Width * 1.5) > Engine.Graphics.PreferredBackBufferWidth)
                {
                    invaderRect.X = (int)(Engine.Graphics.PreferredBackBufferWidth - (invaderRect.Width * 1.5));
                    invaderRect.Y += 96;
                    GoRight = false;
                }
                else if (GoRight)
                    invaderRect.X += 48;
                else if (invaderRect.X < invaderRect.Width * 0.5)
                {
                    invaderRect.X = (int)(invaderRect.Width * 0.5);
                    invaderRect.Y += 96;
                    GoRight = true;
                }
                else if (!GoRight)
                    invaderRect.X -= 48;

                LastUpdate = DateTime.UtcNow;
            }
        }

        private void SlideFooterIntoView(GameTime gameTime)
        {
            if (FooterPosition.Y != FooterDestination.Y)
            {
                FooterPosition.Y -= 1 * (float)gameTime.TotalGameTime.TotalSeconds;
                if (FooterPosition.Y < FooterDestination.Y)
                {
                    FooterPosition.Y = FooterDestination.Y;
                    State = StateEnum.Two;
                }
            }
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