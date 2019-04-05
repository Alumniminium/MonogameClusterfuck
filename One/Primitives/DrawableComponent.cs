using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace MonoGameClusterFuck.Primitives
{
    public abstract class DrawableComponent
    {
        public Texture2D Texture;
        public Vector2 Position { get; set; }
        public Point TextureSize;
        public Point SpriteSize;
        public float Rotation;
        public Vector2 RotationOrigin;
        public Rectangle Source;

        protected DrawableComponent(int size)
        {
            SpriteSize = new Point(size, size);
        }
        
        public virtual void Initialize()
        {
            Source = new Rectangle(0, 0, SpriteSize.X, SpriteSize.Y);
            Position = new Vector2(0, 0);
            Rotation = 0;
            RotationOrigin = new Vector2(SpriteSize.X / 2f, SpriteSize.Y / 2f);
        }

        public virtual void LoadContent()
        {
            if(Texture!=null)
            TextureSize = Texture.Bounds.Size;
        }

        public abstract void Update(GameTime deltaTime);

        public virtual void Draw(Layers.LayerType layer)
        {
            if(Texture!=null)
            Engine.SpriteBatch.Draw(Texture, Position, Source, Color.White, Rotation, RotationOrigin, Vector2.One, SpriteEffects.None, (float)layer/ 100f);
        }
    }
}