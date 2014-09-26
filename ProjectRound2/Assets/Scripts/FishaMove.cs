using UnityEngine;
using System.Collections;

public class FishaMove : MonoBehaviour {

	public float speed;
	public int mode = 0;
	
	private float timeToRotate = 1f;
	private float timer = 0;

	void Update () {
		if (mode == 0) {
			transform.Rotate (0, 0, -1, Space.World);
			transform.position -= transform.right * speed * Time.deltaTime;
		} else if (mode == 1) {
			transform.position += transform.right * speed * Time.deltaTime;
		}
	}
}
