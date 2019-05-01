using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck.Networking;
using MonoGameClusterFuck.Networking.Packets;
using MonoGameClusterFuck.Systems;
using MonoGameClusterFuck.UI.Controls;

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

        public TextBlock TextBlock;

        public Player(int size, float layerDepth) : base(size, layerDepth)
        {
            ThreadedConsole.WriteLine("[Player] Constructor called!");
            TextBlock = new TextBlock();
        }
        public override void Initialize()
        {
            ThreadedConsole.WriteLine("[Player] Initialization handed over to base class...");
            base.Initialize();
            ThreadedConsole.WriteLine("[Player] Initializing components...");
            TextBlock.Initialize();
            Position = new Vector2(720, 256);
            Destination = Position;
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
            Camera = new Camera();
            Camera.Position = Position;
        }

        public override void Update(GameTime deltaTime)
        {
            if (!IsLoaded || !IsInitialized)
                return;
            var delta = (float)deltaTime.ElapsedGameTime.TotalSeconds;
            var velocity = InputManager.Keyboard.GetInputAxisConstrained();

            /*if (velocity == Vector2.Zero)
                CurrentAnimation = WalkAnimations.GetIdleAnimationFrom(CurrentAnimation);
            else
                CurrentAnimation = WalkAnimations.GetWalkingAnimationFrom(velocity);*/

            if ((velocity.X != 0 || velocity.Y != 0) && Position == Destination)
                Destination = Position + (velocity * 32);

            TextBlock.Position.X = Position.X - TextBlock.Width / 2;
            TextBlock.Position.Y = Position.Y - 48;
            TextBlock.Update(deltaTime);
            Camera.Update(deltaTime);
            base.Update(deltaTime);
        }

        public override void Draw()
        {
            TextBlock.Draw();
            base.Draw();
        }
    }
}