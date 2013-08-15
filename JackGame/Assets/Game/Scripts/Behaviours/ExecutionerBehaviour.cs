using UnityEngine;
using System.Collections;
using Pragmatia;

namespace JackGame
{
	public class ExecutionerBehaviour : EnemyBehaviour 
	{
		//private const string SelfName = "Test";
		private static int EnemyNumber;
		public float SPEED = 0.007f;
		public const int HEALTH = 2000;
		
		public ExecutionerBehaviour() 
		{
			//SPEED = Random.Range(0.02f,0.06f);
			SetSpeed(SPEED);
			SetTotalHealth(HEALTH);
			SetCurrentHealth(HEALTH);		
			SetShortAttackAnimation("Attack");
			SetWalkLeftAnimation("Walk");
			SetAttackDelayTotalTime(2);
			SetAttackRangeShort(3);		
			SetHomeLifeDrain(50);
			// Stun probability (float: 0 never - 1 always). Check if the character has this animation!
			SetStunProbability(0.1f);
		}
		
		// Update is called once per frame
		void Update () 
		{
			base.Update();
			
			int attackType = GetAttackTypeToExecute();
			
			if (attackType == Constants.ATTACK_SHORT)
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