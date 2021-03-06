﻿using UnityEngine;
using System.Collections;
using Kinect.Button;
using PaintArcade.Generic;

public enum ETool {Brush = 0, Eraser, None};

public class PlayerHand : MonoBehaviour{

	public Vector2 uvpos = new Vector2 ();
	public Vector2 pos = new Vector2();
	public Vector2 prevPos = new Vector2();
	public bool isHandDown;
	public bool prevIsHandDown;
	public ETool tool = ETool.Brush;
	public Color color = Color.black;
	public bool FixedSize = false;

	public Texture BrushOperateTexture;
	public Texture BrushHoldTexture;
	public Texture EraserOperateTexture;
	public Texture EraserHoldTexture;
	
	private MeshRenderer renderer;
	private CircularProgressBar progressBar;

	private Vector3 originalLocalScale;

	private void Awake() {
		originalLocalScale = transform.localScale;
		renderer = GetComponent<MeshRenderer> ();
		Transform pbObj = transform.FindChild ("ProgressBar");
		if(pbObj != null) {
			progressBar = transform.FindChild("ProgressBar").GetComponent<CircularProgressBar>();
		}
	}

	public void UpdateOutLook() {
		if (tool == ETool.Brush) {
			if(isHandDown) {
				renderer.material.mainTexture = BrushOperateTexture;
			} else {
				renderer.material.mainTexture = BrushHoldTexture;
			}
			if(!FixedSize)
				transform.localScale = originalLocalScale * Consts.BrushCurrentExtraScale;
		} else if (tool == ETool.Eraser) {
			if(isHandDown) {
				renderer.material.mainTexture = EraserOperateTexture;
			} else {
				renderer.material.mainTexture = EraserHoldTexture;
			}
			if(!FixedSize)
				transform.localScale = originalLocalScale * Consts.EraserCurrentExtraScale;
		}

	}

	public void UpdatePosition(Vector3 pos) {
		GameObject tbmObj = GameObject.FindGameObjectWithTag ("tool_button_manager");
		GameObject mbmObj = GameObject.FindGameObjectWithTag ("menu_button_manager");

		if (tbmObj != null) {
			ToolButtonManager tbm = tbmObj.GetComponent<ToolButtonManager>();
			ToolButton button = tbm.NearstestButton(pos);
			if(button != null) {
				Vector3 fixedPos = pos *.25f + button.transform.position *.75f;
				fixedPos.z = pos.z;
				this.pos = fixedPos;
				this.transform.position = fixedPos;
			} else {
				this.pos = pos;
				this.transform.position = pos;
			}

		} 

		if (mbmObj != null) {
			MenuButtonManager mbm = mbmObj.GetComponent<MenuButtonManager>();
			MenuButton button = mbm.NearstestButton(pos);
			if(button != null) {
				Vector3 fixedPos = pos *.25f + button.transform.position *.75f;
				fixedPos.z = pos.z;
				this.pos = fixedPos;
				this.transform.position = fixedPos;
			} else {
				this.pos = pos;
				this.transform.position = pos;
			}
			
		} 

		if (tbmObj == null && mbmObj == null) {
			this.pos = pos;
			this.transform.position = pos;
		}
	}

	public void ShowProgressBar() {
		if(progressBar !=null)
			progressBar.Activate();
	}

	public void HideProgressBar() {
		if (progressBar != null) {
			progressBar.Deactivate();
		}
	}
}
