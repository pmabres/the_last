using UnityEngine;
using System.Collections;

namespace JackGame
{
	public class ActionBulletTimeBehaviour :  DirectActionBehaviour
	{
		private float MotionFactor;
		private bool Active = false;
		
		private float Duration;
		private float DurationTimer;
		
		public void Awake()
		{
			SetChargingTime(1);
			SetMotionFactor(2);
			SetDuration(10);
			SetColorButton(new Color(8, 8, 8));
		}
		
		public void Update()
		{
			base.Update();
			
			if (Active)
			{
				DurationTimer += Time.deltaTime;
				if (DurationTimer >= Duration)
				{
					// Restore the time scale
					Time.timeScale = 1;
					DurationTimer = 0;
					Active = false;
				}	
			}
		}
		
		public void OnMouseDown()
		{
			if (IsEnabled() && !Active)
			{
				Active = true;
				
				Time.timeScale = 1 / GetMotionFactor();
				
				Activate();
			}
		}
		
		public void SetMotionFactor(float MotionFactor)
		{
			this.MotionFactor = MotionFactor;
		}
		
		public float GetMotionFactor()
		{
			return this.MotionFactor;
		}
		
		public void SetDuration(float Duration)
		{
			this.Duration = Duration;
		}
		
		public float GetDuration()
		{
			return this.Duration;
		}
		
		
		
	}
}
