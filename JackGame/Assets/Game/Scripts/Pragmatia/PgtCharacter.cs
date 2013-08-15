using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pragmatia;
using JackGame;

namespace Pragmatia
{
	public class PgtCharacter : PgtBehaviour
	{
		// Character attributes
		private string Name; 				 											// Name
		private int TotalHealth; 		   												// Indicates the total health
		private int CurrentHealth;														// Indicates the current health
		public float Speed;																// Speed 
		private int EvolutionStatus;													// Indicates the state of the evolution
		private int attackTypeToExecute;												// Indicates the type of attack to perform
		private int Collection;															// Collection container
		private GameObject weapon;														// Short, medium or long distance weapon prefab
		private int MaxHopeRelease = 0;													// Maximum Hope the enemy can release. If 0, then doesnt give hope
		private int MinHopeRelease = 0;													// Minimum Hope the enemy can release
		private float RegenerationTime = 0;												// Time until the can regenerate while is not hitted
		private float RegenerationTimeCurrent = 0;										// Regeneration timer counter
		private int RegenerationStep = 0;												// Health units regenerate
		private float RegenerationStepTime = 1;											// Time until can regenerate a step
		private float RegenerationStepTimeCurrent = 0;									// Regeneration timer
		private float ReviveTime;														// Time until the character revive 
		private float ReviveTimeCurrent;												// Timer counter
		private bool Hittable = true;													// Indicates this object can be attacked. Default true
		private float StunProbability;													// Probability of playing the character of being stunned after a damage
		
		// Animations attributes
		private exSpriteAnimation Animations; 											// Sprites
		private string CurrentAnimation;												// Name of the animation playing right now
		private string walkLeftAnimation;												// Name of the left walk animation
		private string walkRightAnimation;												// Name of the right walk animation
		private string runLeftAnimation;												// Name of the run left animation
		private string runRightAnimation;												// Name of the run right animation
		private string shortAttackAnimation;											// Name of the short attack animation
		private string mediumAttackAnimation;											// Name of the medium attack animation
		private string longAttackAnimation;												// Name of the long attack animation
		private string standByAnimation;												// Name of the stand by (not moving) animation
		private string stunAnimation;													// Name of the stun (receive damage) animation
		private string dieAnimation;													// Name of the death animation
		private float attackDelayTotalTime;												// Total delay time between attack and attack
		private float attackDelayTimer;													// Current delay time between attack and attack
		private bool canAttack;															// Indicates if the character can attack after timer reach the delay time
		private int PointingDirection;													// Current direction character pointing
		private bool PlayingAttack = false;												// Indicates if is playing an attack
		private bool stopAndAttack = false;												// Indicates if the character must stop and attack
		private float AttackRangeShort;
		private float AttackRangeMedium;
		private float AttackRangeLong;
		private Vector2 nextTargetDistance;
		//Legacy Text 
		private GameObject HealthDisplay;												//Sprite used in Fonts
		private bool DisplayHealthText;
		private float healthBarTimer;
		
		// Distance detection
		private	float distanceTimer;
		private float distanceUpdateInterval;
		private int collectionToAttack;
		
		public void Awake()
		{
			// Get Component initialization in Start Event
			Animations = gameObject.GetComponent<exSpriteAnimation>();
			distanceTimer = 0;
			distanceUpdateInterval = 0.5f;			
		}
		public void Start() 
		{
			// If character is hittable, then show health display
			if (CanHit()) 
			{
				InstantiateHealthDisplay();
				SetHealthDisplayText(GetCurrentHealth().ToString());
			}
			//CorrectYPosition();
		}
		
