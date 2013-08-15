using UnityEngine;
using System.Collections;
using Pragmatia;

namespace JackGame
{
	public class HopeBehaviour : PgtBehaviour
	{
		// Value of this hope coin
		private int Hope;
		private bool Active = true;
		private bool Warned = false;
		
		private float DestroyTime = Constants.HOPE_DESTROY_TIME;
		private float DestroyAlertTime = Constants.HOPE_ALERT_TIME;
		private float DestroyTimeCurrent = 0f;
		
		public void Awake() 
		{
			gameObject.renderer.material.shader = Shader.Find("Transparent/Diffuse");
			gameObject.renderer.material.color = new Color(4, 4, 0, 1);
		}
		
		public void Start() 
		{
			// Appear animation or effect here
		}
		
		public void Update()
		{
			DestroyTimeCurrent += Time.deltaTime;
			if (DestroyTimeCurrent >= DestroyTime)
			{
				Active = false;
			}
			else if (!Warned && DestroyTimeCurrent >= DestroyAlertTime )
			{
				Hope = (int) Mathf.Floor(Hope / 2);
				gameObject.renderer.material.color = new Color(4, 4, 0, 0.5f);
				Warned = true;
			}
			
			// Just in case of an animation of collect
			if (!IsActive()) 
			{
				// Collect the hope automatically
				Statics.Hope += Hope;
				Destroy(gameObject);
			}
		}
		
		public void OnTriggerEnter(Collider c)
		{
			// Only Jack can collect the hope
			if (c.gameObject.tag == Constants.TAG_JACK)
			{
				Active = false;
			}
		}
		
		public void SetHope(int Hope)
		{
			this.Hope = Hope;
		}
		public int GetHope()
		{
			return this.Hope;
		}
		public bool IsActive()
		{
			return this.Active;
		}
	}
}

