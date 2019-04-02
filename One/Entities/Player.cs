using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck.Primitives;
using MonoGameClusterFuck.Settings;
using MonoGameClusterFuck.Systems;
using MonoGameClusterFuck.Animations;
using System;
using System.Linq;

namespace MonoGameClusterFuck.Entities
{
    public class Player : Sprite
    {
        WalkAnimations WalkAnimations;
        Animation currentAnimation;
        float Speed = 400f;
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
            WalkAnimations = new WalkAnimations();
            currentAnimation = WalkAnimations.idleDown;

            RotationOrigin = new Vector2(Size.X, Size.Y);
            Position = new Vector2(0, 0);
        }
        public override void Update(GameTime deltaTime)
        {
            var delta = (float)deltaTime.ElapsedGameTime.TotalSeconds;
            var keyboard = Engine.InputManager.KManager;
            var currentPosition = Position;

            if (keyboard.KeyDown(PlayerControls.Up))
            {
                Position -= new Vector2(0, Speed * delta);
                currentAnimation = WalkAnimations.walkUp;
            }
            else if (keyboard.KeyDown(PlayerControls.Left))
            {
                Position -= new Vector2(Speed * delta, 0);
                currentAnimation = WalkAnimations.walkLeft;
            }
            else if (keyboard.KeyDown(PlayerControls.Right))
            {
                Position += new Vector2(Speed * delta, 0);
                currentAnimation = WalkAnimations.walkRight;
            }
            else if (keyboard.KeyDown(PlayerControls.Down))
            {
                Position += new Vector2(0, Speed * delta);
                currentAnimation = WalkAnimations.walkDown;
            }
            else
            {
                if (currentAnimation == WalkAnimations.walkUp)
                    currentAnimation = WalkAnimations.idleUp;
                else if (currentAnimation == WalkAnimations.walkLeft)
                    currentAnimation = WalkAnimations.idleLeft;
                else if (currentAnimation == WalkAnimations.walkRight)
                    currentAnimation = WalkAnimations.idleRight;
                else if (currentAnimation == WalkAnimations.walkDown)
                    currentAnimation = WalkAnimations.idleDown;
            }

            currentAnimation.Update(deltaTime);
            Source = currentAnimation.CurrentRectangle;
            var cameraPos = Position - new Vector2(Size.X / 2, Size.Y / 2);
            Engine.Camera.MoveCameraAbs(cameraPos);
        }
        public override void Draw(Layers.LayerType type)
        {
            base.Draw(type);
        }
    }
}