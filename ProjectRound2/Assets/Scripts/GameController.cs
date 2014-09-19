using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public int canvasWidth;
	public int canvasHeight;
	public bool simulateWithMouse;
	public int maxHands;

	Texture2D canvasTexture;
	GameObject canvasObject;
	Color drawColor;
	BrushShape brushShape;
	PlayerHand[] hands;

	void Awake()
	{
		canvasTexture = new Texture2D (canvasWidth, canvasHeight, TextureFormat.RGBA32, false);
		canvasObject = GameObject.Find ("Canvas");
		drawColor = Color.black;
		brushShape = BrushShape.Circle;
		hands = new PlayerHand[maxHands];
	}

	void Start()
	{
		canvasObject.renderer.material.mainTexture = canvasTexture;
		InitCanvasTexture ();
		InitPlayerHands ();
	}

	void Update()
	{
		if (simulateWithMouse) 
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hitInfo;
			int layerMask = (1 << LayerMask.NameToLayer ("Canvas"));
			Debug.DrawRay (ray.origin, ray.direction * 10, Color.yellow);

			Physics.Raycast (ray, out hitInfo, 100, layerMask);
			hands[0].prevPos = hands[0].pos;
			//hands[0].pos = hitInfo.textureCoord;
			hands[0].pos.x = (int) (hitInfo.textureCoord.x * canvasWidth - brushShape.width / 2.0f);
			hands[0].pos.y = (int) (hitInfo.textureCoord.y * canvasHeight - brushShape.height / 2.0f);
			//print (handCoord.x + " " + handCoord.y);

			hands [0].prevIsHandDown = hands [0].isHandDown;
			hands [0].isHandDown = Input.GetMouseButton (0);
		}
		else
		{
			// TODO : kinect part
		}

		for (int i=0; i<maxHands; i++) 
		{
			if (hands [i].isHandDown) 
			{
				DrawBrush (hands[i].pos, drawColor, brushShape);
				ConnectBrushPoint(hands[i]);
			}
		}
	}

	void DrawBrush(Vector2 pos, Color color, BrushShape brushShape)
	{
		//ConvertUVToPixel (ref uv);
		int k = 0;
		int x, y;
		y = (int) pos.y;
		for (int i = brushShape.height - 1; i >= 0; i--) 
		{
			x = (int) pos.x;

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

	void ConnectBrushPoint(PlayerHand hand)
	{
		if (!hand.prevIsHandDown || !hand.isHandDown)
			return;

		//connect current pos and prev pos
		//DrawLine (canvasTexture, (int)hand.prevPos.x, (int)hand.prevPos.y, (int)hand.pos.x, (int)hand.pos.y, drawColor);
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

	void InitPlayerHands()
	{
		for (int i = 0; i<maxHands; i++) 
		{
			hands[i] = new PlayerHand();
		}
	}
}
