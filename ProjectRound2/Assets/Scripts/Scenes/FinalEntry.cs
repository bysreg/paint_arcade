using UnityEngine;
using System.Collections;
using Kinect;

public class FinalEntry : SceneEntry {
	public void Awake () {
		SceneManager.instance.NextSceneName = "CreditScene";
		//SoundManager.instance.PlayBGMusic (4);
		Invoke ("LoadNext", 10f);

	}
	
	public override void ProcessDoneButton() {
	}
	
	void LoadNext() {
		SceneManager.instance.asyncLoadNextSceneWithDelay (1f);
	}
}
