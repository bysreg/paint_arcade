using UnityEngine;
using System.Collections;

public class CircularProgressBar : MonoBehaviour {
	private float currentProgress;

	public void Activate() {
		Deactivate();
		StartCoroutine(RadialProgress(1));
	}

	public void Deactivate() {
		StopAllCoroutines();
		currentProgress = 0;
		gameObject.renderer.material.SetFloat("_Cutoff", 1);

	}
	
	IEnumerator RadialProgress(float time) {
		float rate = 1 / time;
		while (currentProgress < 0.85) {
			currentProgress += Time.deltaTime * rate;
			gameObject.renderer.material.SetFloat("_Cutoff", 1-currentProgress);
			yield return 0;
		}
		gameObject.renderer.material.SetFloat("_Cutoff", 1);
	}
}
