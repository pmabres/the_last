using UnityEngine;
using System.Collections;
using Pragmatia;

namespace JackGame
{
	public class UndeadBossBehaviour : EnemyBehaviour 
	{
		//private const string SelfName = "Test";
		private static int EnemyNumber;
		public float SPEED = 0.5f;
		public const int HEALTH = 5000;
		
		public UndeadBossBehaviour() 
		{
			//SPEED = Random.Range(0.02f,0.06f);
			SetSpeed(SPEED);
			SetTotalHealth(HEALTH);
			SetCurrentHealth(HEALTH);		
			SetShortAttackAnimation("Attack");
			SetWalkLeftAnimation("Walk");
			SetAttackDelayTotalTime(1);
			SetAttackRangeShort(2);
			SetHomeLifeDrain(30);
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