using UnityEngine;
using System.Collections;

namespace JackGame
{
	public class TrapWeaponBehaviour : WeaponBehaviour {
		float TrapTravelSpeed = 0;
		// Use this for initialization
		void Start () {
			base.Start();
			SetTravelSpeed(new Vector2(TrapTravelSpeed,0));
			SetRangeType(Constants.ATTACK_SHORT);
			SetMaxCollisions(0);
			SetDuration(0.1f);
			SetTrajectoryType(Constants.TRAJECTORY_LINEAR);
		}
		
		// Update is called once per frame
		void Update () {
			base.Update();
		}
		
		public void OnTriggerEnter(Collider c)
		{
			base.OnTriggerEnter(c);
			
		}
	}
}