using UnityEngine;
using System.Collections.Generic;

namespace Parabox.Road
{
	public static class RoadMath
	{
		public static Vector3 RotateAroundPoint(this Vector3 v, Vector3 origin, float theta)
		{
			// discard y val
			float cx = origin.x, cy = origin.z;	// origin
			float px = v.x, py = v.z;			// point

			float s = Mathf.Sin(theta);
			float c = Mathf.Cos(theta);

			// translate point back to origin:
			px -= cx;
			py -= cy;

			// rotate point
			float xnew = px * c + py * s;
			float ynew = -px * s + py * c;

			// translate point back:
			px = xnew + cx;
			py = ynew + cy;
			
			return new Vector3(px, v.y, py);
		}

		public static float AngleRadian(this Vector2 a, Vector2 b)
		{
			float opp = b.y - a.y;
			float adj = b.x - a.x;
			// Debug.Log(opp + " / " + adj);
			if(adj == 0)
				return Mathf.Tan(0f);
			else
				return Mathf.Atan( opp / adj );// * Mathf.Rad2Deg;
		}

		public static float AngleDegree(Vector2 a, Vector2 b)
		{
			float theta = RoadMath.AngleRadian(a, b) * Mathf.Rad2Deg;
			float dot = Vector2.Dot(Vector2.right, b-a);
			if(dot < 0) theta = 180f + theta;
			return RoadMath.Wrap(theta, 0f, 360f);
		}

		// a-b vector, c-d vector
		public static bool InterceptPoint(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, out Vector2 intersect)
		{
			float a1, b1, c1, a2, b2, c2;

			a1 = p1.y - p0.y;
			b1 = p0.x - p1.x;
			c1 = a1*p0.x + b1*p0.y;

			a2 = p3.y - p2.y;
			b2 = p2.x - p3.x;
			c2 = a2*p2.x + b2*p2.y;

			// Debug.Log(a1 + " + " + b1 + " = " + c1);
			// Debug.Log(a2 + " + " + b2 + " = " + c2);

			float det = a1*b2 - a2*b1;
			if(det == 0)
			{
	#if DEBUG
				Debug.LogWarning("Lines are parallel");
	#endif
				intersect = Vector2.zero;
				return false;
			}
			else
			{
				float x = (b2*c1 - b1*c2)/det;
				float y = (a1*c2 - a2*c1)/det;
				// Debug.Log("x " + x + "  y " + y);
				intersect = new Vector2(x, y);
				return true;
			}
		}

		public static Vector2 Perpendicular(Vector2 a, Vector2 b)
		{
			float x = a.x;
			float y = a.y;

			float x2 = b.x;
			float y2 = b.y;

			return new Vector2( -(y2-y), x2-x );
		}

		public static Vector2 Perpendicular(Vector2 dir)
		{
			return new Vector2(-dir.y, dir.x);
		}

		// ....what have I done....
		// public static void SlopeIntercept(Vector2 a, Vector2 c, out float m, out float b, out float x, out float y)
		// {
		// 	float y0 = (c.y-a.y);
		// 	float x0 = (c.x-a.x);
			
		// 	if(y0 == 0f)
		// 	{
		// 		x = float.NaN;
		// 		y = a.y;
		// 		m = float.NaN;
		// 		b = float.NaN;
		// 		return;
		// 	}

		// 	if(x0 == 0f)
		// 	{
		// 		x = a.x;
		// 		y = float.NaN;
		// 		m = float.NaN;
		// 		b = float.NaN;
		// 		return;
		// 	}
			
		// 	m = (c.y-a.y)/(c.x-a.x);
		// 	b = (m*a.x)-a.y;

		// 	x = float.NaN;
		// 	y = float.NaN;
		// }

		public static Vector2 ToXZVector2(this Vector3 v)
		{
			return new Vector2(v.x, v.z);
		}

		public static Vector3 ToVector3(this Vector2 v)
		{
			return new Vector3(v.x, 0f, v.y);
		}

		public static float Wrap(float value, float min, float max)
		{
			float range = max - min;

			if( value > max )
				return value % range;
			if( value < min )
				return max + value % range;
			return value;
		}

		public static Vector3 Average(this List<Vector3> arr)
		{
			if(arr == null || arr.Count < 1)
				return Vector3.zero;

			Vector3 n = arr[0];
			for(int i = 1; i < arr.Count; i++)
				n += arr[i];
			return n/(float)arr.Count;
		}
	}
}