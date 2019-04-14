using Microsoft.Xna.Framework;

namespace MonoGameClusterFuck.Primitives
{
    public class Tile : Sprite
    {
        public Tile(int size) : base(size)
        {
        }

        public override void LoadContent()
        {
            
        }

        public override void Update(GameTime deltaTime)
        {

        }

        public override void Draw(Layers.LayerType type)
        {
            base.Draw(type);
        }

        public Tile Clone()
        {
            var clone = new Tile(32) {Texture = Texture, Source = Source, SpriteSize = SpriteSize};
            return clone;
        }
    }
}