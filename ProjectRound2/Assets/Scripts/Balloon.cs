using UnityEngine;
using System.Collections;

public class Balloon : MonoBehaviour {
	
	void Update()
	{
		rigidbody.AddForce (Vector3.up);
	}

}
