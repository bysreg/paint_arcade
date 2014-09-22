using UnityEngine;
using System.Collections;

public class FishMove : MonoBehaviour {

	public float speed;
	public int mode = 0;
	
	private float timeToRotate = 1f;
	private float timer = 0;

	void Start()
	{

	}

	void Update()
	{
//		timer += Time.deltaTime;
//		if (timer >= timeToRotate) 
//		{
//			transform.Rotate (0, 0, Random.Range (-4, 1), Space.World);
//			timer -= timeToRotate;
//			print ("rotate");
//		}

		if (mode == 0) {
			transform.Rotate (0, 0, -1, Space.World);
			transform.position += -transform.up * speed * Time.deltaTime;
		} else if (mode == 1) {
			transform.position += -transform.up * speed * Time.deltaTime;
		}
	}

}
