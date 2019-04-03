using System;
using Microsoft.Xna.Framework;

namespace MonoGameClusterFuck.Animations
{
    public class WalkAnimations
    {
        public Animation WalkDown, IdleDown, WalkUp, IdleUp, WalkLeft, IdleLeft, WalkRight, IdleRight;

        public WalkAnimations()
        {
            WalkDown = new Animation();
            WalkDown.AddFrame(new Rectangle(0, 0, 32, 32), TimeSpan.FromSeconds(.2));
            WalkDown.AddFrame(new Rectangle(32, 0, 32, 32), TimeSpan.FromSeconds(.2));
            WalkDown.AddFrame(new Rectangle(64, 0, 32, 32), TimeSpan.FromSeconds(.2));
            WalkDown.AddFrame(new Rectangle(32, 0, 32, 32), TimeSpan.FromSeconds(.2));

            WalkRight = new Animation();
            WalkRight.AddFrame(new Rectangle(0, 32, 32, 32), TimeSpan.FromSeconds(.2));
            WalkRight.AddFrame(new Rectangle(32, 32, 32, 32), TimeSpan.FromSeconds(.2));
            WalkRight.AddFrame(new Rectangle(64, 32, 32, 32), TimeSpan.FromSeconds(.2));
            WalkRight.AddFrame(new Rectangle(32, 32, 32, 32), TimeSpan.FromSeconds(.2));

            WalkLeft = new Animation();
            WalkLeft.AddFrame(new Rectangle(0, 64, 32, 32), TimeSpan.FromSeconds(.2));
            WalkLeft.AddFrame(new Rectangle(32, 64, 32, 32), TimeSpan.FromSeconds(.2));
            WalkLeft.AddFrame(new Rectangle(64, 64, 32, 32), TimeSpan.FromSeconds(.2));
            WalkLeft.AddFrame(new Rectangle(32, 64, 32, 32), TimeSpan.FromSeconds(.2));

            WalkUp = new Animation();
            WalkUp.AddFrame(new Rectangle(0, 96, 32, 32), TimeSpan.FromSeconds(.2));
            WalkUp.AddFrame(new Rectangle(32, 96, 32, 32), TimeSpan.FromSeconds(.2));
            WalkUp.AddFrame(new Rectangle(64, 96, 32, 32), TimeSpan.FromSeconds(.2));
            WalkUp.AddFrame(new Rectangle(32, 96, 32, 32), TimeSpan.FromSeconds(.2));

            IdleDown = new Animation();
            IdleDown.AddFrame(new Rectangle(32, 0, 32, 32), TimeSpan.FromSeconds(.5));

            IdleRight = new Animation();
            IdleRight.AddFrame(new Rectangle(32, 32, 32, 32), TimeSpan.FromSeconds(.5));

            IdleLeft = new Animation();
            IdleLeft.AddFrame(new Rectangle(32, 64, 32, 32), TimeSpan.FromSeconds(.5));

            IdleUp = new Animation();
            IdleUp.AddFrame(new Rectangle(32, 96, 32, 32), TimeSpan.FromSeconds(.5));

        }
    }
}