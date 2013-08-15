using UnityEngine;
using System.Collections;
using Pragmatia;


namespace JackGame
{
	public class MainCharacter : PgtCharacter 
	{	
		public const int EVOLUTION_STATUS_NORMAL = 0;
		public const int EVOLUTION_STATUS_MUTATED = 1;
		
		private int InitialEvolutionStatus;
		
		private bool Mutated = false;
		private float EvolveDuration;
		private float EvolveDurationTimer;
		private int moveOrderDirection = 0;
		private string BrakesLeftAnimation;
		private string BrakesRightAnimation;
		private float SpeedWalkingLeft;
		private float TimeWalkingLeft = 0;
		private float TimetoRotateLeft;
		private string WalkLeftAnimation;
		private float OriginalSpeed;
		private bool IsRunning = false;
		
		void Awake()
		{
			base.Awake();
			InitialEvolutionStatus = EVOLUTION_STATUS_NORMAL;
			
			// Set the evolution and load the animations
			SetEvolution(EVOLUTION_STATUS_NORMAL);
			
			SetTotalHealth(1000);
			SetCurrentHealth(1000);
			SetSpeed(0.2f);
			SetSpeedWalkingLeft(0.1f);
			OriginalSpeed = GetSpeed();
			SetAttackDelayTotalTime(0);			
			
			// Collection container
			SetCollection(Constants.COLLECTION_ALLIES);
			// Collection to attack
			SetCollectionToAttack(Constants.COLLECTION_ENEMIES);
			// Adding an attack
			//SetProjectile("Arrow");
			SetPointingDirection(Constants.RIGHT);
			SetAttackRangeLong(Constants.ATTACK_RANGE_LONG);
			SetAttackRangeMedium(Constants.ATTACK_RANGE_MEDIUM);
			SetAttackRangeShort(Constants.ATTACK_RANGE_SHORT);
			
			SetRegenerationTime(7);
			SetRegenerationStep(10);
			SetRegenerationStepTime(1f);
			
			SetEvolveDuration(30);
			SetTimetoRotateLeft(2);
			
			// Stun probability (float: 0 never - 1 always). Check if the character has this animation!
			SetStunProbability(0.01f);
			
			// Start default animation
			StandBy();
		}
		// Use this for initialization
		void Start () 
		{
			base.Start();
			
			//Vector3 cameraPosition = Camera.main.ScreenToViewportPoint(new Vector3(Camera.main.pixelWidth * 13 , Camera.main.pixelHeight * 9, 0));
			//SetHealthDisplayPosition(cameraPosition);
			SetHealthDisplayParent(GameObject.FindGameObjectWithTag(Constants.TAG_HUDCAMERA));
			SetHealthDisplayLocalPosition(new Vector2(-11,8));
			SetHealthDisplayLocalScale(new Vector2(0.02f,0.02f));
		}	
		
