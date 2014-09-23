using UnityEngine;
using System.Collections;
using Kinect;

public class Level2Entry : SceneEntry {

	// Use this for initialization
	void Start () {
		Screen.showCursor = false; // FIXME
		SceneManager.instance.NextSceneName = "Level3";
		SoundManager.instance.PlayBGMusic (1);

	}

	public override void ProcessDoneButton() {
		ColorCapture2D[] colorCapture2Ds = GameObject.Find ("GameController").GetComponents<ColorCapture2D>();
		for (int i=0; i<colorCapture2Ds.Length; i++) {
			colorCapture2Ds[i].Impose();
			colorCapture2Ds[i].TargetObj.SetActive(true);
		}

		Invoke ("LoadLevel3", 5f);

	}

	void LoadLevel3() {
		SceneManager.instance.asyncLoadNextSceneWithDelay (1f);
	}
}
