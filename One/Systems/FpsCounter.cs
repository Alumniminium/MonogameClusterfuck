using System.Timers;
using Microsoft.Xna.Framework;
using MonoGameClusterFuck.Layers;
using MonoGameClusterFuck.Primitives;

namespace MonoGameClusterFuck.Systems
{
    public class FpsCounter : Sprite
    {
        public int Fps;
        public Timer Timer = new Timer();
        public int Frames { get; set; }

        public FpsCounter(int size) : base(size)
        {

        }

        private void Elapsed(object sender, ElapsedEventArgs e)
        {
            Fps = Frames;
            Frames = 0;
        }

        public override void LoadContent()
        {

        }

        public override void Initialize()
        {
            Timer.Elapsed += Elapsed;
            Timer.Interval = 1000;
            Timer.Start();
        }

        public override void Draw(LayerType layer)
        {
            var stringSize = Fonts.Generic.MeasureString($"FPS:{Fps}");
            Engine.SpriteBatch.DrawString(Fonts.Generic, $"FPS:{Fps}", new Vector2(Engine.Instance.GraphicsDevice.Viewport.Width - stringSize.X, 0), Color.Red);
            Frames++;
        }

        public override void Update(GameTime deltaTime)
        {

        }
        
    }
}