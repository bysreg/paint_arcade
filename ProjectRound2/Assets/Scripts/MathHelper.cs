using UnityEngine;
using System.Collections;

public class MathHelper : MonoBehaviour {

	//randomize integer, no fractional, but return it as a float
	public static float Randomize(float min, float max)
	{
		return Random.value * (max - min) + min;
	}

}
