using Microsoft.Xna.Framework;

namespace MonoGameClusterFuck.Entities
{
    public class NPC : Entity
    {
        public Vector2 Destination;

        public NPC(int size) : base(size)
        {

        }

        public override void Update(GameTime deltaTime)
        {
            if(Position != Destination)
            {
                var delta = (float)deltaTime.ElapsedGameTime.TotalSeconds;
                var distance = Vector2.Distance(Position, Destination);
                var direction = Vector2.Normalize(Destination - Position);
                var velocity = direction * Speed * delta;

                Position += velocity;

                if (Vector2.Distance(Position, Destination) >= distance)
                {
                    CurrentAnimation = WalkAnimations.GetIdleAnimationFrom(CurrentAnimation);
                    Position = Destination;
                }
                else
                    CurrentAnimation = WalkAnimations.GetWalkingAnimationFrom(direction);
            }

            base.Update(deltaTime);
        }
        public void MoveTo(Vector2 location, bool teleport = false)
        {
            if (teleport)
                Position = Destination = location;
            else
                Destination = location;
        }
    }
}
