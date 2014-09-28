using UnityEngine;
using System.Collections;

public class CloudMove : MonoBehaviour {

	public float speed = 1.0f;

	void Update()
	{
		transform.position += -Vector3.right * speed * Time.deltaTime;
	}

}
