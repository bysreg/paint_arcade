using UnityEngine;
using System.Collections;
using Kinect;

public class CreditEntry : SceneEntry {
	public void Awake () {
		AddToolButtonManager ();
		base.Awake ();
		ActivateGameInSeconds (2f);
		SoundManager.instance.PlayBGMusic (1);	
	}
	
	public override void ProcessDoneButton() {

	}
	
	
}
