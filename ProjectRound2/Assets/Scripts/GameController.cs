using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public int canvasWidth;
	public int canvasHeight;
	public bool simulateWithMouse;

	Texture2D canvasTexture;
	GameObject canvasObject;
	Color drawColor;

	void Awake()
	{
		canvasTexture = new Texture2D (canvasWidth, canvasHeight, TextureFormat.RGBA32, false);
		canvasObject = GameObject.Find ("Canvas");
		drawColor = Color.black;
	}

	void Start()
	{
		canvasObject.renderer.material.mainTexture = canvasTexture;
		InitCanvasTexture ();
	}

	void Update()
	{
		Vector2 handCoord;

		if (simulateWithMouse) 
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hitInfo;
			int layerMask = (1 << LayerMask.NameToLayer ("Canvas"));;
			Debug.DrawRay (ray.origin, ray.direction * 10, Color.yellow);

			Physics.Raycast(ray, out hitInfo, 100, layerMask);
			handCoord = hitInfo.textureCoord;
			//print (handCoord.x + " " + handCoord.y);
			if(Input.GetMouseButton(0))
				DrawBrush(handCoord, drawColor);
		}
	}

	void DrawBrush(Vector2 uv, Color color)
	{
		//ConvertUVToPixel (ref uv);
		canvasTexture.SetPixel ((int)(uv.x * canvasWidth), (int)(uv.y * canvasHeight), drawColor);
		canvasTexture.Apply ();
	}

	void InitCanvasTexture()
	{
		for (int i=0; i<canvasWidth; i++) 
		{
			for(int j=0;j<canvasHeight;j++)
			{
				canvasTexture.SetPixel(i, j, Color.white);
			}
		}
		canvasTexture.Apply ();
	}

	//void ConvertUVToPixel()
}
