using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck.Settings;
using MonoGameClusterFuck.Primitives;
using MonoGameClusterFuck.Animations;
using MonoGameClusterFuck.Networking;
using MonoGameClusterFuck.Networking.Packets;
using MonoGameClusterFuck.Systems;

namespace MonoGameClusterFuck.Entities
{
    public class Player : Entity
    {
        public NetworkClient Socket;
        public Camera Camera;
        public override Vector2 Position
        {
            get => base.Position;
            set => Camera.Position = base.Position = value;
        }
        public Player(int size) : base(size){}

        public override void LoadContent()
        {
            Texture = Engine.Instance.Content.Load<Texture2D>("player_f");
            base.LoadContent();
        }

        public override void Initialize()
        {
            Camera = new Camera();
            Socket = new NetworkClient(this);
            //NetworkClient.ConnectAsync("84.112.111.13", 1337);
            //NetworkClient.Send(MsgLogin.Create("monogame", "password"));
            base.Initialize();
        }
        public override void Update(GameTime deltaTime)
        {
            var velocity = Engine.InputManager.KManager.GetVelocity(Speed);

            if (velocity == Vector2.Zero)
                CurrentAnimation = WalkAnimations.GetIdleAnimationFrom(CurrentAnimation);
            else
            {
                CurrentAnimation = WalkAnimations.GetWalkingAnimationFrom(velocity);
                Socket.NeedsUpdate = true;
            }

            Position += velocity * (float)deltaTime.ElapsedGameTime.TotalSeconds;

            SendMovementPacket();
            Camera.Update(deltaTime);
            base.Update(deltaTime);
        }

        private void SendMovementPacket()
        {
            if (Socket.LastUpdateTick + 50 < Environment.TickCount && Socket.NeedsUpdate)
            {
                //NetworkClient.Send(MsgWalk.Create(UniqueId, Position));
                Socket.LastUpdateTick = Environment.TickCount;
                Socket.NeedsUpdate = false;
            }
        }
    }
}