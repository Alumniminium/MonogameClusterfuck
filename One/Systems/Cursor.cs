using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
            Texture = Engine.Instance.Content.Load<Texture2D>("selectionrect");
        }
        public override void Update(GameTime deltaTime)
        {
            CursorVector.X = Engine.InputManager.MouseState.X;
            CursorVector.Y = Engine.InputManager.MouseState.Y;
            CursorVector = Vector2.Transform(CursorVector, Matrix.Invert(Engine.Camera.Transform));
            var newPosition = new Vector2((int)(CursorVector.X / 32) * 32 + (int)RotationOrigin.X,(int)(CursorVector.Y / 32) * 32 + (int)RotationOrigin.Y);
            Position = newPosition;
        }
        public override void Draw(Layers.LayerType type)
        {
            base.Draw(type);
        }
    }
}