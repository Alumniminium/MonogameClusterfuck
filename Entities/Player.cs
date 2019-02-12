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
        bool idle = false;
        Animation walkDown, idleDown, walkUp, idleUp, walkLeft, idleLeft, walkRight, idleRight;
        Animation currentAnimation;
        public Player(int size) : base(size)
        {
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

            Destination = new Rectangle(Game.Instance.Width/2,Game.Instance.Height/2,32,32);
            currentAnimation = idleDown;
        }
        public override void LoadContent()
        {
            Texture = Game.Instance.Content.Load<Texture2D>("player_f");
        }
        public override void Update(GameTime deltaTime)
        {
            var force = Vector2.Zero;
            var delta = (float)deltaTime.ElapsedGameTime.TotalSeconds;
            var kbstate = Game.Instance.InputManager.KeyboardState;
            if (kbstate.IsKeyDown(PlayerControls.Up))
            {
                idle=false;
                force.Y -=100;
                currentAnimation = walkUp;
            }
            else if (kbstate.IsKeyDown(PlayerControls.Left))
            {
                idle=false;
                force.X -=100;
                currentAnimation = walkLeft;
            }
            else if (kbstate.IsKeyDown(PlayerControls.Right))
            {
                idle=false;
                force.X +=100;
                currentAnimation = walkRight;
            }
            else if (kbstate.IsKeyDown(PlayerControls.Down))
            {
                idle=false;
                force.Y +=100;
                currentAnimation = walkDown;
            }
            else if(!idle)
            {
                if (currentAnimation == walkUp)
                    currentAnimation = idleUp;
                else if (currentAnimation == walkLeft)
                    currentAnimation = idleLeft;
                else if (currentAnimation == walkRight)
                    currentAnimation = idleRight;
                else
                    currentAnimation = idleDown;

                idle=true;
            }
            currentAnimation.Update(deltaTime);
            force.X = force.X*delta;
            force.Y= force.Y*delta;
            Destination.X += (int)force.X;
            Destination.Y += (int)force.Y;
            Game.Instance.Camera.MoveCameraAbs(new Vector2(Destination.X,Destination.Y));
        }
        public override void Draw()
        {
            Source = currentAnimation.CurrentRectangle;
            base.Draw();
        }
    }
}