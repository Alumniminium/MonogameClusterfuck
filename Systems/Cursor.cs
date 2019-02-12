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
        Vector2 SelectionPosition;
        Vector2 SelectionEndPosition;
        Rectangle[] SelectionRectangle;
        Texture2D SelectionTexture;
        Color BorderColor;
        int BorderThickness = 2;
        public Cursor(int size) : base(size)
        {
            CursorVector = Vector2.Zero;
            SelectionPosition = Vector2.Zero;
            SelectionRectangle = new Rectangle[4];
            BorderColor = Color.White;
        }

        public override void LoadContent()
        {
            Texture = Game.Instance.Content.Load<Texture2D>("selectionrect");
            SelectionTexture = new Texture2D(Game.Instance.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            SelectionTexture.SetData(new[] { Color.White });
        }
        public override void Update(GameTime deltaTime)
        {
            CursorVector.X = Game.Instance.InputManager.MouseState.X;
            CursorVector.Y = Game.Instance.InputManager.MouseState.Y;
            CursorVector = Vector2.Transform(CursorVector, Matrix.Invert(Game.Instance.Camera.Transform));
            Position.X = (int)(CursorVector.X / 32) * 32 + (int)RotationOrigin.X;
            Position.Y = (int)(CursorVector.Y / 32) * 32 + (int)RotationOrigin.Y;
            Destination.Location = Position;
            //Not sure what to do here 
            if (Game.Instance.InputManager.MouseState.LeftButton == ButtonState.Pressed && Game.Instance.InputManager.LastMouseState.LeftButton != ButtonState.Pressed)
            {
                SelectionPosition.X = Position.X;
                SelectionPosition.Y = Position.Y;
            }
            if (Game.Instance.InputManager.MouseState.LeftButton == ButtonState.Pressed && Game.Instance.InputManager.LastMouseState.LeftButton == ButtonState.Pressed)
            {
                var x = Math.Min(SelectionPosition.X, CursorVector.X);
                var y = Math.Min(SelectionPosition.Y, CursorVector.Y);
                SelectionEndPosition.X = (Math.Abs(SelectionPosition.X - CursorVector.X) / 32) * 32;
                SelectionEndPosition.Y = (Math.Abs(SelectionPosition.Y - CursorVector.Y) / 32) * 32;

                SelectionRectangle[0].X = (int)x;
                SelectionRectangle[0].Y = (int)y;
                SelectionRectangle[0].Width = (int)SelectionEndPosition.X;
                SelectionRectangle[0].Height = BorderThickness;

                SelectionRectangle[1].X = (int)x;
                SelectionRectangle[1].Y = (int)y;
                SelectionRectangle[1].Width = BorderThickness;
                SelectionRectangle[1].Height = (int)SelectionEndPosition.Y;

                SelectionRectangle[2].X = (int)x + (int)SelectionEndPosition.X - BorderThickness;
                SelectionRectangle[2].Y = (int)y;
                SelectionRectangle[2].Width = BorderThickness;
                SelectionRectangle[2].Height = (int)SelectionEndPosition.Y;

                SelectionRectangle[3].X = (int)x;
                SelectionRectangle[3].Y = (int)y + (int)SelectionEndPosition.Y - BorderThickness;
                SelectionRectangle[3].Width = (int)SelectionEndPosition.X;
                SelectionRectangle[3].Height = BorderThickness;
            }
            if (Game.Instance.InputManager.MouseState.LeftButton == ButtonState.Released)
            {
                for(int i=0;i<SelectionRectangle.Length;i++)
                    SelectionRectangle[i]= Rectangle.Empty;
            }
        }
        public override void Draw()
        {
            for (int i = 0; i < SelectionRectangle.Length; i++)
                Game.Instance.SpriteBatch.Draw(SelectionTexture, SelectionRectangle[i], BorderColor);
            base.Draw();
        }
    }
}