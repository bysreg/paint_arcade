using UnityEngine;
using System.Collections;

public class BirdFlag : MonoBehaviour {

    public Transform bird;

	private Vector3 oriDeltaPos;

	void Start()
	{
		oriDeltaPos = bird.transform.position - this.transform.position;
	}

    void Update()
    {
		this.transform.position = bird.position - oriDeltaPos; 
    }
}
