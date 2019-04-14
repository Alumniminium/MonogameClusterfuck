using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameClusterFuck.Systems
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

            var tl = Vector2.Transform(Vector2.Zero, inverseViewMatrix);
            var tr = Vector2.Transform(new Vector2(Bounds.X, 0), inverseViewMatrix);
            var bl = Vector2.Transform(new Vector2(0, Bounds.Y), inverseViewMatrix);
            var br = Vector2.Transform(new Vector2(Bounds.Width, Bounds.Height), inverseViewMatrix);

            var min = new Vector2(MathHelper.Min(tl.X, MathHelper.Min(tr.X, MathHelper.Min(bl.X, br.X))),
                MathHelper.Min(tl.Y, MathHelper.Min(tr.Y, MathHelper.Min(bl.Y, br.Y))));
            var max = new Vector2(MathHelper.Max(tl.X, MathHelper.Max(tr.X, MathHelper.Max(bl.X, br.X))),
                MathHelper.Max(tl.Y, MathHelper.Max(tr.Y, MathHelper.Max(bl.Y, br.Y))));
            VisibleArea = new Rectangle((int)min.X, (int)min.Y, (int)(max.X - min.X), (int)(max.Y - min.Y));
        }

        private void UpdateMatrix()
        {
            Transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) *
                    Matrix.CreateScale(Zoom) *
                    Matrix.CreateTranslation(new Vector3(Bounds.Width * 0.5f, Bounds.Height * 0.5f, 0));
            UpdateVisibleArea();
        }

        public void AdjustZoom(float zoomAmount)
        {
            Zoom += zoomAmount;
            if (Zoom < .10f)
            {
                Zoom = .10f;
            }
            if (Zoom > 3f)
            {
                Zoom = 3f;
            }
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