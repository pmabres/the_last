using UnityEngine;
using System.Collections;
using Pragmatia;

namespace JackGame
{
	public class ArrowTowerBehaviour : WeaponBehaviour 
	{		
			
		// Use this for initialization
		void Awake ()
		{
			base.Awake();			
			SetRangeType(Constants.ATTACK_LONG);
			SetMaxCollisions(1);			
			SetTrajectoryType(Constants.TRAJECTORY_PARABOLIC);
			SetDuration(0);
			SetShootingAngle(25);
			//SetTravelSpeed( new Vector2(0,3.5f));	
		}
		void Start ()
		{
			base.Start ();		
		}
		
		// Update is called once per frame
		void Update ()
		{
			//if (gameObject.transform.position.y < 1 && gameObject.transform.position.y > -1) Debug.Log(gameObject.transform.position.x);
			base.Update();	
			// Destroy when touchs floor
			if (gameObject.transform.position.y < -3) Destroy(gameObject);
			//base.Update();
			//gameObject.transform.Translate(PgtCommons.Convert(shootingPower));			
			//ApplyPhysics();
			//float angle = Mathf.Atan2(gameObject.transform.position.y,gameObject.transform.position.x) * Mathf.Rad2Deg;
			//gameObject.transform.rotation = Quaternion.Euler( new Vector3(0,0,angle));
		}
		/*void ApplyPhysics()
		{
			if (shootingPower.x >= 0) shootingPower.x -= GetArrowStrength() * 0.01f;			
			shootingPower.y = Mathf.Pow(-shootingPower.x,2) - Constants.GRAVITY_FORCE;			
		}	*/	
		
	}
}