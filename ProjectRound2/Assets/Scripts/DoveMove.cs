using UnityEngine;
using System.Collections;

public class DoveMove : MonoBehaviour {

	public float speed;
	float time;
	float deltaY;
	float oriY;

	void Start()
	{
		oriY = transform.position.y;
	}

	void Update()
	{
		time += Time.deltaTime;
		deltaY = Mathf.Sin (time * 16f) * 0.25f;
		//print (deltaY);

		//transform.position += -transform.right * speed * Time.deltaTime;
		transform.position = new Vector3 (transform.position.x - speed * Time.deltaTime, deltaY + oriY, transform.position.z);
	}

}
