using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
            set
            {
                if(value == Socket.ServerPosition)
                    return;

                Camera.Position = base.Position = value;
                
                if (Socket.LastUpdateTick + 50 < Environment.TickCount && value != Socket.ServerPosition)
                {
                    Socket.Send(MsgWalk.Create(UniqueId, Position));
                    Socket.LastUpdateTick = Environment.TickCount;
                    Socket.ServerPosition = Position;
                }
            }
        }
        public Player(int size) : base(size) { }

        public override void LoadContent()
        {
            Texture = Engine.Instance.Content.Load<Texture2D>("player_f");
            base.LoadContent();
        }

        public override void Initialize()
        {
            Camera = new Camera();
            Camera.Position = Position;
            Socket = new NetworkClient(this);
            Socket.ConnectAsync("84.112.111.13", 13337);
            base.Initialize();
        }
        public override void Update(GameTime deltaTime)
        {
            var delta = (float)deltaTime.ElapsedGameTime.TotalSeconds;
            var velocity = InputManager.Keyboard.GetInputAxis() * Speed;

            if (velocity == Vector2.Zero)
                CurrentAnimation = WalkAnimations.GetIdleAnimationFrom(CurrentAnimation);
            else
                CurrentAnimation = WalkAnimations.GetWalkingAnimationFrom(velocity);

            Position += velocity * delta;
            Destination=Position;
            Camera.Update(deltaTime);
            base.Update(deltaTime);
        }
    }
}