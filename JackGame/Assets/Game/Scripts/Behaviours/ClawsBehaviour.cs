using UnityEngine;
using System.Collections;

namespace JackGame
{
	public class ClawsBehaviour : WeaponBehaviour {
		float clawTravelSpeed = -1;
		// Use this for initialization
		void Start () {
			base.Start();
			SetTravelSpeed(new Vector2(clawTravelSpeed,0));
			SetTrajectoryType(Constants.TRAJECTORY_LINEAR);
			SetRangeType(Constants.ATTACK_SHORT);
			SetMaxCollisions(2);
			SetDuration(0.1f);
		}
		
		// Update is called once per frame
		void Update () {
			base.Update();
		}
	}
}