		public void Update() 
		{		
			// If the character can revive
			if (IsDead() && CanRevive())
			{
				ReviveTimeCurrent += Time.deltaTime;
				if (ReviveTimeCurrent >= ReviveTime)
				{
					Revive();
					ReviveTimeCurrent = 0;
				}
			}
			
			// If the character can regenerate
			if (CurrentHealth < TotalHealth && !IsDead() && CanRegenerate())
			{
				RegenerationTimeCurrent += Time.deltaTime;
				// Time lapsed after being hit
				if (RegenerationTimeCurrent >= RegenerationTime)
				{
					RegenerationStepTimeCurrent += Time.deltaTime;
					// Delay between each regeneration
					if (RegenerationStepTimeCurrent >= RegenerationStepTime)
					{
						CurrentHealth += RegenerationStep;
						UpdateHealthDisplay();
						RegenerationStepTimeCurrent = 0;
					}
				}
			}
			
			// Time calculation for delay attack time
			attackDelayTimer += Time.deltaTime;
			if (attackDelayTimer >= attackDelayTotalTime) 
			{						
				canAttack = true;
				attackDelayTimer = 0;
			}
			
			// Attack type calculation
			//SetAttackTypeToExecute(0);

			distanceTimer += Time.deltaTime;
			if (distanceTimer >= distanceUpdateInterval) 
			{						
				GetDistance();
				distanceTimer = 0;
			}
			
			// Reset attack animation
			if (Animations != null && !Animations.IsPlaying(CurrentAnimation)) 
			{
				PlayingAttack = false;
				CurrentAnimation = "";
			}
			/*
			// Health bar timer
			if (DisplayHealthText)
			{
				healthBarTimer += Time.deltaTime;
				if (healthBarTimer >= Constants.DISPLAY_TEXT_HEALTH_TIME) 
				{
					Destroy(HealthDisplay);
					DisplayHealthText = false;
					healthBarTimer = 0;
				}
			}*/
			
		}
		
		// Move
		public void Move(int Direction) 
		{
			if (!IsDead() && !IsStunned()) {
				PlayingAttack = false;
				Vector2 vector = new Vector2(GetSpeed() * Direction, 0); // Remove direction
				
				if (canMove(Direction)) Move(PgtCommons.Convert(vector));
				
//				if (!Animations.IsPlaying(walkLeftAnimation) && !Animations.IsPlaying(walkRightAnimation)) 
//				{				
					if (Direction == Constants.MOVE_RIGHT) 
					{ 
						PlayAnimation(walkRightAnimation);
					}
					if (Direction == Constants.MOVE_LEFT) 
					{
						PlayAnimation(walkLeftAnimation);
					}
//				}
			}
		}
		
		// Attack
		public void Attack(int range) 
		{
			if (!PlayingAttack && canAttack && !IsDead() && !IsStunned()) 
			{
				switch (range) 
				{
	                case Constants.ATTACK_SHORT:
							if (GetShortAttackAnimation() != null) PlayAnimation(GetShortAttackAnimation());
	                        break;
	                case Constants.ATTACK_MEDIUM:
							if (GetMediumAttackAnimation() != null) PlayAnimation(GetMediumAttackAnimation());
	                        break;
	                case Constants.ATTACK_LONG:
							if (GetLongAttackAnimation() != null) PlayAnimation(GetLongAttackAnimation());
	                        break;
	                default:
	                        break;
	            }				
	            PlayingAttack = true;
	            canAttack = false;
            }
        }
		// Standby animation
		public void StandBy() 
		{
			if (!PlayingAttack && !IsDead()) 
			{
				if (GetStandByAnimation() != null) PlayAnimation(GetStandByAnimation());
			}
		}
		
		// Instance a Weapon to damage the opponent
		public void DoAttack() 
		{	
			if (this.weapon != null)
			{
				DoAttack(Statics.damages.Get(weapon.tag, GetEvolutionStatus()), 0, new Vector2(0, 0));
			}
		}
		
		public void DoAttack(float AngleIncreaser, Vector2 PositionIncreaser) 
		{			
			if (this.weapon != null)
			{
				DoAttack(Statics.damages.Get(weapon.tag, GetEvolutionStatus()), AngleIncreaser, PositionIncreaser);
			}
		}
		
