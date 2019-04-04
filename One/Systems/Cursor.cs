using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck.Primitives;

namespace MonoGameClusterFuck.Systems
{
    public class Cursor : DrawableComponent
    {
        private Vector2 _cursorVector;
        public Cursor(int size)
        {
            _cursorVector = Vector2.Zero;
        }

        public override void LoadContent()
        {
            Texture = Engine.Instance.Content.Load<Texture2D>("selectionrect");
        }
        public override void Update(GameTime deltaTime)
        {
            _cursorVector.X = Engine.InputManager.MouseState.X;
            _cursorVector.Y = Engine.InputManager.MouseState.Y;
            _cursorVector = Vector2.Transform(_cursorVector, Matrix.Invert(Engine.Camera.Transform));
            var newPosition = new Vector2((int)(_cursorVector.X / 32) * 32 + (int)RotationOrigin.X,(int)(_cursorVector.Y / 32) * 32 + (int)RotationOrigin.Y);
            Position = newPosition;
        }
        public override void Draw(Layers.LayerType type)
        {
            base.Draw(type);
        }
    }
}