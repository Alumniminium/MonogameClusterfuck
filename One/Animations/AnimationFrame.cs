using System;
using Microsoft.Xna.Framework;

namespace MonoGameClusterFuck.Animations
{
    public class AnimationFrame
    {
        public Rectangle SourceRectangle { get; set; }
        public TimeSpan Duration { get; set; }
    }
}