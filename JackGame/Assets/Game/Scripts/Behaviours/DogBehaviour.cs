using UnityEngine;
using System.Collections;
using Pragmatia;

namespace JackGame
{
	public class DogBehaviour : PgtCharacter 
	{	
		private const int INITIAL_HEALTH = 150;
		private const float MARGIN_ERROR = 2f;
		private const float MARGIN_TO_JACK = 5f;
		
		private float TargetPosition;
		private float BarricadePosition;
		private float NearestEnemyPosition;
		private float StandbyPosition;
		
		// Needed by direct action button
		private bool Called = false;
		private bool StartCounting = false;
		private float CalledActionRange;
		private float CallAttackTime;		// Attacking time of the dog when called by the direct action
		private float CallAttackTimeCurrent;
		
		// Nearest detection
		private	float nearestTimer;
		private float nearestUpdateInterval = 0.5f;
		
		// Following time
		private float followingTime = 0.7f;
		private float followingTimeCurrent = 0;
		
		void Awake()
		{
			base.Awake();
			SetEvolutionStatus(0);
			SetTotalHealth(INITIAL_HEALTH);
			SetCurrentHealth(INITIAL_HEALTH);
			SetSpeed(0.2f);
			SetAttackDelayTotalTime(0);		
			
			// Collection container
			SetCollection(Constants.COLLECTION_ALLIES);
			// Collection to attack
			SetCollectionToAttack(Constants.COLLECTION_ENEMIES);
			
			SetPointingDirection(Constants.RIGHT);
			SetAttackRangeShort(4);
			
			SetRegenerationTime(7);
			SetRegenerationStep(10);
			SetRegenerationStepTime(1f);
			
			SetReviveTime(5);
			
			SetCallAttackTime(30);
	
			//Set Animations
			
			SetWalkRightAnimation("Walk");
			SetWalkLeftAnimation("WalkBack");
			SetShortAttackAnimation(Constants.ATTACK_SHORT_ANIMATION_DOG);
			SetStunAnimation(Constants.STUN_ANIMATION);
			SetDieAnimation(Constants.DIE_ANIMATION);
			SetStandByAnimation(Constants.STANDBY_ANIMATION_DOG);
			
			// At begin, Standard position is where dog spawned
			StandbyPosition = gameObject.transform.position.x;
			TargetPosition = StandbyPosition;
			
			SetCalledActionRange(50);
			
			StartCounting = false;
			
			// Start default animation
			StandBy();
		}
		
		void Start()
		{
			// Get the position of the barricade
			base.Start();
			BarricadePosition = GameObject.FindWithTag(Constants.TAG_BARRICADE).transform.position.x;
		}
		
