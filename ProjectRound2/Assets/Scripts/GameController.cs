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
		drawColor = Color.blue;
		drawColor.a = 0f;
		brushShape = BrushShape.CreateSquare (5, 5);
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
			Debug.DrawRay (ray.origin, ray.direction * 100, Color.yellow);

			bool inCanvas = Physics.Raycast (ray, out hitInfo, 100, layerMask);

			hands [0].prevIsHandDown = hands [0].isHandDown;
			hands[0].prevPos = hands[0].pos;
			hands[0].pos.x = (int) (hitInfo.textureCoord.x * canvasWidth);
			hands[0].pos.y = (int) (hitInfo.textureCoord.y * canvasHeight);
			//print (hitInfo.textureCoord.x + " " + hitInfo.textureCoord.y);

			if (inCanvas)
				hands [0].isHandDown = Input.GetMouseButton (0);
			else
				hands [0].isHandDown = false;
		}
		else
		{
			PlayerHand handData = KinectRightHand.GetComponent<PlayerHand>();
			hands[0].prevIsHandDown = handData.isHandDown;
			hands[0].prevPos = hands[0].pos;
			hands[0].isHandDown = handData.isHandDown;

			float x = KinectRightHand.transform.position.x;
			float y = KinectRightHand.transform.position.y;
			float width = canvasObject.collider.bounds.size.x;
			float height = canvasObject.collider.bounds.size.y;
			int px = (int)((width*.5f+x)/width*canvasWidth);
			int py = (int)((height*.5f+y)/height*canvasHeight);

			hands[0].pos = new Vector2(px, py);
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
		int k = 0;
		int x, y;
		y = (int) (pos.y - brushShape.height/ 2.0f);
		for (int i = brushShape.height - 1; i >= 0; i--) 
		{
			x = (int) (pos.x - brushShape.width / 2.0f);

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

		if (hand.pos.x < 0 || hand.pos.x >= canvasWidth)
			return;
		if (hand.pos.y < 0 || hand.pos.y >= canvasHeight)
            return;
        
        //print (pos0.x + " " + pos0.y + " " + pos1.x + " " + pos1.y + " " + hand.pos);
			 
 		//connect current pos and prev pos
		DrawLine (hand.prevPos, hand.pos, drawColor);
	}

	// NOTE : pos0.x, pos0.y, pos1.x, pos1.y, must be integer
	void DrawLine(Vector2 pos0, Vector2 pos1, Color color)
	{
		float sx, sy, err, e2;
		float dx = Mathf.Abs(pos1.x - pos0.x);
		float dy = Mathf.Abs(pos1.y - pos0.y); 
		if (pos0.x < pos1.x)
			sx = 1; 
		else 
			sx = -1;
		if (pos0.y < pos1.y)  
			sy = 1; 
		else 
			sy = -1;
		err = dx - dy;
		
		while(true)
		{
			DrawBrush(pos0, color, brushShape);
			if (pos0.x == pos1.x && pos0.y == pos1.y) 
				break;
			e2 = 2 * err;
			if (e2 > -dy) 
			{
				err = err - dy;
				pos0.x = pos0.x + sx;
			}
			if (e2 < dx)
			{
				err = err + dx;
				pos0.y = pos0.y + sy ;
			}
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
	
	void InitPlayerHands()
	{
		for (int i = 0; i<maxHands; i++) 
		{
			hands[i] = new PlayerHand();
		}
	}
}
