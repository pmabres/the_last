using UnityEngine;
using System.Collections;
using Pragmatia;

namespace JackGame
{
	public class CameraBehaviour : MonoBehaviour {
	    public float dampTime = 0.15f;
	    private Vector3 velocity = Vector3.zero;    
	 	public Transform target;
		private GameObject HopeDisplay;
		private GameObject HomeLifeDisplay;
							
		private float touchDelta = 0.0F;
		private Vector2 prevDist = new Vector2(0,0);
		private Vector2 curDist = new Vector2(0,0);
		private float zoomFactor = 1F;
		private float curCameraZoomSpeed = 0;
		private float SceneHeight = 21;
		private float PrevOrthograficSize;
		private Vector2 CameraPrevPos;
		private Vector2 HudPrevPos;
		private float MAXSCALE = 30;
		private float MINSCALE = 12;
		public static int CameraStatus=0;
		
		void Awake()
		{
			
		}
		void Start() 
		{					
			target = GameObject.FindWithTag(Constants.TAG_JACK).transform;			
		
			PrevOrthograficSize = camera.orthographicSize;
			
			CameraPrevPos = new Vector2(camera.transform.position.x,camera.transform.position.y);
			
			// Create hope counter
			if (HopeDisplay != null) Destroy(HopeDisplay);			
			HopeDisplay = Instantiate(Resources.Load("Prefabs/HealthDisplay"), new Vector3(Constants.SCREEN_RIGHT_MARGIN_METERS / 2 + 1, Constants.SCREEN_HEIGHT_METERS / 2 - 1f, Constants.CAMERA_GUI_Z), Quaternion.identity) as GameObject;
			HopeDisplay.transform.localScale = new Vector3(0.02f,0.02f);
			HopeDisplay.transform.parent = GameObject.FindGameObjectWithTag(Constants.TAG_HUDCAMERA).transform;
			HopeDisplay.transform.localPosition = new Vector3(11,8,0);
			
			// Create home life counter
			if (HomeLifeDisplay != null) Destroy(HomeLifeDisplay);			
			HomeLifeDisplay = Instantiate(Resources.Load("Prefabs/HealthDisplay"), new Vector3(Constants.SCREEN_RIGHT_MARGIN_METERS / 2 + 1, Constants.SCREEN_HEIGHT_METERS / 2 - 2f, Constants.CAMERA_GUI_Z), Quaternion.identity) as GameObject;
			HomeLifeDisplay.transform.localScale = new Vector3(0.02f,0.02f);
			HomeLifeDisplay.transform.parent = GameObject.FindGameObjectWithTag(Constants.TAG_HUDCAMERA).transform;
			HomeLifeDisplay.transform.localPosition = new Vector3(-7,8,0);
		}
	    // Update is called once per frame
		void Update () 
		{		
			HopeDisplay.GetComponent<exSpriteFont>().text = Statics.Hope.ToString();
			HomeLifeDisplay.GetComponent<exSpriteFont>().text = Statics.HomeLife.ToString();
			
			if (Input.touchCount < 2) 
			{				
				CameraStatus = Constants.CAMERA_STATUS_IDLE;
			}
			
	       	if (target && target.transform.position.x > Constants.SCREEN_LEFT_MARGIN_METERS && target.transform.position.x < Constants.SCREEN_WIDTH_METERS - Constants.SCREEN_RIGHT_MARGIN_METERS)
	       	{			
	        	Vector3 point = gameObject.camera.WorldToViewportPoint(target.position);
	         	Vector3 delta = target.position - gameObject.camera.ViewportToWorldPoint(new Vector3(0.25f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
	         	Vector3 destination = transform.position + delta;
				destination.y = target.transform.position.y + 3;
				
	         	transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
	       	}
			
			//Correct camera and objects Position
			CorrectPosition();
			float zoomSpeed = 0;
			
			//Pinch and zoom
			if (Input.touchCount == 2 && Input.touches[0].phase == TouchPhase.Moved && Input.touches[0].phase == TouchPhase.Moved) 
			{				
				CameraStatus = Constants.CAMERA_STATUS_ZOOMING;
				
		        curDist = Input.touches[0].position - Input.touches[1].position; //current distance between finger touches
		        prevDist = ((Input.touches[0].position - Input.touches[0].deltaPosition) - (Input.touches[1].position - Input.touches[1].deltaPosition)); //difference in previous locations using delta positions
		        touchDelta = curDist.magnitude - prevDist.magnitude;
				zoomSpeed = -touchDelta * zoomFactor;
			}
			
			// If wheel up or down
			if (Input.GetAxis("Mouse ScrollWheel") != 0)
			{
				int InOut = -1;
				if (Input.GetAxis("Mouse ScrollWheel") < 0) 
				{
					InOut = 1;
				}
				zoomSpeed = InOut * zoomFactor;
			}

			if (zoomSpeed != 0 && (camera.orthographicSize + zoomSpeed < MAXSCALE) && (camera.orthographicSize + zoomSpeed > MINSCALE))
			{									
				camera.orthographicSize = Mathf.SmoothStep(camera.orthographicSize, camera.orthographicSize + zoomSpeed, 0.5F);	
				CorrectPosition();						 
			}			
			
	    }
		private void CorrectPosition()
		{	
			//Correct Current Camera Y position
			float scaleProportion = camera.orthographicSize / PrevOrthograficSize;
			float currentY = (scaleProportion * CameraPrevPos.y);
		    camera.transform.position = new Vector3(camera.transform.position.x, currentY,camera.transform.position.z);
			
			
		}
	}
}