		void Update()
		{
			base.Update();			
			if (IsDead())
			{
				SetCalled(false);
				// Respawn dog at the standby position
				TargetPosition = StandbyPosition;
				gameObject.transform.position = new Vector3(TargetPosition, gameObject.transform.position.y, gameObject.transform.position.z);	
			}
			
			int attackType = GetAttackTypeToExecute();
			
			nearestTimer += Time.deltaTime;
			if (nearestTimer >= nearestUpdateInterval) 
			{						
				NearestEnemy();
				nearestTimer = 0;
			}
			
			// Recalculates the time when dog will act normally
			if (IsCalled() && !IsDead() && CallAttackTime > 0)
			{
				// Recalculate the default target (if not attacking) of dog is near to Jack position
				followingTimeCurrent += Time.deltaTime;
				if (followingTimeCurrent >= followingTime) 
				{
					TargetPosition = GameObject.FindGameObjectWithTag(Constants.TAG_JACK).transform.position.x + MARGIN_ERROR;
					followingTimeCurrent = 0;
				}
				// Start counting when dog reaches Jack or started attacking
				if (StartCounting)
				{
					CallAttackTimeCurrent += Time.deltaTime;
					if (CallAttackTimeCurrent >= CallAttackTime) 
					{						
						SetCalled(false);
						StartCounting = false;
						// Restore default position the spawn position
						TargetPosition = StandbyPosition;
						CallAttackTimeCurrent = 0;
					}
				}
			}
			
			float nearEnemyPosition = GetNearestEnemyPosition();
			
			// The dog perform different actions depending if it's acting normal or it's called by the direct action
			
			// Attack
			if (attackType != Constants.ATTACK_NONE && CanAttack) // Can Attack only while not having animations
			{
				Attack(attackType);
			}
			// Move the Dog to the nearest enemy between home and barricade when normal or to the next enemy when called
			// When called, the dog will move in a range
			else if (0 < nearEnemyPosition && ( nearEnemyPosition <= BarricadePosition || ( IsCalled() && (nearEnemyPosition - gameObject.transform.position.x) <= GetCalledActionRange() ) ) ) 
			{
				MoveToTarget(nearEnemyPosition);
			}
			// Move dog to house (if normal) or near to Jack (if called)
			else if ((!IsCalled() && Mathf.Abs(TargetPosition - gameObject.transform.position.x) > MARGIN_ERROR) || (IsCalled() && Mathf.Abs(TargetPosition - gameObject.transform.position.x) > MARGIN_ERROR + MARGIN_TO_JACK))
			{
				MoveToTarget(TargetPosition);
			}
			else
			{
				StandBy();
			}
			
			if (IsCalled() && !StartCounting && (Mathf.Abs(TargetPosition - gameObject.transform.position.x) < MARGIN_ERROR + MARGIN_TO_JACK))
			{
				StartCounting = true;
			}
			
		}
		
		private void MoveToTarget(float Target)
		{
			// Target at left of dog and right of home (and tries to move a bit more)
			if (Target < gameObject.transform.position.x && Target >= 0)
			{
				Move(Constants.MOVE_LEFT);
			} 
			// Target at right of dog and at left of barricade (only when not called)
			else if (Target > gameObject.transform.position.x && (Target <= BarricadePosition || IsCalled()))
			{
				Move(Constants.MOVE_RIGHT);
			}
		}
		
		private void NearestEnemy()
		{
			float nearestEnemyPosition = 10000;
			float nearestDistance = 10000;
			if (Statics.characters.Instances.ContainsKey(GetCollectionToAttack()))
			{
				for (int i = 0; i < Statics.characters.Instances[GetCollectionToAttack()].Count; i++) 
				{
					// Skip if character is dead
					if (Statics.characters.Instances[GetCollectionToAttack()][i].GetComponent<PgtCharacter>().IsDead() || !Statics.characters.Instances[GetCollectionToAttack()][i].GetComponent<PgtCharacter>().CanHit()) continue;
				
					float tmpDistance = Mathf.Abs(Statics.characters.Instances[GetCollectionToAttack()][i].transform.position.x - this.gameObject.transform.position.x);
	
					if (tmpDistance < nearestDistance) 
					{
						nearestEnemyPosition = Statics.characters.Instances[GetCollectionToAttack()][i].transform.position.x;
						nearestDistance = tmpDistance;
					}
				}
			}
			
			SetNearestEnemyPosition(nearestEnemyPosition);
		}
		
		private void SetNearestEnemyPosition(float EnemyPosition)
		{
			this.NearestEnemyPosition = EnemyPosition;
		}
		
		private float GetNearestEnemyPosition()
		{
			return this.NearestEnemyPosition;
		}
		
		public void SetCalled(bool Called)
		{
			this.Called = Called;	
			// Fetch the new target
			TargetPosition = GameObject.FindGameObjectWithTag(Constants.TAG_JACK).transform.position.x + MARGIN_ERROR;
		}
		
		public bool IsCalled()
		{
			return this.Called;
		}
		
		public void SetCallAttackTime(float CallAttackTime)
		{
			this.CallAttackTime = CallAttackTime;
		}
		
		public float GetCallAttackTime()
		{
			return this.CallAttackTime;
		}
		
		public void SetCalledActionRange(float CalledActionRange)
		{
			this.CalledActionRange = CalledActionRange;
		}
		
		public float GetCalledActionRange()
		{
			return this.CalledActionRange;
		}
		
	}
}