		public void DoAttack(int Damage, float AngleIncreaser, Vector2 PositionIncreaser) 
		{			
			if (this.weapon != null)
			{
				GameObject weaponInstance = Instantiate(this.weapon, this.gameObject.transform.position, gameObject.transform.rotation) as GameObject;
				weaponInstance.transform.parent = GameObject.FindGameObjectWithTag(Constants.TAG_MAIN).transform;
				//weaponInstance.transform.parent = gameObject.transform;		
				// Weapon has the damage attached
				weaponInstance.GetComponent<WeaponBehaviour>().SetDamage(Damage);				
				weaponInstance.GetComponent<WeaponBehaviour>().SetCollection(GetCollection());
				weaponInstance.GetComponent<WeaponBehaviour>().SetOriginTag(gameObject.tag);
				weaponInstance.GetComponent<WeaponBehaviour>().SetOriginEvolutionStatus(GetEvolutionStatus());
				weaponInstance.GetComponent<WeaponBehaviour>().SetTargetDistance(GetNextTargetDistance() + PositionIncreaser);				
				weaponInstance.GetComponent<WeaponBehaviour>().SetShootingAngle(weaponInstance.GetComponent<WeaponBehaviour>().GetShootingAngle() + AngleIncreaser);
				
				// TODO: CORRECT THE ARROW POSITION PROPERLY
				if (gameObject.tag == Constants.TAG_JACK && weaponInstance.tag == Constants.TAG_ARROW)
				{
					//The bow where the arrow has to go out is at 3/4th of jack 
					weaponInstance.transform.position = new Vector3(weaponInstance.transform.position.x,
														weaponInstance.transform.position.y + gameObject.renderer.bounds.size.y / 4,
														weaponInstance.transform.position.z);
				}
				weaponInstance.GetComponent<WeaponBehaviour>().Shoot();
			}
		}
		
		// Method called to receive damage in character
		public int ReceiveDamage (GameObject Source) 
		{
			if (!IsDead() && CanHit()) {
				// The source object who is hitting the gameObject
				string tag = Source.tag;
				// The gameObject is the target who got hit
				int Damage =  Source.GetComponent<WeaponBehaviour>().GetDamage();
				
				if (Damage >= 0)
				{
					// Get the resistances of the target being hitted and absorb damage
					
					Damage = (int) Mathf.Floor(Damage * Statics.resistances.Get(gameObject.tag,Source.tag));
					//int Damage = Statics.damages.Get(tag, Source.GetComponent<WeaponBehaviour>().GetOriginEvolutionStatus());
					//int Damage = Statics.damages.Get(tag, Source.transform.parent.GetComponent<PgtCharacter>().GetEvolutionStatus());
					if (Source.tag == Constants.TAG_ROCK) Source.GetComponent<RockBehaviour>().ReduceDamage(this.GetCurrentHealth());
					
					ReduceHealth(Damage);
					
					if (GetCurrentHealth() <= 0)
					{
						Die();
					}
					else
					{
						// Can be stunned
						float RandomNumber = Random.Range(0, 1);
						if (GetStunProbability() != 0 && RandomNumber <= GetStunProbability())
						{
							if (GetStunAnimation() != null) PlayAnimation(GetStunAnimation());
						}
					}
					
					// Restart regeneration timer
					RegenerationTimeCurrent = 0;
					
					return Damage;
				}
			}
			return 0;
		}
		
		// Plays die animation
		public void Die() {
			ReleaseHope(); // Release the hope
			if (GetDieAnimation() != null) PlayAnimation(GetDieAnimation());
			Destroy (HealthDisplay);
			
		}
		
		// Revives the character
		public void Revive()
		{
			this.SetCurrentHealth(GetTotalHealth());
			InstantiateHealthDisplay();
			UpdateHealthDisplay();
		}
		
		// Destroy character and unset from collection
		public void DestroyCharacter() {
			if (!CanRevive())
			{
				Statics.characters.Remove(GetCollection(), gameObject);
				// Jack doesnt destroy. Just stays on the floor dead
				if (this.tag != Constants.TAG_JACK)
				{
					Destroy (gameObject);
				}
			}
		}
		
