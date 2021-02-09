using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Parabox.Road
{
	public static class RoadUtils
	{
		public static string ToFormattedString<T>(this List<T> list, string delim)
		{
			if(list.Count < 1) 
				return "";
			
			StringBuilder sb = new StringBuilder();
			for(int i = 0; i < list.Count-1; i++)
			{
				sb.Append(list[i].ToString() + delim);
			}
			sb.Append(list[list.Count-1].ToString());
			return sb.ToString();
		}


		public static string ToFormattedString<T>(this T[] arr, string delimiter)
		{
			StringBuilder sb = new StringBuilder();
			for(int i = 0; i < arr.Length-1; i++)
				sb.Append(arr[i].ToString() + delimiter);
			sb.Append(arr[arr.Length-1]);
			return sb.ToString();
		}

		public static Vector3 ToVector3Y(this Vector2 vec, float y)
		{
			return new Vector3(vec.x, y, vec.y);
		}

		/**
		 *	\brief Attemps to move the vector to the nearest point below (then) above.  If no mesh found, no change is made.
		 */
		public static Vector3 GroundHeight(Vector3 v)
		{
			RaycastHit ground = new RaycastHit();

			if(Physics.Raycast(v, -Vector3.up, out ground, Mathf.Infinity))//, 1 << terrainLayer))
				v.y = ground.point.y;

			if(Physics.Raycast(v, Vector3.up, out ground, Mathf.Infinity))//, 1 << terrainLayer))
				v.y = ground.point.y;

			// try casting from really high up next
			if(Physics.Raycast(v + Vector3.up*1000f, -Vector3.up, out ground))
				v.y = ground.point.y;

			return v;
		}	
	}
}