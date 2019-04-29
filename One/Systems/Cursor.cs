using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck.Primitives;

namespace MonoGameClusterFuck.Systems
{
    public class Cursor : Sprite
    {
        private Vector2 _cursorVector;
        

        public Cursor(int size) : base(size,0.01f)
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

            _cursorVector.X = (int)_cursorVector.X / 32;
            _cursorVector.Y = (int)_cursorVector.Y / 32;
            _cursorVector *= 32;


            _cursorVector.X += 16;
            _cursorVector.Y += 16;

            Position = _cursorVector;
        }
        public override void Draw()
        {
            base.Draw();
        }

    }
}