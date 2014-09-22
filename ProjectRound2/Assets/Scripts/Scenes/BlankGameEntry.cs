using UnityEngine;
using System.Collections;

public class BlankGameEntry : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		SoundManager.instance.PlayBGMusic (0);
	}
}
