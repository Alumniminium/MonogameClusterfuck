using Microsoft.Xna.Framework;

namespace MonoGameClusterFuck.Primitives
{
    public class Tile : DrawableComponent
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
        public override bool Equals(object obj)
        {
            return Source.X == (obj as Tile).Source.X && Source.Y == (obj as Tile).Source.Y;
        }
       public override int GetHashCode()
       {
            return Source.X + Source.Y;
        }
    }
}