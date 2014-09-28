using UnityEngine;
using System.Collections;
using Kinect;

public class Level1Entry : SceneEntry {

	public void Awake () {
		AddToolButtonManager ();
		base.Awake ();
		ActivateGameInSeconds (2f);
		SceneManager.instance.NextSceneName = "Level4";
		SoundManager.instance.PlayBGMusic (1);	
	}
	
	public override void ProcessDoneButton() {
		DeactivateGame ();
		Invoke ("LoadNext", 1f);
	}
	
	void LoadNext() {
		SoundManager.instance.PlayButtonSound (6);
		SceneManager.instance.asyncLoadNextSceneWithDelay (1f);
	}

}
