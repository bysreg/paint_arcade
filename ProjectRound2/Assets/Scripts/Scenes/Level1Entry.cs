using UnityEngine;
using System.Collections;
using Kinect;

public class Level1Entry : SceneEntry {

	// Use this for initialization
	void Start () {
		Screen.showCursor = false; // FIXME : 
		SceneManager.instance.NextSceneName = "Level2";
		SoundManager.instance.PlayBGMusic (0);
	}
	
	public override void ProcessDoneButton() {
		SceneManager.instance.asyncLoadNextSceneWithDelay (2f);
	}

}
