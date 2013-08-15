using UnityEngine;
using System.Collections;

namespace JackGame
{
	public class BodyPoisonBehaviour : WeaponBehaviour {
		float BodyPoisonTravelSpeed = 0;
		float Width;
		
		void Awake() 
		{
			SetWidth(4); // Size of the body
		}
		// Use this for initialization
		void Start () 
		{
			base.Start();
			SetTravelSpeed(new Vector2(BodyPoisonTravelSpeed,0));
			SetRangeType(Constants.ATTACK_SHORT);
			SetMaxCollisions(0);
			SetDuration(0.1f);
			SetTrajectoryType(Constants.TRAJECTORY_LINEAR);
		}
		
		// Update is called once per frame
		void Update () 
		{
			base.Update();
		}
		
		public void OnTriggerEnter(Collider c)
		{
			base.OnTriggerEnter(c);
		}
		
		public void SetWidth(float Width)
		{
			this.Width = Width;
			this.gameObject.transform.localScale = new Vector3(
				Width, 
				this.gameObject.transform.localScale.x, 
				this.gameObject.transform.localScale.z
			);
		}
		
		public float GetWidth()
		{
			return this.Width;
		}
	}
}