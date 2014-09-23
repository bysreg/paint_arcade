using UnityEngine;
using System.Collections;
using Kinect.Button;

public class PlayerHand : MonoBehaviour{

	public Vector2 pos = new Vector2();
	public Vector2 prevPos = new Vector2();
	public bool isHandDown;
	public bool prevIsHandDown;
	public ETool tool = ETool.Brush;
	public Color color = Color.blue;

	public enum ETool {Brush = 0, Eraser, None};

	public Texture BrushOperateTexture;
	public Texture BrushHoldTexture;
	public Texture EraserOperateTexture;
	public Texture EraserHoldTexture;
	
	private MeshRenderer renderer;


	private void Start() {
		renderer = GetComponent<MeshRenderer> ();
	}

	public void UpdateOutLook() {
		if (tool == ETool.Brush) {
			if(isHandDown) {
				renderer.material.mainTexture = BrushOperateTexture;
			} else {
				renderer.material.mainTexture = BrushHoldTexture;
			}
		} else if (tool == ETool.Eraser) {
			if(isHandDown) {
				renderer.material.mainTexture = EraserOperateTexture;
			} else {
				renderer.material.mainTexture = EraserHoldTexture;
			}
		}
	}

	public void UpdatePosition(Vector3 pos) {
		ToolButtonManager tbm = GameObject.FindGameObjectWithTag ("tool_button_manager").GetComponent<ToolButtonManager>();;
		if (tbm != null) {
			float distanceFromButton = tbm.DistanceFromNearestButton(pos);
			Debug.Log(distanceFromButton);
		} else {
			this.pos = pos;

		}


	}

}
