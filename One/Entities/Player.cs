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
                if (Socket == null || value == Socket.ServerPosition)
                    return;

                Camera.Position = base.Position = value;
                ThreadedConsole.WriteLine($"[Player][Position] X={value.X} Y={value.Y}");
                if (Socket.LastUpdateTick + 50 < Environment.TickCount && value != Socket.ServerPosition)
                {
                    Socket.Send(MsgWalk.Create(UniqueId, Position));
                    Socket.LastUpdateTick = Environment.TickCount;
                    Socket.ServerPosition = Position;
                }
            }
        }
        public Player(int size, float layerDepth) : base(size, layerDepth)
        {
            ThreadedConsole.WriteLine("[Player] Constructor called!");
        }
        public override void Initialize()
        {
            ThreadedConsole.WriteLine("[Player] Initialization handed over to base class...");
            base.Initialize();
            ThreadedConsole.WriteLine("[Player] Initializing components...");
            Socket = new NetworkClient(this);
        }

        public override void LoadContent()
        {
            ThreadedConsole.WriteLine("[Player] Loading Content...");
            Texture = Engine.Instance.Content.Load<Texture2D>("player_f");
            ThreadedConsole.WriteLine("[Player] Loading handed over to base class...");
            base.LoadContent();
            Socket.ConnectAsync("127.0.0.1", 13338);
            Socket.Send(MsgLogin.Create("Test", "123"));
            Position = new Vector2(512, 512);
            Camera = new Camera();
            Camera.Position = Position;
        }

        public override void Update(GameTime deltaTime)
        {
            if (!IsLoaded || !IsInitialized)
                return;
            var delta = (float)deltaTime.ElapsedGameTime.TotalSeconds;
            var velocity = InputManager.Keyboard.GetInputAxis() * Speed;

            if (velocity == Vector2.Zero)
                CurrentAnimation = WalkAnimations.GetIdleAnimationFrom(CurrentAnimation);
            else
                CurrentAnimation = WalkAnimations.GetWalkingAnimationFrom(velocity);

            Position += velocity * delta;
            Destination = Position;
            Camera.Update(deltaTime);
            base.Update(deltaTime);
        }
    }
}