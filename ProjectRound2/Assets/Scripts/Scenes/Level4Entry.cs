using UnityEngine;
using System.Collections;
using Kinect;

public class Level4Entry : SceneEntry {

	public bool showCursor;

	void Start () {
		Screen.showCursor = showCursor; //FIXME :
		SceneManager.instance.NextSceneName = "Level2";
		SoundManager.instance.PlayBGMusic (2);

		AddToolButtonManager ();
	}

	public override void ProcessDoneButton() {
		ColorCapture2D[] colorCapture2Ds = GameObject.Find ("GameController").GetComponents<ColorCapture2D>();
		for (int i=0; i<colorCapture2Ds.Length; i++) {
			colorCapture2Ds[i].Impose();
			colorCapture2Ds[i].TargetObj.SetActive(true);
		}


		Invoke ("LoadNext", 5f);

	}

	void LoadNext() {
		SceneManager.instance.asyncLoadNextSceneWithDelay (1f);
	}
}
