using UnityEngine;
using System.Collections;

public class DoveMove : MonoBehaviour {

	public float speed;

	void Update()
	{
		transform.position += -transform.right * speed * Time.deltaTime;
	}

}
