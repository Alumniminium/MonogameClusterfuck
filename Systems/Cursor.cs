using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck.Systems;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGameClusterFuck.Systems
{
    public class Cursor
    {
        Texture2D SelectionRect;
        Point Position;
        Point Size;
        Vector2 CursorVector;
        Rectangle Destination;

        public Cursor(int size)
        {
            Size = new Point(size);
            Position = new Point(0, 0);
            CursorVector = Vector2.Zero;
            Destination = new Rectangle(Position, Size);
        }

        public void LoadContent()
        {
            SelectionRect = Game.Instance.Content.Load<Texture2D>("selectionrect");
        }
        public void Draw()
        {
            CursorVector.X = Game.Instance.InputManager.MouseState.X;
            CursorVector.Y = Game.Instance.InputManager.MouseState.Y;
            CursorVector = Vector2.Transform(CursorVector, Matrix.Invert(Game.Instance.Camera.Transform));

            Position.X = (int)(CursorVector.X / 32) * 32;
            Position.Y = (int)(CursorVector.Y / 32) * 32;

            Destination.Location = Position;

            Game.Instance.SpriteBatch.Draw(SelectionRect, Destination, Color.White);
        }
    }
}
