using UnityEngine;
using System.Collections;
using Pragmatia;

namespace JackGame
{
	public class TowerBodiesBehaviour : TowerBehaviour
	{		
		private int BodiesQuantity = 0;
		private float CollectRange;
		private float CollectTime;
		private float CollectTimeCurrent;
		
		public void Awake() 
		{
			base.Awake();
			SetCollectRange(10);
			SetCollectTime(1);
			SetAutoAttack(false);
			SetAttackRangeMedium(5); // From here the tower can't attack
		}
		
		// Update is called once per frame
		void Update () 
		{
			base.Update();
			
			// Check if can collect bodies
			CollectTimeCurrent += Time.deltaTime;
			if (CollectTimeCurrent >= CollectTime)
			{
				CollectBodies();
				CollectTimeCurrent = 0;
			}

			// Attack
			if (CanThrowBody())
			{
				SetAttackTypeToExecute(Constants.ATTACK_LONG);
				SetWeaponByAttackType();
				DoAttack();	
				CanAttack = false;
				SetAttackDelayTotalTimer(0);
				BodiesQuantity--;
			}
				
		}
		
		public void CollectBodies()
		{
			if (Statics.characters.Instances.ContainsKey(GetCollectionToAttack()) && Statics.characters.Instances[GetCollectionToAttack()].Count > 0)
			{
				for (int i = 0; i < Statics.characters.Instances[GetCollectionToAttack()].Count; i++) 
				{
					// Only take care of dead bodies
					if (Statics.characters.Instances[GetCollectionToAttack()][i].GetComponent<PgtCharacter>().IsDead())
					{
						// If in range
						if (Statics.characters.Instances[GetCollectionToAttack()][i].transform.position.x >= this.gameObject.transform.position.x - GetCollectRange() && 
							Statics.characters.Instances[GetCollectionToAttack()][i].transform.position.x <= this.gameObject.transform.position.x + GetCollectRange()
							)
						{
							BodiesQuantity++;
						}
					}
				}
			}
		}
		
		public bool CanThrowBody()
		{
			return (GetBodiesQuantity() > 0 && CanAttack);
		}
		
		public void SetBodiesQuantity(int BodiesQuantity)
		{
			this.BodiesQuantity = BodiesQuantity;
		}
		
		public int GetBodiesQuantity()
		{
			return this.BodiesQuantity;
		}
		
		public void SetCollectRange(float CollectRange)
		{
			this.CollectRange = CollectRange;
		}
		
		public float GetCollectRange()
		{
			return this.CollectRange;
		}
		
		public void SetCollectTime(float CollectTime)
		{
			this.CollectTime = CollectTime;
		}

	}
}