		// Returns the attack type by the distance of the object in a collection
		public void GetDistance()
		{
			float characterPositionX = gameObject.transform.position.x;
			float distance = 10000;
			// Get the max range of attack (assuming that the long range is greater than medium and medium than short)
			
			// Nearest of each range
			
			// Short
			float nearestShortDistance = distance;
			GameObject nearestShortCharacter = null;
			
			// Medium
			float nearestMediumDistance = distance;
			GameObject nearestMediumCharacter = null;
			
			// Range
			float nearestLongDistance = distance;
			GameObject nearestLongCharacter = null;
			
			
			
			// Creates a sorted list of the distance with the nearest enemies
			if (Statics.characters.Instances.ContainsKey(GetCollectionToAttack()) && !Statics.characters.IsEmpty(GetCollectionToAttack()))
			{
				for (int i = 0; i < Statics.characters.Instances[GetCollectionToAttack()].Count; i++) 
				{
					// Skip if character is dead
					if (Statics.characters.Instances[GetCollectionToAttack()][i].GetComponent<PgtCharacter>().IsDead() || !Statics.characters.Instances[GetCollectionToAttack()][i].GetComponent<PgtCharacter>().CanHit()) continue;
				
					// Distance from this and the current character
					float tmpDistance = Statics.characters.Instances[GetCollectionToAttack()][i].transform.position.x - characterPositionX;
					
					// Nasty! Enemies are always in front of Jack
					if (GetCollectionToAttack() == Constants.COLLECTION_ALLIES) tmpDistance = Mathf.Abs(tmpDistance);
					
					if (tmpDistance > 0)
					{
						// Update the nearest short (in short and range and closer than previous)
						if (tmpDistance <= GetAttackRangeShort() && tmpDistance < nearestShortDistance)
						{
							nearestShortDistance = tmpDistance;
							nearestShortCharacter = Statics.characters.Instances[GetCollectionToAttack()][i];
						}
						
						// Update the nearest medium (in medium and range and closer than previous)
						if (tmpDistance > GetAttackRangeShort() && tmpDistance <= GetAttackRangeMedium() && tmpDistance < nearestMediumDistance)
						{
							nearestMediumDistance = tmpDistance;
							nearestMediumCharacter = Statics.characters.Instances[GetCollectionToAttack()][i];
						}
						
						// Update the nearest short (in long and range and closer than previous)
						if (tmpDistance <= GetAttackRangeLong() && tmpDistance < nearestLongDistance)
						{
							nearestLongDistance = tmpDistance;
							nearestLongCharacter = Statics.characters.Instances[GetCollectionToAttack()][i];
						}
					}
				}
			}
			
			if (nearestShortCharacter != null)
			{
				SetAttackTypeToExecute(Constants.ATTACK_SHORT);
				SetNextTargetDistance(new Vector2(nearestShortDistance, nearestShortCharacter.transform.position.y));
				SetWeaponByAttackType();
			}
			else if (nearestMediumCharacter != null)
			{
				SetAttackTypeToExecute(Constants.ATTACK_MEDIUM);
				SetNextTargetDistance(new Vector2(nearestMediumDistance, nearestMediumCharacter.transform.position.y));
				SetWeaponByAttackType();
			}
			else if (nearestLongCharacter != null)
			{
				SetAttackTypeToExecute(Constants.ATTACK_LONG);
				SetNextTargetDistance(new Vector2(nearestLongDistance, nearestLongCharacter.transform.position.y));
				SetWeaponByAttackType();
			}
			else 
			{
				SetAttackTypeToExecute(Constants.ATTACK_NONE);
			}
		}
		
		public void ReduceHealth(int r) 
		{
			if (r >= 0)
			{
				this.CurrentHealth -= r;
				if (this.CurrentHealth <= 0) 
				{
					this.CurrentHealth = 0;
									
				}
				//InstantiateHealthDisplay();
				//DisplayHealthText = true;
				UpdateHealthDisplay();
			}
		}
		
		public void InstantiateHealthDisplay()
		{
			if (HealthDisplay != null) Destroy(HealthDisplay);
			HealthDisplay = Instantiate(Resources.Load("Prefabs/HealthDisplay"), gameObject.transform.position + new Vector3(0, gameObject.renderer.bounds.size.y / 2 + 1, 0), Quaternion.identity) as GameObject; // new Vector3(0, 2, 0)
			//int textHeight = (int) Mathf.Floor(Screen.height / 480);
			HealthDisplay.transform.localScale = new Vector3(0.01f,0.01f,0f);
			HealthDisplay.transform.parent = gameObject.transform;
		}
		
