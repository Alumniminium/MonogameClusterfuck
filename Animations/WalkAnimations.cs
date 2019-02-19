using System;
using Microsoft.Xna.Framework;
using MonoGameClusterFuck.Entities;

namespace MonoGameClusterFuck.Animations
{
    public class WalkAnimations
    {

        public Animation walkDown, idleDown, walkUp, idleUp, walkLeft, idleLeft, walkRight, idleRight;

        public WalkAnimations()
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

        }
    }
}