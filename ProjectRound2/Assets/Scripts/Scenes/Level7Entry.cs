using UnityEngine;
using System.Collections;
using Kinect;

public class Level7Entry : SceneEntry {
	public void Awake () {
		AddToolButtonManager ();
		base.Awake ();
		ActivateGameInSeconds (2f);
		SceneManager.instance.NextSceneName = "Level8";
		SoundManager.instance.PlayBGMusic (4);	
	}
	
	public override void ProcessDoneButton() {
		ColorCapture2D[] colorCapture2Ds = GameObject.Find ("GameController").GetComponents<ColorCapture2D>();
		for (int i=0; i<colorCapture2Ds.Length; i++) {
			colorCapture2Ds[i].Impose();
			colorCapture2Ds[i].TargetObj.SetActive(true);
		}
		
		SoundManager.instance.PlayAnimationSound (2);
		DeactivateGame ();
		Invoke ("LoadNext", 5f);
	}
	
	void LoadNext() {
		SoundManager.instance.PlayButtonSound (6);
		SceneManager.instance.asyncLoadNextSceneWithDelay (1f);
	}
}
