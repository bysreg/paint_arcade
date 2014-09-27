using UnityEngine;
using System.Collections;

public class CircularProgressBar : MonoBehaviour {
	private float currentProgress;
	void Start() {
		gameObject.renderer.material.SetFloat ("_Cutoff", 1);
	}

	public void Activate() {
		Deactivate();
		this.renderer.enabled = true;
		StartCoroutine(RadialProgress(1));
	}

	public void Deactivate() {
		this.renderer.enabled = false;
		StopAllCoroutines();
		currentProgress = 0;
		gameObject.renderer.material.SetFloat("_Cutoff", 1f);
	}

	IEnumerator RadialProgress(float time) {
		float rate = 1 / time;
		while (currentProgress < 0.99f) {
			currentProgress += Time.deltaTime * rate;
			gameObject.renderer.material.SetFloat("_Cutoff", 1-currentProgress);
			yield return 0;
		}
		Deactivate ();
	}
}
