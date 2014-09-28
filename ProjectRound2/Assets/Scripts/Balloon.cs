using UnityEngine;
using System.Collections;

public class Balloon : MonoBehaviour {

	public float force;

	void Update()
	{
		rigidbody.AddForce (Vector3.up * force);
	}

}
