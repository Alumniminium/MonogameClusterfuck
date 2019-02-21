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
        bool idle = true;
        WalkAnimations WalkAnimations;
        Animation currentAnimation;
        Vector2 StartPosition;
        Vector2 EndPosition;
        float Timer;
        float Speed = 40f;
        public Player(int size) : base(size)
        {
            
        }

        public override void LoadContent()
        {
            Texture = GlobalState.Game.Content.Load<Texture2D>("player_f");
        }

        public override void Initialize()
        {
            base.Initialize();
            WalkAnimations = new WalkAnimations();
            currentAnimation = WalkAnimations.idleDown;

            RotationOrigin = new Vector2(Size.X, Size.Y);
            Position= new Vector2(0,0);
        }
        public void Move(Vector2 position)
        {
            /* 
            idle = true;

            if(Position == EndPosition)
            {
                StartPosition = Position;
                if (keyboard.KeyDown(PlayerControls.Up))
                {
                    idle = false;
                    EndPosition = new Vector2(StartPosition.X, StartPosition.Y - 32);
                    Timer = 0;
                    currentAnimation = WalkAnimations.walkUp;
                }
                else if (keyboard.KeyDown(PlayerControls.Left))
                {
                    idle = false;
                    EndPosition = new Vector2(StartPosition.X - 32, StartPosition.Y);
                    Timer = 0;
                    currentAnimation = WalkAnimations.walkLeft;
                }
                else if (keyboard.KeyDown(PlayerControls.Right))
                {
                    idle = false;
                    EndPosition = new Vector2(StartPosition.X + 32, StartPosition.Y);
                    Timer = 0;
                    currentAnimation = WalkAnimations.walkRight;
                }
                else if (keyboard.KeyDown(PlayerControls.Down))
                {
                    idle = false;
                    EndPosition = new Vector2(StartPosition.X, StartPosition.Y + 32);
                    Timer = 0;
                    currentAnimation = WalkAnimations.walkDown;
                }

                if (idle)
                {
                    if (currentAnimation == WalkAnimations.walkUp)
                        currentAnimation = WalkAnimations.idleUp;
                    else if (currentAnimation == WalkAnimations.walkLeft)
                        currentAnimation = WalkAnimations.idleLeft;
                    else if (currentAnimation == WalkAnimations.walkRight)
                        currentAnimation = WalkAnimations.idleRight;
                    else if(currentAnimation == WalkAnimations.walkDown)
                        currentAnimation = WalkAnimations.idleDown;
                }
            }
            currentAnimation.Update(deltaTime);
            Source = currentAnimation.CurrentRectangle;
            if (Position != EndPosition)
            {
                Timer += delta * Speed;
                Timer = MathHelper.Min(Timer, 1);
                Position = Vector2.Lerp(StartPosition, EndPosition, Timer);
            }        */
        }
        public override void Update(GameTime deltaTime)
        {
            var delta = (float)deltaTime.ElapsedGameTime.TotalSeconds;
            var keyboard = GlobalState.Game.InputManager.KManager;
            
            if (keyboard.KeyDown(PlayerControls.Up))
                {
                    idle = false;
                    Position += new Vector2(StartPosition.X, StartPosition.Y - (Speed*delta));
                    Timer = 0;
                    currentAnimation = WalkAnimations.walkUp;
                }
                else if (keyboard.KeyDown(PlayerControls.Left))
                {
                    idle = false;
                    Position += new Vector2(StartPosition.X - (Speed*delta), StartPosition.Y);
                    Timer = 0;
                    currentAnimation = WalkAnimations.walkLeft;
                }
                else if (keyboard.KeyDown(PlayerControls.Right))
                {
                    idle = false;
                    Position += new Vector2(StartPosition.X + (Speed*delta), StartPosition.Y);
                    Timer = 0;
                    currentAnimation = WalkAnimations.walkRight;
                }
                else if (keyboard.KeyDown(PlayerControls.Down))
                {
                    idle = false;
                    Position += new Vector2(StartPosition.X, StartPosition.Y + (Speed*delta));
                    Timer = 0;
                    currentAnimation = WalkAnimations.walkDown;
                }

                if (idle)
                {
                    if (currentAnimation == WalkAnimations.walkUp)
                        currentAnimation = WalkAnimations.idleUp;
                    else if (currentAnimation == WalkAnimations.walkLeft)
                        currentAnimation = WalkAnimations.idleLeft;
                    else if (currentAnimation == WalkAnimations.walkRight)
                        currentAnimation = WalkAnimations.idleRight;
                    else if(currentAnimation == WalkAnimations.walkDown)
                        currentAnimation = WalkAnimations.idleDown;
                }
                
            currentAnimation.Update(deltaTime);
            Source = currentAnimation.CurrentRectangle;
            var cameraPos = Position - new Vector2(Size.X/2,0);
            GlobalState.Game.Camera.MoveCameraAbs(cameraPos);
        }
        public override void Draw()
        {
            base.Draw();
        }
    }
}