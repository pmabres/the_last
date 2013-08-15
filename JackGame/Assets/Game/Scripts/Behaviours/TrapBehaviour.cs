using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pragmatia;

namespace JackGame
{
	public struct Affected
	{
		private GameObject Character;
		private float OriginalSpeed;
		
		public Affected (GameObject Character, float OriginalSpeed)
		{
			this.Character = Character;
			this.OriginalSpeed = OriginalSpeed;
		}
		
		public GameObject GetCharacter()
		{
			return this.Character;
		}
		
		public float GetOriginalSpeed()
		{
			return this.OriginalSpeed;
		}
	}
	
	public class TrapBehaviour : PgtCharacter 
	{
		// EvolutionStatus is the attack level
		
		// Current level of the trap
		private int SpeedLevel = Constants.TRAP_NONE;
		
		// Characters in the trap and its original speed
		private List<Affected> EnemiesInTrap = new List<Affected>();
		
		// Speed divider to apply in enemies
		private float SpeedDivider = 0;
		
		// Standard divider
		public const float StandardDivider = 1.1f;
		
		// Interval
		private float IntervalAttackTime = 0;
		
		// Interval current
		private float CurrentIntervalAttackTime = 0;
		
		// Time interval for check dead enemies
		private float UpdateTime = 1f;
		
		// Timer for check dead enemies
		private float UpdateTimeCurrent = 0f;
		
		// Color timer
		private float ColorTimer;
		
		// Color increaser
		private float ColorIncr = 1f;
		
		// Current timer for glowing
	
		// Use this for initialization
		void Start () {
			ResetTrap();
			SetIntervalAttackTime(2);
			SetAttackTypeToExecute(Constants.ATTACK_SHORT);
			SetCollection(Constants.COLLECTION_ALLIES);
			SetCollectionToAttack(Constants.COLLECTION_ENEMIES);
			SetHittable(false); // Cannot be hitted
			// Instantiate the controls
			
		}
		
		// Update is called once per frame
		public void Update () 
		{
			// Move to activate the trigger
			gameObject.transform.Translate(0, 0, 0);
			
			// Drain life of affected enemies (if trap is available)
			if (GetEvolutionStatus() != Constants.TRAP_NONE) { 
				CurrentIntervalAttackTime += Time.deltaTime;
				if (CurrentIntervalAttackTime >= IntervalAttackTime && EnemiesInTrap.Count > 0 )
				{
					// Update alive enemies. Destroy unexistents
					for (int i = EnemiesInTrap.Count - 1; i >= 0; i--)
					{
						// Remove character if not exists
					    if (EnemiesInTrap[i].GetCharacter() == null) 
						{
							EnemiesInTrap.RemoveAt(i);
						}
					}
					// Check if exists enemies to attack
					if (EnemiesInTrap.Count > 0)
					{
						SetAttackTypeToExecute(Constants.ATTACK_SHORT);
						SetWeaponByAttackType();
						// Perform attack
						DoAttack();
					}
					CurrentIntervalAttackTime = 0;
				}
			}
			else
			{
				UpdateTimeCurrent = 0;
			}
			
			// Increase color when level is 0 (attack or speed)
			if (this.GetEvolutionStatus() == Constants.TRAP_NONE)
			{
				ColorTimer += Time.deltaTime;
				if (ColorTimer >= 0.1f)
				{
					gameObject.renderer.material.color = new Color(
						gameObject.renderer.material.color.r + ColorIncr,
						gameObject.renderer.material.color.g + ColorIncr,
						gameObject.renderer.material.color.b + ColorIncr
					);
					if (gameObject.renderer.material.color.r >= 4 || gameObject.renderer.material.color.r <= 0)
					{
						ColorIncr = - ColorIncr;
					}
					ColorTimer = 0;
				}
			}
		}
		
		public void OnMouseDown()
		{
			if (!this.IsActive() && GetEvolutionStatus() == Constants.TRAP_NONE)
			{
				gameObject.renderer.material.color = new Color(0,0,0);
				AttackLevelUp(true);
				SpeedLevelUp(true);
				SetSpeedDivider(StandardDivider);
				// Instantiate controls of the trap
				GameObject controlAttack = Instantiate(Resources.Load("Prefabs/" + Constants.BUTTON_TRAP_ATTACK_LEVEL), this.gameObject.transform.position + new Vector3(-3f, 10, -1f), Quaternion.identity) as GameObject;
				controlAttack.renderer.material.color = new Color(1,0,0);				
				controlAttack.transform.localScale = new Vector3(2, 1, 1);
				controlAttack.transform.parent = GameObject.FindGameObjectWithTag(Constants.TAG_HUD).transform;
				
				GameObject controlSpeed = Instantiate(Resources.Load("Prefabs/" + Constants.BUTTON_TRAP_SPEED_LEVEL), this.gameObject.transform.position + new Vector3(3f, 10, -1f), Quaternion.identity) as GameObject;
				controlSpeed.renderer.material.color = new Color(0, 1, 0);
				controlSpeed.transform.localScale = new Vector3(2, 1, 1);
				controlSpeed.transform.parent = GameObject.FindGameObjectWithTag(Constants.TAG_HUD).transform;
				
			}
		}
		
