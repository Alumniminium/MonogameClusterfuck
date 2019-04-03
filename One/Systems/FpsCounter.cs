using System.Timers;
using Microsoft.Xna.Framework;

namespace MonoGameClusterFuck.Systems
{
    public class FpsCounter
    {
        public int Fps = 0;
        public Timer Timer = new Timer();

        public FpsCounter()
        {
            Timer.Elapsed += Elapsed;
            Timer.Interval = 1000;
            Timer.Start();
        }

        private void Elapsed(object sender, ElapsedEventArgs e)
        {
            Fps = Engine.Frames;
            Engine.Frames = 0;
        }

        public void Draw()
        {
            var stringSize = Fonts.Generic.MeasureString($"FPS:{Fps}");
            Engine.SpriteBatch.DrawString(Fonts.Generic, $"FPS:{Fps}", new Vector2(Engine.Instance.GraphicsDevice.Viewport.Width - stringSize.X, 0), Color.Red);
        }
    }
}