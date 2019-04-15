using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck.Settings;
using MonoGameClusterFuck.Animations;
using MonoGameClusterFuck.Networking;
using MonoGameClusterFuck.Networking.Packets;
using MonoGameClusterFuck.Systems;

namespace MonoGameClusterFuck.Entities
{
    public class Player : Entity
    {
        public Stopwatch LatencyWatch = Stopwatch.StartNew();
        public Camera Camera;
        private float SprintFactor = 3;

        public NetworkClient NetworkClient;
        public uint UniqueId = 0;
        public int LastUpdateTick = Environment.TickCount;
        public bool NeedsUpdate = true;
        public Player(int size) : base(size)
        {
        }

        public DateTime LastPing { get; set; }

        public override void LoadContent()
        {
            Texture = Engine.Instance.Content.Load<Texture2D>("player_f");
            base.LoadContent();
        }

        public override void Initialize()
        {
            Camera = new Camera();
            NetworkClient = new NetworkClient(this);
            NetworkClient.ConnectAsync("84.112.111.13", 1337);
            NetworkClient.Send(MsgLogin.Create("monogame", "password"));
            base.Initialize();
        }
        public override void Update(GameTime deltaTime)
        {
            var delta = (float)deltaTime.ElapsedGameTime.TotalSeconds;
            var keyboard = Engine.InputManager.KManager;
            var velocity = Vector2.Zero;

            if (keyboard.KeyDown(PlayerControls.Up))
            {
                NeedsUpdate = true;
                velocity.Y = -Speed;
                CurrentAnimation = WalkAnimations.WalkUp;
            }
            if (keyboard.KeyDown(PlayerControls.Down))
            {
                NeedsUpdate = true;
                velocity.Y = Speed;
                CurrentAnimation = WalkAnimations.WalkDown;
            }
            if(keyboard.KeyDown(PlayerControls.Left))
            {
                NeedsUpdate = true;
                velocity.X = -Speed;
                CurrentAnimation = WalkAnimations.WalkLeft;
            }
            if (keyboard.KeyDown(PlayerControls.Right))
            {
                NeedsUpdate = true;
                velocity.X = Speed;
                CurrentAnimation = WalkAnimations.WalkRight;
            }
            if (keyboard.KeyDown(PlayerControls.Sprint))
            {
                NeedsUpdate = true;
                velocity.X *= SprintFactor;
                velocity.Y *= SprintFactor;
            }
            if (Math.Abs(velocity.X) < 1  && Math.Abs(velocity.Y) < 1)
            {
                if (CurrentAnimation == WalkAnimations.WalkUp)
                    CurrentAnimation = WalkAnimations.IdleUp;
                else if (CurrentAnimation == WalkAnimations.WalkLeft)
                    CurrentAnimation = WalkAnimations.IdleLeft;
                else if (CurrentAnimation == WalkAnimations.WalkRight)
                    CurrentAnimation = WalkAnimations.IdleRight;
                else if (CurrentAnimation == WalkAnimations.WalkDown)
                    CurrentAnimation = WalkAnimations.IdleDown;
            }

            if (Math.Abs(Math.Abs(velocity.Y) - Speed) < 1 && Math.Abs(Math.Abs(velocity.X) - Speed) < 1)
                velocity /= 1.45f;

            Position += velocity * delta;
            if (LastUpdateTick + 50 < Environment.TickCount && NeedsUpdate)
            {
                NetworkClient.Send(MsgWalk.Create(UniqueId, Position));
                LastUpdateTick = Environment.TickCount;
                NeedsUpdate = false;
            }
            Camera.Position = Position;
            Camera.Update(deltaTime);
            base.Update(deltaTime);
        }
    }
}