		public void SetHealthDisplayText(string text)
		{
			HealthDisplay.GetComponent<exSpriteFont>().text = text;
		}
		public void SetHealthDisplayPosition(Vector2 position)
		{
			HealthDisplay.transform.position = PgtCommons.Convert(position);
		}
		public void SetHealthDisplayLocalPosition(Vector2 position)
		{
			HealthDisplay.transform.localPosition = PgtCommons.Convert(position);
		}
		public Vector2 GetHealthDisplayPosition()
		{
			return HealthDisplay.transform.position;
		}
		public void SetHealthDisplayLocalScale(Vector2 scale)
		{
			HealthDisplay.transform.localScale = PgtCommons.Convert(scale);
		}
		public void SetHealthDisplayParent(GameObject parent)
		{
			HealthDisplay.transform.parent = parent.transform;
		}
		public void UpdateHealthDisplay()
		{
			SetHealthDisplayText(GetCurrentHealth().ToString());
		}
		private void ReleaseHope()
		{
			// If MaxHope is 0 or less the character doesnt release hope
			if (MaxHopeRelease > 0)
			{
				int HopeValue = Random.Range(MinHopeRelease - 1, MaxHopeRelease);
				if (HopeValue > 0)
				{
					GameObject HopeInstance;
					HopeInstance = MonoBehaviour.Instantiate(Resources.Load("Prefabs/" + Constants.TAG_HOPE, typeof(GameObject)), this.gameObject.transform.position - new Vector3(0,0,-0.5f), this.gameObject.transform.rotation) as GameObject;
					//weaponInstance.transform.parent = gameObject.transform;		
					HopeInstance.GetComponent<HopeBehaviour>().SetHope(HopeValue);
				}
				
			}
		}
		public void OnTriggerEnter(Collider c) 
		{			
			// Care about collitions between weeapon and something else
			if (c.gameObject.GetComponent<WeaponBehaviour>() != null && CanHit())
			{
				//int collection = c.gameObject.transform.parent.gameObject.GetComponent<PgtCharacter>().GetCollection();
				int collection = c.gameObject.GetComponent<WeaponBehaviour>().GetCollection();
				// TODO: See to change the logic here so the enemy doesn't have to check which object is colliding with
				// 		to see if it receives damage.		
									
				// Don't receive damage from its weapon and from its collection
				// c.gameObject.transform.parent != this.gameObject.transform
				if (this.GetCollection() != collection)
				{
					if (c.GetComponent<WeaponBehaviour>().CanDamage())
					{
						ReceiveDamage(c.gameObject);
					}
				}
			}
		}
		private void CorrectYPosition()
		{
			Vector3 position = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y + gameObject.renderer.bounds.size.y / 2 ,gameObject.transform.position.z);
			//Move(position);
		}
		// Method to say if character can move
		public bool canMove(int Direction) 
		{
			return ((gameObject.transform.position.x > -5 && gameObject.transform.position.x < Constants.SCREEN_WIDTH_METERS) ||
					(gameObject.transform.position.x <= -5 && Direction == Constants.MOVE_RIGHT) ||
					(gameObject.transform.position.x >= Constants.SCREEN_WIDTH_METERS && Direction == Constants.MOVE_LEFT));
		}
		
		public bool IsDead() 
		{
			return (this.CurrentHealth <= 0);
		}
		
		public bool IsStunned()
		{
			return (GetStunAnimation() != null && IsPlayingAnimation(GetStunAnimation()));
		}
		
		public void PlayAnimation(string AnimationName)
		{
			try 
			{
				if(!Animations.IsPlaying(AnimationName)) 
				{
					Animations.Play(AnimationName);
					CurrentAnimation = AnimationName;
				}
			} 
			catch (System.Exception ex) 
			{
				Debug.Log("Animation " + AnimationName + " doesn't exist: " + ex.ToString());
			}
		}
		
		public bool IsPlayingAnimation(string AnimationName)
		{
			try 
			{
				return Animations.IsPlaying(AnimationName);
			} 
			catch (System.Exception ex) 
			{
				Debug.Log("Animation " + AnimationName + " doesn't exist: " + ex.ToString());
				return false;
			}
		}
		
