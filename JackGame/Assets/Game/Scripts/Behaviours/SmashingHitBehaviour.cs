using UnityEngine;
using System.Collections;

namespace JackGame
{
	public class SmashingHitBehaviour : WeaponBehaviour {
		float smashingHitTravelSpeed = 0;
		// Use this for initialization
		void Start () {
			base.Start();
			SetTravelSpeed(new Vector2(smashingHitTravelSpeed,0));
			SetTrajectoryType(Constants.TRAJECTORY_LINEAR);
			SetRangeType(Constants.ATTACK_SHORT);
			SetMaxCollisions(5);
			SetDuration(0.1f);
		}
		
		// Update is called once per frame
		void Update () {
			base.Update();
		}
	}
}