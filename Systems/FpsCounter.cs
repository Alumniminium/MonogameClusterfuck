using System.Timers;
using Microsoft.Xna.Framework;

namespace MonoGameClusterFuck.Systems
{
    public class FpsCounter
    {
        public int FPS = 0;
        public Timer Timer = new Timer();

        public FpsCounter()
        {
            Timer.Elapsed += Elapsed;
            Timer.Interval = 1000;
            Timer.Start();
        }

        private void Elapsed(object sender, ElapsedEventArgs e)
        {
            FPS = GlobalState.Frames;
            GlobalState.Frames = 0;
        }

        public void Draw()
        {
            Game.Instance.SpriteBatch.DrawString(Fonts.Generic, $"FPS:{FPS}", new Vector2(Game.Instance.Width-150, 32), Color.Red);
        }
    }
}