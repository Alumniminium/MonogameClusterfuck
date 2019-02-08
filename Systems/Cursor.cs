using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck.Primitives;

namespace MonoGameClusterFuck.Systems
{
    public class Cursor : Sprite
    {
        Vector2 CursorVector;

        public Cursor(int size) : base(size)
        {
            CursorVector = Vector2.Zero;
        }

        public override void LoadContent()
        {
            Texture = Game.Instance.Content.Load<Texture2D>("selectionrect");
        }
        public override void Update(GameTime deltaTime)
        {
            //Not sure what to do here 
        }
        public override void Draw()
        {
            CursorVector.X = Game.Instance.InputManager.MouseState.X;
            CursorVector.Y = Game.Instance.InputManager.MouseState.Y;
            CursorVector = Vector2.Transform(CursorVector, Matrix.Invert(Game.Instance.Camera.Transform));

            Position.X = (int)(CursorVector.X / 32) * 32 + (int)RotationOrigin.X;
            Position.Y = (int)(CursorVector.Y / 32) * 32 + (int)RotationOrigin.Y;

            Destination.Location = Position;
            base.Draw();
        }
    }
}
