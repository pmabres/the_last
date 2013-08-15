using UnityEngine;
using System.Collections;
using Pragmatia;

namespace JackGame
{
	public class BarricadeBehaviour : PgtCharacter 
	{	
		private const int INITIAL_HEALTH = 1000;
		private GameObject controlLevel;
		private int level = 0;
		void Awake()
		{
			SetTotalHealth(INITIAL_HEALTH);
			SetCurrentHealth(INITIAL_HEALTH);
			SetSpeed(0);
			SetAttackDelayTotalTime(1);
			// Collection container
			SetCollection(Constants.COLLECTION_ALLIES);
			// Collection to attack
			SetCollectionToAttack(Constants.COLLECTION_ENEMIES);
			SetReviveTime(60);
		}
		
		void Start()
		{
			base.Start();
			// Instantiate controls of the barricade
			controlLevel = Instantiate(Resources.Load("Prefabs/" + Constants.BUTTON_BARRICADE_LEVEL), this.gameObject.transform.position + new Vector3(0, 8, -1.5f), Quaternion.identity) as GameObject;
			controlLevel.renderer.material.color = new Color(0,0,5);
			controlLevel.transform.localScale = new Vector3(2, 1, 1);
			controlLevel.transform.parent = GameObject.FindGameObjectWithTag(Constants.TAG_HUD).transform;
			SetHealthDisplayPosition(new Vector2(GetHealthDisplayPosition().x,GetHealthDisplayPosition().y + 2));
		}
		
		void Update()
		{
			base.Update();
			//InactiveCurrentTime += Time.deltaTime;
			if (!IsDead())
			{			
				gameObject.renderer.material.color = new Color(8, 8 - 1.5f * this.level, 8 - 1.5f*this.level, 1);
			}
			else
			{				
				gameObject.renderer.material.color = new Color(0, 0, 0, 255);
			}
						
			// Move to activate the trigger
			//gameObject.transform.Translate(0, 0, 0);
		}
		
		public void Upgrade()
		{
			int actualEvolution = this.level;
			
			// I upgraded it!
			if (actualEvolution < Constants.BARRICADE_MAXLEVEL)
			{
				this.level++;
				SetTotalHealth((int) (GetTotalHealth() + (GetTotalHealth() /2)) );
				
				// Restore health
				this.SetCurrentHealth(GetTotalHealth());
				
				// Reduce revive time
				this.SetReviveTime(GetReviveTime() * 0.75f);
				
				UpdateHealthDisplay();
			}
			
			// Level limit
			if (this.level == Constants.BARRICADE_MAXLEVEL && controlLevel != null)
			{
				Destroy(controlLevel);
			}
			
			
		}
	}
}