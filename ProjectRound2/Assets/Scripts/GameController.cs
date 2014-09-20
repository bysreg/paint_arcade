using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public int canvasWidth;
	public int canvasHeight;
	public bool simulateWithMouse;
	public int maxHands;
	public GameObject KinectLeftHand;
	public GameObject KinectRightHand;

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
			PlayerHand handData = KinectRightHand.GetComponent<PlayerHand>();
			if(handData.isHandDown) {
				float x = KinectRightHand.transform.position.x;
				float y = KinectRightHand.transform.position.y;
				float width = canvasObject.collider.bounds.size.x;
				float height = canvasObject.collider.bounds.size.y;
				int px = (int) ((width*.5f+x)/width*canvasWidth);
				int py = (int) ((height*.5f+y)/height*canvasHeight);
				
				DrawBrush (new Vector2(px, py), drawColor, brushShape);
			}

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

//	void DrawLine(Vector2 pos0, Vector2 pos1, Color color)
//	{
//		float m = (pos1.y - pos0.y
//	}

	void plotLineWidth(int x0, int y0, int x1, int y1, float wd)
   	{ 
		int dx = Mathf.Abs(x1-x0), sx = x0 < x1 ? 1 : -1; 
		int dy = Mathf.Abs(y1-y0), sy = y0 < y1 ? 1 : -1; 
		int err = dx-dy, e2, x2, y2;                          /* error value e_xy */
		float ed = dx+dy == 0 ? 1 : Mathf.Sqrt((float)dx*dx+(float)dy*dy);

		for (wd = (wd+1)/2; ; ) {                                   /* pixel loop */
			setPixelColor(x0, y0, Mathf.Max(0,255*(Mathf.Abs(err-dx+dy)/ed-wd+1));
		    e2 = err; x2 = x0;
		    if (2*e2 >= -dx) {                                           /* x step */
				for (e2 += dy, y2 = y0; e2 < ed*wd && (y1 != y2 || dx > dy); e2 += dx)
					canvasTexture.SetPixel(x0, y2 += sy, Mathf.Max(0,255*(Mathf.Abs(e2)/ed-wd+1));
				if (x0 == x1) break;
				e2 = err; err -= dy; x0 += sx; 
			} 
			if (2*e2 <= dy) {                                            /* y step */
				for (e2 = dx-e2; e2 < ed*wd && (x1 != x2 || dx < dy); e2 += dy)
					canvasTexture.SetPixel(x2 += sx, y0, Mathf.Max(0,255*(Mathf.Abs(e2)/ed-wd+1));
				if (y0 == y1) break;
				err += dx; y0 += sy; 
			}
		}
	}
}
