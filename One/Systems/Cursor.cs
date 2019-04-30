using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck.Primitives;
using MonoGameClusterFuck.SceneManagement.Scenes;

namespace MonoGameClusterFuck.Systems
{
    public class Cursor : Sprite
    {
        private Vector2 _cursorVector;
        public string ToolTip = "Floor";
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

            var val = InfiniteWorld.NoiseGen.GetCellular(_cursorVector.X,_cursorVector.Y);
            
            if (val > 0.25f)
                ToolTip = "Wall Tile: " + _cursorVector.X+","+_cursorVector.Y;
            else
                ToolTip = "Floor Tile: " + _cursorVector.X+","+_cursorVector.Y;

            if(_cursorVector.Y >=0)
            _cursorVector.Y += 16;
            else
            _cursorVector.Y -= 16;

            if(_cursorVector.X >=0)
            _cursorVector.X += 16;
            else
            _cursorVector.X -= 16;

            Position = _cursorVector;
        }
        public override void Draw()
        {
            Engine.SpriteBatch.DrawString(Fonts.ProFont,ToolTip,Position + new Vector2(-16,-48),Color.White);
            base.Draw();
        }

    }
}