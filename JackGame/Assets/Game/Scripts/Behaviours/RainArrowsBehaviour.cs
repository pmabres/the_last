using UnityEngine;
using System.Collections;
using Pragmatia;

namespace JackGame
{
	public class RainArrowsBehaviour : PgtCharacter
	{		
		private int QuantityPerRain; // Arrow quantity per rain
		private int Counter = 0;
		
		public void Awake() 
		{
			// Collection container
			SetCollection(Constants.COLLECTION_ALLIES);
			// Collection to attack
			SetCollectionToAttack(Constants.COLLECTION_ENEMIES);
			SetAttackDelayTotalTime(0);
			SetAttackRangeMedium(0); // From here the tower can't attack
			SetHittable(false); // Can't be hittable
			SetQuantityPerRain(50);			
			SetAttackRangeLong(100);
			SetAttackTypeToExecute(Constants.ATTACK_LONG);
			SetWeaponByAttackType();			
		}
		
		public void Rain()
		{
			GetDistance();
			float Target = Vector3.Distance(new Vector3(gameObject.transform.position.x, 0, 0), new Vector3(GameObject.FindGameObjectWithTag(Constants.TAG_JACK).transform.position.x, 0, 0)) * 2.3f;
			if (GetNextTargetDistance().x <= GetAttackRangeLong() && 0 < GetNextTargetDistance().x )
			{
				Target = GetNextTargetDistance().x;
			}
			for (int i=0; i <= GetQuantityPerRain(); i++)
			{			
				//AttackIncreaseAngle( (i*2) );
				DoAttack(0, new Vector2(Target + i*0.04f, i*0.1f));
			}
			
			// This characters inactivates itsefl after performed the rain
			//this.gameObject.SetActive(false);
		}

		public void SetQuantityPerRain(int QuantityPerRain)
		{
			this.QuantityPerRain = QuantityPerRain;
		}
		public int GetQuantityPerRain()
		{
			return this.QuantityPerRain;
		}

	}
}