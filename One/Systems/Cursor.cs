using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameClusterFuck.Primitives;

namespace MonoGameClusterFuck.Systems
{
    public class SelectionRectangle : Sprite
    {
        Vector2 SelectionPosition;
        Vector2 SelectionEndPosition;
        Rectangle[] Lines;
        Color BorderColor;
        int BorderThickness = 2;
        public SelectionRectangle(int size) : base(size)
        {
            SelectionPosition = Vector2.Zero;
            Lines = new Rectangle[4];
            BorderColor = Color.White;
        }

        public override void LoadContent()
        {
            Texture = new Texture2D(Engine.Instance.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            Texture.SetData(new[] { Color.White });
        }
        public override void Update(GameTime deltaTime)
        {
            var X = Position.X;
            var Y = Position.Y;
            if (Engine.InputManager.MouseState.LeftButton == ButtonState.Pressed && Engine.InputManager.LastMouseState.LeftButton != ButtonState.Pressed)
            {
                SelectionPosition.X = X;
                SelectionPosition.Y = Y;
            }
            if (Engine.InputManager.MouseState.LeftButton == ButtonState.Pressed && Engine.InputManager.LastMouseState.LeftButton == ButtonState.Pressed)
            {
                var x = Math.Min(SelectionPosition.X, X);
                var y = Math.Min(SelectionPosition.Y, Y);
                SelectionEndPosition.X = (Math.Abs(SelectionPosition.X - X) / 32) * 32;
                SelectionEndPosition.Y = (Math.Abs(SelectionPosition.Y - Y) / 32) * 32;

                Lines[0].X = (int)x;
                Lines[0].Y = (int)y;
                Lines[0].Width = (int)SelectionEndPosition.X;
                Lines[0].Height = BorderThickness;

                Lines[1].X = (int)x;
                Lines[1].Y = (int)y;
                Lines[1].Width = BorderThickness;
                Lines[1].Height = (int)SelectionEndPosition.Y;

                Lines[2].X = (int)x + (int)SelectionEndPosition.X - BorderThickness;
                Lines[2].Y = (int)y;
                Lines[2].Width = BorderThickness;
                Lines[2].Height = (int)SelectionEndPosition.Y;

                Lines[3].X = (int)x;
                Lines[3].Y = (int)y + (int)SelectionEndPosition.Y - BorderThickness;
                Lines[3].Width = (int)SelectionEndPosition.X;
                Lines[3].Height = BorderThickness;
            }
            if (Engine.InputManager.MouseState.LeftButton == ButtonState.Released)
            {
                for (int i = 0; i < Lines.Length; i++)
                    Lines[i] = Rectangle.Empty;
            }
        }
        public override void Draw()
        {
            for (int i = 0; i < Lines.Length; i++)
                Engine.SpriteBatch.Draw(Texture, Lines[i], Source, Color.White);
        }

        public void SetPosition(Vector2 pos)
        {
            Position = pos;
        }
    }
    public class Cursor : Sprite
    {
        SelectionRectangle SelectionRect;
        Vector2 CursorVector;
        public Cursor(int size) : base(size)
        {
            CursorVector = Vector2.Zero;
            SelectionRect = new SelectionRectangle(1);
        }

        public override void LoadContent()
        {
            Texture = Engine.Instance.Content.Load<Texture2D>("selectionrect");
            SelectionRect.LoadContent();
        }
        public override void Update(GameTime deltaTime)
        {
            CursorVector.X = Engine.InputManager.MouseState.X;
            CursorVector.Y = Engine.InputManager.MouseState.Y;
            CursorVector = Vector2.Transform(CursorVector, Matrix.Invert(Engine.Camera.Transform));
            var newPosition = new Vector2((int)(CursorVector.X / 32) * 32 + (int)RotationOrigin.X,(int)(CursorVector.Y / 32) * 32 + (int)RotationOrigin.Y);
            Position = newPosition;
            SelectionRect.SetPosition(Position);
            SelectionRect.Update(deltaTime);
        }
        public override void Draw()
        {
            base.Draw();
            SelectionRect.Draw();
        }
    }
}