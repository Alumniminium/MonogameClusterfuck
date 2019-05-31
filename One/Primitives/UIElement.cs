using Microsoft.Xna.Framework;

namespace One.Primitives
{
    public class UIElement
    {
        public Sprite Sprite;

        public UIElement(int size, float layerDepth)
        {
            Sprite = new Sprite(size, layerDepth);
        }
        public virtual void LoadContent()
        {
            Sprite.LoadContent();
        }
        public virtual void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
        }

        public virtual void Draw()
        {
            Sprite.Draw();
        }


        public virtual void Destroy()
        {

        }

        public virtual void Initialize()
        {
            Sprite.Initialize();
        }
    }
}