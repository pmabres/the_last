using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pragmatia;

namespace JackGame
{
	public class BodyBehaviour : PgtCharacter 
	{
		private float LifeTime;
		private float LifeTimeCurrent;
		
		void Start () {
			SetAttackDelayTotalTime(3);
			SetLifeTime(7);
			SetAttackTypeToExecute(Constants.ATTACK_SHORT);
			SetCollection(Constants.COLLECTION_ALLIES);
			SetCollectionToAttack(Constants.COLLECTION_ENEMIES);
			SetHittable(false);	
		}
		
		// Update is called once per frame
		public void Update () 
		{
			base.Update();
			
			// Check if it has to be destroyed
			LifeTimeCurrent += Time.deltaTime;
			if (LifeTimeCurrent >= LifeTime)
			{
				Destroy(gameObject);
			}
			
			// Attack
			if (CanAttack)
			{
				SetAttackTypeToExecute(Constants.ATTACK_SHORT);
				SetWeaponByAttackType();
				DoAttack();	
				CanAttack = false;
				SetAttackDelayTotalTimer(0);
			}
		}
		
		public void SetLifeTime(float LifeTime)
		{
			this.LifeTime = LifeTime;
		}
		
		public float GetLifeTime()
		{
			return this.LifeTime;
		}
	}
}