		// Total Health setter
		public void SetTotalHealth(int h) 
		{
			this.TotalHealth = h;
		}
		
		// Total Health getter
		public int GetTotalHealth() 
		{
			return this.TotalHealth;
		}
		
		// Current Health setter
		public void SetCurrentHealth(int h) 
		{
			this.CurrentHealth = h;
		}
		
		// Current Health getter
		public int GetCurrentHealth() 
		{
			return this.CurrentHealth;
		}
		
		// Add a new attack to enemy
	
		// GetSpeed
		public float GetSpeed() 
		{
			return this.Speed;
		}		
		
		// SetSpeed
		public void SetSpeed(float speed) 
		{
			this.Speed = speed;
		}
		
		// Get character projectile gameobject
		public GameObject GetWeapon()
		{
			return this.weapon;
		}
		
		// Set character projectile gameobject
		public void SetWeaponByAttackType()
		{
			string weapon = Statics.weapons.GetWeapon(gameObject.tag, GetAttackTypeToExecute());
			SetWeapon(weapon);
		}
		
		public void SetWeapon(string Weapon)
		{
			this.weapon = Resources.Load("Prefabs/" + Weapon, typeof(GameObject)) as GameObject;
		}
		
		public void SetPointingDirection(int d) 
		{
			this.PointingDirection = d;
		}
		
		public int GetPointingDirection() {
			return this.PointingDirection;
		}			
		
		public string GetShortAttackAnimation()
		{
			return this.shortAttackAnimation;
		}
		public void SetShortAttackAnimation(string attAnimation)
		{
			this.shortAttackAnimation = attAnimation;
		}
		public string GetMediumAttackAnimation()
		{
			return this.mediumAttackAnimation;
		}
		public void SetMediumAttackAnimation(string attAnimation)
		{
			this.mediumAttackAnimation = attAnimation;
		}
		public string GetLongAttackAnimation()
		{
			return this.longAttackAnimation;
		}
		public void SetLongAttackAnimation(string attAnimation)
		{
			this.longAttackAnimation = attAnimation;
		}
		
		public void SetWalkLeftAnimation(string animation)
		{
			this.walkLeftAnimation = animation;
		}
		
		public string GetWalkLeftAnimation()
		{
			return this.walkLeftAnimation;
		}
		
		public void SetWalkRightAnimation(string animation)
		{
			this.walkRightAnimation = animation;
		}
		
		public string GetWalkRightAnimation()
		{
			return this.walkRightAnimation;
		}
		
		public void SetRunLeftAnimation(string animation)
		{
			this.runLeftAnimation = animation;
		}
		
		public string GetRunLeftAnimation()
		{
			return this.runLeftAnimation;
		}
		
		public void SetRunRightAnimation(string animation)
		{
			this.runRightAnimation = animation;
		}
		
		public string GetRunRightAnimation()
		{
			return this.runRightAnimation;
		}
	
		public void SetStandByAnimation(string animation)
		{
			this.standByAnimation = animation;
		}
		
		public string GetStandByAnimation()
		{
			return this.standByAnimation;
		}
		
		public void SetDieAnimation(string animation)
		{
			this.dieAnimation = animation;
		}
		
		public string GetDieAnimation()
		{
			return this.dieAnimation;
		}
		
		public void SetStunAnimation(string animation)
		{
			this.stunAnimation = animation;
		}
		
		public string GetStunAnimation()
		{
			return this.stunAnimation;
		}

		public void SetAttackDelayTotalTime(float time)
		{
			this.attackDelayTotalTime = time;
		}
		
		public float GetAttackDelayTotalTime()
		{
			return this.attackDelayTotalTime;
		}
		
		public void SetAttackDelayTotalTimer(float time)
		{
			this.attackDelayTimer = time;
		}
		
		public float GetAttackDelayTotalTimer()
		{
			return this.attackDelayTimer;
		}
		
		public void SetAttackTypeToExecute(int type)
		{
			this.attackTypeToExecute = type;
		}
		
		public int GetAttackTypeToExecute()
		{
			return this.attackTypeToExecute;
		}
		
		public void SetCollectionToAttack(int collection)
		{
			this.collectionToAttack = collection;
		}
		
