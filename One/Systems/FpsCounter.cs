using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameClusterFuck.Systems
{
    public class FpsCounter
    {
        public static int Fps,Ping;
        public Timer Timer = new Timer();
        public int Frames { get; set; }
        public static double Frametime { get; internal set; }

        public Texture2D Background;

        public FpsCounter()
        {
            Background = new Texture2D(Engine.Graphics.GraphicsDevice, 1, 1);
            Background.SetData(new []{ new Color(255,255,255,150) });
            Timer.Elapsed += Elapsed;
            Timer.Interval = 1000;
            Timer.Start();
        }
        private void Elapsed(object sender, ElapsedEventArgs e)
        {
            if(Frames==0)
                return;

            Fps = Frames;
            Frames = 0;
        }
        public void Draw()
        {
            var stringSize = Fonts.ProFont.MeasureString($"FPS:{Fps:000} (FrameTime: {Frametime:00.00}ms) PING: {Ping:000}");
            var stringPos =  new Vector2(Engine.Instance.GraphicsDevice.Viewport.Width - stringSize.X, 0);
            var bgRect = new Rectangle((int)stringPos.X-4,(int)stringPos.Y,(int)stringSize.X+4,(int)stringSize.Y);
            Engine.SpriteBatch.Draw(Background,bgRect,new Rectangle(0,0,1,1),Color.Black);
            Engine.SpriteBatch.DrawString(Fonts.ProFont, $"FPS:{Fps:000} (FrameTime: {Frametime:00.00}ms) PING: {Ping:000}",stringPos, Color.DarkViolet,0, Vector2.Zero, Vector2.One, SpriteEffects.None,0);

            Frames++;
        }        
    }
}