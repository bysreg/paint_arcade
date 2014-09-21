using UnityEngine;
using System;
using System.Collections;

public class GameController : MonoBehaviour {
	
	public int canvasWidth;
	public int canvasHeight;
	public bool simulateWithMouse;
	public int maxHands;
	public GameObject KinectLeftHand;
	public GameObject KinectRightHand;
	public Texture2D canvasBg;

	Texture2D canvasTexture;
	GameObject canvasObject;
	BrushShape brushShape;
	PlayerHand[] hands;

	void Awake()
	{
		canvasObject = GameObject.Find ("Canvas");
		brushShape = BrushShape.CreateSquare (5, 5);
		hands = new PlayerHand[maxHands];
	}

	void Start()
	{
		InitCanvasTexture ();
		InitPlayerHands ();

		canvasObject.renderer.material.mainTexture = canvasTexture;
		brushShape = CustomBrushShapes.brushShapes [0];
	}

	void Update()
	{
		HandleKeyPress ();

		if (simulateWithMouse) 
		{
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hitInfo;
			int layerMask = (1 << LayerMask.NameToLayer ("Canvas"));
			Debug.DrawRay (ray.origin, ray.direction * 100, Color.yellow);

			bool inCanvas = Physics.Raycast (ray, out hitInfo, 100, layerMask);

			//if it's not in canvas try to hit the background
			if(!inCanvas)
			{
				//int layerMask = (1 << LayerMask.NameToLayer("Background")); TODO : test
			}

			hands[0].prevIsHandDown = hands [0].isHandDown;
			hands[0].prevPos = hands[0].pos;
			hands[0].pos.x = (int) (hitInfo.textureCoord.x * canvasWidth);
			hands[0].pos.y = (int) (hitInfo.textureCoord.y * canvasHeight);
			//print (hitInfo.textureCoord.x + " " + hitInfo.textureCoord.y);

			//simulate the right hand object with the mouse
			PlayerHand handData = KinectRightHand.GetComponent<PlayerHand>();
			handData.isHandDown = hands[0].isHandDown;
			handData.prevIsHandDown = hands[0].prevIsHandDown;
			handData.color = hands[0].color;
			handData.tool = hands[0].tool;
			handData.pos = hands[0].pos;
			handData.prevPos = hands[0].prevPos;

			float px = hands[0].pos.x;
			float py = hands[0].pos.y;
			float width = canvasObject.collider.bounds.size.x;
			float height = canvasObject.collider.bounds.size.y;
			
			KinectRightHand.transform.position = new Vector3(hitInfo.point.x, hitInfo.point.y, -0.1f);

			if (inCanvas)
				hands [0].isHandDown = Input.GetMouseButton (0);
			else
				hands [0].isHandDown = false;
		}
		else
		{
			//Right Hand
			{
				PlayerHand handData = KinectRightHand.GetComponent<PlayerHand>();
				hands[0].prevIsHandDown = handData.isHandDown;
				hands[0].prevPos = hands[0].pos;
				hands[0].isHandDown = handData.isHandDown;
				hands[0].color = handData.color;
				hands[0].tool = handData.tool;

				float x = KinectRightHand.transform.position.x;
				float y = KinectRightHand.transform.position.y;
				float width = canvasObject.collider.bounds.size.x;
				float height = canvasObject.collider.bounds.size.y;
				int px = (int)((width*.5f+x)/width*canvasWidth);
				int py = (int)((height*.5f+y)/height*canvasHeight);

				hands[0].pos = new Vector2(px, py);
			}

			//Left Hand
			{
				PlayerHand handData = KinectLeftHand.GetComponent<PlayerHand>();
				hands[1].prevIsHandDown = handData.isHandDown;
				hands[1].prevPos = hands[1].pos;
				hands[1].isHandDown = handData.isHandDown;
				hands[1].color = handData.color;
				hands[1].tool = handData.tool;

				float x = KinectLeftHand.transform.position.x;
				float y = KinectLeftHand.transform.position.y;
				float width = canvasObject.collider.bounds.size.x;
				float height = canvasObject.collider.bounds.size.y;
				int px = (int)((width*.5f+x)/width*canvasWidth);
				int py = (int)((height*.5f+y)/height*canvasHeight);
				
				hands[1].pos = new Vector2(px, py);
			}		
		}

		for (int i=0; i<maxHands; i++) 
		{
			if (hands [i].isHandDown) 
			{
				if(hands[i].tool == PlayerHand.ETool.Brush)
				{
					ConnectBrushPoint(hands[i], DrawBrush);
				}
				else if(hands[i].tool == PlayerHand.ETool.Eraser)
				{
					ConnectBrushPoint(hands[i], Erase);
				}
			}
		}
	}

	void Erase(Vector2 pos, Color color, BrushShape brushShape)
	{
		int k = 0;
		int x, y;
		y = (int) (pos.y - brushShape.height / 2.0f);
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
				float u = x * 1.0f / (canvasWidth - 1);
				float v = y * 1.0f / (canvasHeight - 1);
				Color oriColor = canvasBg.GetPixelBilinear(u, v);

                if(brushShape.matrix[i*brushShape.width + j] == 1)
                    canvasTexture.SetPixel (x, y, oriColor);
                x++;
            }
            y++;
        }
        
        //intentionally not using canvasTexture.Apply, it will be done in ConnectBrushPoint
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

	void ConnectBrushPoint(PlayerHand hand, Action<Vector2, Color, BrushShape> drawf)
	{
		if (!hand.prevIsHandDown || !hand.isHandDown)
			return;

		if (hand.pos.x < 0 || hand.pos.x >= canvasWidth)
			return;
		if (hand.pos.y < 0 || hand.pos.y >= canvasHeight)
            return;
        
        //print (pos0.x + " " + pos0.y + " " + pos1.x + " " + pos1.y + " " + hand.pos);
			 
 		//connect current pos and prev pos
		DrawLine (hand.prevPos, hand.pos, hand.color, drawf);
	}

	// NOTE : pos0.x, pos0.y, pos1.x, pos1.y, must be integer
	void DrawLine(Vector2 pos0, Vector2 pos1, Color color, Action<Vector2, Color, BrushShape> drawf)
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
			//DrawBrush(pos0, color, brushShape);
			drawf(pos0, color, brushShape);
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

	void HandleKeyPress()
	{
		if (Input.GetKeyDown (KeyCode.E)) 
		{
			hands[0].tool = PlayerHand.ETool.Eraser;
		}
		if (Input.GetKeyDown (KeyCode.B)) 
		{
			hands[0].tool = PlayerHand.ETool.Brush;
		}
	}

	void InitCanvasTexture()
	{
		canvasTexture = new Texture2D (canvasWidth, canvasHeight, TextureFormat.RGBA32, false);
		for (int i=0; i<canvasHeight; i++) 
		{
			for(int j=0;j<canvasWidth;j++)
			{
				if(canvasBg != null)
				{
					float u =  j * 1.0f / (canvasWidth - 1);
					float v = i * 1.0f / (canvasHeight - 1);

					canvasTexture.SetPixel(j, i, canvasBg.GetPixelBilinear(u, v));
				}
				else
				{
					canvasTexture.SetPixel(j, i, Color.white);
				}
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

	public Texture2D GetCanvasTexture()
	{
		return canvasTexture;
	}

	public PlayerHand[] GetPlayerHands()
	{
		return hands;
	}
}