		// Update is called once per frame
		new void FixedUpdate () 
		{
			base.Update();
			
			// If Jack is mutated by a direct action button, start timer
			if (IsMutated())
			{
				EvolveDurationTimer += Time.deltaTime;
				if (EvolveDurationTimer >= EvolveDuration)
				{
					// Restore evolution to the initial evolution 
					SetEvolution(InitialEvolutionStatus);
					EvolveDurationTimer = 0;
					Mutated = false;
				}
			}
			
			int attackType = GetAttackTypeToExecute();
		 	if (Input.GetMouseButton(0) && CameraBehaviour.CameraStatus == Constants.CAMERA_STATUS_IDLE)
			{
				// Holding click or touch to move 
				if (moveOrderDirection != 0) 
				{
					// Jack modify its moving left animation after a time
					if (moveOrderDirection == Constants.MOVE_LEFT)
					{
						TimeWalkingLeft += Time.deltaTime;
						if (TimeWalkingLeft >= TimetoRotateLeft)
						{
							// Modify walk animation to run 
							SetWalkLeftAnimation(Constants.RUN_LEFT_ANIMATION_JACK);
							TimeWalkingLeft = 0;
							// Reasign the original speed
							SetSpeed(OriginalSpeed);
							IsRunning = true;
						}
					}
					
					Move(moveOrderDirection);
				} 
				else 
				{
					// New move click
					Vector2 clickPosition = PgtCommons.Convert(Input.mousePosition);
					
					if (clickPosition.x < Camera.current.GetScreenWidth() / 2) 
					{
						Move(Constants.MOVE_LEFT);
						moveOrderDirection = Constants.MOVE_LEFT;
						
						// Modify the speed for the walking back speed
						OriginalSpeed = GetSpeed();
						SetSpeed(GetSpeedWalkingLeft());
						IsRunning = false;
					} 
					else 
					{
						Move (Constants.MOVE_RIGHT);
						moveOrderDirection = Constants.MOVE_RIGHT;
						
						IsRunning = true;
					}
				}
			} 
			else if (attackType != 0)
			{
				Attack(attackType);
				moveOrderDirection = 0;
			} 
			else 
			{
				// Uses brakes only when running
				if (moveOrderDirection != 0 && IsRunning) {
					Brake();
				}
				else if (!IsPlayingAnimation(GetBrakesLeftAnimation()) && !IsPlayingAnimation(GetBrakesRightAnimation())) 
				{
					StandBy();
					SetPointingDirection(Constants.RIGHT);
				}
				moveOrderDirection = 0;
				IsRunning = false;
			}
			
			if (moveOrderDirection == 0 || IsPlayingAttack())
			{
				RestoreWalkLeft();
			}
		}
		
		// Plays brake left or right by the direction
		private void Brake()
		{
			string BrakesAnimation = GetBrakesRightAnimation();
			if (moveOrderDirection == Constants.MOVE_LEFT)
			{
				BrakesAnimation = GetBrakesLeftAnimation();
			}
			PlayAnimation(BrakesAnimation);
		}
		
		// Change the left animation to the walk left and reset the speed to the original
		private void RestoreWalkLeft()
		{
			SetWalkLeftAnimation(WalkLeftAnimation);
			SetSpeed(OriginalSpeed);
		}
		
		// Smash hit called by the Direct Action button
		public void SmashingHit()
		{
			string originalAnimation = GetShortAttackAnimation();
			
			// Change the animation
			SetShortAttackAnimation(Constants.ATTACK_SMASHINGHIT_ANIMATION_JACK);
			
			// Set the weapon for this attack
			SetWeapon(Constants.TAG_SMASHING_HIT);

			
			// Play the animation
			DoAttack();	 // Do attack for now
			SetPlayingAttack(true);
			
			//Attack(Constants.ATTACK_SPECIAL_1);
			moveOrderDirection = 0;
			
			// Restore the animation
			SetShortAttackAnimation(originalAnimation);
		}
		
		// Evolve called by Direct Action
		public void Evolve()
		{
			if (!IsMutated() && InitialEvolutionStatus <= Constants.JACK_MAX_EVOLUTION_LEVEL)
			{
				int NewEvolutionStatus = GetEvolutionStatus() + 1;
				SetEvolution(NewEvolutionStatus);
				Mutated = true;
			}
		}
		
		public void OnTriggerEnter(Collider c) 
		{
			base.OnTriggerEnter(c);
			
			// Collect hope
			if (c.gameObject.tag == Constants.TAG_HOPE && c.gameObject.GetComponent<HopeBehaviour>().IsActive())
			{
				Statics.Hope += c.gameObject.GetComponent<HopeBehaviour>().GetHope();
			}
		}
		
