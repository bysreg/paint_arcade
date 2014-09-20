using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {
	private Vector2 originalSize1;
	// Use this for initialization
	void Start () {
		Debug.Log (transform.collider.bounds.size.x);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
