using UnityEngine;
using System.Collections;
using Pragmatia;

namespace JackGame
{
	public class ShotgunBehaviour : WeaponBehaviour {
		float shotgunSpeed = 2;
		// Use this for initialization
		void Start () {
			base.Start();
			SetTravelSpeed(new Vector2(shotgunSpeed,0));
			SetTrajectoryType(Constants.TRAJECTORY_LINEAR);
			SetRangeType(Constants.ATTACK_MEDIUM);
			SetMaxCollisions(4);
		}
		
	}
}