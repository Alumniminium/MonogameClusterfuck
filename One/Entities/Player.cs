using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck.Networking;
using MonoGameClusterFuck.Networking.Packets;
using MonoGameClusterFuck.Systems;
using One.Systems;

namespace MonoGameClusterFuck.Entities
{
    public class Player : Entity
    {
        public new float LayerDepth = 1;
        public NetworkClient Socket;
        public Camera Camera;
        public override Vector2 Position
        {
            get => base.Position;
            set
            {
                if (value == Socket.ServerPosition)
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
        public Player(int size, float layerDepth) : base(size, layerDepth)
        {
            ThreadedConsole.WriteLine("[Player] Constructor called!");
        }

        public override void LoadContent()
        {
            ThreadedConsole.WriteLine("[Player] Loading Content...");
            Texture = Engine.Instance.Content.Load<Texture2D>("player_f");
            ThreadedConsole.WriteLine("[Player] Loading handed over to base class...");
            base.LoadContent();
        }

        public override void Initialize()
        {
            ThreadedConsole.WriteLine("[Player] Initializing components...");
            Camera = new Camera();
            Camera.Position = Position;
            Socket = new NetworkClient(this);     
            ThreadedConsole.WriteLine("[Player] Initialization handed over to base class...");       
            base.Initialize();
            //if (!Debugger.IsAttached)
            //    Socket.ConnectAsync("84.112.111.13", 13338);
            //else
            ThreadedConsole.WriteLine("Connecting to Server...");
            Socket.ConnectAsync("127.0.0.1", 13338);
            ThreadedConsole.WriteLine("Logging in...");
            Socket.Send(MsgLogin.Create("Test", "123"));
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