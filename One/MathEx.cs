using System;
using System.Runtime.CompilerServices;

namespace One
{
    public class MathEx
    {
        [MethodImpl( MethodImplOptions.AggressiveInlining )]
		public static float InverseLerp( float start, float finish, float position )
		{
			if( start < finish )
			{
				if( position < start )
					return 0.0f;
				else if( position > finish )
					return 1.0f;
			}
			else
			{
				if( position < finish )
					return 1.0f;
				else if( position > start )
					return 0.0f;
			}

			return ( position - start ) / ( finish - start );
		}
		public static float CosineInterpolate(float a, float b, float x)
		{
				float ft = x * 3.1415927f;
				float f = (1 - (float)Math.Cos(ft))*0.5f;
				return a*(1-f)+b*f;
		}
    }
}