using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck.Primitives;
using MonoGameClusterFuck.Settings;
using MonoGameClusterFuck.Animations;
using MonoGameClusterFuck.Systems;

namespace MonoGameClusterFuck.Entities
{
    public class Player : Sprite
    {
        public Camera Camera;
        private WalkAnimations _walkAnimations;
        private Animation _currentAnimation;
        private float Speed = 200;
        private float SprintFactor = 3;

        public Player(int size) : base(size)
        {
        }

        public override void LoadContent()
        {
            Texture = Engine.Instance.Content.Load<Texture2D>("player_f");
            base.LoadContent();
        }

        public override void Initialize()
        {
            _walkAnimations = new WalkAnimations();
            _currentAnimation = _walkAnimations.IdleDown;
            Camera = new Camera();
            base.Initialize();
        }
        public override void Update(GameTime deltaTime)
        {
            var delta = (float)deltaTime.ElapsedGameTime.TotalSeconds;
            var keyboard = Engine.InputManager.KManager;
            var velocity = Vector2.Zero;

            if (keyboard.KeyDown(PlayerControls.Up))
            {
                velocity.Y = -Speed;
                _currentAnimation = _walkAnimations.WalkUp;
            }
            if (keyboard.KeyDown(PlayerControls.Down))
            {
                velocity.Y = Speed;
                _currentAnimation = _walkAnimations.WalkDown;
            }
            if(keyboard.KeyDown(PlayerControls.Left))
            {
                velocity.X = -Speed;
                _currentAnimation = _walkAnimations.WalkLeft;
            }
            if (keyboard.KeyDown(PlayerControls.Right))
            {
                velocity.X = Speed;
                _currentAnimation = _walkAnimations.WalkRight;
            }
            if (keyboard.KeyDown(PlayerControls.Sprint))
            {
                velocity.X *= SprintFactor;
                velocity.Y *= SprintFactor;
            }
            if (Math.Abs(velocity.X) < 1  && Math.Abs(velocity.Y) < 1)
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

            if (Math.Abs(Math.Abs(velocity.Y) - Speed) < 1 && Math.Abs(Math.Abs(velocity.X) - Speed) < 1)
                velocity /= 1.45f;

            Position += velocity * delta;
            Camera.Position = Position;
            Camera.Update(deltaTime);
        }
        public override void Draw(Layers.LayerType type)
        {
            base.Draw(type);
        }
    }
}