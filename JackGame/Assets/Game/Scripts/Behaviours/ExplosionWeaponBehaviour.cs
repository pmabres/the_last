using UnityEngine;
using System.Collections;

namespace JackGame
{
	public class ExplosionWeaponBehaviour : WeaponBehaviour {
		float TravelSpeed = 0;
		
		
		// Use this for initialization
		void Start () {
			base.Start();
			SetTravelSpeed(new Vector2(TravelSpeed,0));
			SetRangeType(Constants.ATTACK_SHORT);
			SetMaxCollisions(0); // Infinite collisions
			SetDuration(1.2f);
			SetTrajectoryType(Constants.TRAJECTORY_LINEAR);
			SetCollection(Constants.COLLECTION_NONE);
		}

		// Update is called once per frame
		void Update () {
			base.Update();
		}
		
		public void OnTriggerEnter(Collider c)
		{
			// Change the collection when exploding
			base.OnTriggerEnter(c);
			
		}
	}
}