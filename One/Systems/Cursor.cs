using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck.Primitives;

namespace MonoGameClusterFuck.Systems
{
    public class Cursor : Sprite
    {
        private Vector2 _cursorVector;
        

        public Cursor(int size) : base(size,2)
        {
        }

        public override void LoadContent()
        {
            Texture = Engine.Instance.Content.Load<Texture2D>("selectionrect");
        }
        public override void Update(GameTime deltaTime)
        {
            _cursorVector.X = InputManager.MouseState.X;
            _cursorVector.Y = InputManager.MouseState.Y;
            _cursorVector = Vector2.Transform(_cursorVector, Matrix.Invert(Camera.Transform));
            var newPosition = new Vector2((int)(_cursorVector.X / 32) * 32 - (int)RotationOrigin.X,(int)(_cursorVector.Y / 32) * 32 - (int)RotationOrigin.Y);
            Position = newPosition;
        }
        public override void Draw()
        {
            base.Draw();
        }

    }
}