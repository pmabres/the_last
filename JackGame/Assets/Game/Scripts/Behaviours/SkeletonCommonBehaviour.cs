using UnityEngine;
using System.Collections;
using Pragmatia;

namespace JackGame
{
	public class SkeletonCommonBehaviour : EnemyBehaviour 
	{		
		private static int EnemyNumber;
		public float SPEED = 0.8f;
		public const int HEALTH = 400;
		
		public SkeletonCommonBehaviour() 
		{
			//SPEED = Random.Range(0.02f,0.06f);
			SetSpeed(SPEED);
			SetTotalHealth(HEALTH);
			SetCurrentHealth(HEALTH);		
			SetShortAttackAnimation("Attack");
			SetWalkLeftAnimation("Walk");
			SetAttackDelayTotalTime(2);
			SetAttackRangeShort(2);
			SetHopeRelease(5, 10);
			SetHomeLifeDrain(3);
			// Stun probability (float: 0 never - 1 always). Check if the character has this animation!
			SetStunProbability(0.3f);
		}
		
		// Update is called once per frame
		void Update () 
		{
			base.Update();
			
			int attackType = GetAttackTypeToExecute();
			if (attackType == 1)
			{	
				Attack(attackType);
			} 
			else 
			{
				base.Move();
			}
		}
	}
}