using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck.Primitives;
using MonoGameClusterFuck.Systems;
using System;
using System.Linq;

namespace MonoGameClusterFuck.Entities
{
    public class Player : Sprite
    {
        bool idle = true;
        Animation walkDown, idleDown, walkUp, idleUp, walkLeft, idleLeft, walkRight, idleRight;
        Animation currentAnimation;
        Point DestinationPosition;
        public Player(int size) : base(size)
        {
            
        }

        public override void LoadContent()
        {
            Texture = Game.Instance.Content.Load<Texture2D>("player_f");
        }

        public override void Initialize()
        {
            base.Initialize();
            walkDown = new Animation();
            walkDown.AddFrame(new Rectangle(0, 0, 32, 32), TimeSpan.FromSeconds(.2));
            walkDown.AddFrame(new Rectangle(32, 0, 32, 32), TimeSpan.FromSeconds(.2));
            walkDown.AddFrame(new Rectangle(64, 0, 32, 32), TimeSpan.FromSeconds(.2));
            walkDown.AddFrame(new Rectangle(32, 0, 32, 32), TimeSpan.FromSeconds(.2));

            walkRight = new Animation();
            walkRight.AddFrame(new Rectangle(0, 32, 32, 32), TimeSpan.FromSeconds(.2));
            walkRight.AddFrame(new Rectangle(32, 32, 32, 32), TimeSpan.FromSeconds(.2));
            walkRight.AddFrame(new Rectangle(64, 32, 32, 32), TimeSpan.FromSeconds(.2));
            walkRight.AddFrame(new Rectangle(32, 32, 32, 32), TimeSpan.FromSeconds(.2));

            walkLeft = new Animation();
            walkLeft.AddFrame(new Rectangle(0, 64, 32, 32), TimeSpan.FromSeconds(.2));
            walkLeft.AddFrame(new Rectangle(32, 64, 32, 32), TimeSpan.FromSeconds(.2));
            walkLeft.AddFrame(new Rectangle(64, 64, 32, 32), TimeSpan.FromSeconds(.2));
            walkLeft.AddFrame(new Rectangle(32, 64, 32, 32), TimeSpan.FromSeconds(.2));

            walkUp = new Animation();
            walkUp.AddFrame(new Rectangle(0, 96, 32, 32), TimeSpan.FromSeconds(.2));
            walkUp.AddFrame(new Rectangle(32, 96, 32, 32), TimeSpan.FromSeconds(.2));
            walkUp.AddFrame(new Rectangle(64, 96, 32, 32), TimeSpan.FromSeconds(.2));
            walkUp.AddFrame(new Rectangle(32, 96, 32, 32), TimeSpan.FromSeconds(.2));

            idleDown = new Animation();
            idleDown.AddFrame(new Rectangle(32, 0, 32, 32), TimeSpan.FromSeconds(.5));

            idleRight = new Animation();
            idleRight.AddFrame(new Rectangle(32, 32, 32, 32), TimeSpan.FromSeconds(.5));

            idleLeft = new Animation();
            idleLeft.AddFrame(new Rectangle(32, 64, 32, 32), TimeSpan.FromSeconds(.5));

            idleUp = new Animation();
            idleUp.AddFrame(new Rectangle(32, 96, 32, 32), TimeSpan.FromSeconds(.5));

            currentAnimation = idleDown;

            Position= new Vector2(128,128);
        }



        Vector2 StartPosition;
        Vector2 EndPosition;
        float Timer;
        float Speed = 1f;
        public override void Update(GameTime deltaTime)
        {
            var delta = (float)deltaTime.ElapsedGameTime.TotalSeconds;
            var keyboard = Game.Instance.InputManager.KManager;
            if (idle)
            {
                if (keyboard.KeyDown(PlayerControls.Up))
                {
                    idle = false;
                    StartPosition = Position;
                    EndPosition = new Vector2(StartPosition.X, StartPosition.Y - 32);
                    Timer = 0;
                    currentAnimation = walkUp;
                }
                else if (keyboard.KeyDown(PlayerControls.Left))
                {
                    idle = false;
                    StartPosition = Position;
                    EndPosition = new Vector2(StartPosition.X - 32, StartPosition.Y);
                    Timer = 0;
                    currentAnimation = walkLeft;
                }
                else if (keyboard.KeyDown(PlayerControls.Right))
                {
                    idle = false;
                    StartPosition = Position;
                    EndPosition = new Vector2(StartPosition.X + 32, StartPosition.Y);
                    Timer = 0;
                    currentAnimation = walkRight;
                }
                else if (keyboard.KeyDown(PlayerControls.Down))
                {
                    idle = false;
                    StartPosition = Position;
                    EndPosition = new Vector2(StartPosition.X, StartPosition.Y + 32);
                    Timer = 0;
                    currentAnimation = walkDown;
                }
                else if (!idle)
                {
                    if (currentAnimation == walkUp)
                        currentAnimation = idleUp;
                    else if (currentAnimation == walkLeft)
                        currentAnimation = idleLeft;
                    else if (currentAnimation == walkRight)
                        currentAnimation = idleRight;
                    else
                        currentAnimation = idleDown;
                }
            }
            currentAnimation.Update(deltaTime);

            if (Position != EndPosition)
            {
                Timer += delta * Speed;
                Timer = MathHelper.Min(Timer, 1);
                Position = Vector2.Lerp(StartPosition, EndPosition, Timer);
            }
            else
            {
                idle = true;
            }
            Game.Instance.Camera.MoveCameraAbs(Position);
        }
        public override void Draw()
        {
            Source = currentAnimation.CurrentRectangle;
            base.Draw();
        }
    }
}