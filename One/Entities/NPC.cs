using Microsoft.Xna.Framework;

namespace MonoGameClusterFuck.Entities
{
    public class NPC : Entity
    {
        Vector2 Destination;
        bool moving;
        public NPC(int size) : base(size)
        {
        }

        public override void Update(GameTime deltaTime)
        {
            float distance = Vector2.Distance(Position, Destination);
            Vector2 direction = Vector2.Normalize(Destination - Position);
            if(moving == true)
            {
                Position += direction * Speed * (float)deltaTime.ElapsedGameTime.TotalSeconds;
                if(Vector2.Distance(Position, Destination) >= distance)
                {
                    Position = Destination;
                    moving = false;
                }
            }
        }
        public void Move(Vector2 location)
        {
            Destination = location;
            moving=true;
        }
    }
}
