using UnityEngine;
using System.Collections;
using Kinect;

public class Level1Entry : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Screen.showCursor = false;
		SceneManager.instance.NextSceneName = "Level1";
	}

}
