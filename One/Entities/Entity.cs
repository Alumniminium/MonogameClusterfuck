using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameClusterFuck.Animations;
using MonoGameClusterFuck.Primitives;

namespace MonoGameClusterFuck.Entities
{
    public class Entity : Sprite
    {
        public WalkAnimations WalkAnimations;
        public Animation CurrentAnimation;
        public float Speed = 200;
        public uint UniqueId { get; set; }


        public Entity(int size) : base(size)
        {
        }

        public override void LoadContent()
        {
            Texture = Engine.Instance.Content.Load<Texture2D>("player_f");
            base.LoadContent();
        }

        public override void Initialize()
        {
            WalkAnimations = new WalkAnimations();
            CurrentAnimation = WalkAnimations.IdleDown;
            base.Initialize();
        }
        public override void Update(GameTime deltaTime)
        {
            CurrentAnimation.Update(deltaTime);
            Source = CurrentAnimation.CurrentRectangle;
        }
        public override void Draw(Layers.LayerType type)
        {
            base.Draw(type);
        }

        public void Move(Vector2 location)
        {
            Position = location;
        }
    }
}
