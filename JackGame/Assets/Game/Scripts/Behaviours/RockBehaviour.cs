using UnityEngine;
using System.Collections;
using Pragmatia;

namespace JackGame
{
	public class RockBehaviour : WeaponBehaviour {
		float rockTravelSpeed = 0.3f;
		// Use this for initialization
		void Start () {
			base.Start();
			SetTravelSpeed(new Vector2(rockTravelSpeed,0));
			SetTrajectoryType(Constants.TRAJECTORY_LINEAR);
			SetRangeType(Constants.ATTACK_SHORT);
			SetMaxCollisions(0);
			SetDuration(0);
		}
		
		// Update is called once per frame
		void Update () {
			base.Update();
			
			// Destroy the rock when cannot inflicts more damage or reaches the end of the scene
			if (GetDamage() <= 0 || gameObject.transform.position.x > Constants.SCREEN_WIDTH_METERS + Constants.SCREEN_RIGHT_MARGIN_METERS)
			{
				Destroy(gameObject);
			}
		}
		
		// When the rock collides with an enemy, it reduces its damage to the HP of the enemy
		public void ReduceDamage(int Reduce)
		{
			int newDamage = GetDamage() - Reduce;
			
			if (newDamage <= 0) 
			{
				SetDamage(0);
				Destroy(gameObject);
			}
			
			this.SetDamage(newDamage);
			
		}
	}
}