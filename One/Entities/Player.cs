using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck.Primitives;
using MonoGameClusterFuck.Settings;
using MonoGameClusterFuck.Animations;

namespace MonoGameClusterFuck.Entities
{
    public class Player : Sprite
    {
        private WalkAnimations _walkAnimations;
        private Animation _currentAnimation;
        private float Speed = 250f;
        public Player(int size) : base(size)
        {
            
        }

        public override void LoadContent()
        {
            Texture = Engine.Instance.Content.Load<Texture2D>("player_f");
        }

        public override void Initialize()
        {
            base.Initialize();
            _walkAnimations = new WalkAnimations();
            _currentAnimation = _walkAnimations.IdleDown;

            RotationOrigin = new Vector2(Size.X, Size.Y);
            Position = new Vector2(0, 0);
        }
        public override void Update(GameTime deltaTime)
        {
            var delta = (float)deltaTime.ElapsedGameTime.TotalSeconds;
            var keyboard = Engine.InputManager.KManager;

            if (keyboard.KeyDown(PlayerControls.Up))
            {
                Position -= new Vector2(0, Speed * delta);
                _currentAnimation = _walkAnimations.WalkUp;
            }
            else if (keyboard.KeyDown(PlayerControls.Left))
            {
                Position -= new Vector2(Speed * delta, 0);
                _currentAnimation = _walkAnimations.WalkLeft;
            }
            else if (keyboard.KeyDown(PlayerControls.Right))
            {
                Position += new Vector2(Speed * delta, 0);
                _currentAnimation = _walkAnimations.WalkRight;
            }
            else if (keyboard.KeyDown(PlayerControls.Down))
            {
                Position += new Vector2(0, Speed * delta);
                _currentAnimation = _walkAnimations.WalkDown;
            }
            else
            {
                if (_currentAnimation == _walkAnimations.WalkUp)
                    _currentAnimation = _walkAnimations.IdleUp;
                else if (_currentAnimation == _walkAnimations.WalkLeft)
                    _currentAnimation = _walkAnimations.IdleLeft;
                else if (_currentAnimation == _walkAnimations.WalkRight)
                    _currentAnimation = _walkAnimations.IdleRight;
                else if (_currentAnimation == _walkAnimations.WalkDown)
                    _currentAnimation = _walkAnimations.IdleDown;
            }

            _currentAnimation.Update(deltaTime);
            Source = _currentAnimation.CurrentRectangle;
            var cameraPos = Position - new Vector2(Size.X / 2f, Size.Y / 2f);
            Engine.Camera.MoveCameraAbs(cameraPos);
        }
        public override void Draw(Layers.LayerType type)
        {
            base.Draw(type);
        }
    }
}