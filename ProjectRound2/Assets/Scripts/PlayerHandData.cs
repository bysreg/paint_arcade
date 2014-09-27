using UnityEngine;
using System.Collections;
using Kinect.Button;

public class PlayerHandData {

	public Vector2 uvpos = new Vector2 ();
	public Vector2 pos = new Vector2();
	public Vector2 prevPos = new Vector2();
	public bool isHandDown;
	public bool prevIsHandDown;
	public ETool tool = ETool.Brush;
	public Color color = Color.black;

}
