using UnityEngine;
using System.Collections;
using Pragmatia;

namespace JackGame
{
	public class BodyProjectileBehaviour : WeaponBehaviour 
	{		
		private float CollectRange;	
		// Use this for initialization
		void Awake ()
		{
			base.Awake();			
			SetRangeType(Constants.ATTACK_LONG);
			SetMaxCollisions(0);			
			SetTrajectoryType(Constants.TRAJECTORY_PARABOLIC);
			SetDuration(0);
			SetShootingAngle(25);
		}
		void Start ()
		{
			base.Start ();		
		}
		
		// Update is called once per frame
		void Update ()
		{
			base.Update();	
			// Destroy when touchs floor and spawn a new Body
			if (gameObject.transform.position.y < 0) 
			{
				GameObject newBody = MonoBehaviour.Instantiate(Resources.Load("Prefabs/"+Constants.TAG_BODY, typeof(GameObject)), gameObject.transform.position + new Vector3(0,0,-2), Quaternion.identity) as GameObject;
				Destroy(gameObject);
			}
		}
		
	}
}