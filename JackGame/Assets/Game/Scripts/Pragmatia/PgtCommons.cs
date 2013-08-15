using UnityEngine;
using System.Collections;
using JackGame;


namespace Pragmatia
{
	public class PgtCommons
	{
		// Transform a Vector3 (x,y,z) to a Vector2(x,y) for a 2D game
		public static Vector3 Convert (Vector2 Vector) 
		{
			return new Vector3(Vector.x, Vector.y, 0);
		}
	}
}

