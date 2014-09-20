using UnityEngine;
using System.Collections;

public class PlayerHand : MonoBehaviour{

	public Vector2 pos = new Vector2();
	public Vector2 prevPos = new Vector2();
	public bool isHandDown;
	public bool prevIsHandDown;
	public ETool tool = ETool.Brush;
	public Color color = Color.blue;

	public enum ETool {Brush = 0, Eraser};

}
