using System;
using Microsoft.Xna.Framework;

namespace One.Animations
{
    public class WalkAnimations
    {
        public Animation WalkDown, IdleDown, WalkUp, IdleUp, WalkLeft, IdleLeft, WalkRight, IdleRight;

        public WalkAnimations()
        {
            WalkDown = new Animation();
            WalkDown.AddFrame(new Rectangle(0, 0, 32, 32), TimeSpan.FromSeconds(.1));
            WalkDown.AddFrame(new Rectangle(32, 0, 32, 32), TimeSpan.FromSeconds(.1));
            WalkDown.AddFrame(new Rectangle(64, 0, 32, 32), TimeSpan.FromSeconds(.1));
            WalkDown.AddFrame(new Rectangle(32, 0, 32, 32), TimeSpan.FromSeconds(.1));

            WalkRight = new Animation();
            WalkRight.AddFrame(new Rectangle(0, 32, 32, 32), TimeSpan.FromSeconds(.1));
            WalkRight.AddFrame(new Rectangle(32, 32, 32, 32), TimeSpan.FromSeconds(.1));
            WalkRight.AddFrame(new Rectangle(64, 32, 32, 32), TimeSpan.FromSeconds(.12));
            WalkRight.AddFrame(new Rectangle(32, 32, 32, 32), TimeSpan.FromSeconds(.1));

            WalkLeft = new Animation();
            WalkLeft.AddFrame(new Rectangle(0, 64, 32, 32), TimeSpan.FromSeconds(.1));
            WalkLeft.AddFrame(new Rectangle(32, 64, 32, 32), TimeSpan.FromSeconds(.1));
            WalkLeft.AddFrame(new Rectangle(64, 64, 32, 32), TimeSpan.FromSeconds(.1));
            WalkLeft.AddFrame(new Rectangle(32, 64, 32, 32), TimeSpan.FromSeconds(.1));

            WalkUp = new Animation();
            WalkUp.AddFrame(new Rectangle(0, 96, 32, 32), TimeSpan.FromSeconds(.1));
            WalkUp.AddFrame(new Rectangle(32, 96, 32, 32), TimeSpan.FromSeconds(.1));
            WalkUp.AddFrame(new Rectangle(64, 96, 32, 32), TimeSpan.FromSeconds(.1));
            WalkUp.AddFrame(new Rectangle(32, 96, 32, 32), TimeSpan.FromSeconds(.1));

            IdleDown = new Animation();
            IdleDown.AddFrame(new Rectangle(32, 0, 32, 32), TimeSpan.FromSeconds(.01));

            IdleRight = new Animation();
            IdleRight.AddFrame(new Rectangle(32, 32, 32, 32), TimeSpan.FromSeconds(.01));

            IdleLeft = new Animation();
            IdleLeft.AddFrame(new Rectangle(32, 64, 32, 32), TimeSpan.FromSeconds(.01));

            IdleUp = new Animation();
            IdleUp.AddFrame(new Rectangle(32, 96, 32, 32), TimeSpan.FromSeconds(.01));

        }
        public Animation GetWalkingAnimationFrom(Vector2 velocity)
        {
            if (velocity.Y < 0)
                return WalkUp;
            else if (velocity.Y > 0)
                return WalkDown;
            else if (velocity.X < 0)
                return WalkLeft;
            else
                return WalkRight;
        }
        public Animation GetIdleAnimationFrom(Vector2 velocity)
        {
            if (velocity.Y < 0)
                return IdleUp;
            else if (velocity.Y > 0)
                return IdleDown;
            else if (velocity.X < 0)
                return IdleLeft;
            else
                return IdleRight;
        }
        public Animation GetIdleAnimationFrom(Animation currentAnimation)
        {
            if (currentAnimation == WalkUp)
                currentAnimation = IdleUp;
            else if (currentAnimation == WalkLeft)
                currentAnimation = IdleLeft;
            else if (currentAnimation == WalkRight)
                currentAnimation = IdleRight;
            else if (currentAnimation == WalkDown)
                currentAnimation = IdleDown;
            return currentAnimation;
        }
    }
}