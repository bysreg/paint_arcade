using UnityEngine;
using System;
using System.Collections;
using Kinect;

public class GameController : MonoBehaviour {
	
	public int canvasWidth;
	public int canvasHeight;
	public bool simulateWithMouse;
	public int maxHands;
	public PlayerHand KinectLeftHand;
	public PlayerHand KinectRightHand;
	public Texture2D canvasBg;
	public Texture2D canvasDrawableArea;

	Texture2D canvasTexture;
	GameObject canvasObject;
	GameObject canvasBgObject;
	BrushShape brushShape;
	PlayerHand[] hands;

	void Awake()
	{
		canvasObject = GameObject.Find ("Canvas");
		canvasBgObject = GameObject.Find ("CanvasBg");
		hands = new PlayerHand[maxHands];
	}

	void Start()
	{
		InitCanvasTexture ();
		InitPlayerHands ();

		canvasObject.renderer.material.mainTexture = canvasTexture;
		brushShape = CustomBrushShapes.brushShapes [0];
		brushShape = BrushShape.CreateSquare (30, 30);
	}

	void OnGUI()
	{
		if(GUI.Button(new Rect(20, 40, 100, 35), "Done"))  {
			ColorCapture2D[] colorCapture2Ds = GetComponents<ColorCapture2D>();
			for(int i=0; i<colorCapture2Ds.Length;i++)
			{
				colorCapture2Ds[i].Impose();
				//activate that object
				colorCapture2Ds[i].TargetObj.SetActive(true);
			}
		}
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

			hands[0].prevIsHandDown = hands [0].isHandDown;
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
			//Right Hand
			{
				hands[0].prevIsHandDown = KinectRightHand.isHandDown;
				hands[0].prevPos = hands[0].pos;
				hands[0].isHandDown = KinectRightHand.isHandDown;
				hands[0].color = KinectRightHand.color;
				hands[0].tool = KinectRightHand.tool;
				
				float x = KinectRightHand.transform.position.x;
				float y = KinectRightHand.transform.position.y;
				float width = canvasObject.collider.bounds.size.x;
				float height = canvasObject.collider.bounds.size.y;
				int px = (int)((width*.5f+x)/width*canvasWidth+canvasObject.transform.position.x);
				int py = (int)((height*.5f+y)/height*canvasHeight+canvasObject.transform.position.y);
				
				hands[0].pos = new Vector2(px-brushShape.width*.5f, py+brushShape.height*2f);
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
				float u = x * 1.0f / (canvasWidth - 1);
				float v = y * 1.0f / (canvasHeight - 1);
				Color oriColor = canvasBg.GetPixelBilinear(u, v);
				oriColor.a = 0f; // the part of the image will become transparent if erase. if this is equal to one, it will result in drawing the oiginal canvasBg

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

		//intentionally not using canvasTexture.Apply, it will be done in ConnectBrushPoint
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
		if (Input.GetKeyDown (KeyCode.Z)) 
		{
			hands[0].tool = PlayerHand.ETool.Eraser;
		}
		if (Input.GetKeyDown (KeyCode.X)) 
		{
			hands[0].tool = PlayerHand.ETool.Brush;
		}
		if(Input.GetKeyDown (KeyCode.N))
		{
			hands[0].color = Color.blue;
		}
		if(Input.GetKeyDown (KeyCode.M))
		{
			hands[0].color = Color.red;
		}
		if (Input.GetKeyDown (KeyCode.Q)) 
		{
			hands[0].color = Color.black;
		}
		if (Input.GetKeyDown (KeyCode.W)) 
		{
			hands[0].color = Color.white;
		}
		if (Input.GetKeyDown (KeyCode.E)) 
		{
			hands[0].color = Color.red;
		}
		if (Input.GetKeyDown (KeyCode.R)) 
		{
			hands[0].color = Color.magenta;
		}
		if (Input.GetKeyDown (KeyCode.T)) 
		{
			hands[0].color = Color.yellow;
		}
		if (Input.GetKeyDown (KeyCode.Y)) 
		{
			hands[0].color = Color.green;
		}
		if (Input.GetKeyDown (KeyCode.U)) 
		{
			hands[0].color = Color.blue;
		}
		if (Input.GetKeyDown (KeyCode.I)) 
		{
			hands[0].color = Color.cyan;
		}
		if (Input.GetKeyDown (KeyCode.B)) 
		{
			SceneManager.instance.asyncLoadNextSceneWithDelay (0f);
		}
	}

	void InitCanvasTexture()
	{
		canvasTexture = new Texture2D (canvasWidth, canvasHeight, TextureFormat.ARGB32, false);
		for (int i=0; i<canvasHeight; i++) 
		{
			for(int j=0;j<canvasWidth;j++)
			{
//				if(canvasBg != null)
//				{
//					float u =  j * 1.0f / (canvasWidth - 1);
//					float v = i * 1.0f / (canvasHeight - 1);
//					Color color = canvasBg.GetPixelBilinear(u, v);
//
//					canvasTexture.SetPixel(j, i, color);
//				}
//				else
//				{
					Color color = Color.white;
					color.a = 0f;

					canvasTexture.SetPixel(j, i, color);
//				}
			}
		}
		canvasTexture.Apply ();

		if(canvasBgObject != null)
		{
			canvasBgObject.renderer.material.mainTexture = canvasBg;
		}
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

	public Texture2D GetCanvasBg()
	{
		return canvasBg;
	}

	public PlayerHand[] GetPlayerHands()
	{
		return hands;
	}
}
