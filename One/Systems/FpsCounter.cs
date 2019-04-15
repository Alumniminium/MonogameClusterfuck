using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck.Layers;
using MonoGameClusterFuck.Primitives;

namespace MonoGameClusterFuck.Systems
{
    public class FpsCounter : Sprite
    {
        public static int Fps,Ping;
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
            var stringSize = Fonts.Generic.MeasureString($"FPS:{Fps:000} PING: {Ping:000}");
            Engine.SpriteBatch.DrawString(Fonts.Generic, $"FPS:{Fps:000} PING: {Ping:000}", new Vector2(Engine.Instance.GraphicsDevice.Viewport.Width - stringSize.X*4, 0), Color.Red,0, RotationOrigin, scale: new Vector2(4,4), SpriteEffects.None,0);
            Frames++;
        }

        public override void Update(GameTime deltaTime)
        {

        }
        
    }
}