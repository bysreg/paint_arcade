using UnityEngine;
using System.Collections;

public class Level6Entry : SceneEntry {

	void Start () {
		Screen.showCursor = false; //FIXME :
		SoundManager.instance.PlayBGMusic (2);
		
	}

	public override void ProcessDoneButton() {
		ColorCapture2D[] colorCapture2Ds = GameObject.Find ("GameController").GetComponents<ColorCapture2D>();
		for (int i=0; i<colorCapture2Ds.Length; i++) {
			colorCapture2Ds[i].Impose();
			colorCapture2Ds[i].TargetObj.SetActive(true);
		}
		
	}
}