		public void SetEvolution(int EvolutionStatus)
		{
			if (EvolutionStatus >= 0 && EvolutionStatus <= Constants.JACK_MAX_EVOLUTION_LEVEL)
			{
				switch(EvolutionStatus)
				{
					case 0:
						SetWalkLeftAnimation(Constants.WALK_LEFT_ANIMATION_JACK);
						SetWalkRightAnimation(Constants.WALK_RIGHT_ANIMATION_JACK);
						SetRunLeftAnimation(Constants.RUN_LEFT_ANIMATION_JACK);
						SetBrakesLeftAnimation(Constants.BRAKES_LEFT_ANIMATION_JACK);
						SetBrakesRightAnimation(Constants.BRAKES_RIGHT_ANIMATION_JACK);
						SetStandByAnimation(Constants.STANDBY_ANIMATION);
						SetStunAnimation(Constants.STUN_ANIMATION);
						SetDieAnimation(Constants.DIE_ANIMATION);			
						
						SetShortAttackAnimation(Constants.ATTACK_SHORT_ANIMATION_JACK);
						SetMediumAttackAnimation(Constants.ATTACK_MEDIUM_ANIMATION_JACK);
						SetLongAttackAnimation(Constants.ATTACK_LONG_ANIMATION_JACK);
					
						break;
					case 1:
						SetWalkLeftAnimation(Constants.EVOLUTION_1_WALK_RIGHT_ANIMATION_JACK); // Inverted to watch it while not having animations
						SetWalkRightAnimation(Constants.EVOLUTION_1_WALK_LEFT_ANIMATION_JACK);
						SetRunLeftAnimation(Constants.EVOLUTION_1_RUN_LEFT_ANIMATION_JACK);
						SetBrakesLeftAnimation(Constants.EVOLUTION_1_BRAKES_LEFT_ANIMATION_JACK);
						SetBrakesRightAnimation(Constants.EVOLUTION_1_BRAKES_RIGHT_ANIMATION_JACK);
						SetStandByAnimation(Constants.EVOLUTION_1_STANDBY_ANIMATION);
						SetStunAnimation(Constants.EVOLUTION_1_STUN_ANIMATION);
						SetDieAnimation(Constants.EVOLUTION_1_DIE_ANIMATION);			
						
						SetShortAttackAnimation(Constants.EVOLUTION_1_ATTACK_SHORT_ANIMATION_JACK);
						SetMediumAttackAnimation(Constants.EVOLUTION_1_ATTACK_MEDIUM_ANIMATION_JACK);
						SetLongAttackAnimation(Constants.EVOLUTION_1_ATTACK_LONG_ANIMATION_JACK);
					
						break;
					default:
						break;
				}
				WalkLeftAnimation = GetWalkLeftAnimation();
				SetEvolutionStatus(EvolutionStatus);
			}
		}
		
		public void SetInitialEvolutionStatus(int InitialEvolutionStatus)
		{
			this.InitialEvolutionStatus = InitialEvolutionStatus;
		}
		
		public int GetInitialEvolutionStatus()
		{
			return this.InitialEvolutionStatus;
		}
		
		public bool IsMutated()
		{
			return this.Mutated;
		}
		
		public void SetEvolveDuration(float EvolveDuration)
		{
			this.EvolveDuration = EvolveDuration;
		}
		
		public float GetEvolveDuration()
		{
			return this.EvolveDuration;
		}
		
		public void SetTimetoRotateLeft(float TimetoRotateLeft)
		{
			this.TimetoRotateLeft = TimetoRotateLeft;
		}
		
		public float GetTimetoRotateLeft()
		{
			return this.TimetoRotateLeft;
		}
							
		public void SetSpeedWalkingLeft(float SpeedWalkingLeft)
		{
			this.SpeedWalkingLeft = SpeedWalkingLeft;
		}
		
		public float GetSpeedWalkingLeft()
		{
			return this.SpeedWalkingLeft;
		}
		
		public void SetBrakesLeftAnimation(string BrakesLeftAnimation)
		{
			this.BrakesLeftAnimation = BrakesLeftAnimation;
		}
		
		public string GetBrakesLeftAnimation()
		{
			return this.BrakesLeftAnimation;
		}
		
		public void SetBrakesRightAnimation(string BrakesRightAnimation)
		{
			this.BrakesRightAnimation = BrakesRightAnimation;
		}
		
		public string GetBrakesRightAnimation()
		{
			return this.BrakesRightAnimation;
		}
	}
}