		public int GetCollectionToAttack()
		{
			return this.collectionToAttack;
		}
		
		//setters and getters for the evolution Status
		public void SetEvolutionStatus(int EvolutionStatus) 
		{
			this.EvolutionStatus = EvolutionStatus;
		}
		public int GetEvolutionStatus()
		{
			return this.EvolutionStatus;
		}
		public void SetAttackRangeShort(float Range)
		{
			this.AttackRangeShort = Range;
		}
		public float GetAttackRangeShort()
		{
			return this.AttackRangeShort;
		}
		public void SetAttackRangeMedium(float Range)
		{
			this.AttackRangeMedium = Range;
		}
		public float GetAttackRangeMedium()
		{
			return this.AttackRangeMedium;
		}
		public void SetAttackRangeLong(float Range)
		{
			this.AttackRangeLong = Range;
		}
		public float GetAttackRangeLong()
		{
			return this.AttackRangeLong;
		}
		public void SetCollection(int Collection)
		{
			this.Collection = Collection;
		}
		public int GetCollection()
		{
			return this.Collection;
		}
		public void SetHopeRelease(int MinHopeRelease, int MaxHopeRelease)
		{
			this.MinHopeRelease = MinHopeRelease;
			this.MaxHopeRelease = MaxHopeRelease;
		}
		public void SetMaxHopeRelease(int MaxHopeRelease)
		{
			this.MaxHopeRelease = MaxHopeRelease;
		}
		public int GetMaxHopeRelease()
		{
			return this.MaxHopeRelease;
		}
		public void SetMinHopeRelease(int MinHopeRelease)
		{
			this.MinHopeRelease = MinHopeRelease;
		}
		public int GetMinHopeRelease()
		{
			return this.MinHopeRelease;
		}
		
		public void SetRegenerationTime(float RegenerationTime)
		{
			this.RegenerationTime = RegenerationTime;
		}
		public float GetRegenerationTime()
		{
			return this.RegenerationTime;
		}
		
		public void SetRegenerationStep(int RegenerationStep)
		{
			this.RegenerationStep = RegenerationStep;
		}
		public int GetRegenerationStep()
		{
			return this.RegenerationStep;
		}
		
		public void SetRegenerationStepTime(float RegenerationStepTime)
		{
			this.RegenerationStepTime = RegenerationStepTime;
		}
		public float GetRegenerationStepTime()
		{
			return this.RegenerationStepTime;
		}
		
		public bool CanRegenerate()
		{
			return (this.GetRegenerationTime() > 0 && GetRegenerationStep() > 0);
		}
		
		public void SetReviveTime(float ReviveTime)
		{
			this.ReviveTime = ReviveTime;
		}
		
		public float GetReviveTime()
		{
			return this.ReviveTime;
		}
		public bool CanRevive()
		{
			return (this.ReviveTime > 0);
		}
		public bool IsEnemy(GameObject gobject) 
		{
			return (IsCharacter(gobject) && gobject.GetComponent<PgtCharacter>().GetCollection() == Constants.COLLECTION_ENEMIES);
		}
		
		public bool IsCharacter(GameObject gobject)
		{
			return (gobject.GetComponent<PgtCharacter>() != null);
		}
		public Vector2 GetNextTargetDistance()
		{
			return this.nextTargetDistance;
		}
		public void SetNextTargetDistance(Vector2 nextTargetDistance)
		{
			this.nextTargetDistance = nextTargetDistance;
		}
		public bool CanAttack
		{
			set
			{
				this.canAttack = value;
			}
			get
			{
				return this.canAttack;
			}
		}
		public bool CanHit()
		{
			return this.Hittable;
		}
		public void SetHittable(bool Hittable)
		{
			this.Hittable = Hittable;
		}
		
		public void SetPlayingAttack(bool PlayingAttack)
		{
			this.PlayingAttack = PlayingAttack;
		}
		
		public bool IsPlayingAttack()
		{
			return this.PlayingAttack;
		}
		
		public void SetStunProbability(float StunProbability)
		{
			this.StunProbability = StunProbability;
		}
		
		public float GetStunProbability()
		{
			return this.StunProbability;
		}
	}
}

