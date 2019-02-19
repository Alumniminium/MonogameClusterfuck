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
            var stringSize = Fonts.Generic.MeasureString($"FPS:{FPS}");
            GlobalState.Game.SpriteBatch.DrawString(Fonts.Generic, $"FPS:{FPS}", new Vector2(GlobalState.Game.GraphicsDevice.Viewport.Width - stringSize.X, 0), Color.Red);
        }
    }
}