		public void AttackLevelUp(bool CheckActive)
		{
			if ((this.IsActive() || CheckActive) && GetEvolutionStatus() < Constants.TRAP_ATTACK_MAXLEVEL)
			{
				SetEvolutionStatus(GetEvolutionStatus() + 1 );
				gameObject.renderer.material.color = new Color(
					gameObject.renderer.material.color.r + (1f / Constants.TRAP_ATTACK_MAXLEVEL * GetEvolutionStatus()),
					gameObject.renderer.material.color.g,
					gameObject.renderer.material.color.b
				);
			}
		}
		
		public void SpeedLevelUp(bool CheckActive)
		{
			if ((this.IsActive() || CheckActive) && GetSpeedLevel() < Constants.TRAP_SPEED_MAXLEVEL)
			{
				SetSpeedLevel(GetSpeedLevel() + 1);
				SetSpeedDivider(GetSpeedDivider() * 2);
				gameObject.renderer.material.color = new Color(
					gameObject.renderer.material.color.r,
					gameObject.renderer.material.color.g + (1f / Constants.TRAP_SPEED_MAXLEVEL * GetSpeedLevel()),
					gameObject.renderer.material.color.b
				);
				
				// If the trap already has enemies, update its velocity
				foreach(Affected affected in EnemiesInTrap)
				{
					ApplySpeedDivider(affected.GetCharacter());
				}
			}
		}
		
		public void SellTrap(bool CheckActive)
		{
			if ((this.IsActive() || CheckActive))
			{
				ResetTrap();
			}
		}
		
		private void ApplySpeedDivider(GameObject character) 
		{
			if (IsEnemy(character) && IsAffected(character) && SpeedDivider != 0)
			{
				//float originalSpeed = character.gameObject.GetComponent<PgtCharacter>().GetSpeed();
				character.GetComponent<PgtCharacter>().SetSpeed(AffectedOriginalSpeed(character) / SpeedDivider);
				
			}
		}
		
		private void ResetSpeedDivider(GameObject character) 
		{
			if (IsEnemy(character) && IsAffected(character) && SpeedDivider != 0)
			{
				//float currentSpeed = character.GetComponent<PgtCharacter>().GetSpeed();
				//character.GetComponent<PgtCharacter>().SetSpeed(currentSpeed * SpeedDivider);
				
				// Returns its original speed
				character.GetComponent<PgtCharacter>().SetSpeed(AffectedOriginalSpeed(character));
			}
		}
		
		// Enemy enters in the trap
		public void OnTriggerStay(Collider c)
		{
			// Care about only unaffected enemies
			if ( this.IsActive() && IsEnemy(c.gameObject) && !IsAffected(c.gameObject))
			{
				// Add enemy and its original speed
				EnemiesInTrap.Add(new Affected(c.gameObject, c.gameObject.GetComponent<PgtCharacter>().GetSpeed()));
				// Speed reductor applies one time
				ApplySpeedDivider(c.gameObject);
			}
		}
		
		// Enemy exits the trap
		public void OnTriggerExit(Collider c)
		{	
			// Care only affected enemies
			if ( IsEnemy(c.gameObject) && IsAffected(c.gameObject))
			{
				ResetSpeedDivider(c.gameObject);
				RemoveAffect(c.gameObject);
			}
		}
		
		private void RemoveAffect(GameObject c)
		{
			for (int i=0; i < EnemiesInTrap.Count; i++)
			{
				if (EnemiesInTrap[i].GetCharacter() == c)
				{
					EnemiesInTrap.RemoveAt(i);
					return;
				}
			}
		}
		
		private bool IsAffected(GameObject c)
		{
			for (int i=0; i < EnemiesInTrap.Count; i++)
			{
				if (EnemiesInTrap[i].GetCharacter() == c) return true;
			}
			return false;
		}
		
		private float AffectedOriginalSpeed(GameObject c)
		{
			for (int i=0; i < EnemiesInTrap.Count; i++)
			{
				if (EnemiesInTrap[i].GetCharacter() == c) return EnemiesInTrap[i].GetOriginalSpeed();
			}
			return 1;
		}
		
		private bool IsActive()
		{
			return (GetSpeedLevel() > 0 && GetEvolutionStatus() > 0);
		}
		
		private void ResetTrap()
		{
			SetSpeedLevel(0);
			SetEvolutionStatus(0);
			SetSpeedDivider(0);
			gameObject.renderer.material.color = new Color(0,0,0);
			ColorIncr = Mathf.Abs(ColorIncr);
			ColorTimer = 0;
		}
		
		public void SetSpeedLevel(int Level)
		{
			this.SpeedLevel = Level;
		}
		
		public int GetSpeedLevel()
		{
			return this.SpeedLevel;
		}
		
		public void SetSpeedDivider(float SpeedDivider)
		{
			this.SpeedDivider = SpeedDivider;
		}
		
		public float GetSpeedDivider()
		{
			return this.SpeedDivider;
		}
		
		public void SetIntervalAttackTime(float Interval)
		{
			this.IntervalAttackTime = Interval;
		}
		
	}
}
