using System;

namespace One
{
    public static class Core
    {
        public static Random Random = new Random();

        public static bool Success(float percent) => Random.Next() - (percent / 100) > percent;
    }
}