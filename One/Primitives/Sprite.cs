using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace MonoGameClusterFuck.Primitives
{
    public class Sprite
    {
        public SpriteState State = SpriteState.Initializing;
        public float LayerDepth;
        public Texture2D Texture;
        public virtual Vector2 Position { get; set; }
        public Point TextureSize;
        public Point SpriteSize;
        public float Rotation;
        public Vector2 RotationOrigin;
        public Rectangle Source;

        public Sprite(int size, float layerDepth)
        {
            LayerDepth = layerDepth;
            SpriteSize = new Point(size, size);
        }

        public virtual void Initialize()
        {
            Source = new Rectangle(0, 0, SpriteSize.X, SpriteSize.Y);
            Rotation = 0;
            RotationOrigin = new Vector2(SpriteSize.X / 2f, SpriteSize.Y / 2f);
            State = SpriteState.Loading;
        }

        public virtual void LoadContent()
        {
            if (Texture != null)
                TextureSize = Texture.Bounds.Size;
            State = SpriteState.Starting;
            Start();
        }

        public virtual void Update(GameTime deltaTime)
        {

        }
        public virtual void Start()
        {
            State = SpriteState.Ready;
        }
        public virtual void Draw()
        {
            if (Texture != null || State != SpriteState.Ready)
            {
                Engine.SpriteBatch.Draw(Texture, Position, Source, Color.White, Rotation, RotationOrigin, Vector2.One, SpriteEffects.None, LayerDepth);
            }
        }
        public Sprite Clone()
        {
            var clone = new Sprite(32, LayerDepth) { Texture = Texture, Source = Source, SpriteSize = SpriteSize };
            return clone;
        }
    }
}