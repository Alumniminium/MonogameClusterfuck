using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace One.Systems
{
    public class Camera
    {
        public float Zoom { get; set; }
        public static Vector2 Position { get; set; }
        public Rectangle Bounds { get; protected set; }
        public static Rectangle VisibleArea { get; protected set; }
        public static Matrix Transform { get; protected set; }

        private float _currentMouseWheelValue, _previousMouseWheelValue, _zoom, _previousZoom;

        public Camera()
        {
            Bounds = Engine.Graphics.GraphicsDevice.Viewport.Bounds;
            Zoom = 1f;
            Position = new Vector2(Engine.Graphics.GraphicsDevice.Viewport.Width / 2f, Engine.Graphics.GraphicsDevice.Viewport.Height / 2f);
        }

        private void UpdateVisibleArea()
        {
            var inverseViewMatrix = Matrix.Invert(Transform);

            var (x, y) = Vector2.Transform(Vector2.Zero, inverseViewMatrix);
            var (f, f1) = Vector2.Transform(new Vector2(Bounds.X, 0), inverseViewMatrix);
            var (x1, y1) = Vector2.Transform(new Vector2(0, Bounds.Y), inverseViewMatrix);
            var (f2, y2) = Vector2.Transform(new Vector2(Bounds.Width, Bounds.Height), inverseViewMatrix);

            var (x2, f3) = new Vector2(MathHelper.Min(x, MathHelper.Min(f, MathHelper.Min(x1, f2))),MathHelper.Min(y, MathHelper.Min(f1, MathHelper.Min(y1, y2))));
            var (x3, y3) = new Vector2(MathHelper.Max(x, MathHelper.Max(f, MathHelper.Max(x1, f2))),MathHelper.Max(y, MathHelper.Max(f1, MathHelper.Max(y1, y2))));
            VisibleArea = new Rectangle((int)x2, (int)f3, (int)(x3 - x2), (int)(y3 - f3));
        }

        private void UpdateMatrix()
        {
            Transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                    Matrix.CreateScale(Zoom,Zoom,1) *
                    Matrix.CreateTranslation(new Vector3(Bounds.Width * 0.5f, Bounds.Height * 0.5f, 0));
            UpdateVisibleArea();
        }

        public void AdjustZoom(float zoomAmount)
        {
            Zoom += zoomAmount;
            if (Zoom < .05f)
            {
                Zoom = .05f;
            }
            if (Zoom > 2f)
            {
                Zoom = 2f;
            }

            ThreadedConsole.WriteLine("[Camera][Zoom] = "+Zoom);
        }
        public static bool IsOnScreen(Vector2 position, Vector2 size)
        {
            var tl = new Vector2(Position.X,Position.Y);
            var tr = new Vector2(Position.X+size.X,Position.Y);
            var bl = new Vector2(Position.X,Position.Y+size.Y);
            var br = new Vector2(Position.X + size.X,position.Y+size.Y);

            if(tr.X < VisibleArea.Left && tr.Y < VisibleArea.Top)
                return false;
            if(tl.X > VisibleArea.Right && tl.Y > VisibleArea.Bottom)
                return false;
            if(br.X < VisibleArea.Left && br.Y > VisibleArea.Bottom)
                return false;
            if(bl.X > VisibleArea.Right && bl.Y < VisibleArea.Top)
                return false;
                
            return true;
        }
        public static Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, Transform);
        }
        public static Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, Matrix.Invert(Transform));
        }
        public void Update(GameTime deltaTime)
        {
            Bounds = Engine.Graphics.GraphicsDevice.Viewport.Bounds;
            UpdateMatrix();

            _previousMouseWheelValue = _currentMouseWheelValue;
            _currentMouseWheelValue = Mouse.GetState().ScrollWheelValue;

            if (_currentMouseWheelValue > _previousMouseWheelValue)
            {
                AdjustZoom(.1f);
            }

            if (_currentMouseWheelValue < _previousMouseWheelValue)
            {
                AdjustZoom(-.1f);
            }

            _previousZoom = _zoom;
            _zoom = Zoom;
        }
    }
}