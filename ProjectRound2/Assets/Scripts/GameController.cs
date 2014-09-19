using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public int canvasWidth;
	public int canvasHeight;
	public bool simulateWithMouse;

	Texture2D canvasTexture;
	GameObject canvasObject;
	Color drawColor;
	BrushShape brushShape;

	void Awake()
	{
		canvasTexture = new Texture2D (canvasWidth, canvasHeight, TextureFormat.RGBA32, false);
		canvasObject = GameObject.Find ("Canvas");
		drawColor = Color.black;
		brushShape = BrushShape.Circle;
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
				DrawBrush(handCoord, drawColor, brushShape);
		}
	}

	void DrawBrush(Vector2 uv, Color color, BrushShape brushShape)
	{
		//ConvertUVToPixel (ref uv);
		int k = 0;
		int x, y;
		y = (int) (uv.y * canvasHeight - brushShape.height / 2.0f);
		for (int i = brushShape.height - 1; i >= 0; i--) 
		{
			x = (int) (uv.x * canvasWidth - brushShape.width / 2.0f);

			for(int j = 0;j < brushShape.width; j++)
			{
				if (x < 0 || x >= canvasWidth)
					continue;
				if (y < 0 || y >= canvasHeight)
					continue;

				//print (x + " " + y + " " + i + " " + j);
				if(brushShape.matrix[i*brushShape.width + j] == 1)
					canvasTexture.SetPixel (x, y, color);
				x++;
			}
			y++;
		}

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
	
}
