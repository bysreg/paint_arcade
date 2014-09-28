using UnityEngine;
using System.Collections;

public class TreeMove : MonoBehaviour {
	
	private float speed;
	private float maxAngle = 355;
	private float minAngle = 5;
	private Vector3 rotationV;

	void Start()
	{
		speed = Random.value * 20.0f - 10.0f;
		if(speed > 0)
		{
			speed = Mathf.Max(speed, 0.4f);
		}
		else
		{
			speed = Mathf.Min(speed, -0.4f);
		}
		rotationV = new Vector3(0, 0, speed);
	}

	void Update()
	{
		transform.Rotate (rotationV * Time.deltaTime);
		//print (rotationV.z + " " + transform.rotation.eulerAngles.z); 
		if((rotationV.z > 0 && transform.rotation.eulerAngles.z < 180 && transform.rotation.eulerAngles.z > minAngle) || 
		   (rotationV.z < 0 && transform.rotation.eulerAngles.z > 180 && transform.rotation.eulerAngles.z < maxAngle))
		{
			rotationV.z *= -1;
		}
	}

}
