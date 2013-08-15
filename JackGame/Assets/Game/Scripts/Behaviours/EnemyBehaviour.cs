using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pragmatia;


namespace JackGame
{
	public class EnemyBehaviour : PgtCharacter 
	{
		private int HomeLifeDrain;
		private bool isMoving = true;
		public void Awake () 
		{
			base.Awake();
			// Collection container
			SetCollection(Constants.COLLECTION_ENEMIES);
			// Collection to attack
			SetCollectionToAttack(Constants.COLLECTION_ALLIES);
			SetDieAnimation(Constants.DIE_ANIMATION);
		}
		
		public void Start() 
		{
			base.Start(); // Call generic Character start
			this.isMoving = true;
			SetPointingDirection(Constants.LEFT);
			isMoving = true;
		}
		
		public void Update() {
			base.Update();
			if (GetAttackTypeToExecute() != Constants.ATTACK_NONE) 
			{
				isMoving = false;
			}
			else {
				isMoving = true;
			}
			
			if (gameObject.transform.position.x <= -3)
			{				
				Statics.HomeLife -= GetHomeLifeDrain();
				DestroyCharacter();
			}
			
		}
		
		public void Attack(int range) {
		 	//SetWeapon();
			base.Attack(range);
		}
		
		public void Move()
		{
			if (isMoving) Move(Constants.MOVE_LEFT);
		}
		
		
		public void OnTriggerEnter(Collider c) 
		{
			base.OnTriggerEnter(c);
		}
			/*
			string colliderTag = c.gameObject.tag;
			// TODO: See to change the logic here so the enemy doesn't have to check which object is colliding with
			// 		to see if it receives damage.			
			if (colliderTag == Constants.TAG_ARROW || colliderTag == Constants.TAG_SHOTGUN || colliderTag == Constants.TAG_AXES) 
			{			
				ReceiveDamage(c.gameObject);
			}
			*/
//			if ((c.gameObject.tag == Constants.TAG_JACK || c.gameObject.tag == Constants.TAG_BARRICADE) && !c.gameObject.GetComponent<PgtCharacter>().IsDead())
//			{
//				this.isMoving = false;
//			}
//		}
//
//		public void OnTriggerStay(Collider c)
//		{
//			if ((c.gameObject.tag == Constants.TAG_JACK || c.gameObject.tag == Constants.TAG_BARRICADE))
//			{
//				if (c.gameObject.GetComponent<PgtCharacter>().IsDead())
//				{
//					this.isMoving = true;
//				}
//			}
//		}
//		
//		public void OnTriggerExit(Collider c)
//		{		
//			if (c.gameObject.tag == Constants.TAG_JACK || c.gameObject.tag == Constants.TAG_BARRICADE)
//			{
//				this.isMoving = true;
//			}
//		}
		
		public void SetHomeLifeDrain(int HomeLifeDrain)
		{
			this.HomeLifeDrain = HomeLifeDrain;
		}
		
		public int GetHomeLifeDrain()
		{
			return this.HomeLifeDrain;
		}
		
		public bool Moving 
		{
			set
			{
				this.isMoving = value;
			}
			get
			{
				return this.isMoving;
			}
		}
		
	}
}