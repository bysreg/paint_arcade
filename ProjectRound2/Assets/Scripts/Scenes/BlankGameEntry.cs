using UnityEngine;
using System.Collections;

public class BlankGameEntry : SceneEntry {
	

	public void Awake () {
		AddToolButtonManager ();
		base.Awake ();
		Screen.showCursor = false;
		ActivateGameInSeconds (7f);
		SoundManager.instance.PlayBGMusic (0);

	}

	public override void ProcessDoneButton() {
		/*
		ColorCapture2D[] colorCapture2Ds = GameObject.Find ("GameController").GetComponents<ColorCapture2D>();
		for (int i=0; i<colorCapture2Ds.Length; i++) {
			colorCapture2Ds[i].Impose();
			colorCapture2Ds[i].TargetObj.SetActive(true);
		}
		
		
		Invoke ("LoadNext", 5f);
		*/
		
	}
}
